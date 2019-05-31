using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class TaskCategoryRepository : IRepository<TaskCategories>
    {
        private readonly ApplicationDbContext _context;

        public TaskCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<TaskCategories, bool> predicate)
        {
            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task)
                .Any(predicate);
        }

        public void Create(TaskCategories taskCategory)
        {
            _context.Add(taskCategory);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            TaskCategories taskCategory = _context.TaskCategories.Find(id);
            if (taskCategory != null)
            {
                _context.TaskCategories.Remove(taskCategory);
                _context.SaveChanges();
            }
        }

        public void Delete(TaskCategories taskCategory)
        {
            if (taskCategory != null)
            {
                _context.TaskCategories.Remove(taskCategory);
                _context.SaveChanges();
            }
        }

        public TaskCategories Find(string id)
        {
            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task)
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public TaskCategories Find(Func<TaskCategories, bool> predicate)
        {
            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task)
                .Where(predicate)
                .SingleOrDefault();
        }

        public TaskCategories FindAsNoTracking(string id)
        {
            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task)
                .Where(p => p.Id == id)
                .AsNoTracking()
                .SingleOrDefault();
        }

        public IEnumerable<TaskCategories> GetAll()
        {
            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task);
        }

        public IEnumerable<TaskCategories> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> taskCategoriesIds = new HashSet<string>(ids);

            return _context.TaskCategories
                .Include(c => c.Category)
                .Include(t => t.Task)
                .Where(p => taskCategoriesIds.Contains(p.Id));
        }

        public IEnumerable<TaskCategories> GetAllWhere(Func<TaskCategories, bool> predicate)
        {
            return _context.TaskCategories
                .Include(c=> c.Category)
                .Include(t=>t.Task)
                .AsNoTracking()
                .Where(predicate);
        }

        public void Update(TaskCategories taskCategory)
        {
            if (taskCategory == null)
            {
                throw new ArgumentNullException("TaskCategory entity not found");
            }
            _context.TaskCategories.Update(taskCategory);
            _context.SaveChanges();
        }
    }
}
