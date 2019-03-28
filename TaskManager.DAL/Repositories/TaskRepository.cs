using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class TaskRepository : IRepository<TaskItem>
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes);
        }

        public TaskItem Find(string id)
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(p => p.Id == id)
                .FirstOrDefault();
        }

        public void Create(TaskItem task)
        {
            _context.Tasks.Add(task);
        }

        public void Update(TaskItem task)
        {
            _context.Tasks.Update(task);
        }

        public IEnumerable<TaskItem> GetAllWhere(Func<TaskItem, Boolean> predicate)
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(predicate);
        }

        public TaskItem Find(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(predicate)
                .FirstOrDefault();
        }

        public void Delete(string id)
        {
            TaskItem task = _context.Tasks.Find(id);
            if (task != null)
                _context.Tasks.Remove(task);
        }

        public void Remove(TaskItem item)
        {
            _context.Tasks.Remove(item);
        }

        public async Task CreateAsync(TaskItem item)
        {
            await _context.Tasks.AddAsync(item);
        }

        public async Task DeleteAsync(string id)
        {
            TaskItem task = await _context.Tasks.FindAsync(id);
            if (task != null)
                _context.Tasks.Remove(task);
        }

        public TaskItem SingleOrDefault(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .SingleOrDefault(predicate);
        }

        public bool Any(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Any(predicate);
        }

        public IEnumerable<TaskItem> GetAllByIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}
