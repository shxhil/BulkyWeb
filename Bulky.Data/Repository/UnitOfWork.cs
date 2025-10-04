using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using BUlky.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositoryf
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }
        public IproductRepository Product { get; private set; }

        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
           
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
