using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CustomerItem : BaseEntity
    {
        [Required(ErrorMessage = "Please Enter User Id...!")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Required(ErrorMessage = "Please Enter Item Id...!")]
        public Guid ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
