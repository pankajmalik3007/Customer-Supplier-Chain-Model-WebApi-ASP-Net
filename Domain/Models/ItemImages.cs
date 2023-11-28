using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ItemImages : BaseEntity
    {
        [Required(ErrorMessage = "Please Select Item...!")]
        public Guid ItemId { get; set; }

        public string ItemImage { get; set; }

        [JsonIgnore]
        public virtual Item Item { get; set; }
    }
}
