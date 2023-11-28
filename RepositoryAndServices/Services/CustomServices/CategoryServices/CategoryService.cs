using Domain.Models;
using Domain.ViewModels;
using System.Linq.Expressions;
using RepositoryAndServices.Repository;

namespace RepositoryAndServices.Services.CustomServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _serviceCategory;

        public CategoryService(IRepository<Category> category)
        {
            _serviceCategory = category;
        }

        public async Task<ICollection<CategoryViewModel>> GetAll()
        {
            ICollection<CategoryViewModel> categoryViewModel = new List<CategoryViewModel>();
            ICollection<Category> result = await _serviceCategory.GetAll();
            foreach (Category category in result)
            {
                CategoryViewModel categoryView = new()
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                };
                categoryViewModel.Add(categoryView);
            }
            return categoryViewModel;
        }

        public async Task<CategoryViewModel> Get(Guid Id)
        {
            var result = await _serviceCategory.Get(Id);
            if (result == null)
                return null;
            else
            {
                CategoryViewModel categoryViewModel = new()
                {
                    Id = result.Id,
                    CategoryName = result.CategoryName
                };
                return categoryViewModel;
            }
        }

        public Category GetLast()
        {
            return _serviceCategory.GetLast();
        }
        public Task<bool> Insert(CategoryInsertModel categoryInsertModel)
        {
            Category category = new()
            {
                CategoryName = categoryInsertModel.CategoryName,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now,
                IsActive = true
            };
            return _serviceCategory.Insert(category);
        }


        public async Task<bool> Update(CategoryUpdateModel categoryUpdateModel)
        {
            Category category = await _serviceCategory.Get(categoryUpdateModel.Id);
            if (category != null)
            {
                category.CategoryName = categoryUpdateModel.CategoryName;
                category.UpdatedOn = System.DateTime.Now;
                var result = await _serviceCategory.Update(category);
                return result;
            }
            else
                return false;
        }

        public async Task<bool> Delete(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                Category category = await _serviceCategory.Get(Id);
                if (category != null)
                {
                    _ = _serviceCategory.Delete(category);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

        }
        public Task<Category> Find(Expression<Func<Category, bool>> match)
        {
            return _serviceCategory.Find(match);
        }
    }
}
