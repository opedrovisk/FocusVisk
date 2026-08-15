using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class SettingsProfile : Profile
{
    public SettingsProfile()
    {
        CreateMap<AppSettings, AppSettingsDto>();
        CreateMap<AppSettingsUpdateDto, AppSettings>();
    }
}