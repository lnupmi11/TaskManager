using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManager.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAllWhere(Func<T, Boolean> predicate);
        IEnumerable<T> GetAllByIds(IEnumerable<string> ids);
        T Find(string id);
        T Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
        void Delete(T item);
        bool Any(Func<T, Boolean> predicate);
    }
}
