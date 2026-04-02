using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Tasneem_Shop.Entities.Models;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<ApplicationUser> GetAll(string currentUserId);

        ApplicationUser GetFirstOrDefault(string id);
    }
}
