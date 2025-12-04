using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HistoryExport_EBI.Application.Dto;
using HistoryExport_EBI.Domain.Entities;

namespace HistoryExport_EBI.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Point, PointDto>()
            .ForMember(dest => dest.HistoryFast, opt => opt.MapFrom(src => src.HistoryFast ?? false))
            .ForMember(dest => dest.HistorySlow, opt => opt.MapFrom(src => src.HistorySlow ?? false))
            .ForMember(dest => dest.HistoryExtd, opt => opt.MapFrom(src => src.HistoryExtd ?? false))
            .ForMember(dest => dest.HistoryFastArch, opt => opt.MapFrom(src => src.HistoryFastArch ?? false))
            .ForMember(dest => dest.HistorySlowArch, opt => opt.MapFrom(src => src.HistorySlowArch ?? false))
            .ForMember(dest => dest.HistoryExtdArch, opt => opt.MapFrom(src => src.HistoryExtdArch ?? false));
    }
}
