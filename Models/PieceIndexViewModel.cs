namespace CGMD.Models
{
    public class PieceIndexViewModel
    {
        public IEnumerable<Piece> Pieces { get; set; }
        public string SearchPhrase { get; set; }
        public string CurrentSort { get; set; }
    }
}
