using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskManager.DAL.EF;
using TaskManager.DAL.Interfaces;
using TaskManager.DAL.Models;

namespace TaskManager.DAL.Repositories
{
    public class UserRepository : IRepository<UserProfile>
    {
        private readonly ApplicationDbContext _context;
        private DbSet<UserProfile> _users;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _users = context.UserProfiles;
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes);
        }

        public IEnumerable<UserProfile> GetAllWhere(Func<UserProfile, bool> predicate)
        {
            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(predicate);
        }

        public IEnumerable<UserProfile> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);

            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(p => usersId.Contains(p.Id));
        }

        public UserProfile Find(string id)
        {
            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .SingleOrDefault(p => p.Id == id);
        }

        public UserProfile Find(Func<UserProfile, bool> predicate)
        {
            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(predicate)
                .SingleOrDefault();
        }

        public void Create(UserProfile user)
        {
            throw new NotImplementedException();
        }

        public void Update(UserProfile user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("UserProfile entity not found");
            }
            _users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            UserProfile user = _users.Find(id);
            if (user != null)
            {
                _users.Remove(user);
                _context.SaveChanges();
            }
        }

        public void Delete(UserProfile user)
        {
            if (user != null)
            {
                _users.Remove(user);
                _context.SaveChanges();
            }
        }

        public bool Any(Func<UserProfile, bool> predicate)
        {
            return _users
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Any(predicate);
        }
    }
}
