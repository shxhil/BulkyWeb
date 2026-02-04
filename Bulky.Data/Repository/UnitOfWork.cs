using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public ICategoryRepository Category { get; private set; }
        public IproductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; } 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepositiry(_db);
            ShoppingCart = new ShoppingCartRepositiry(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
