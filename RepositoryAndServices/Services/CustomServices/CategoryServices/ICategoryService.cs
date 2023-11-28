using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;

namespace RepositoryAndServices.Services.CustomServices.CategoryServices
{
    public interface ICategoryService
    {
        Task<ICollection<CategoryViewModel>> GetAll();
        Task<CategoryViewModel> Get(Guid Id);
        Category GetLast();
        Task<bool> Insert(CategoryInsertModel categoryInsertModel);
        Task<bool> Update(CategoryUpdateModel categoryUpdateModel);
        Task<bool> Delete(Guid Id);
        Task<Category> Find(Expression<Func<Category, bool>> match);
    }
}
