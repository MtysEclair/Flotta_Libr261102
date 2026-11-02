
using System.ComponentModel.DataAnnotations;

namespace KonyvtariKolcsonzes.Models
{
    public class Konyv
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Cim { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Szerzo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string ISBN { get; set; } = string.Empty;

        public int KiadasEve { get; set; }

        public int? OlvasoId { get; set; }

        public Olvaso? Olvaso { get; set; }
    }
}
