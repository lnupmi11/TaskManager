using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class UserRepository : IRepository<ApplicationUser>
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<ApplicationUser, bool> predicate)
        {
            return _context.AppUsers.Any(predicate);
        }

        public void Create(ApplicationUser item)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(ApplicationUser item)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetAllWhere(Func<ApplicationUser, bool> predicate)
        {
            return _context.AppUsers.Where(predicate);
        }

        public ApplicationUser Find(Func<ApplicationUser, bool> predicate)
        {
            return _context.AppUsers.Where(predicate).FirstOrDefault();
        }

        public ApplicationUser Find(string id)
        {
            return _context.AppUsers.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.AppUsers;
        }

        public void Remove(ApplicationUser item)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser SingleOrDefault(Func<ApplicationUser, bool> predicate)
        {
            return _context.AppUsers.SingleOrDefault(predicate);
        }

        public void Update(ApplicationUser item)
        {
            _context.AppUsers.Update(item);
        }

        public IEnumerable<ApplicationUser> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);
            return _context.AppUsers.Where(p => usersId.Contains(p.Id));
        }
    }
}
