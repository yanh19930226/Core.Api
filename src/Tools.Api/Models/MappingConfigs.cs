using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools.Api.Dtos;
using Tools.Api.Models.Mongos;

namespace Tools.Api.Models
{
    public class MappingConfigs : Profile
    {
        public MappingConfigs()
        {
            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<UpdateBookDto, Book>()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
