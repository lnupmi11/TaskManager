using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class WorkContext : IUnitOfWork
    {
        private ApplicationDbContext _context;
        private TaskRepository _taskRepository;
        private UserRepository _userRepositiry;


        public WorkContext(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<TaskItem> TaskItems 
        {
            get
            {
                if (_taskRepository == null)
                    _taskRepository = new TaskRepository(_context);
                return _taskRepository;
            }
        }

        public IRepository<UserProfile> Users
        {
            get
            {
                if (_userRepositiry == null)
                    _userRepositiry = new UserRepository(_context);
                return _userRepositiry;
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        #region IDisposable Support

        private bool disposedValue = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
