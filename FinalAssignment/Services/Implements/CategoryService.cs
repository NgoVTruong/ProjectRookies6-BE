using Data.Entities;
using FinalAssignment.DTOs.Asset;
using FinalAssignment.Repositories.Interfaces;
using FinalAssignment.Services.Interfaces;

namespace FinalAssignment.Services.Implements
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Category?> Create(CategoryRequest createRequest)
        {
            if (createRequest == null) return null;
            var newCategory = new Category
            {
                Id = Guid.NewGuid(),
                CategoryCode = createRequest.CategoryCode,
                CategoryName = createRequest.CategoryName,
            };
            var createCategory = await _categoryRepository.CreateAsync(newCategory);
            _categoryRepository.SaveChanges();

            return new Category
            {
                Id = Guid.NewGuid(),
                CategoryCode =createRequest.CategoryCode,
                CategoryName=createCategory.CategoryName,
            };
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByName(string categoryName)
        {
            return await _categoryRepository.GetOneAsync(x => x.CategoryName == categoryName);
        }
    }
}