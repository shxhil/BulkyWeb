using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepostory
{
    public interface IRepository<T> where T: class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);// eg:Func<Product, bool> filter = p => p.Id == 5;

        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
