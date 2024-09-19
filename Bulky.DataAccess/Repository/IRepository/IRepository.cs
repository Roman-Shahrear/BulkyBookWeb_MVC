using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace BulkyBook.DataAccess.Repository.IRepository
=======
namespace Bulky.DataAccess.Repository.IRepository
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
{
    public interface IRepository<T> where T : class
    {
        T Get(Expression<Func<T, bool>> filter);  // single entity based on a condition
        IEnumerable<T> GetAll();  //get all entities
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
