using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Item : BaseEntity
    {
        [Required(ErrorMessage = "Please Enter ItemCode...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "Item Code start with # and Only Number and character are allowed eg(#Item1001)")]
        [StringLength(10)]
        public string ItemCode { get; set; }

        [Required(ErrorMessage = "Please Enter Item Name...!")]
        [StringLength(100)]
        public string ItemName { get; set; }

        [Required(ErrorMessage = "Please Enter Item Description...!")]
        public string ItemDescription { get; set; }

        [Required(ErrorMessage = "Please Enter Item Price...!")]
        public double ItemPrice { get; set; }

        [Required(ErrorMessage = "Please select Category...!")]
        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public virtual List<ItemImages> ItemImages { get; set; }

        public virtual SupplierItem SupplierItem { get; set; }

        public virtual CustomerItem CustomerItem { get; set; }

        public virtual Category Category { get; set; }
    }
}