using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class UserType : BaseEntity
    {
        [Required(ErrorMessage = "User Type is required...!")]
        [StringLength(10)]
        public string TypeName { get; set; }

        [JsonIgnore]
        public virtual List<User> Users { get; set; }
    }
}
