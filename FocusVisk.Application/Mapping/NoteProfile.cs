using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Mapping;

public class NoteProfile : Profile
{
    public NoteProfile()
    {
        CreateMap<QuickNote, QuickNoteDto>();
        CreateMap<QuickNoteCreateDto, QuickNote>();
        CreateMap<QuickNoteUpdateDto, QuickNote>();
    }
}