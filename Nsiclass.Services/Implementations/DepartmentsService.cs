using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nsiclass.Data;
using Nsiclass.Data.Models;

namespace Nsiclass.Services.Implementations
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly ClassDbContext _db;

        public DepartmentsService(ClassDbContext db)
        {
            this._db = db;
        }

        public List<Department> GetAllDepartments()
        {
            return this._db.Departments.ToList();
        }
    }
}
