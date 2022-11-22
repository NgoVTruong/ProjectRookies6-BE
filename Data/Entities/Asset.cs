using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Data.Entities
{
    public class Asset
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public string AssetCode { get; set; }

        [Required(ErrorMessage = "Asset name is required")]
        [MinLength(5)]
        [MaxLength(20)]
        public string AssetName { get; set; }

        public string? CategoryName { get; set; }
        public AssetStateEnum AssetStatus { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public string Location { get; set; }
        public bool IsDeleted { get; set; } //false


        public virtual Category Category { get; set; }
        public virtual ICollection<Assignment> Assignments { get; set; }
    }
}
