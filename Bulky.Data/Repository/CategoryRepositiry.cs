using Bulky.DataAccess.Repository.IRepostory;
using Bulky.Models;
using Bulky.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category> , ICategoryRepository
    {

        // private ApplicationDbContext _db;

        // NO NEED for a private _db field here anymore. The parent has it.

        // This constructor's only job is to pass the 'db' context to the parent.

        private ApplicationDbContext _db;
        public CategoryRepository( ApplicationDbContext db) :base(db)
        //When a class inherits from another, the child's constructor is responsible for calling the parent's constructor.(base(db))
        //If the base class needs parameters in its constructor, the derived class must pass them using : base(...).
        {
            _db = db;
        }

        public void Save()
        {//_db is available from parent class (Repository)
         //bcz it is passed there as protected readonly ApplicationDbContext _db;
         //<protected=> major highlight of the reason>
            _db.SaveChanges();
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
