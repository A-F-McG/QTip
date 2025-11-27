using System.ComponentModel.DataAnnotations;

namespace Backend.models
{
    public class TokenisedSubmission
    {
        public int Id { get; set; }
        [Required]
        public required string TokenisedText {  get; set; }
    }
}
