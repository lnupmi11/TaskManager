using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class CategoryRepository : IRepository<CategoryItem>
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<CategoryItem, bool> predicate)
        {
            return _context.Categories
               .Include(t => t.TaskCategories)
               .Include(u => u.User)
               .Any(predicate);
        }

        public void Create(CategoryItem category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            CategoryItem category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public void Delete(CategoryItem category)
        {
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public CategoryItem Find(string id)
        {
            return _context.Categories
                .Include(t => t.TaskCategories)
                .Include(u => u.User)
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public CategoryItem Find(Func<CategoryItem, bool> predicate)
        {
            return _context.Categories
                .Include(t => t.TaskCategories)
                .Include(u => u.User)
                .Where(predicate)
                .SingleOrDefault();
        }

        public CategoryItem FindAsNoTracking(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryItem> GetAll()
        {
            return _context.Categories
                .Include(t => t.TaskCategories)
                .Include(u => u.User);
        }

        public IEnumerable<CategoryItem> GetAllByIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<CategoryItem> GetAllWhere(Func<CategoryItem, bool> predicate)
        {
            return _context.Categories
                .Include(t => t.TaskCategories)
                .Include(u => u.User)
                .Where(predicate);
        }

        public void Update(CategoryItem category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("CategoryItem entity not found");
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
        }
    }
}
