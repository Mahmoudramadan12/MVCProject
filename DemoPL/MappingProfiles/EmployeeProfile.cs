using AutoMapper;
using Demo.DAL.Models;
using DemoPL.Models;

namespace DemoPL.MappingProfiles
{
    public class EmployeeProfile :Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel, Employee>().ReverseMap();
        }
    }
}
