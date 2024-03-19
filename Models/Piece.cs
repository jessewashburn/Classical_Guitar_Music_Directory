namespace CGMD.Models
{
    /// <summary>
    /// Represents a musical piece or composition.
    /// Includes details like composer, year of birth/death, nationality, and other attributes.
    /// </summary>
    public class Piece
    {
        public int ID { get; set; }
        public string Composer { get; set; } = string.Empty;
        public int? YOB { get; set; }
        public int? YOD { get; set; }
        public string? Nation { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Inst { get; set; }
        public string? Tags { get; set; }

    }
}
