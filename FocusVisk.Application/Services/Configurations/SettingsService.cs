using AutoMapper;
using FocusVisk.Application.DTOs;
using FocusVisk.Core.Interfaces;
using FocusVisk.Core.Models;

namespace FocusVisk.Application.Services;

public class SettingsService : ISettingsService
{
    private readonly ISettingsRepository _repository;
    private readonly IMapper _mapper;
    public SettingsService(ISettingsRepository repository, IMapper mapper) { _repository = repository; _mapper = mapper; }

    public async Task<AppSettingsDto> GetAsync(string userId)
    {
        var settings = await _repository.GetAsync(userId);

        if (settings is null) // primeiro acesso: cria com valores default
        {
            settings = new AppSettings { UserId = userId };
            await _repository.AddAsync(settings);
            await _repository.SaveChangesAsync();
        }

        return _mapper.Map<AppSettingsDto>(settings);
    }

    public async Task<AppSettingsDto> UpdateAsync(string userId, AppSettingsUpdateDto dto)
    {
        var settings = await _repository.GetAsync(userId);
        if (settings is null)
        {
            settings = new AppSettings { UserId = userId };
            await _repository.AddAsync(settings);
        }

        _mapper.Map(dto, settings);
        _repository.Update(settings);
        await _repository.SaveChangesAsync();

        return _mapper.Map<AppSettingsDto>(settings);
    }
}