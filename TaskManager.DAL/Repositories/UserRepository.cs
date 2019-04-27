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

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<UserProfile> GetAll()
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes);
        }

        public virtual IEnumerable<UserProfile> GetAllWhere(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(predicate);
        }

        public virtual IEnumerable<UserProfile> GetAllByIds(IEnumerable<string> ids)
        {
            HashSet<string> usersId = new HashSet<string>(ids);

            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(p => usersId.Contains(p.Id));
        }

        public virtual UserProfile Find(string id)
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .SingleOrDefault(p => p.Id == id);
        }

        public UserProfile FindAsNoTracking(string id)
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .AsNoTracking()
                .SingleOrDefault(p => p.Id == id);
        }


        public virtual UserProfile Find(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Where(predicate)
                .SingleOrDefault();
        }

        public virtual void Create(UserProfile user)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(UserProfile user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("UserProfile entity not found");
            }
            _context.UserProfiles.Update(user);
            _context.SaveChanges();
        }

        public virtual void Delete(string id)
        {
            UserProfile user = _context.UserProfiles.Find(id);
            if (user != null)
            {
                _context.UserProfiles.Remove(user);
                _context.SaveChanges();
            }
        }

        public virtual void Delete(UserProfile user)
        {
            if (user != null)
            {
                _context.UserProfiles.Remove(user);
                _context.SaveChanges();
            }
        }

        public virtual bool Any(Func<UserProfile, bool> predicate)
        {
            return _context.UserProfiles
                .Include(t => t.Tasks)
                .ThenInclude(c => c.Changes)
                .Any(predicate);
        }
    }
}
