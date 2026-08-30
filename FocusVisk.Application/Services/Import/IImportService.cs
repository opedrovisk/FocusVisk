using FocusVisk.Application.DTOs;

namespace FocusVisk.Application.Services;

public interface IImportService
{
    Task<ImportResultDto> ImportAsync(string userId, ImportBackupDto backup);
}
