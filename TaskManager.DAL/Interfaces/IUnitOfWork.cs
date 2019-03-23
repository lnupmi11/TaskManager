using System;
using System.Threading.Tasks;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TaskItem> TaskItems { get; }
        IRepository<ApplicationUser> Users { get; }
        Task SaveAsync();
    }
}
