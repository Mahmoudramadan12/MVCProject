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
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly MvcAppContext _dbContext;

        public GenericRepository(MvcAppContext dbContext) 
        {
           _dbContext = dbContext;
        }
        public async Task AddAsync(T item)
        {
             await _dbContext.AddAsync(item);
        }

        public void Delete(T item)
        {
            _dbContext.Remove(item);
        }

        public async Task< IEnumerable<T>> GetAllAsync()
        {
            if (typeof (T) == typeof (Employee))
            {
                return (IEnumerable<T>) await _dbContext.Employees.Include(e => e.Department).ToListAsync();
            }
            return await _dbContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)

        {
            if (typeof(T) == typeof(Employee))
            {
               return _dbContext.Employees
             .Include(e => e.Department)
             .FirstOrDefault(e => e.Id == id) as T;
            }
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public void Update(T item)
        {
           _dbContext.Update(item);
        }
    }
}
