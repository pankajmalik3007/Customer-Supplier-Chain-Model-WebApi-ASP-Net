using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;

namespace RepositoryAndServices.Services.CustomServices.UserTypeServices
{
    public interface IUserTypeService
    {
        Task<ICollection<UserTypeViewModel>> GetAll();
        Task<UserTypeViewModel> Get(Guid Id);
        UserType GetLast();
        Task<bool> Insert(UserTypeInsertModel userInsertModel);
        Task<bool> Update(UserTypeUpdateModel userUpdateModel);
        Task<bool> Delete(Guid Id);
        Task<UserType> Find(Expression<Func<UserType, bool>> match);
    }
}
