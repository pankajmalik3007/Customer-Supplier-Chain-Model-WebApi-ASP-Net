using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Category : BaseEntity
    {
        [Required(ErrorMessage = "Please Enter Category Name...!")]
        [StringLength(50)]
        public string CategoryName { get; set; }

        [JsonIgnore]
        public virtual List<Item> Items { get; set; }
    }
}
