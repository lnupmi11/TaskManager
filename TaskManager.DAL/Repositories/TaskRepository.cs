using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return _context.Tasks;
        }

        public TaskItem Find(string id)
        {
            return _context.Tasks.Find(id);
        }

        public void Create(TaskItem book)
        {
            _context.Tasks.Add(book);
        }

        public void Update(TaskItem book)
        {
            _context.Tasks.Update(book);
        }

        public IEnumerable<TaskItem> GetAllWhere(Func<TaskItem, Boolean> predicate)
        {
            return _context.Tasks.Where(predicate);
        }

        public TaskItem Find(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks.FirstOrDefault();
        }

        public void Delete(string id)
        {
            TaskItem book = _context.Tasks.Find(id);
            if (book != null)
                _context.Tasks.Remove(book);
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
            TaskItem book = await _context.Tasks.FindAsync(id);
            if (book != null)
                _context.Tasks.Remove(book);
        }

        public TaskItem SingleOrDefault(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks.SingleOrDefault(predicate);
        }

        public bool Any(Func<TaskItem, bool> predicate)
        {
            return _context.Tasks.Any(predicate);
        }

        public IEnumerable<TaskItem> GetAllByIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}
