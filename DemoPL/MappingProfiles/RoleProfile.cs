using AutoMapper;
using DemoPL.Models;
using Microsoft.AspNetCore.Identity;

namespace DemoPL.MappingProfiles
{
    public class RoleProfile: Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d => d.RoleName , o => o.MapFrom(s=>s.Name))
                .ReverseMap();
        }
    }
}
