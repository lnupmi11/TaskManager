using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using TaskManager.BLL.Extensions.Identity;
using TaskManager.BLL.Interfaces;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;
using TaskManager.DTO.Models.Category;

namespace TaskManager.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<CategoryItem> _categoryRepository;
        private readonly IRepository<UserProfile> _userRepository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<CategoryItem> categoryRepository,
                               IRepository<UserProfile> userRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<CategoryItemDTO> GetAllByUserId(string userId)
        {
            var categoriesDTO = _categoryRepository
                .GetAllWhere(c => c.UserId == null || c.UserId == userId)
                .Select(category => _mapper.Map<CategoryItemDTO>(category))
                .ToList();

            return categoriesDTO;
        }

        public List<CategoryItemDTO> GetAllByUser(ClaimsPrincipal principal)
        {
            var categoriesDTO = GetAllByUserId(principal.GetUserId());

            return categoriesDTO;
        }
    }
}
