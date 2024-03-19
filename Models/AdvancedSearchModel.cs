namespace CGMD.Models
{
    /// <summary>
    /// Represents the model for advanced search functionality.
    /// Allows filtering based on various criteria such as title, composer, year of birth, etc.
    /// </summary>
    public class AdvancedSearchModel
    {
        public string? Title { get; set; }
        public string? Composer { get; set; }
        public int? MinYOB { get; set; }
        public int? MaxYOB { get; set; }
        public string? Nation { get; set; }
        public string? Inst { get; set; }
        public string? Tags { get; set; }
        public string? searchPhrase { get; set; }

    }
}
