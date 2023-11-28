using Domain.Models;
using Services.common;
using Domain.ViewModels;
using System.Linq.Expressions;
using RepositoryAndServices.Repository;
using RepositoryAndServices.Services.CustomServices.UserTypeServices;

namespace RepositoryAndServices.Services.CustomServices.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly IRepository<User> _user;
        private readonly IUserTypeService _userType;

        public CustomerService(IRepository<User> user, IUserTypeService userType)
        {
            _user = user;
            _userType = userType;
        }

        public async Task<bool> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                User userAsSupplier = await _user.Get(Id);
                if (userAsSupplier != null)
                {
                    _ = _user.Delete(userAsSupplier);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public async Task<UserViewModel> Get(Guid Id)
        {
            var result = await _user.Get(Id);
            var usertype = await _userType.Find(x => x.TypeName == "Customer");

            if (result == null)
                return null;
            else
            {
                if (result.UserTypeId == usertype.Id)
                {
                    UserViewModel userView = new()
                    {
                        Id = result.Id,
                        UserID = result.UserID,
                        UserName = result.UserName,
                        UserEmail = result.UserEmail,
                        UserPassword = Encryptor.DecryptString(result.UserPassword),
                        UserAddress = result.UserAddress,
                        UserPhoneno = result.UserPhoneno,
                        UserPhoto = result.UserPhoto
                    };
                    UserTypeViewModel view = new();
                    if (usertype != null)
                    {
                        view.Id = usertype.Id;
                        view.TypeName = usertype.TypeName;
                        userView.UserType.Add(view);
                    }
                    return userView;
                }
                return null;
            }

        }

        public async Task<ICollection<UserViewModel>> GetAll()
        {
            var usertype = await _userType.Find(x => x.TypeName == "Customer");
            ICollection<UserViewModel> supplierViewModels = new List<UserViewModel>();

            ICollection<User> result = await _user.FindAll(x => x.UserTypeId == usertype.Id);
            foreach (User supplier in result)
            {
                UserViewModel supplierView = new()
                {
                    Id = supplier.Id,
                    UserID = supplier.UserID,
                    UserName = supplier.UserName,
                    UserEmail = supplier.UserEmail,
                    UserPassword = Encryptor.DecryptString(supplier.UserPassword),
                    UserAddress = supplier.UserAddress,
                    UserPhoneno = supplier.UserPhoneno,
                    UserPhoto = supplier.UserPhoto
                };
                UserTypeViewModel userView = new();
                if (usertype != null)
                {
                    userView.Id = usertype.Id;
                    userView.TypeName = usertype.TypeName;
                    supplierView.UserType.Add(userView);
                }
                supplierViewModels.Add(supplierView);
            }
            if (result == null)
                return null;
            return supplierViewModels;
        }

        public User GetLast()
        {
            return _user.GetLast();
        }

        public async Task<bool> Insert(UserInsertModel supplierInsertModel, string photo)
        {
            var usertype = await _userType.Find(x => x.TypeName == "Customer");
            if (usertype != null)
            {

                User supplier = new()
                {
                    UserID = supplierInsertModel.UserID,
                    UserName = supplierInsertModel.UserName,
                    UserEmail = supplierInsertModel.Email,
                    UserPassword = Encryptor.EncryptString(supplierInsertModel.Password),
                    UserAddress = supplierInsertModel.UserAddress,
                    UserPhoneno = supplierInsertModel.Phoneno,
                    UserTypeId = usertype.Id,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now,
                    IsActive = supplierInsertModel.IsActive,
                    UserPhoto = photo
                };
                var result = await _user.Insert(supplier);
                return result;
            }
            else
                return false;
        }

        public async Task<bool> Update(UserUpdateModel userUpdateModel, string photo)
        {
            User supplier = await _user.Get(userUpdateModel.Id);
            if (supplier != null)
            {
                supplier.UserID = userUpdateModel.UserID;
                supplier.UserName = userUpdateModel.UserName;
                supplier.UserEmail = userUpdateModel.Email;
                supplier.UserPassword = Encryptor.EncryptString(userUpdateModel.Password);
                supplier.UserAddress = userUpdateModel.UserAddress;
                supplier.UserPhoneno = userUpdateModel.Phoneno;
                supplier.UserTypeId = supplier.UserTypeId;
                supplier.CreatedOn = supplier.CreatedOn;
                supplier.UpdatedOn = DateTime.Now;
                supplier.IsActive = userUpdateModel.IsActive;
                if (photo == " ")
                    supplier.UserPhoto = supplier.UserPhoto;
                else
                    supplier.UserPhoto = photo;

                var result = await _user.Update(supplier);
                return result;
            }
            else
            {
                return false;
            }
        }
        public Task<User> Find(Expression<Func<User, bool>> match)
        {
            return _user.Find(match);
        }
    }
}
