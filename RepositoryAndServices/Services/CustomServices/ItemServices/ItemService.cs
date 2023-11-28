using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;
using RepositoryAndServices.Repository;
using RepositoryAndServices.Services.CustomServices.CategoryServices;
using RepositoryAndServices.Services.CustomServices.CustomerServices;
using RepositoryAndServices.Services.CustomServices.UserTypeServices;
using RepositoryAndServices.Services.CustomServices.SupplierServices;

namespace RepositoryAndServices.Services.CustomServices.ItemServices
{
    public class ItemService : IItemService
    {
        private readonly IRepository<Item> _item;
        private readonly IRepository<User> _user;
        private readonly IUserTypeService _userType;
        private readonly ISupplierService _supplier;
        private readonly ICustomerService _customer;
        private readonly IRepository<ItemImages> _itemImages;
        private readonly ICategoryService _category;
        private readonly IRepository<SupplierItem> _supplierItem;
        private readonly IRepository<CustomerItem> _customerItem;

        public ItemService(IRepository<Item> item, IUserTypeService userType, IRepository<User> user, IRepository<SupplierItem> serviceSupplierItem, ICustomerService serviceCustomer, ISupplierService serviceSupplier, IRepository<CustomerItem> customerItem, ICategoryService serviceCategory, IRepository<ItemImages> serviceItemImages)
        {
            _item = item;
            _user = user;
            _userType = userType;
            _supplierItem = serviceSupplierItem;
            _category = serviceCategory;
            _supplier = serviceSupplier;
            _customer = serviceCustomer;
            _itemImages = serviceItemImages;
            _customerItem = customerItem;
        }
        public async Task<ItemViewModel> Get(Guid Id)
        {
            ItemViewModel itemViewModels = new();

            SupplierItem result = await _supplierItem.Find(x => x.ItemId == Id);
            if (result != null)
            {
                Item item = await _item.Find(x => x.Id == result.ItemId);

                ItemViewModel itemView = new()
                {
                    ItemId = result.ItemId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice
                };
                Category category = await _category.Find(x => x.Id == item.CategoryId);
                CategoryViewModel categoryView = new()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                };
                itemView.Category.Add(categoryView);
                User supplier = await _supplier.Find(x => x.Id == result.UserId);
                UserView supplierView = new()
                {
                    Id = supplier.Id,
                    UserName = supplier.UserName,
                    UserPhoneno = supplier.UserPhoneno,
                    UserEmail = supplier.UserEmail,
                    UserAddress = supplier.UserAddress,
                    UserID = supplier.UserID,
                    UserPhoto = supplier.UserPhoto
                };
                itemView.User.Add(supplierView);
                //Get all itemimages and make forloop to storing it in single view
                ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == result.ItemId);
                foreach (var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.ItemImage,
                        IsActive = img.IsActive
                    };
                    itemView.ItemImages.Add(imgView);
                }
                return itemView;
                // itemViewModels.Add(itemView);
                // _logger.LogInformation("Sucessfully Fetch Item count : " + itemViewModels.Count);
            }
            else
            {
                CustomerItem customerItem = await _customerItem.Find(x => x.ItemId == Id);
                Item item = await _item.Find(x => x.Id == customerItem.ItemId);

                ItemViewModel itemView = new()
                {
                    ItemId = customerItem.ItemId,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    ItemPrice = item.ItemPrice
                };
                Category category = await _category.Find(x => x.Id == item.CategoryId);
                CategoryViewModel categoryView = new()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                };
                itemView.Category.Add(categoryView);
                User customer = await _customer.Find(x => x.Id == customerItem.UserId);
                UserView customerView = new()
                {
                    Id = customer.Id,
                    UserName = customer.UserName,
                    UserPhoneno = customer.UserPhoneno,
                    UserEmail = customer.UserEmail,
                    UserAddress = customer.UserAddress,
                    UserID = customer.UserID,
                    UserPhoto = customer.UserPhoto
                };
                itemView.User.Add(customerView);
                //Get all itemimages and make forloop to storing it in single view
                ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == customerItem.ItemId);
                foreach (var img in image)
                {
                    ItemImagesView imgView = new()
                    {
                        Id = img.Id,
                        ItemId = img.ItemId,
                        ItemImage = img.ItemImage,
                        IsActive = img.IsActive
                    };
                    itemView.ItemImages.Add(imgView);
                }
                return itemView;
                // itemViewModels.Add(itemView);
            }
        }
        public async Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id)
        {
            ICollection<ItemViewModel> itemViewModels = new List<ItemViewModel>();
            var Supplier = await _supplier.Find(x => x.Id == id);
            var Customer = await _customer.Find(x => x.Id == id); ;
            var UserType = await _userType.Find(x => x.Id == Supplier.UserTypeId || x.Id == Customer.UserTypeId);
            Console.WriteLine(UserType.TypeName, "     " + Customer.UserName + "     " + Supplier.UserName);

            if (UserType.TypeName == "Supplier")
            {
                ICollection<SupplierItem> result = await _supplierItem.FindAll(x => x.UserId == id);
                if (result != null)
                {
                    foreach (SupplierItem item in result)
                    {
                        Item items = await _item.Find(x => x.Id == item.ItemId);
                        ItemViewModel itemView = new()
                        {
                            ItemId = item.ItemId,
                            ItemCode = items.ItemCode,
                            ItemName = items.ItemName,
                            ItemDescription = items.ItemDescription,
                            ItemPrice = items.ItemPrice
                        };
                        Category category = await _category.Find(x => x.Id == items.CategoryId);
                        CategoryViewModel categoryView = new()
                        {
                            Id = category.Id,
                            CategoryName = category.CategoryName
                        };
                        itemView.Category.Add(categoryView);
                        User user = await _supplier.Find(x => x.Id == item.UserId);
                        UserView view = new()
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            UserPhoneno = user.UserPhoneno,
                            UserEmail = user.UserEmail,
                            UserAddress = user.UserAddress,
                            UserID = user.UserID,
                            UserPhoto = user.UserPhoto
                        };
                        itemView.User.Add(view);
                        //Get all itemimages and make forloop to storing it in single view
                        ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                        foreach (var img in image)
                        {
                            ItemImagesView imgView = new()
                            {
                                Id = img.Id,
                                ItemId = img.ItemId,
                                ItemImage = img.ItemImage,
                                IsActive = img.IsActive
                            };
                            itemView.ItemImages.Add(imgView);
                        }
                        itemViewModels.Add(itemView);
                    }
                    return itemViewModels;
                }
                else
                    return itemViewModels;
            }
            else
            {
                ICollection<CustomerItem> result = await _customerItem.FindAll(x => x.UserId == id);
                foreach (CustomerItem item in result)
                {
                    Item items = await _item.Find(x => x.Id == item.ItemId);
                    ItemViewModel itemView = new()
                    {
                        ItemId = item.ItemId,
                        ItemCode = items.ItemCode,
                        ItemName = items.ItemName,
                        ItemDescription = items.ItemDescription,
                        ItemPrice = items.ItemPrice
                    };
                    Category category = await _category.Find(x => x.Id == items.CategoryId);
                    CategoryViewModel categoryView = new()
                    {
                        Id = category.Id,
                        CategoryName = category.CategoryName
                    };
                    itemView.Category.Add(categoryView);
                    User user = await _customer.Find(x => x.Id == item.UserId);
                    UserView view = new()
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        UserPhoneno = user.UserPhoneno,
                        UserEmail = user.UserEmail,
                        UserAddress = user.UserAddress,
                        UserID = user.UserID,
                        UserPhoto = user.UserPhoto
                    };
                    itemView.User.Add(view);
                    //Get all itemimages and make forloop to storing it in single view
                    ICollection<ItemImages> image = await _itemImages.FindAll(x => x.ItemId == item.ItemId);
                    foreach (var img in image)
                    {
                        ItemImagesView imgView = new()
                        {
                            Id = img.Id,
                            ItemId = img.ItemId,
                            ItemImage = img.ItemImage,
                            IsActive = img.IsActive
                        };
                        itemView.ItemImages.Add(imgView);
                    }
                    itemViewModels.Add(itemView);
                }
                return itemViewModels;
            }
        }

        public async Task<bool> Insert(ItemInsertModel itemInsertModel, string image)
        {
            var user = await _user.Find(x => x.Id == itemInsertModel.UserId);
            var UserType = await _userType.Find(x => x.Id == user.UserTypeId);
            Console.WriteLine(user.UserName + "     " + UserType.TypeName);
            if (UserType.TypeName == "Supplier")
            {
                Item item = new()
                {
                    ItemCode = itemInsertModel.ItemCode,
                    ItemName = itemInsertModel.ItemName,
                    ItemDescription = itemInsertModel.ItemDescription,
                    ItemPrice = itemInsertModel.ItemPrice,
                    CategoryId = itemInsertModel.CategoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _item.Insert(item);
                if (result == true)
                {
                    SupplierItem supplierItem = new()
                    {
                        ItemId = item.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    ItemImages itemImage = new()
                    {
                        ItemId = item.Id,
                        ItemImage = image,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultItemImage = await _itemImages.Insert(itemImage);
                    if (resultItemImage == true)
                    {
                        await _supplierItem.Insert(supplierItem);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                Item item = new()
                {
                    ItemCode = itemInsertModel.ItemCode,
                    ItemName = itemInsertModel.ItemName,
                    ItemDescription = itemInsertModel.ItemDescription,
                    ItemPrice = itemInsertModel.ItemPrice,
                    CategoryId = itemInsertModel.CategoryId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemInsertModel.IsActive
                };
                var result = await _item.Insert(item);
                if (result == true)
                {
                    CustomerItem customerItem = new()
                    {
                        ItemId = item.Id,
                        UserId = itemInsertModel.UserId,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    ItemImages itemImage = new()
                    {
                        ItemId = item.Id,
                        ItemImage = image,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        IsActive = itemInsertModel.IsActive
                    };
                    var resultItemImage = await _itemImages.Insert(itemImage);
                    if (resultItemImage == true)
                    {
                        await _customerItem.Insert(customerItem);
                        return true;
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
        }
        public async Task<bool> InsertExistingItem(ExistingItemInsertModel itemModel)
        {
            var user = await _user.Find(x => x.Id == itemModel.UserId);
            var UserType = await _userType.Find(x => x.Id == user.UserTypeId);
            Console.WriteLine(user.UserName + "     " + UserType.TypeName);
            if (UserType.TypeName == "Supplier")
            {
                SupplierItem supplierItem = new()
                {
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _supplierItem.Insert(supplierItem);
                if (result == true)
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                CustomerItem customerItem = new()
                {
                    ItemId = itemModel.Id,
                    UserId = itemModel.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = itemModel.IsActive
                };
                var result = await _customerItem.Insert(customerItem);
                if (result == true)
                {
                    return true;
                }
                else
                    return false;
            }
        }
        public async Task<bool> Update(ItemUpdateModel itemUpdateModel, string image)
        {
            Item item = await _item.Get(itemUpdateModel.Id);
            item.ItemCode = itemUpdateModel.ItemCode;
            item.ItemName = itemUpdateModel.ItemName;
            item.ItemDescription = itemUpdateModel.ItemDescription;
            item.ItemPrice = itemUpdateModel.ItemPrice;
            item.CreatedOn = item.CreatedOn;
            item.UpdatedOn = DateTime.Now;
            item.IsActive = itemUpdateModel.IsActive;

            ItemImages itemImage = await _itemImages.Find(x => x.ItemId == itemUpdateModel.Id);
            itemImage.ItemId = item.Id;
            itemImage.CreatedOn = DateTime.Now;
            itemImage.UpdatedOn = DateTime.Now;
            itemImage.IsActive = itemUpdateModel.IsActive;
            if (image == null)
                itemImage.ItemImage = itemImage.ItemImage;
            else
                itemImage.ItemImage = image;
            var result = await _item.Update(item);
            if (result == true)
            {
                var resultItemImage = await _itemImages.Update(itemImage);
                if (resultItemImage == true)
                    return true;
                else
                    return false;
            }
            else
                return false;

        }
        public async Task<bool> Delete(Guid Id)
        {
            Item item = await _item.Get(Id);
            if (item != null)
            {
                SupplierItem supplier = await _supplierItem.Find(x => x.ItemId == item.Id);
                ItemImages itemImages = await _itemImages.Find(x => x.ItemId == item.Id);
                if (supplier != null)
                {
                    var resultSupplier = await _supplierItem.Delete(supplier);
                    if (resultSupplier == true)
                    {
                        var result = await DeleteItemAndItemImages(itemImages, item);
                        return result;
                    }
                    else
                    {
                        var result = await DeleteItemAndItemImages(itemImages, item);
                        return result;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }
        private async Task<bool> DeleteItemAndItemImages(ItemImages itemImages, Item item)
        {
            if (itemImages != null)
            {
                var resultItemImages = await _itemImages.Delete(itemImages);
                if (resultItemImages == true)
                {
                    var resultItem = await _item.Delete(item);
                    if (resultItem == true)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
            {
                var resultItem = await _item.Delete(item);
                if (resultItem == true)
                    return true;
                else
                    return false;
            }
        }
        public Task<Item> Find(Expression<Func<Item, bool>> match)
        {
            return _item.Find(match);
        }
    }
}
