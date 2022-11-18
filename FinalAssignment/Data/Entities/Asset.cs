using System.ComponentModel.DataAnnotations;
using Common.Enums;

namespace Data.Entities
{
    public class Asset
    {
        public Guid AssetId { get; set; }
        public string AssetCode { get; set; }

        [Required(ErrorMessage = "Asset name is required")]
        public string AssetName { get; set; }
        public Guid? CategoryForeignId { get; set; }
        public string? CategoryName { get; set; }
        public AssetStateEnum AssetStatus { get; set; }
        public string Specification { get; set; }
        public DateTime InstalledDate { get; set; }
        public string Location { get; set; }

        public virtual Category Category { get; set; }
    }
}
