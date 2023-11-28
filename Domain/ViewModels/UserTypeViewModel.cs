using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class UserTypeViewModel
    {
        public Guid Id { get; set; }
        public string TypeName { get; set; }
    }
    public class UserTypeInsertModel
    {
        [Required(ErrorMessage = "User Type is required...!")]
        [StringLength(10)]
        public string TypeName { get; set; } 
    }
    public class UserTypeUpdateModel : UserTypeInsertModel
    {
        [Required(ErrorMessage = "Id is neccessory for updation...!")]
        public Guid Id { get; set; }

    }
}
