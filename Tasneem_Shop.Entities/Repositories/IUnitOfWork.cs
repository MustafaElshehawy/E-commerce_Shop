using System;
using System.Collections.Generic;
using System.Text;

namespace Tasneem_Shop.Entities.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository Category { get; }

        int Complate();
    }
}
