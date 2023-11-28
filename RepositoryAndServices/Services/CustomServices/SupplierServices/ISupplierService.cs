using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;

namespace RepositoryAndServices.Services.CustomServices.SupplierServices
{
    public interface ISupplierService
    {
        Task<ICollection<UserViewModel>> GetAll();
        Task<UserViewModel> Get(Guid Id);
        User GetLast();
        Task<bool> Insert(UserInsertModel userInsertModel, string photo);
        Task<bool> Update(UserUpdateModel userUpdateModel, string photo);
        Task<bool> Delete(Guid Id);
        Task<User> Find(Expression<Func<User, bool>> match);
    }
}
