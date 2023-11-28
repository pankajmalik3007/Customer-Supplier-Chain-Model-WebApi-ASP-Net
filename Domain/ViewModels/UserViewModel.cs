using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserAddress { get; set; }
        public string UserPhoneno { get; set; }
        public string UserPhoto { get; set; }
        public List<UserTypeViewModel> UserType { get; set; } = new List<UserTypeViewModel>();
    }
    public class UserInsertModel
    {
        [Required(ErrorMessage = "Please Enter UserId...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "UserID start with # and Only Number and character are allowed eg(#User1001)...!")]
        [StringLength(10)]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Please Enter UserName...!")]
        [StringLength(100)]
        public string UserName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$", ErrorMessage = "Enter Valid Email...!")]
        [Required(ErrorMessage = "Please Enter Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter Address...!")]
        [StringLength(500)]
        public string UserAddress { get; set; }

        [RegularExpression(@"^[+]?([0-9]{1,2})+\-([0-9]{10})", ErrorMessage = "Enter Valid Phone No eg(+91-1234578596)...!")]
        public string Phoneno { get; set; }

        public IFormFile ProfilePhoto { get; set; }
        public bool? IsActive { get; set; }

    }
    public class UserUpdateModel : UserInsertModel
    {
        [Required(ErrorMessage = "Id is neccessory for updation...!")]
        public Guid Id { get; set; }

    }
    public class LoginModel
    {

        [Required(ErrorMessage = "Please Enter UserName...!")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }
    }
}
