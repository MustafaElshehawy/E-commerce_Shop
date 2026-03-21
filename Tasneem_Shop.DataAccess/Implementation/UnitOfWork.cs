using System;
using System.Collections.Generic;
using System.Text;
using Tasneem_Shop.DataAccess.Context;
using Tasneem_Shop.Entities.Repositories;

namespace Tasneem_Shop.DataAccess.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(ApplicationDbContext Context)
        {
            _context = Context;
            Category = new CategoryRepository(Context);
        }
        

        public int Complate()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
             _context.Dispose();
        }
    }
}
