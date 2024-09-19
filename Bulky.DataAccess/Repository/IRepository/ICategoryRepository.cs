<<<<<<< HEAD
﻿using BulkyBook.Models;
=======
﻿using Bulky.Models;
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace BulkyBook.DataAccess.Repository.IRepository
=======
namespace Bulky.DataAccess.Repository.IRepository
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category obj);
<<<<<<< HEAD
=======
        void Save();
>>>>>>> ff020d72a70de9930dcff6a546e98ba02efb5e87
    }
}
