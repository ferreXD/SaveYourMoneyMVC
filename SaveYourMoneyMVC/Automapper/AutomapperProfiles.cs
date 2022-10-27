using AutoMapper;
using SaveYourMoneyMVC.DTOs.Analisis.Filtros;
using SaveYourMoneyMVC.DTOs.Gastos;
using SaveYourMoneyMVC.Entities;
using SaveYourMoneyMVC.Models.Analisis;
using SaveYourMoneyMVC.Models.Files;
using SaveYourMoneyMVC.Models.Gastos;

namespace SaveYourMoneyMVC.Automapper
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Gasto, GastoViewModel>()
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.File.Name))
                .ForMember(dest => dest.FilePath, opt => opt.MapFrom(src => src.File.Path))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.File.Type))
                .ForMember(dest => dest.FileContent, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.File, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<FileEntity, FileViewModel>().ReverseMap();

            CreateMap<Gasto, GastoCsvDTO>().ForMember(gv => gv.Created, opt => opt.MapFrom(g => g.Created.ToString("dd/MM/yyyy"))).ForMember(gv => gv.FileName, opt => opt.MapFrom(g => g.File != null ? g.File.Name : ""));

            CreateMap<AnalisisViewModel, AnalisisFiltroDTO>()
                .ForMember(filtro => filtro.TipoGasto, opt => opt.MapFrom(vm => vm.TipoGasto.HasValue ? vm.TipoGasto : null))
                .ForMember(filtro => filtro.IntervaloInicio, opt => opt.MapFrom(vm => vm.IntervaloInicio.HasValue ? vm.IntervaloInicio : null))
                .ForMember(filtro => filtro.IntervaloFin, opt => opt.MapFrom(vm => vm.IntervaloFin.HasValue ? vm.IntervaloFin : null));
        }
    }
}
