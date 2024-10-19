using AutoMapper;
using Demo.DAL.Models;
using DemoPL.Models;

namespace DemoPL.MappingProfiles
{
	public class DepartmentProfile :Profile
	{
        public DepartmentProfile()
        {
			CreateMap<DepartmentViewModel, Department>().ReverseMap();

		}
	}
}
