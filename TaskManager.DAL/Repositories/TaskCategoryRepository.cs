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
            throw new NotImplementedException();
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

        public void Delete(TaskCategories item)
        {
            if (item != null)
            {
                _context.TaskCategories.Remove(item);
                _context.SaveChanges();
            }
        }

        public TaskCategories Find(string id)
        {
            throw new NotImplementedException();
        }

        public TaskCategories Find(Func<TaskCategories, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public TaskCategories FindAsNoTracking(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskCategories> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskCategories> GetAllByIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskCategories> GetAllWhere(Func<TaskCategories, bool> predicate)
        {
            return _context.TaskCategories
                .Include(c=> c.Category)
                .Include(t=>t.Task)
                .AsNoTracking()
                .Where(predicate);
        }

        public void Update(TaskCategories item)
        {
            throw new NotImplementedException();
        }
    }
}
