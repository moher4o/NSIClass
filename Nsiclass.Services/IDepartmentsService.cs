using Nsiclass.Data.Models;
using System.Collections.Generic;

namespace Nsiclass.Services
{
    public interface IDepartmentsService
    {
        List<Department> GetAllDepartments();
    }
}
