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
    public class ApplicatinUserRepository : Repository<ApplicationUser> , IApplicationUserRepository
    {

        // private ApplicationDbContext _db;

        // NO NEED for a private _db field here anymore. The parent has it.

        // This constructor's only job is to pass the 'db' context to the parent.

        private ApplicationDbContext _db;
        public ApplicatinUserRepository( ApplicationDbContext db) :base(db)
        //When a class inherits from another, the child's constructor is responsible for calling the parent's constructor.(base(db))
        //If the base class needs parameters in its constructor, the derived class must pass them using : base(...).
        {
            _db = db;
        }
      
    }
}
