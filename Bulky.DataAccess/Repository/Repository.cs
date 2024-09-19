<<<<<<< HEAD
﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
=======
﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


<<<<<<< HEAD
namespace BulkyBook.DataAccess.Repository
=======
namespace Bulky.DataAccess.Repository
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>(); //_db.Categories == dbSet
            //_db.SaveChanges();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}

