<<<<<<< HEAD
﻿using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
=======
﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.DataAcess.Data;
using Bulky.Models;
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
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
    public class CategoryRepository : Repository<Category>,ICategoryRepository
    {
        private ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

<<<<<<< HEAD
=======
        public void Save()
        {
            _db.SaveChanges();
        }

>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
