using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;

namespace RepositoryAndServices.Services.CustomServices.ItemServices
{
    public interface IItemService
    {
        Task<ICollection<ItemViewModel>> GetAllItemByUser(Guid id);
        Task<ItemViewModel> Get(Guid Id);
        Task<bool> Insert(ItemInsertModel itemInsertModel, string photo);
        Task<bool> Update(ItemUpdateModel itemUpdateModel, string image);
        Task<bool> Delete(Guid Id);
        Task<Item> Find(Expression<Func<Item, bool>> match);
        Task<bool> InsertExistingItem(ExistingItemInsertModel itemModel);
    }
}
