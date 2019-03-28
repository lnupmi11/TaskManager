using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;

namespace TaskManager.BLL.Managers
{
    public class TasksManager : IDisposable
    {
        private readonly WorkContext _context;

        public TasksManager(WorkContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskItem> GetAll()
        {
            return _context.TaskItems.GetAll();
        }

        public async Task CreateAsync(TaskItem task)
        {
            _context.TaskItems.Create(task);
            await _context.SaveAsync();
        }

        public TaskItem Find(string id)
        {
            return _context.TaskItems.Find(id);
        }

        public bool IsTaskExists(string id)
        {
            return _context.TaskItems.Any(e => e.Id == id);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && _context != null)
                {
                    _context.Dispose(true);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
