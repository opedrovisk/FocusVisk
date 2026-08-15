namespace FocusVisk.Application.Services;

public interface IExportService
{
    Task<object> ExportAllAsync(string userId);
}