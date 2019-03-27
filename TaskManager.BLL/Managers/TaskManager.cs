using System;
using System.Collections.Generic;
using TaskManager.DAL.Models;
using TaskManager.DAL.Repositories;
namespace TaskManager.BLL.Managers
{
    public class TaskManager<TTaskItem> : IDisposable where TTaskItem : class
    {
        private readonly WorkContext _context;
        public TaskManager(WorkContext context)
        {
            _context = context;
        }

        public IEnumerable<TaskItem> GetAllTasks()
        {
            var tasks = _context.TaskItems.GetAll();
            return tasks;
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
