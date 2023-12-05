namespace CGMD.Models
{
    /// <summary>
    /// Represents the model for a contact form.
    /// Includes properties for the user's name, email, subject line, and message content.
    /// </summary>
    public class ContactFormModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SubjectLine { get; set; }
        public string Message { get; set; }
    }
}
