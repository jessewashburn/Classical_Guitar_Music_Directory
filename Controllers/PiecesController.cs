using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CGMD.Data;
using CGMD.Models;
using Newtonsoft.Json;

namespace CGMD.Controllers
{

    /// <summary>
    /// Controller for handling operations related to classical guitar pieces.
    /// </summary>
    /// 
    public class PiecesController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PiecesController"/> class.
        /// </summary>
        /// <param name="context">The database context used for interacting with entity models.</param>
        /// 
        public PiecesController(ApplicationDbContext context, ILogger<PiecesController> logger)
        {
            _context = context;
        }

        /// <summary>
        /// Displays the list of pieces with optional sorting and search capabilities.
        /// </summary>
        /// <param name="sortOrder">The order in which to sort the pieces.</param>
        /// <param name="searchPhrase">The search term to filter pieces.</param>
        /// <param name="clearSearch">A flag indicating whether to clear the current search.</param>
        /// <returns>The Index view populated with a list of pieces.</returns>
        public async Task<IActionResult> Index(string sortOrder, string searchPhrase = "", int page = 1, int pageSize = 200, bool clearSearch = false)
        {
            if (clearSearch)
            {
                HttpContext.Session.Remove("SearchModel");
                return RedirectToAction(nameof(Index));
            }

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "YOB_DESC" : sortOrder;
            ViewData["CurrentSort"] = sortOrder;

            AdvancedSearchModel searchModel = new AdvancedSearchModel();
            if (string.IsNullOrEmpty(searchPhrase))
            {
                var searchModelJson = HttpContext.Session.GetString("SearchModel");
                if (!string.IsNullOrEmpty(searchModelJson))
                {
                    searchModel = JsonConvert.DeserializeObject<AdvancedSearchModel>(searchModelJson) ?? new AdvancedSearchModel();
                }
            }
            else
            {
                searchModel.searchPhrase = searchPhrase;
                HttpContext.Session.SetString("SearchModel", JsonConvert.SerializeObject(searchModel));
            }
            ViewData["SearchPhrase"] = searchModel?.searchPhrase;

            IQueryable<Piece> query = _context.Piece.AsQueryable();

            if (!string.IsNullOrEmpty(searchModel?.searchPhrase))
            {
                var searchTerms = searchModel.searchPhrase.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                query = searchTerms.Aggregate(query, (currentQuery, term) =>
                    currentQuery.Where(p => p.Title.Contains(term) || p.Composer.Contains(term) || (p.Tags != null && p.Tags.Contains(term))));
            }

            query = ApplySorting(query, sortOrder);

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            var pieces = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return View(pieces);
        }


        /// <summary>
        /// Applies sorting to the query based on the specified sort order.
        /// </summary>
        /// <param name="query">The query to be sorted.</param>
        /// <param name="sortOrder">The order in which to sort the query.</param>
        /// <returns>The sorted query.</returns>
        private IQueryable<Piece> ApplySorting(IQueryable<Piece> query, string sortOrder)
        {
            return sortOrder switch
            {
                "YOB_ASC" => query.OrderBy(p => p.YOB),
                "YOB_DESC" => query.OrderByDescending(p => p.YOB),
                "Composer_ASC" => query.OrderBy(p => p.Composer),
                "Composer_DESC" => query.OrderByDescending(p => p.Composer),
                _ => query.OrderByDescending(p => p.YOB), // Default sort order
            };
        }


        /// <summary>
        /// Displays details for a specific piece.
        /// </summary>
        /// <param name="id">The ID of the piece to display.</param>
        /// <returns>The Details view for the specified piece.</returns>
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

        /// <summary>
        /// Displays the Create view for adding a new piece.
        /// </summary>
        /// <returns>The Create view.</returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Displays the search form.
        /// </summary>
        /// <returns>The search form view.</returns>
        public IActionResult ShowSearchForm()
        {
            return View();
        }


        /// <summary>
        /// Performs a search based on the provided search phrase and displays the results.
        /// </summary>
        /// <param name="searchPhrase">The search term used to filter the results.</param>
        /// <returns>The Index view populated with search results.</returns>
        public async Task<IActionResult> ShowSearchResults(String searchPhrase)
        {
            // Set the search phrase to ViewData to be displayed in the view
            ViewData["SearchPhrase"] = searchPhrase;

            // Perform the search and pass the result to the view
            var result = await _context.Piece
                .Where(p => p.Title.Contains(searchPhrase) || p.Composer.Contains(searchPhrase) 
                || (p.Tags != null && p.Tags.Contains(searchPhrase))).ToListAsync();

            return View("Index", result);
        }

        /// <summary>
        /// Displays the advanced search form.
        /// </summary>
        /// <returns>The advanced search form view.</returns>
        public IActionResult ShowAdvSearch()
        {
            // Retrieve the advanced search model from session if it exists
            var searchModelJson = HttpContext.Session.GetString("AdvancedSearchCriteria");
            var searchModel = string.IsNullOrEmpty(searchModelJson) ?
                new AdvancedSearchModel() :
                JsonConvert.DeserializeObject<AdvancedSearchModel>(searchModelJson);

            // Pass the model to the view to make the form sticky
            return View(searchModel);
        }


        /// <summary>
        /// Displays the search results based on advanced search criteria.
        /// </summary>
        /// <param name="model">The model containing the advanced search criteria.</param>
        /// <param name="sortOrder">The order in which to sort the results.</param>
        /// <returns>The Index view populated with advanced search results.</returns>
 
        [HttpPost]
        public IActionResult ShowAdvResults(AdvancedSearchModel model, string sortOrder)
        {
            // Store the search model in Session
            HttpContext.Session.SetString("AdvancedSearchCriteria", JsonConvert.SerializeObject(model));

            // Build the query based on the advanced search criteria
            var query = BuildQueryBasedOnSearchCriteria(model);

            // Include the sortOrder in the ViewData to maintain the sort order on postback
            ViewData["CurrentSort"] = sortOrder;

            // Execute the query and return the Index view with results
            var result = query.ToList(); // Use ToList instead of ToListAsync
            return View("Index", result);
        }

        /// <summary>
        /// Checks if a Piece with a given ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the piece to check.</param>
        /// <returns>True if the piece exists, otherwise false.</returns>
        private bool PieceExists(int id)
        {
            return _context.Piece.Any(e => e.ID == id);
        }

        /// <summary>
        /// Handles the POST request for confirming the submission of a new piece.
        /// Displays a confirmation page after a piece is successfully submitted.
        /// </summary>
        /// <param name="piece">The Piece object that has been submitted.</param>
        /// <returns>A view displaying confirmation of the submitted piece.</returns>
        [HttpPost]
        public IActionResult PostConf(Piece piece)
        {
            return View();
        }

        /// <summary>
        /// Builds a search phrase based on the advanced search criteria.
        /// </summary>
        /// <param name="model">The model containing the advanced search criteria.</param>
        /// <returns>The constructed search phrase.</returns>
        private string BuildSearchPhrase(AdvancedSearchModel model)
        {
            var phrases = new List<string>();
            if (!string.IsNullOrEmpty(model.Title)) phrases.Add(model.Title);
            if (!string.IsNullOrEmpty(model.Composer)) phrases.Add(model.Composer);
            // ... include other fields as needed ...
            return string.Join(" ", phrases);
        }

        /// <summary>
        /// Builds a query based on the provided advanced search criteria.
        /// </summary>
        /// <param name="model">The model containing the advanced search criteria.</param>
        /// <returns>An IQueryable representing the filtered query.</returns>
        private IQueryable<Piece> BuildQueryBasedOnSearchCriteria(AdvancedSearchModel model)
        {
            var query = _context.Piece.AsQueryable();

            // Apply filters based on the model properties
            if (!string.IsNullOrEmpty(model.Title))
            {
                // Split the search phrases by space to search for each word
                var searchPhrases = model.Title.Split(' ');
                query = query.Where(p => p.Title.Contains(model.Title) ||
                                    p.Composer.Contains(model.Title) ||
                                    (p.Tags != null && p.Tags.Contains(model.Title)));
            }

            //Apply filters for tags 
            if (!string.IsNullOrEmpty(model.Tags))
            {
                // Force client-side evaluation with ToList().
                var piecesWithTags = query.ToList().Where(p => p.Tags != null &&
                    model.Tags.Split(' ').Any(tag => p.Tags.Contains(tag)));
                // Re-create the IQueryable from the filtered list.
                query = piecesWithTags.AsQueryable();
            }
            
            //Apply filter for composers
            if (!string.IsNullOrEmpty(model.Composer))
                query = query.Where(p => p.Composer.Contains(model.Composer));

            if (model.MinYOB.HasValue && model.MaxYOB.HasValue)
            {
                query = query.Where(p => p.YOB >= model.MinYOB.Value && p.YOB <= model.MaxYOB.Value);
            }

            // Search by instrument
            if (!string.IsNullOrEmpty(model.Inst))
            {
                var searchInst = model.Inst.Trim().ToLower();
                query = query.Where(p => !string.IsNullOrEmpty(p.Inst) && p.Inst.ToLower().Contains(searchInst));
            }

            // Search by nationality
            if (!string.IsNullOrEmpty(model.Nation))
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.Nation) && p.Nation.Contains(model.Nation));
            }

            return query;
        }

        /// <summary>
        /// Clears the current search criteria from the session and redirects to the Index action.
        /// </summary>
        /// <returns>A redirection to the Index action.</returns>
        public IActionResult ClearSearchAndRedirectToIndex()
        {
            // Clear search criteria from Session
            HttpContext.Session.Remove("AdvancedSearchCriteria");

            // Redirect to Index action
            return RedirectToAction(nameof(Index));
        }
    }
}
