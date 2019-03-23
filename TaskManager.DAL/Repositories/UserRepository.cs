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
            return _context.UserProfiles.Any(predicate);
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
            return _context.UserProfiles.Where(predicate);
        }

        public ApplicationUser Find(Func<ApplicationUser, bool> predicate)
        {
            return _context.UserProfiles.Where(predicate).FirstOrDefault();
        }

        public ApplicationUser Find(string id)
        {
            return _context.UserProfiles.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.UserProfiles;
        }

        public void Remove(ApplicationUser item)
        {
            throw new NotImplementedException();
        }

        public ApplicationUser SingleOrDefault(Func<ApplicationUser, bool> predicate)
        {
            return _context.UserProfiles.SingleOrDefault(predicate);
        }

        public void Update(ApplicationUser item)
        {
            _context.UserProfiles.Update(item);
        }

        public IEnumerable<ApplicationUser> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);
            return _context.UserProfiles.Where(p => usersId.Contains(p.Id));
        }
    }
}
