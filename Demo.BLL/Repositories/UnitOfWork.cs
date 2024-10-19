using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class UnitOfWork : IUnitOfWork , IDisposable
    {
        private readonly MvcAppContext _dbcontext;

        public IEmployeeRepository EmployeeRepository { get ; set ; }
        public IDepartmentRepository DepartmentRepository { get; set; }


        public UnitOfWork(MvcAppContext dbcontext)
        {
            EmployeeRepository = new EmployeeRepository(dbcontext);
            DepartmentRepository = new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }

        public async Task< int> CompleteAsync()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}
