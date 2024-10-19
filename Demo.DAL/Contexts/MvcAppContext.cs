using Demo.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Contexts
{
    public class MvcAppContext : IdentityDbContext<ApplicationUser>
    {



        public MvcAppContext(DbContextOptions options) : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=>   optionsBuilder.UseSqlServer("Server=.; Database=MvcApp; Integrated Security=True; Trusted_Connection=True; Encrypt=false ");



        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //public DbSet<IdentityUser> Users { get; set; }

        //public DbSet<IdentityRole> Roles { get; set; }


    }
}
