using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class UserRepository : IRepository<UserProfile>
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Any(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.Any(predicate);
        }

        public void Create(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(UserProfile item)
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

        public IEnumerable<UserProfile> GetAllWhere(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.Where(predicate);
        }

        public UserProfile Find(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.Where(predicate).FirstOrDefault();
        }

        public UserProfile Find(string id)
        {
            return _context.UserProfiles.SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _context.UserProfiles;
        }

        public void Remove(UserProfile item)
        {
            throw new NotImplementedException();
        }

        public UserProfile SingleOrDefault(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles.SingleOrDefault(predicate);
        }

        public void Update(UserProfile item)
        {
            _context.UserProfiles.Update(item);
        }

        public IEnumerable<UserProfile> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);
            return _context.UserProfiles.Where(p => usersId.Contains(p.Id));
        }
    }
}
