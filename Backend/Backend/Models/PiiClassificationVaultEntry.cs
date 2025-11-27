using System.ComponentModel.DataAnnotations;

namespace Backend.models
{
    public class PiiClassificationVaultEntry
    {
        public int Id { get; set; }
        [Required]
        public required string TokenizedPii { get; set; }
        [Required]
        public required byte[] EncryptedPii { get; set; }   
        [Required]
        public required byte[] Iv { get; set; }
        [Required]
        public required string Type { get; set; }
    }
}
