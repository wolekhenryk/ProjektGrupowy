﻿using AutoMapper;
using ProjektGrupowy.API.DTOs.Scientist;
using ProjektGrupowy.API.Models;

namespace ProjektGrupowy.API.Mapper;

public class ScientistMap : Profile
{
    public ScientistMap()
    {
        CreateMap<Scientist, ScientistResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName));
    }
}
