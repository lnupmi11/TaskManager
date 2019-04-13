using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class TaskRepository : IRepository<TaskItem>
    {
        private readonly ApplicationDbContext _context;
        private DbSet<TaskItem> _tasks;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
            _tasks = context.Tasks;
        }

        public virtual IEnumerable<TaskItem> GetAll()
        {
            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes);
        }

        public virtual IEnumerable<TaskItem> GetAllWhere(Func<TaskItem, Boolean> predicate)
        {
            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(predicate);
        }

        public virtual IEnumerable<TaskItem> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> tasksIds = new HashSet<string>(ids);

            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(p => tasksIds.Contains(p.Id));
        }

        public virtual TaskItem Find(string id)
        {
            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(p => p.Id == id)
                .SingleOrDefault();
        }

        public virtual TaskItem Find(Func<TaskItem, bool> predicate)
        {
            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Where(predicate)
                .SingleOrDefault();
        }

        public virtual void Create(TaskItem task)
        {
            _tasks.Add(task);
            _context.SaveChanges();
        }

        public virtual void Update(TaskItem task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("TaskItem entity not found");
            }
            _tasks.Update(task);
            _context.SaveChanges();
        }

        public virtual void Delete(string id)
        {
            TaskItem task = _tasks.Find(id);
            if (task != null)
            {
                _tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(TaskItem task)
        {
            if (task != null)
            {
                _tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public virtual bool Any(Func<TaskItem, bool> predicate)
        {
            return _tasks
                .Include(u => u.User)
                .Include(c => c.Changes)
                .Any(predicate);
        }
    }
}
