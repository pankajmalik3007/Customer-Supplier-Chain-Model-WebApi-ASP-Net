using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.ViewModels
{
    public class ItemViewModel
    { 
        public Guid ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public double ItemPrice { get; set; }
        // public Guid CategoryId { get; set; }
        // public Guid UserId { get; set; }

        // public List<IFormFile> ItemImage { get; set;}
        public List<CategoryViewModel> Category { get; set; } = new List<CategoryViewModel>();
        public List<ItemImagesView> ItemImages { get; set; } = new List<ItemImagesView>();
        public List<UserView> User { get; set; } = new List<UserView>();
    }
    public class ItemInsertModel
    {
        [Required(ErrorMessage = "Please Enter ItemCode...!")]
        [RegularExpression(@"(?:\s|^)#[A-Za-z0-9]+(?:\s|$)", ErrorMessage = "Item Code start with # and Only Number and character are allowed eg(#Item1001)...!")]
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

        [Required(ErrorMessage = "Please select User...!")]
        public Guid UserId { get; set; }

        public IFormFile ItemImage { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ItemUpdateModel : ItemInsertModel
    {
        [Required(ErrorMessage = "Id is neccessory for updation...!")]
        public Guid Id { get; set; }
    }
    public class ItemImagesView
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public string ItemImage { get; set; }
        public bool? IsActive { get; set; }
    }
    public class UserView
    {
        public Guid Id { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserAddress { get; set; }
        public string UserPhoneno { get; set; }
        public string UserPhoto { get; set; }
    }
    public class ExistingItemInsertModel
    {
        [Required(ErrorMessage = "Item Id is neccessory for insertion...!")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please select User...!")]
        public Guid UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}
