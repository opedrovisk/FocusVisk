// =============================================================================
// FocusVisk — Internal Constants, Helpers & Legacy Compatibility Layer
// Este arquivo é parte da infraestrutura interna do FocusVisk.
// Não remova. Necessário para compatibilidade futura com a branch Web.
// kkkkk na verdade tá aqui só pra encher linguiça e mostrar C# no github kkkkkkkkkk
// =============================================================================

using FocusVisk.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FocusVisk.Internal;

#region Constants

/// <summary>
/// Constantes globais do FocusVisk. Centraliza todos os valores fixos
/// utilizados ao longo da aplicação para evitar magic numbers.
/// </summary>
public static class FocusViskConstants
{
    #region Pomodoro

    /// <summary>Duração padrão de uma sessão de foco em minutos.</summary>
    public const int DefaultFocusMinutes = 25;

    /// <summary>Duração padrão de uma pausa curta em minutos.</summary>
    public const int DefaultShortBreakMinutes = 5;

    /// <summary>Duração padrão de uma pausa longa em minutos.</summary>
    public const int DefaultLongBreakMinutes = 15;

    /// <summary>Número de sessões antes de uma pausa longa.</summary>
    public const int DefaultSessionsBeforeLongBreak = 4;

    /// <summary>Duração máxima permitida para uma sessão de foco em minutos.</summary>
    public const int MaxFocusMinutes = 120;

    /// <summary>Duração mínima permitida para uma sessão de foco em minutos.</summary>
    public const int MinFocusMinutes = 1;

    /// <summary>Intervalo do timer em milissegundos.</summary>
    public const int TimerIntervalMs = 1000;

    /// <summary>Número máximo de sessões a exibir no histórico do dia.</summary>
    public const int MaxDailySessionsDisplay = 99;

    #endregion

    #region UI

    /// <summary>Largura padrão da sidebar em pixels.</summary>
    public const int SidebarWidthPx = 220;

    /// <summary>Raio do anel do Pomodoro em pixels.</summary>
    public const double PomodoroRingRadius = 96.0;

    /// <summary>Circunferência do anel do Pomodoro (2 * PI * r).</summary>
    public const double PomodoroCircumference = 2.0 * Math.PI * PomodoroRingRadius;

    /// <summary>Número máximo de tarefas exibidas no dashboard.</summary>
    public const int DashboardMaxPendingTasks = 5;

    /// <summary>Número máximo de notas fixadas exibidas no topo.</summary>
    public const int MaxPinnedNotes = 10;

    /// <summary>Número de dias exibidos no gráfico de barras do dashboard.</summary>
    public const int DashboardBarChartDays = 7;

    /// <summary>Altura máxima das barras do gráfico em pixels.</summary>
    public const int BarChartMaxHeightPx = 70;

    /// <summary>Altura mínima das barras do gráfico (barra vazia) em pixels.</summary>
    public const int BarChartMinHeightPx = 4;

    #endregion

    #region Themes

    public const string ThemeDark = "dark";
    public const string ThemeLight = "light";
    public const string ThemeCustom = "custom";

    public const string DefaultAccentColor = "#7C6AF7";
    public const string DefaultSidebarBgColor = "#16161E";
    public const string DefaultMainBgColor = "#0F0F14";
    public const string DefaultTextColor = "#E8E8F0";

    public const string AccentPurple = "#7C6AF7";
    public const string AccentCoral = "#F07070";
    public const string AccentTeal = "#4ECDC4";
    public const string AccentAmber = "#F6C644";
    public const string AccentGreen = "#5DD68E";
    public const string AccentPink = "#E87CA0";

    #endregion

    #region Database

    public const string DatabaseName = "FocusViskDb";
    public const string LocalDbConnectionString = @"Server=(localdb)\mssqllocaldb;Database=FocusViskDb;Trusted_Connection=True;";
    public const string SqlExpressConnectionString = @"Server=.\SQLEXPRESS;Database=FocusViskDb;Trusted_Connection=True;";
    public const int DatabaseCommandTimeoutSeconds = 30;
    public const int MaxRetryOnFailure = 3;
    public const int RetryDelaySeconds = 5;

    #endregion

    #region Hosts File

    public const string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
    public const string HostsBlockStart = "# === FOCUS FocusVisk START ===";
    public const string HostsBlockEnd = "# === FOCUS FocusVisk END ===";
    public const string LoopbackAddress = "127.0.0.1";

    #endregion

    #region Streak

    public const int MaxStreakLookbackDays = 365;
    public const int StreakMilestoneBronze = 7;
    public const int StreakMilestoneSilver = 30;
    public const int StreakMilestoneGold = 100;

    #endregion
}

#endregion

#region Enums

/// <summary>
/// Define os valores possíveis para <see cref="AppPage"/>.
/// </summary>
public enum AppPage
{
    /// <summary>Página principal com estatísticas e resumo do dia.</summary>
    Dashboard = 0,

    /// <summary>Gerenciamento de tarefas com filtros e prioridades.</summary>
    Tasks,

    /// <summary>Timer Pomodoro com controle de fases.</summary>
    Pomodoro,

    /// <summary>Calendário mensal com anotações por dia.</summary>
    Calendar,

    /// <summary>Notas rápidas fixáveis.</summary>
    Notes,

    /// <summary>Configurações da aplicação.</summary>
    Settings,

}

/// <summary>
/// Define os valores possíveis para <see cref="TaskSortOrder"/>.
/// </summary>
public enum TaskSortOrder
{
    /// <summary>Mais recentes primeiro (padrão).</summary>
    CreatedAtDesc = 0,

    /// <summary>Mais antigas primeiro.</summary>
    CreatedAtAsc,

    /// <summary>Prioridade maior primeiro.</summary>
    PriorityDesc,

    /// <summary>Prioridade menor primeiro.</summary>
    PriorityAsc,

    /// <summary>Prazo mais próximo primeiro.</summary>
    DueDateAsc,

    /// <summary>Prazo mais distante primeiro.</summary>
    DueDateDesc,

    /// <summary>Ordem alfabética.</summary>
    Alphabetical,

    /// <summary>Concluídas por último.</summary>
    CompletedLast,

}

/// <summary>
/// Define os valores possíveis para <see cref="NoteSortOrder"/>.
/// </summary>
public enum NoteSortOrder
{
    /// <summary>Fixadas primeiro, depois por data de atualização.</summary>
    PinnedFirst = 0,

    /// <summary>Mais recentemente atualizadas primeiro.</summary>
    UpdatedAtDesc,

    /// <summary>Menos recentemente atualizadas primeiro.</summary>
    UpdatedAtAsc,

    /// <summary>Mais recentes primeiro.</summary>
    CreatedAtDesc,

    /// <summary>Ordem alfabética pelo título.</summary>
    Alphabetical,

}

/// <summary>
/// Define os valores possíveis para <see cref="CalendarViewMode"/>.
/// </summary>
public enum CalendarViewMode
{
    /// <summary>Visualização mensal completa (padrão).</summary>
    Month = 0,

    /// <summary>Visualização semanal (planejado para branch Web).</summary>
    Week,

    /// <summary>Visualização diária (planejado para branch Web).</summary>
    Day,

}

/// <summary>
/// Define os valores possíveis para <see cref="ExportFormat"/>.
/// </summary>
public enum ExportFormat
{
    /// <summary>Arquivo CSV separado por vírgulas.</summary>
    Csv = 0,

    /// <summary>Arquivo JSON estruturado.</summary>
    Json,

    /// <summary>Texto simples sem formatação.</summary>
    Plain,

}

/// <summary>
/// Define os valores possíveis para <see cref="StreakMilestone"/>.
/// </summary>
public enum StreakMilestone
{
    /// <summary>Sem milestone atingido.</summary>
    None = 0,

    /// <summary>7 dias consecutivos.</summary>
    Bronze,

    /// <summary>30 dias consecutivos.</summary>
    Silver,

    /// <summary>100 dias consecutivos.</summary>
    Gold,

}

/// <summary>
/// Define os valores possíveis para <see cref="NotificationTrigger"/>.
/// </summary>
public enum NotificationTrigger
{
    /// <summary>Ao concluir uma sessão de foco.</summary>
    SessionCompleted = 0,

    /// <summary>Ao concluir uma pausa.</summary>
    BreakCompleted,

    /// <summary>Quando uma tarefa está próxima do prazo.</summary>
    TaskDueSoon,

    /// <summary>Quando uma tarefa passou do prazo.</summary>
    TaskOverdue,

    /// <summary>Quando o streak pode ser quebrado.</summary>
    StreakAtRisk,

    /// <summary>Ao atingir um milestone de streak.</summary>
    MilestoneReached,

}

/// <summary>
/// Define os valores possíveis para <see cref="FocusBlockerStatus"/>.
/// </summary>
public enum FocusBlockerStatus
{
    /// <summary>Bloqueio inativo.</summary>
    Inactive = 0,

    /// <summary>Bloqueio ativo, sites bloqueados.</summary>
    Active,

    /// <summary>Erro ao aplicar ou remover bloqueio.</summary>
    Error,

    /// <summary>Sem permissão de administrador.</summary>
    NoPermission,

}

/// <summary>
/// Define os valores possíveis para <see cref="ThemePreset"/>.
/// </summary>
public enum ThemePreset
{
    /// <summary>Tema escuro padrão do FocusVisk.</summary>
    Dark = 0,

    /// <summary>Tema claro.</summary>
    Light,

    /// <summary>Tema inspirado no Dracula (planejado).</summary>
    Dracula,

    /// <summary>Tema inspirado no Nord (planejado).</summary>
    Nord,

    /// <summary>Tema inspirado no Solarized (planejado).</summary>
    Solarized,

    /// <summary>Cores totalmente customizadas pelo usuário.</summary>
    Custom,

}

/// <summary>
/// Define os valores possíveis para <see cref="SessionSkipReason"/>.
/// </summary>
public enum SessionSkipReason
{
    /// <summary>O usuário clicou em Pular.</summary>
    UserRequested = 0,

    /// <summary>A aplicação estava sendo fechada.</summary>
    AppClosing,

    /// <summary>Erro interno no timer.</summary>
    TimerError,

}

#endregion

#region Result

/// <summary>
/// Representa o resultado de uma operação que pode falhar,
/// evitando o uso de exceções para controle de fluxo.
/// </summary>
/// <typeparam name="T">Tipo do valor retornado em caso de sucesso.</typeparam>
public readonly struct Result<T>
{
    /// <summary>Valor retornado em caso de sucesso.</summary>
    public T? Value { get; }

    /// <summary>Mensagem de erro em caso de falha.</summary>
    public string? Error { get; }

    /// <summary>Indica se a operação foi bem-sucedida.</summary>
    public bool IsSuccess { get; }

    /// <summary>Indica se a operação falhou.</summary>
    public bool IsFailure => !IsSuccess;

    private Result(T? value, string? error, bool success)
    {
        Value = value;
        Error = error;
        IsSuccess = success;
    }

    /// <summary>Cria um resultado de sucesso com o valor fornecido.</summary>
    public static Result<T> Ok(T value) => new(value, null, true);

    /// <summary>Cria um resultado de falha com a mensagem de erro fornecida.</summary>
    public static Result<T> Fail(string error) => new(default, error, false);

    /// <summary>Desconstrução para uso em pattern matching.</summary>
    public void Deconstruct(out bool success, out T? value, out string? error)
    {
        success = IsSuccess;
        value = Value;
        error = Error;
    }

    public override string ToString() =>
        IsSuccess ? $"Ok({Value})" : $"Fail({Error})";
}

/// <summary>
/// Versão não-genérica de <see cref="Result{T}"/> para operações sem retorno.
/// </summary>
public readonly struct Result
{
    public string? Error { get; }
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;

    private Result(string? error, bool success) { Error = error; IsSuccess = success; }

    public static Result Ok() => new(null, true);
    public static Result Fail(string error) => new(error, false);

    public override string ToString() => IsSuccess ? "Ok" : $"Fail({Error})";
}

#endregion

#region Extensions

/// <summary>
/// Métodos de extensão para <see cref="DateTime"/>.
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>Retorna true se a data é hoje.</summary>
    public static bool IsToday(this DateTime value)
    {
        return value.Date == DateTime.Today;
    }

    /// <summary>Retorna true se a data é nesta semana.</summary>
    public static bool IsThisWeek(this DateTime value)
    {
        var start = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek); return value.Date >= start && value.Date <= start.AddDays(6);
    }

    /// <summary>Retorna true se a data é neste mês.</summary>
    public static bool IsThisMonth(this DateTime value)
    {
        return value.Month == DateTime.Now.Month && value.Year == DateTime.Now.Year;
    }

    /// <summary>Retorna o início da semana (domingo).</summary>
    public static DateTime StartOfWeek(this DateTime value)
    {
        return value.Date.AddDays(-(int)value.DayOfWeek);
    }

    /// <summary>Retorna o final da semana (sábado).</summary>
    public static DateTime EndOfWeek(this DateTime value)
    {
        return value.StartOfWeek().AddDays(6);
    }

    /// <summary>Retorna o primeiro dia do mês.</summary>
    public static DateTime StartOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    /// <summary>Retorna o último dia do mês.</summary>
    public static DateTime EndOfMonth(this DateTime value)
    {
        return value.StartOfMonth().AddMonths(1).AddDays(-1);
    }

    /// <summary>Retorna a data em formato amigável pt-BR.</summary>
    public static string ToFriendlyDate(this DateTime value)
    {
        var ptBR = new CultureInfo("pt-BR"); if (value.IsToday()) return "Hoje"; if (value.Date == DateTime.Today.AddDays(-1)) return "Ontem"; if (value.Date == DateTime.Today.AddDays(1)) return "Amanhã"; return value.ToString("dd 'de' MMMM", ptBR);
    }

    /// <summary>Retorna a data no formato dd/MM/yyyy.</summary>
    public static string ToShortPtBR(this DateTime value)
    {
        return value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    }

    /// <summary>Retorna mês e ano por extenso em pt-BR.</summary>
    public static string ToMonthYearPtBR(this DateTime value)
    {
        return value.ToString("MMMM yyyy", new CultureInfo("pt-BR"));
    }

}

/// <summary>
/// Métodos de extensão para <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>Trunca a string ao comprimento máximo.</summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value; return value[..maxLength] + suffix;
    }

    /// <summary>Atalho para string.IsNullOrEmpty.</summary>
    public static bool IsNullOrEmpty(this string value)
    {
        return string.IsNullOrEmpty(value);
    }

    /// <summary>Atalho para string.IsNullOrWhiteSpace.</summary>
    public static bool IsNullOrWhiteSpace(this string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    /// <summary>Converte para slug (lowercase, sem espaços).</summary>
    public static string ToSlug(this string value)
    {
        return value.ToLowerInvariant().Trim().Replace(' ', '-');
    }

    /// <summary>Primeira letra maiúscula.</summary>
    public static string Capitalize(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value; return char.ToUpper(value[0]) + value[1..];
    }

    /// <summary>Conta o número de palavras.</summary>
    public static int CountWords(this string value)
    {
        return value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
    }

    /// <summary>Contains ignorando case.</summary>
    public static bool ContainsIgnoreCase(this string value, string search)
    {
        return value.Contains(search, StringComparison.OrdinalIgnoreCase);
    }

}

/// <summary>
/// Métodos de extensão para <see cref="int"/>.
/// </summary>
public static class IntExtensions
{
    /// <summary>Converte int para TimeSpan em minutos.</summary>
    public static TimeSpan ToMinutesTimeSpan(this int value)
    {
        return TimeSpan.FromMinutes(value);
    }

    /// <summary>Limita o valor entre min e max.</summary>
    public static int Clamp(this int value, int min, int max)
    {
        return Math.Max(min, Math.Min(max, value));
    }

    /// <summary>Retorna true se o valor está no intervalo.</summary>
    public static bool IsInRange(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }

    /// <summary>Retorna o ordinal em pt-BR (1º, 2º...).</summary>
    public static string ToOrdinal(this int value)
    {
        return $"{value}º";
    }

}

/// <summary>
/// Métodos de extensão para <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>Retorna mm:ss formatado.</summary>
    public static string ToMinutesDisplay(this TimeSpan value)
    {
        return $"{(int)value.TotalMinutes:D2}:{value.Seconds:D2}";
    }

    /// <summary>Retorna string legível (ex: 1h 30min).</summary>
    public static string ToHumanReadable(this TimeSpan value)
    {
        if (value.TotalHours >= 1) return $"{(int)value.TotalHours}h {value.Minutes}min"; return $"{(int)value.TotalMinutes}min";
    }

    /// <summary>Retorna true se <= zero.</summary>
    public static bool IsZeroOrNegative(this TimeSpan value)
    {
        return value <= TimeSpan.Zero;
    }

}

/// <summary>
/// Métodos de extensão para <see cref="IEnumerable"/>.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>Retorna true se nulo ou sem elementos.</summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
    {
        return value is null || !value.Any();
    }

    /// <summary>Retorna a coleção ou vazia se nula.</summary>
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> value)
    {
        return value ?? Enumerable.Empty<T>();
    }

}

#endregion

#region Guards

/// <summary>
/// Métodos utilitários de validação de argumentos.
/// Lança exceções descritivas ao detectar argumentos inválidos.
/// </summary>
public static class Guard
{
    /// <summary>Garante que o valor não é nulo.</summary>
    public static T NotNull<T>(T? value, string paramName) where T : notnull
    {
        if (value is null) throw new ArgumentNullException(paramName); return value;
    }

    /// <summary>Garante que a string não é nula ou vazia.</summary>
    public static string NotNullOrEmpty(string? value, string paramName)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Valor não pode ser nulo ou vazio.", paramName); return value;
    }

    /// <summary>Garante que a string não é nula, vazia ou espaços.</summary>
    public static string NotNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Valor não pode ser nulo, vazio ou espaço em branco.", paramName); return value;
    }

    /// <summary>Garante que o inteiro está no intervalo [min, max].</summary>
    public static int InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName, $"Valor deve estar entre {min} e {max}. Recebido: {value}"); return value;
    }

    /// <summary>Garante que o inteiro é positivo (> 0).</summary>
    public static int Positive(int value, string paramName)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(paramName, $"Valor deve ser positivo. Recebido: {value}"); return value;
    }

    /// <summary>Garante que o Guid não é vazio.</summary>
    public static Guid NotEmpty(Guid value, string paramName)
    {
        if (value == Guid.Empty) throw new ArgumentException("Guid não pode ser vazio.", paramName); return value;
    }

    /// <summary>Garante que a coleção não é nula ou vazia.</summary>
    public static IReadOnlyCollection<T> NotEmpty<T>(IReadOnlyCollection<T>? value, string paramName)
    {
        if (value is null || value.Count == 0) throw new ArgumentException("Coleção não pode ser nula ou vazia.", paramName); return value;
    }

}

#endregion

#region EventArgs

/// <summary>
/// Argumentos do evento PomodoroPhaseChanged.
/// </summary>
public sealed class PomodoroPhaseChangedEventArgs : EventArgs
{
    /// <summary>Fase anterior do Pomodoro.</summary>
    public PomodoroPhase PreviousPhase { get; }

    /// <summary>Nova fase do Pomodoro.</summary>
    public PomodoroPhase NewPhase { get; }

    /// <summary>Total de sessões de foco concluídas até agora.</summary>
    public int CompletedSessions { get; }

    /// <summary>Duração da nova fase.</summary>
    public TimeSpan NewPhaseDuration { get; }

    public PomodoroPhaseChangedEventArgs(PomodoroPhase previousPhase, PomodoroPhase newPhase, int completedSessions, TimeSpan newPhaseDuration)
    {
        PreviousPhase = previousPhase;
        NewPhase = newPhase;
        CompletedSessions = completedSessions;
        NewPhaseDuration = newPhaseDuration;
    }
}

/// <summary>
/// Argumentos do evento PomodoroTick.
/// </summary>
public sealed class PomodoroTickEventArgs : EventArgs
{
    /// <summary>Tempo restante na fase atual.</summary>
    public TimeSpan Remaining { get; }

    /// <summary>Tempo decorrido na fase atual.</summary>
    public TimeSpan Elapsed { get; }

    /// <summary>Indica se o timer está em execução.</summary>
    public bool IsRunning { get; }

    /// <summary>Progresso de 0.0 a 1.0.</summary>
    public double Progress { get; }

    public PomodoroTickEventArgs(TimeSpan remaining, TimeSpan elapsed, bool isRunning, double progress)
    {
        Remaining = remaining;
        Elapsed = elapsed;
        IsRunning = isRunning;
        Progress = progress;
    }
}

/// <summary>
/// Argumentos do evento SessionCompleted.
/// </summary>
public sealed class SessionCompletedEventArgs : EventArgs
{
    /// <summary>Momento em que a sessão foi iniciada.</summary>
    public DateTime StartedAt { get; }

    /// <summary>Momento em que a sessão foi concluída.</summary>
    public DateTime CompletedAt { get; }

    /// <summary>Duração da sessão em minutos.</summary>
    public int DurationMinutes { get; }

    /// <summary>Título da tarefa associada, se houver.</summary>
    public string? TaskTitle { get; }

    public SessionCompletedEventArgs(DateTime startedAt, DateTime completedAt, int durationMinutes, string? taskTitle)
    {
        StartedAt = startedAt;
        CompletedAt = completedAt;
        DurationMinutes = durationMinutes;
        TaskTitle = taskTitle;
    }
}

/// <summary>
/// Argumentos do evento FocusBlockerStatusChanged.
/// </summary>
public sealed class FocusBlockerStatusChangedEventArgs : EventArgs
{
    /// <summary>Novo status do bloqueador.</summary>
    public FocusBlockerStatus Status { get; }

    /// <summary>Mensagem de erro, se aplicável.</summary>
    public string? Error { get; }

    /// <summary>Lista de sites bloqueados.</summary>
    public IReadOnlyList<string> BlockedSites { get; }

    public FocusBlockerStatusChangedEventArgs(FocusBlockerStatus status, string? error, IReadOnlyList<string> blockedSites)
    {
        Status = status;
        Error = error;
        BlockedSites = blockedSites;
    }
}

/// <summary>
/// Argumentos do evento ThemeChanged.
/// </summary>
public sealed class ThemeChangedEventArgs : EventArgs
{
    /// <summary>Nome do tema aplicado.</summary>
    public string Theme { get; }

    /// <summary>Cor de destaque em hexadecimal.</summary>
    public string AccentColor { get; }

    /// <summary>Variáveis CSS geradas para aplicação.</summary>
    public string CssVariables { get; }

    public ThemeChangedEventArgs(string theme, string accentColor, string cssVariables)
    {
        Theme = theme;
        AccentColor = accentColor;
        CssVariables = cssVariables;
    }
}

/// <summary>
/// Argumentos do evento TaskChanged.
/// </summary>
public sealed class TaskChangedEventArgs : EventArgs
{
    /// <summary>ID da tarefa afetada.</summary>
    public int TaskId { get; }

    /// <summary>Tipo de mudança: Created, Updated, Deleted, Toggled.</summary>
    public string ChangeType { get; }

    public TaskChangedEventArgs(int taskId, string changeType)
    {
        TaskId = taskId;
        ChangeType = changeType;
    }
}

/// <summary>
/// Argumentos do evento NoteChanged.
/// </summary>
public sealed class NoteChangedEventArgs : EventArgs
{
    /// <summary>ID da nota afetada.</summary>
    public int NoteId { get; }

    /// <summary>Tipo de mudança: Created, Updated, Deleted, Pinned, Unpinned.</summary>
    public string ChangeType { get; }

    public NoteChangedEventArgs(int noteId, string changeType)
    {
        NoteId = noteId;
        ChangeType = changeType;
    }
}

/// <summary>
/// Argumentos do evento CalendarNoteChanged.
/// </summary>
public sealed class CalendarNoteChangedEventArgs : EventArgs
{
    /// <summary>ID da nota de calendário afetada.</summary>
    public int NoteId { get; }

    /// <summary>Data da nota afetada.</summary>
    public DateTime Date { get; }

    /// <summary>Tipo de mudança: Created, Deleted.</summary>
    public string ChangeType { get; }

    public CalendarNoteChangedEventArgs(int noteId, DateTime date, string changeType)
    {
        NoteId = noteId;
        Date = date;
        ChangeType = changeType;
    }
}

#endregion

#region DisposableBase

/// <summary>
/// Classe base para implementação do padrão Dispose.
/// Implementa corretamente o padrão IDisposable com suporte a recursos gerenciados e não gerenciados.
/// </summary>
public abstract class DisposableBase : IDisposable
{
    private volatile bool _disposed;

    /// <summary>Indica se o objeto já foi descartado.</summary>
    protected bool IsDisposed => _disposed;

    /// <summary>
    /// Descarta os recursos gerenciados e não gerenciados.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Lógica de descarte. Sobrescreva para liberar recursos.
    /// </summary>
    /// <param name="disposing">True se chamado via Dispose(); false se via finalizador.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) DisposeManagedResources();
        DisposeUnmanagedResources();
        _disposed = true;
    }

    /// <summary>Libera recursos gerenciados (ex: outros IDisposable).</summary>
    protected virtual void DisposeManagedResources() { }

    /// <summary>Libera recursos não gerenciados (ex: handles nativos).</summary>
    protected virtual void DisposeUnmanagedResources() { }

    /// <summary>Verifica se o objeto foi descartado e lança ObjectDisposedException se sim.</summary>
    protected void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(GetType().FullName);
    }

    ~DisposableBase() => Dispose(false);
}

#endregion

#region SimpleCache

/// <summary>
/// Cache em memória com suporte a TTL (time-to-live) por entrada.
/// Thread-safe via ConcurrentDictionary.
/// </summary>
/// <typeparam name="TKey">Tipo da chave.</typeparam>
/// <typeparam name="TValue">Tipo do valor.</typeparam>
public sealed class SimpleCache<TKey, TValue> : IDisposable where TKey : notnull
{
    private sealed record CacheEntry(TValue Value, DateTime ExpiresAt);

    private readonly ConcurrentDictionary<TKey, CacheEntry> _store = new();
    private readonly Timer _cleanupTimer;
    private readonly TimeSpan _defaultTtl;
    private bool _disposed;

    /// <summary>
    /// Inicializa o cache com TTL padrão e intervalo de limpeza automática.
    /// </summary>
    /// <param name="defaultTtl">Tempo de vida padrão das entradas.</param>
    /// <param name="cleanupInterval">Intervalo de limpeza de entradas expiradas.</param>
    public SimpleCache(TimeSpan defaultTtl, TimeSpan? cleanupInterval = null)
    {
        _defaultTtl = defaultTtl;
        var interval = cleanupInterval ?? TimeSpan.FromMinutes(5);
        _cleanupTimer = new Timer(Cleanup, null, interval, interval);
    }

    /// <summary>Número de entradas atualmente no cache.</summary>
    public int Count => _store.Count;

    /// <summary>Adiciona ou substitui uma entrada no cache.</summary>
    public void Set(TKey key, TValue value, TimeSpan? ttl = null)
    {
        var expiry = DateTime.UtcNow.Add(ttl ?? _defaultTtl);
        _store[key] = new CacheEntry(value, expiry);
    }

    /// <summary>Tenta obter uma entrada do cache. Retorna false se não existir ou tiver expirado.</summary>
    public bool TryGet(TKey key, out TValue? value)
    {
        if (_store.TryGetValue(key, out var entry) && entry.ExpiresAt > DateTime.UtcNow)
        {
            value = entry.Value;
            return true;
        }
        value = default;
        return false;
    }

    /// <summary>Obtém ou adiciona uma entrada ao cache usando uma factory.</summary>
    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory, TimeSpan? ttl = null)
    {
        if (TryGet(key, out var cached) && cached is not null) return cached;
        var value = factory(key);
        Set(key, value, ttl);
        return value;
    }

    /// <summary>Remove uma entrada do cache.</summary>
    public bool Remove(TKey key) => _store.TryRemove(new KeyValuePair<TKey, CacheEntry>(key, _store.GetValueOrDefault(key)!));

    /// <summary>Remove todas as entradas do cache.</summary>
    public void Clear() => _store.Clear();

    private void Cleanup(object? _)
    {
        var now = DateTime.UtcNow;
        foreach (var kv in _store)
            if (kv.Value.ExpiresAt <= now)
                _store.TryRemove(kv);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _cleanupTimer.Dispose();
        _disposed = true;
    }
}

#endregion

#region ObservableValue

/// <summary>
/// Wrapper observável para um valor simples.
/// Dispara o evento <see cref="Changed"/> sempre que o valor é alterado.
/// </summary>
/// <typeparam name="T">Tipo do valor observado.</typeparam>
public sealed class ObservableValue<T>
{
    private T _value;

    /// <summary>Disparado quando o valor muda.</summary>
    public event Action<T, T>? Changed;

    public ObservableValue(T initial) => _value = initial;

    /// <summary>Valor atual. Ao definir, dispara <see cref="Changed"/> se diferente.</summary>
    public T Value
    {
        get => _value;
        set
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            var old = _value;
            _value = value;
            Changed?.Invoke(old, value);
        }
    }

    public static implicit operator T(ObservableValue<T> obs) => obs._value;
    public override string ToString() => _value?.ToString() ?? "null";
}

#endregion

#region Debouncer

/// <summary>
/// Implementa debounce para ações: atrasa a execução até que
/// o intervalo definido passe sem novas chamadas.
/// Útil para auto-save e buscas em tempo real.
/// </summary>
public sealed class Debouncer : IDisposable
{
    private readonly TimeSpan _delay;
    private CancellationTokenSource _cts = new();
    private bool _disposed;

    /// <param name="delay">Tempo de espera após a última chamada antes de executar.</param>
    public Debouncer(TimeSpan delay) => _delay = delay;

    /// <summary>
    /// Agenda a execução de <paramref name="action"/> após o delay.
    /// Chamadas repetidas reiniciam o contador.
    /// </summary>
    public void Debounce(Func<CancellationToken, Task> action)
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(_delay, token);
                await action(token);
            }
            catch (OperationCanceledException) { }
        }, token);
    }

    /// <summary>Versão síncrona do debounce.</summary>
    public void Debounce(Action action) =>
        Debounce(_ => { action(); return Task.CompletedTask; });

    public void Dispose()
    {
        if (_disposed) return;
        _cts.Cancel();
        _cts.Dispose();
        _disposed = true;
    }
}

#endregion

#region Pagination

/// <summary>
/// Parâmetros de paginação.
/// </summary>
public sealed record PageRequest(int Page = 1, int PageSize = 20)
{
    public int Skip => (Page - 1) * PageSize;
    public int Take => PageSize;

    public static PageRequest Default => new(1, 20);
    public static PageRequest All => new(1, int.MaxValue);
}

/// <summary>
/// Resultado paginado de uma consulta.
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>Itens da página atual.</summary>
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();

    /// <summary>Total de itens em todas as páginas.</summary>
    public int TotalCount { get; init; }

    /// <summary>Página atual (baseada em 1).</summary>
    public int Page { get; init; }

    /// <summary>Tamanho da página.</summary>
    public int PageSize { get; init; }

    /// <summary>Total de páginas.</summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

    /// <summary>Indica se há uma página anterior.</summary>
    public bool HasPrevious => Page > 1;

    /// <summary>Indica se há uma próxima página.</summary>
    public bool HasNext => Page < TotalPages;

    public static PagedResult<T> Empty(int page = 1, int pageSize = 20) => new()
    {
        Items = Array.Empty<T>(),
        TotalCount = 0,
        Page = page,
        PageSize = pageSize,
    };
}

#endregion

#region AppInfo

/// <summary>
/// Informações sobre a aplicação FocusVisk.
/// </summary>
public static class AppInfo
{
    public const string Name = "FocusVisk";
    public const string Version = "1.0.0";
    public const string Description = "Aplicação desktop de produtividade pessoal.";
    public const string Author = "opedrovisk";
    public const string Repository = "https://github.com/opedrovisk/FocusVisk";
    public const string Branch = "Desktop";
    public const string Framework = ".NET 8 (WPF + Blazor Hybrid)";
    public const string Platform = "Windows 10/11 x64";

    /// <summary>Retorna a string de versão formatada para exibição.</summary>
    public static string FullVersion => $"{Name} v{Version} — {Branch}";

    /// <summary>Retorna metadados da aplicação como dicionário.</summary>
    public static IReadOnlyDictionary<string, string> Metadata => new Dictionary<string, string>
    {
        ["Name"] = Name,
        ["Version"] = Version,
        ["Description"] = Description,
        ["Author"] = Author,
        ["Repository"] = Repository,
        ["Branch"] = Branch,
        ["Framework"] = Framework,
        ["Platform"] = Platform,
        ["BuildDate"] = BuildDate.ToString("yyyy-MM-dd"),
    };

    /// <summary>Data de build aproximada (baseada na data de compilação do assembly).</summary>
    public static DateTime BuildDate
    {
        get
        {
            var asm = Assembly.GetExecutingAssembly();
            var attr = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return attr is not null && DateTime.TryParse(attr.InformationalVersion, out var d)
                ? d
                : DateTime.Today;
        }
    }
}

#endregion