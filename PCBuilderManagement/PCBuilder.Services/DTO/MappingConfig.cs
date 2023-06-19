using AutoMapper;
using PCBuilder.Repository.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PCBuilder.Services.DTO
{
    public class MappingConfig: Profile
    {
        public MappingConfig()
        {
            CreateMap<Pc, PcDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Component, ComponentDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Pc, PCInformationDTO>()
                .ForMember(dto => dto.Components, opt => opt.MapFrom(x => x.PcComponents.Select(y => y.Component)));

        }
    }
}
