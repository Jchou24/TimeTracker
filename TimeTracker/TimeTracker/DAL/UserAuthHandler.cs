using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TimeTracker.DAL.DBModels;
using TimeTracker.DAL.Models;

namespace TimeTracker.DAL
{
    public class UserAuthHandler : IUserAuthHandler<User, int>
    {
        private readonly ApplicationDbContext _context;

        public UserAuthHandler(ApplicationDbContext context)
        {
            this._context = context;
        }

        public CreateAccountResult Create(User entity)
        {
            var check = _context.User.SingleOrDefault(x => x.Email == entity.Email);
            if (check != null)
            {
                return CreateAccountResult.AccountExist;
            }

            _context.User.Add(entity);
            _context.SaveChanges();

            _context.MapUserRoles.Add(new MapUserRole()
            {
                UserId = entity.Id,
                UserRolesId = (int)Models.UserRoles.User,
            });
            _context.SaveChanges();

            return CreateAccountResult.Ok;
        }

        public void Delete(int id)
        {
            var currentUser = FindById(id);
            if (currentUser == null)
            {
                return;
            }
            _context.User.Remove(currentUser);
            _context.SaveChanges();
        }

        public void Update(User entity)
        {
            var currentUser = FindById(entity.Id);
            if (currentUser == null)
            {
                return;
            }
            entity.SetUpdatedDate();
            _context.Entry(currentUser).CurrentValues.SetValues(entity);
            _context.SaveChanges();
        }

        public IEnumerable<User> Find(Expression<Func<User, bool>> expression)
        {
            return _context.User.Where(expression);
        }

        public User FindById(int id)
        {
            return _context.User
                .Include(x => x.UserRoles)
                .SingleOrDefault(x => x.Id == id);
        }
    }
}
