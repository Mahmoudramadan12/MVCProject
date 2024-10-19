using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly MvcAppContext _dbContext;

        public EmployeeRepository(MvcAppContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Employee> GetEmployeeByAddress(string address)
        => _dbContext.Employees.Where(E=> E.Address == address);
        public IQueryable<Employee> GetEmployeeByName(string name)
         => _dbContext.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower())).Include(D=>D.Department);
       
        #region With out GenericRepository
        //    private readonly MvcAppContext _dbContext;

        //    public EmployeeRepository(MvcAppContext dbContext)
        //    {
        //        _dbContext = dbContext;
        //    }
        //    public int Add(Employee employee)
        //    {
        //        _dbContext.Add(employee);
        //        return _dbContext.SaveChanges();
        //    }

        //    public int Delete(Employee id)
        //    {
        //        _dbContext.Remove(id);
        //        return _dbContext.SaveChanges();
        //    }

        //    public IEnumerable<Employee> GetAll()

        //     =>   _dbContext.Employees.ToList();


        //    public Employee GetById(int id)
        //    => _dbContext.Employees.Find(id);


        //    public int Update(Employee employee)
        //    {
        //        _dbContext.Update(employee);
        //        return _dbContext.SaveChanges();
        //    }
        //} 
        #endregion
    }
}
