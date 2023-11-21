using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CGMD.Data;
using CGMD.Models;
using Microsoft.Data.SqlClient;

namespace CGMD.Controllers
{
    public class PiecesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PiecesController(ApplicationDbContext context, ILogger<PiecesController> logger)
        {
            _context = context;
        }

        // GET: Pieces
        public async Task<IActionResult> Index(string sortOrder, bool clearSearch = false)
        {
            // If clearSearch is true, clear the search criteria
            if (clearSearch)
            {
                TempData["CurrentSearchPhrase"] = string.Empty;
            }

            ViewBag.CurrentSort = sortOrder;

            var query = _context.Piece.AsQueryable();

            // Attempt to retrieve search criteria from TempData if not cleared
            var searchPhrase = clearSearch ? string.Empty : TempData["CurrentSearchPhrase"] as string ?? string.Empty;

            // Apply search based on the stored advanced search phrase
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                query = query.Where(p => p.Work.Contains(searchPhrase) || p.Composer.Contains(searchPhrase) || (p.Tags != null && p.Tags.Contains(searchPhrase)));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "YOB_ASC":
                    query = query.OrderBy(p => p.YOB);
                    break;
                case "YOB_DESC":
                    query = query.OrderByDescending(p => p.YOB);
                    break;
                case "Composer_ASC":
                    query = query.OrderBy(p => p.Composer);
                    break;
                case "Composer_DESC":
                    query = query.OrderByDescending(p => p.Composer);
                    break;
                // Add more cases as needed for other sorting options
                default:
                    // Default to sorting by Year of Birth descending
                    query = query.OrderByDescending(p => p.YOB);
                    break;
            }

            // Execute the query and retrieve the result
            var result = await query.ToListAsync();

            // Return the result as a view
            return View(result);
        }

        // GET: Pieces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Piece == null)
            {
                return NotFound();
            }

            var piece = await _context.Piece
                .FirstOrDefaultAsync(m => m.ID == id);
            if (piece == null)
            {
                return NotFound();
            }

            return View(piece);
        }

        // GET: Pieces/Create
        public IActionResult Create()
        {
            return View();
        }

        // GET: Pieces/ShowSearchForm
        public IActionResult ShowSearchForm()
        {
            return View();
        }

        // Post: Pieces/ShowSearchResults
        public async Task<IActionResult> ShowSearchResults(String searchPhrase)
        {
            return View("Index", await _context.Piece.Where(p => p.Work.Contains(searchPhrase) || p.Composer.Contains(searchPhrase) || (p.Tags != null && p.Tags.Contains(searchPhrase))).ToListAsync());
        }

        // GET: Pieces/ShowAdvSearch
        public IActionResult ShowAdvSearch()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowAdvResults(AdvancedSearchModel model, bool clearSearch = false)
        {
            // If clearSearch is true, clear the search criteria
            if (clearSearch)
            {
                TempData["CurrentSearchPhrase"] = string.Empty;
            }
            else
            {
                // If the form was submitted with a search, use the form's searchPhrase
                TempData["CurrentSearchPhrase"] = model.searchPhrase;
            }

            // Build the query based on the advanced search criteria
            var query = BuildQueryBasedOnSearchCriteria(model);

            // Execute the query and return the Index view with results
            var result = await query.ToListAsync();
            return View("Index", result);
        }

        //end advSearchResults

        private bool PieceExists(int id)
        {
            return _context.Piece.Any(e => e.ID == id);
        }

        [HttpPost]
        public IActionResult PostConf(Piece piece)
        {
            return View();
        }

        // Helper method to build search phrase based on advanced search criteria
        private string BuildSearchPhrase(AdvancedSearchModel model)
        {
            var phrases = new List<string>();
            if (!string.IsNullOrEmpty(model.Title)) phrases.Add(model.Title);
            if (!string.IsNullOrEmpty(model.Composer)) phrases.Add(model.Composer);
            // ... include other fields as needed ...
            return string.Join(" ", phrases);
        }

        // Helper method to build query based on advanced search criteria
        private IQueryable<Piece> BuildQueryBasedOnSearchCriteria(AdvancedSearchModel model)
        {
            var query = _context.Piece.AsQueryable();

            // Apply filters based on the model properties
            if (!string.IsNullOrEmpty(model.Title))
                query = query.Where(p => p.Work.Contains(model.Title));

            if (!string.IsNullOrEmpty(model.Composer))
                query = query.Where(p => p.Composer.Contains(model.Composer));

            // Apply additional filter for year of birth range
            if (int.TryParse(model.MinYOB, out var minYOB) && int.TryParse(model.MaxYOB, out var maxYOB))
            {
                // Assuming YOB is stored as a string, we need to compare as integers.
                // We convert the string YOB to an integer before the comparison.
                query = query.Where(p => p.YOB != null && EF.Functions.Like(p.YOB, $"{minYOB}%") && EF.Functions.Like(p.YOB, $"{maxYOB}%"));
            }

            // Search by instrument
            if (!string.IsNullOrEmpty(model.Inst))
            {
                var searchInst = model.Inst.Trim().ToLower();
                query = query.Where(p => !string.IsNullOrEmpty(p.Inst) && p.Inst.ToLower().Contains(searchInst));
            }

            // Search by key
            if (!string.IsNullOrEmpty(model.keyOf))
            {
                var searchKey = model.keyOf.Trim().ToLower();
                query = query.Where(p => p.keyOf != null && p.keyOf.ToLower().Contains(searchKey));
            }

            // Search by nationality
            if (!string.IsNullOrEmpty(model.Nation))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.Nation) && p.Nation.Contains(model.Nation));
            }

            return query;
        }
    }
}
