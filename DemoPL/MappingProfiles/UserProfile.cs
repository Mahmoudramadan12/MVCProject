using AutoMapper;
using Demo.DAL.Models;
using DemoPL.Models;

namespace DemoPL.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser , UserViewModel>().ReverseMap();
        }
    }
}
