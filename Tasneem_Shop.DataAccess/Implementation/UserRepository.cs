using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Models;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext Context) 
        {
            _context = Context;
        }
        public IEnumerable<ApplicationUser> GetAll(string currentUserId)
        {
            return _context.ApplicationUsers.Where(x => x.Id != currentUserId).ToList();
        }

        public ApplicationUser GetFirstOrDefault(string id)
        {
            return _context.ApplicationUsers.FirstOrDefault(x => x.Id == id);
        }
    }
}

