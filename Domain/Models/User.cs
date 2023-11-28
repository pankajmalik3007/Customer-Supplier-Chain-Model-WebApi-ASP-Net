using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Please Enter UserId...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "UserID start with # and Only Number and character are allowed eg(#User1001)")]
        [StringLength(10)]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Please Enter UserName...!")]
        [StringLength(100)]
        public string UserName { get; set; }

        [RegularExpression(@"/\S+@\S+\.\S/", ErrorMessage = "Enter Valid Email...!")]
        public string UserEmail { get; set; }

        [Required]
        [StringLength(50)]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Please Enter Address...!")]
        [StringLength(500)]
        public string UserAddress { get; set; }

        [RegularExpression(@"/^[+]?(\d{1,2})+\-(\d{10})/", ErrorMessage = "Enter Valid Phone No eg(+91-1234578596)...!")]
        public string UserPhoneno { get; set; }

        public string UserPhoto { get; set; }

        [Required(ErrorMessage = "Please Select UserType...!")]
        public Guid UserTypeId { get; set; }

        [JsonIgnore]
        public virtual UserType UserType { get; set; }

        public virtual List<SupplierItem> SupplierItems { get; set; }

        public virtual List<CustomerItem> CustomerItems { get; set; }
    }
}
