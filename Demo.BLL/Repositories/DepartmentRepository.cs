using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(MvcAppContext dbContext):base(dbContext)
        {
            
        }

        #region With out GenericRepository
        //private readonly MvcAppContext _dbContext;

        //public DepartmentRepository(MvcAppContext dbContext) // Ask CLR  For Object From Dbcontext
        //{
        //    _dbContext = dbContext;

        //    //dbContext = new MvcAppContext();


        //}
        //public int Add(Department department)
        //{
        //   _dbContext.Add(department);
        //    return _dbContext.SaveChanges();
        //}

        //public int Delete(Department department)
        //{
        //    _dbContext.Remove(department);
        //    return _dbContext.SaveChanges();
        //}

        //public IEnumerable<Department> GetAll()
        //{
        //    return _dbContext.Departments.ToList();
        //}

        //public Department GetById(int id)
        //{
        //    //var department = _dbContext.Departments.Local.Where(D=>D.Id == id).FirstOrDefault();
        //    //if (department is null )
        //    //     department = _dbContext.Departments.Where(D => D.Id == id).FirstOrDefault();

        //    //return department;

        //    return _dbContext.Departments.Find(id);




        //}

        //public int Update(Department department)
        //{

        //    _dbContext.Update(department);
        //    return _dbContext.SaveChanges();
        //} 
        #endregion
    }
}
