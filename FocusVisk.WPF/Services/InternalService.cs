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
using System.Text.RegularExpressions;
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

    /// <summary>Duração padrão de uma micro-pausa (modo ultradian) em minutos.</summary>
    public const int DefaultMicroBreakMinutes = 2;

    /// <summary>Número máximo de sessões por dia antes de sugerir descanso prolongado.</summary>
    public const int MaxRecommendedDailySessions = 12;

    /// <summary>Tolerância em segundos para considerar uma sessão como concluída no caso de lag.</summary>
    public const int TimerLagToleranceSeconds = 3;

    /// <summary>Intervalo de auto-save do estado do timer em milissegundos.</summary>
    public const int TimerStateSaveIntervalMs = 5000;

    /// <summary>Duração máxima de uma pausa longa em minutos.</summary>
    public const int MaxLongBreakMinutes = 60;

    /// <summary>Número mínimo de sessões configurável antes de pausa longa.</summary>
    public const int MinSessionsBeforeLongBreak = 1;

    /// <summary>Número máximo de sessões configurável antes de pausa longa.</summary>
    public const int MaxSessionsBeforeLongBreak = 10;

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

    /// <summary>Largura mínima da janela principal em pixels.</summary>
    public const int WindowMinWidthPx = 900;

    /// <summary>Altura mínima da janela principal em pixels.</summary>
    public const int WindowMinHeightPx = 600;

    /// <summary>Largura padrão da janela principal em pixels.</summary>
    public const int WindowDefaultWidthPx = 1280;

    /// <summary>Altura padrão da janela principal em pixels.</summary>
    public const int WindowDefaultHeightPx = 800;

    /// <summary>Espessura do anel do Pomodoro em pixels.</summary>
    public const double PomodoroRingStrokeWidth = 8.0;

    /// <summary>Duração da animação de transição entre páginas em milissegundos.</summary>
    public const int PageTransitionDurationMs = 200;

    /// <summary>Duração da animação de fade de notificações em milissegundos.</summary>
    public const int ToastFadeDurationMs = 300;

    /// <summary>Tempo de exibição de um toast de sucesso em milissegundos.</summary>
    public const int ToastSuccessDurationMs = 3000;

    /// <summary>Tempo de exibição de um toast de erro em milissegundos.</summary>
    public const int ToastErrorDurationMs = 5000;

    /// <summary>Número máximo de caracteres no título de uma tarefa.</summary>
    public const int TaskTitleMaxLength = 200;

    /// <summary>Número máximo de caracteres na descrição de uma tarefa.</summary>
    public const int TaskDescriptionMaxLength = 2000;

    /// <summary>Número máximo de caracteres no título de uma nota.</summary>
    public const int NoteTitleMaxLength = 150;

    /// <summary>Número máximo de caracteres no corpo de uma nota.</summary>
    public const int NoteBodyMaxLength = 50000;

    /// <summary>Número máximo de itens recentes exibidos na busca.</summary>
    public const int SearchRecentItemsMax = 8;

    /// <summary>Comprimento mínimo da query de busca para disparar resultados.</summary>
    public const int SearchMinQueryLength = 2;

    #endregion

    #region Themes

    public const string ThemeDark = "dark";
    public const string ThemeLight = "light";
    public const string ThemeCustom = "custom";
    public const string ThemeDracula = "dracula";
    public const string ThemeNord = "nord";
    public const string ThemeSolarized = "solarized";

    public const string DefaultAccentColor = "#7C6AF7";
    public const string DefaultSidebarBgColor = "#16161E";
    public const string DefaultMainBgColor = "#0F0F14";
    public const string DefaultTextColor = "#E8E8F0";
    public const string DefaultSubtextColor = "#8888A0";
    public const string DefaultBorderColor = "#2A2A3A";
    public const string DefaultSurfaceColor = "#1C1C28";

    public const string AccentPurple = "#7C6AF7";
    public const string AccentCoral = "#F07070";
    public const string AccentTeal = "#4ECDC4";
    public const string AccentAmber = "#F6C644";
    public const string AccentGreen = "#5DD68E";
    public const string AccentPink = "#E87CA0";
    public const string AccentBlue = "#5B9CF6";
    public const string AccentOrange = "#F6834A";
    public const string AccentCyan = "#44D4F0";
    public const string AccentLavender = "#B8ACFF";

    // Dracula palette
    public const string DraculaBackground = "#282A36";
    public const string DraculaForeground = "#F8F8F2";
    public const string DraculaPurple = "#BD93F9";
    public const string DraculaPink = "#FF79C6";
    public const string DraculaGreen = "#50FA7B";
    public const string DraculaYellow = "#F1FA8C";
    public const string DraculaCyan = "#8BE9FD";

    // Nord palette
    public const string NordPolarNight0 = "#2E3440";
    public const string NordPolarNight1 = "#3B4252";
    public const string NordSnowStorm0 = "#D8DEE9";
    public const string NordFrost0 = "#8FBCBB";
    public const string NordFrost3 = "#88C0D0";
    public const string NordAuroraGreen = "#A3BE8C";
    public const string NordAuroraPurple = "#B48EAD";

    #endregion

    #region Database

    public const string DatabaseName = "FocusViskDb";
    public const string LocalDbConnectionString = @"Server=(localdb)\mssqllocaldb;Database=FocusViskDb;Trusted_Connection=True;";
    public const string SqlExpressConnectionString = @"Server=.\SQLEXPRESS;Database=FocusViskDb;Trusted_Connection=True;";
    public const int DatabaseCommandTimeoutSeconds = 30;
    public const int MaxRetryOnFailure = 3;
    public const int RetryDelaySeconds = 5;

    /// <summary>Versão atual do schema do banco de dados.</summary>
    public const int DatabaseSchemaVersion = 4;

    /// <summary>Número máximo de registros retornados em queries sem paginação.</summary>
    public const int DatabaseMaxUnpagedResults = 1000;

    /// <summary>Tempo máximo de espera por conexão em segundos.</summary>
    public const int DatabaseConnectionTimeoutSeconds = 15;

    /// <summary>Tamanho do pool de conexões.</summary>
    public const int DatabaseConnectionPoolSize = 10;

    /// <summary>Intervalo de vacuum automático do SQLite em dias.</summary>
    public const int DatabaseVacuumIntervalDays = 30;

    #endregion

    #region Hosts File

    public const string HostsFilePath = @"C:\Windows\System32\drivers\etc\hosts";
    public const string HostsBlockStart = "# === FOCUS FocusVisk START ===";
    public const string HostsBlockEnd = "# === FOCUS FocusVisk END ===";
    public const string LoopbackAddress = "127.0.0.1";
    public const string LoopbackAddressV6 = "::1";

    /// <summary>Tamanho máximo permitido do arquivo hosts em bytes (segurança).</summary>
    public const int HostsFileMaxSizeBytes = 1_048_576; // 1 MB

    /// <summary>Número máximo de domínios bloqueáveis simultaneamente.</summary>
    public const int MaxBlockedDomains = 500;

    /// <summary>Extensão do arquivo de backup do hosts.</summary>
    public const string HostsBackupExtension = ".focusvisk.bak";

    #endregion

    #region Streak

    public const int MaxStreakLookbackDays = 365;
    public const int StreakMilestoneBronze = 7;
    public const int StreakMilestoneSilver = 30;
    public const int StreakMilestoneGold = 100;

    /// <summary>Milestone de diamante: 365 dias consecutivos.</summary>
    public const int StreakMilestoneDiamond = 365;

    /// <summary>Milestone de obsidiana: 500 dias consecutivos.</summary>
    public const int StreakMilestoneObsidian = 500;

    /// <summary>Janela em horas para considerar uma sessão válida para o streak do dia.</summary>
    public const int StreakDayWindowHours = 24;

    /// <summary>Número de dias de graça permitidos para não quebrar streak (feature futura).</summary>
    public const int StreakGraceDays = 0;

    #endregion

    #region Export & Import

    /// <summary>Versão do formato de exportação.</summary>
    public const string ExportFormatVersion = "1.0";

    /// <summary>Extensão padrão para exports CSV.</summary>
    public const string ExportExtensionCsv = ".csv";

    /// <summary>Extensão padrão para exports JSON.</summary>
    public const string ExportExtensionJson = ".json";

    /// <summary>Extensão padrão para exports de texto.</summary>
    public const string ExportExtensionPlain = ".txt";

    /// <summary>Tamanho máximo de arquivo para import em bytes.</summary>
    public const int ImportMaxFileSizeBytes = 10_485_760; // 10 MB

    /// <summary>Separador de campos no CSV exportado.</summary>
    public const char CsvDelimiter = ',';

    /// <summary>Encoding utilizado nos arquivos exportados.</summary>
    public const string ExportEncoding = "UTF-8";

    #endregion

    #region Notifications

    /// <summary>Antecedência padrão para notificar prazo de tarefa em horas.</summary>
    public const int TaskDueSoonNotificationHours = 24;

    /// <summary>Número máximo de notificações na fila simultaneamente.</summary>
    public const int MaxNotificationQueueSize = 50;

    /// <summary>Intervalo de verificação de prazos em minutos.</summary>
    public const int DeadlineCheckIntervalMinutes = 15;

    /// <summary>Duração da notificação do sistema operacional em segundos.</summary>
    public const int SystemNotificationDurationSeconds = 5;

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

    /// <summary>Relatórios e analytics de produtividade.</summary>
    Reports,

    /// <summary>Gerenciador de bloqueio de sites.</summary>
    Blocker,
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

    /// <summary>Mais sessões Pomodoro associadas primeiro.</summary>
    MostSessionsFirst,

    /// <summary>Mais recentemente atualizadas primeiro.</summary>
    UpdatedAtDesc,
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

    /// <summary>Por tamanho do conteúdo, maiores primeiro.</summary>
    LongestFirst,

    /// <summary>Por número de palavras, menor primeiro.</summary>
    ShortestFirst,
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

    /// <summary>Visualização anual compacta.</summary>
    Year,
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

    /// <summary>Markdown formatado.</summary>
    Markdown,
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

    /// <summary>365 dias consecutivos.</summary>
    Diamond,

    /// <summary>500 dias consecutivos.</summary>
    Obsidian,
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

    /// <summary>Ao iniciar a primeira sessão do dia.</summary>
    DailyStart,

    /// <summary>Ao bater a meta diária de sessões.</summary>
    DailyGoalReached,

    /// <summary>Lembrete de iniciar sessão após inatividade prolongada.</summary>
    IdleReminder,
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

    /// <summary>Arquivo hosts corrompido ou inesperado.</summary>
    HostsFileCorrupted,

    /// <summary>Bloqueio sendo ativado (transição).</summary>
    Activating,

    /// <summary>Bloqueio sendo desativado (transição).</summary>
    Deactivating,
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

    /// <summary>Tema alto contraste para acessibilidade.</summary>
    HighContrast,

    /// <summary>Tema Catppuccin Mocha.</summary>
    CatppuccinMocha,
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

    /// <summary>Sessão cancelada por inatividade detectada.</summary>
    InactivityDetected,

    /// <summary>Sessão pulada por mudança de configuração.</summary>
    SettingsChanged,
}

/// <summary>
/// Define o nível de prioridade de uma tarefa.
/// </summary>
public enum TaskPriority
{
    /// <summary>Sem prioridade definida.</summary>
    None = 0,

    /// <summary>Prioridade baixa.</summary>
    Low,

    /// <summary>Prioridade média (padrão).</summary>
    Medium,

    /// <summary>Prioridade alta.</summary>
    High,

    /// <summary>Urgente — deve ser feita hoje.</summary>
    Urgent,
}

/// <summary>
/// Define o estado atual de uma tarefa.
/// </summary>
public enum TaskStatus
{
    /// <summary>Tarefa pendente.</summary>
    Pending = 0,

    /// <summary>Tarefa em progresso.</summary>
    InProgress,

    /// <summary>Tarefa concluída.</summary>
    Completed,

    /// <summary>Tarefa arquivada.</summary>
    Archived,

    /// <summary>Tarefa cancelada.</summary>
    Cancelled,
}

/// <summary>
/// Define o tipo de recorrência de uma tarefa.
/// </summary>
public enum TaskRecurrence
{
    /// <summary>Sem recorrência.</summary>
    None = 0,

    /// <summary>Repete diariamente.</summary>
    Daily,

    /// <summary>Repete semanalmente.</summary>
    Weekly,

    /// <summary>Repete mensalmente.</summary>
    Monthly,

    /// <summary>Repete em dias úteis.</summary>
    Weekdays,

    /// <summary>Repete conforme intervalo customizado.</summary>
    Custom,
}

/// <summary>
/// Define a fase atual do timer Pomodoro.
/// </summary>
public enum PomodoroPhase
{
    /// <summary>Sessão de foco ativa.</summary>
    Focus = 0,

    /// <summary>Pausa curta.</summary>
    ShortBreak,

    /// <summary>Pausa longa.</summary>
    LongBreak,

    /// <summary>Timer aguardando início.</summary>
    Idle,
}

/// <summary>
/// Define o nível de log interno da aplicação.
/// </summary>
public enum LogLevel
{
    /// <summary>Informações de diagnóstico detalhadas.</summary>
    Verbose = 0,

    /// <summary>Informações gerais de execução.</summary>
    Information,

    /// <summary>Avisos que não impedem o funcionamento.</summary>
    Warning,

    /// <summary>Erros recuperáveis.</summary>
    Error,

    /// <summary>Erros críticos que encerram a aplicação.</summary>
    Fatal,
}

/// <summary>
/// Define o tipo de entrada no histórico de atividades.
/// </summary>
public enum ActivityType
{
    /// <summary>Sessão de foco concluída.</summary>
    FocusSession = 0,

    /// <summary>Tarefa criada.</summary>
    TaskCreated,

    /// <summary>Tarefa concluída.</summary>
    TaskCompleted,

    /// <summary>Nota criada.</summary>
    NoteCreated,

    /// <summary>Milestone de streak atingido.</summary>
    MilestoneReached,

    /// <summary>Bloqueador ativado.</summary>
    BlockerActivated,

    /// <summary>Configuração alterada.</summary>
    SettingsChanged,
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

    /// <summary>Executa <paramref name="onSuccess"/> se Ok, ou retorna o erro propagado.</summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> onSuccess)
    {
        return IsSuccess ? Result<TOut>.Ok(onSuccess(Value!)) : Result<TOut>.Fail(Error!);
    }

    /// <summary>Executa <paramref name="action"/> apenas se o resultado for Ok.</summary>
    public Result<T> OnSuccess(Action<T> action)
    {
        if (IsSuccess) action(Value!);
        return this;
    }

    /// <summary>Executa <paramref name="action"/> apenas se o resultado for Fail.</summary>
    public Result<T> OnFailure(Action<string> action)
    {
        if (IsFailure) action(Error!);
        return this;
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

    /// <summary>Combina dois Results; retorna Fail se qualquer um falhar.</summary>
    public static Result Combine(params Result[] results)
    {
        foreach (var r in results)
            if (r.IsFailure) return r;
        return Ok();
    }

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
        => value.Date == DateTime.Today;

    /// <summary>Retorna true se a data é nesta semana.</summary>
    public static bool IsThisWeek(this DateTime value)
    {
        var start = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        return value.Date >= start && value.Date <= start.AddDays(6);
    }

    /// <summary>Retorna true se a data é neste mês.</summary>
    public static bool IsThisMonth(this DateTime value)
        => value.Month == DateTime.Now.Month && value.Year == DateTime.Now.Year;

    /// <summary>Retorna true se a data é neste ano.</summary>
    public static bool IsThisYear(this DateTime value)
        => value.Year == DateTime.Now.Year;

    /// <summary>Retorna true se a data está no passado (antes de hoje).</summary>
    public static bool IsInPast(this DateTime value)
        => value.Date < DateTime.Today;

    /// <summary>Retorna true se a data está no futuro (depois de hoje).</summary>
    public static bool IsInFuture(this DateTime value)
        => value.Date > DateTime.Today;

    /// <summary>Retorna true se a data é ontem.</summary>
    public static bool IsYesterday(this DateTime value)
        => value.Date == DateTime.Today.AddDays(-1);

    /// <summary>Retorna true se a data é amanhã.</summary>
    public static bool IsTomorrow(this DateTime value)
        => value.Date == DateTime.Today.AddDays(1);

    /// <summary>Retorna o início da semana (domingo).</summary>
    public static DateTime StartOfWeek(this DateTime value)
        => value.Date.AddDays(-(int)value.DayOfWeek);

    /// <summary>Retorna o final da semana (sábado).</summary>
    public static DateTime EndOfWeek(this DateTime value)
        => value.StartOfWeek().AddDays(6);

    /// <summary>Retorna o primeiro dia do mês.</summary>
    public static DateTime StartOfMonth(this DateTime value)
        => new DateTime(value.Year, value.Month, 1);

    /// <summary>Retorna o último dia do mês.</summary>
    public static DateTime EndOfMonth(this DateTime value)
        => value.StartOfMonth().AddMonths(1).AddDays(-1);

    /// <summary>Retorna o primeiro dia do ano.</summary>
    public static DateTime StartOfYear(this DateTime value)
        => new DateTime(value.Year, 1, 1);

    /// <summary>Retorna o último dia do ano.</summary>
    public static DateTime EndOfYear(this DateTime value)
        => new DateTime(value.Year, 12, 31);

    /// <summary>Retorna a data sem o componente de horário (equivalente a .Date).</summary>
    public static DateTime ToDateOnly(this DateTime value)
        => value.Date;

    /// <summary>Retorna a data em formato amigável pt-BR.</summary>
    public static string ToFriendlyDate(this DateTime value)
    {
        var ptBR = new CultureInfo("pt-BR");
        if (value.IsToday()) return "Hoje";
        if (value.IsYesterday()) return "Ontem";
        if (value.IsTomorrow()) return "Amanhã";
        return value.ToString("dd 'de' MMMM", ptBR);
    }

    /// <summary>Retorna a data e hora em formato amigável pt-BR.</summary>
    public static string ToFriendlyDateTime(this DateTime value)
    {
        var ptBR = new CultureInfo("pt-BR");
        var datePart = value.ToFriendlyDate();
        return $"{datePart} às {value.ToString("HH:mm", ptBR)}";
    }

    /// <summary>Retorna a data no formato dd/MM/yyyy.</summary>
    public static string ToShortPtBR(this DateTime value)
        => value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    /// <summary>Retorna mês e ano por extenso em pt-BR.</summary>
    public static string ToMonthYearPtBR(this DateTime value)
        => value.ToString("MMMM yyyy", new CultureInfo("pt-BR"));

    /// <summary>Retorna o horário no formato HH:mm.</summary>
    public static string ToShortTime(this DateTime value)
        => value.ToString("HH:mm");

    /// <summary>Retorna a diferença relativa ao momento atual em pt-BR (ex: "há 3 minutos").</summary>
    public static string ToRelativeTime(this DateTime value)
    {
        var diff = DateTime.Now - value;
        if (diff.TotalSeconds < 60) return "agora mesmo";
        if (diff.TotalMinutes < 60) return $"há {(int)diff.TotalMinutes} minuto{((int)diff.TotalMinutes != 1 ? "s" : "")}";
        if (diff.TotalHours < 24) return $"há {(int)diff.TotalHours} hora{((int)diff.TotalHours != 1 ? "s" : "")}";
        if (diff.TotalDays < 7) return $"há {(int)diff.TotalDays} dia{((int)diff.TotalDays != 1 ? "s" : "")}";
        return value.ToFriendlyDate();
    }

    /// <summary>Retorna o número da semana do ano (ISO 8601).</summary>
    public static int WeekOfYear(this DateTime value)
        => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(value, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

    /// <summary>Verifica se dois DateTimes estão no mesmo dia.</summary>
    public static bool IsSameDay(this DateTime value, DateTime other)
        => value.Date == other.Date;
}

/// <summary>
/// Métodos de extensão para <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>Trunca a string ao comprimento máximo.</summary>
    public static string Truncate(this string value, int maxLength, string suffix = "...")
    {
        if (string.IsNullOrEmpty(value) || value.Length <= maxLength) return value;
        return value[..maxLength] + suffix;
    }

    /// <summary>Atalho para string.IsNullOrEmpty.</summary>
    public static bool IsNullOrEmpty(this string value)
        => string.IsNullOrEmpty(value);

    /// <summary>Atalho para string.IsNullOrWhiteSpace.</summary>
    public static bool IsNullOrWhiteSpace(this string value)
        => string.IsNullOrWhiteSpace(value);

    /// <summary>Converte para slug (lowercase, sem espaços).</summary>
    public static string ToSlug(this string value)
        => value.ToLowerInvariant().Trim().Replace(' ', '-');

    /// <summary>Primeira letra maiúscula.</summary>
    public static string Capitalize(this string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return char.ToUpper(value[0]) + value[1..];
    }

    /// <summary>Converte para Title Case em pt-BR.</summary>
    public static string ToTitleCase(this string value)
        => CultureInfo.GetCultureInfo("pt-BR").TextInfo.ToTitleCase(value.ToLowerInvariant());

    /// <summary>Conta o número de palavras.</summary>
    public static int CountWords(this string value)
        => value.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

    /// <summary>Conta o número de linhas.</summary>
    public static int CountLines(this string value)
        => value.Split('\n').Length;

    /// <summary>Contains ignorando case.</summary>
    public static bool ContainsIgnoreCase(this string value, string search)
        => value.Contains(search, StringComparison.OrdinalIgnoreCase);

    /// <summary>StartsWith ignorando case.</summary>
    public static bool StartsWithIgnoreCase(this string value, string search)
        => value.StartsWith(search, StringComparison.OrdinalIgnoreCase);

    /// <summary>EndsWith ignorando case.</summary>
    public static bool EndsWithIgnoreCase(this string value, string search)
        => value.EndsWith(search, StringComparison.OrdinalIgnoreCase);

    /// <summary>Equals ignorando case.</summary>
    public static bool EqualsIgnoreCase(this string value, string other)
        => value.Equals(other, StringComparison.OrdinalIgnoreCase);

    /// <summary>Remove acentos e diacríticos da string.</summary>
    public static string RemoveDiacritics(this string value)
    {
        var normalized = value.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in normalized)
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    /// <summary>Repete a string <paramref name="count"/> vezes.</summary>
    public static string Repeat(this string value, int count)
        => string.Concat(Enumerable.Repeat(value, count));

    /// <summary>Retorna null se a string for nula ou vazia.</summary>
    public static string? NullIfEmpty(this string value)
        => string.IsNullOrEmpty(value) ? null : value;

    /// <summary>Retorna null se a string for nula, vazia ou apenas espaços.</summary>
    public static string? NullIfWhiteSpace(this string value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>Remove múltiplos espaços consecutivos, substituindo por um único.</summary>
    public static string CollapseSpaces(this string value)
        => Regex.Replace(value.Trim(), @"\s+", " ");

    /// <summary>Verifica se a string é um e-mail válido (simplificado).</summary>
    public static bool IsValidEmail(this string value)
        => Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    /// <summary>Verifica se a string contém apenas dígitos.</summary>
    public static bool IsNumeric(this string value)
        => !string.IsNullOrEmpty(value) && value.All(char.IsDigit);

    /// <summary>Extrai apenas os dígitos da string.</summary>
    public static string OnlyDigits(this string value)
        => new(value.Where(char.IsDigit).ToArray());
}

/// <summary>
/// Métodos de extensão para <see cref="int"/>.
/// </summary>
public static class IntExtensions
{
    /// <summary>Converte int para TimeSpan em minutos.</summary>
    public static TimeSpan ToMinutesTimeSpan(this int value)
        => TimeSpan.FromMinutes(value);

    /// <summary>Converte int para TimeSpan em segundos.</summary>
    public static TimeSpan ToSecondsTimeSpan(this int value)
        => TimeSpan.FromSeconds(value);

    /// <summary>Limita o valor entre min e max.</summary>
    public static int Clamp(this int value, int min, int max)
        => Math.Max(min, Math.Min(max, value));

    /// <summary>Retorna true se o valor está no intervalo.</summary>
    public static bool IsInRange(this int value, int min, int max)
        => value >= min && value <= max;

    /// <summary>Retorna o ordinal em pt-BR (1º, 2º...).</summary>
    public static string ToOrdinal(this int value)
        => $"{value}º";

    /// <summary>Retorna true se o valor é par.</summary>
    public static bool IsEven(this int value)
        => value % 2 == 0;

    /// <summary>Retorna true se o valor é ímpar.</summary>
    public static bool IsOdd(this int value)
        => value % 2 != 0;

    /// <summary>Retorna true se o valor é positivo.</summary>
    public static bool IsPositive(this int value)
        => value > 0;

    /// <summary>Retorna true se o valor é zero ou positivo.</summary>
    public static bool IsNonNegative(this int value)
        => value >= 0;

    /// <summary>Retorna o valor absoluto.</summary>
    public static int Abs(this int value)
        => Math.Abs(value);

    /// <summary>Formata o número com separador de milhar em pt-BR (ex: 1.234).</summary>
    public static string ToFormattedNumber(this int value)
        => value.ToString("N0", new CultureInfo("pt-BR"));

    /// <summary>Converte minutos para string legível (ex: 1h 30min ou 45min).</summary>
    public static string MinutesToHumanReadable(this int value)
    {
        if (value >= 60) return $"{value / 60}h {value % 60}min";
        return $"{value}min";
    }
}

/// <summary>
/// Métodos de extensão para <see cref="double"/>.
/// </summary>
public static class DoubleExtensions
{
    /// <summary>Limita o valor double entre min e max.</summary>
    public static double Clamp(this double value, double min, double max)
        => Math.Max(min, Math.Min(max, value));

    /// <summary>Retorna true se está no intervalo [0, 1].</summary>
    public static bool IsNormalizedProgress(this double value)
        => value >= 0.0 && value <= 1.0;

    /// <summary>Converte fração [0,1] para porcentagem inteira.</summary>
    public static int ToPercent(this double value)
        => (int)Math.Round(value.Clamp(0, 1) * 100);

    /// <summary>Arredonda para N casas decimais.</summary>
    public static double RoundTo(this double value, int decimals)
        => Math.Round(value, decimals);
}

/// <summary>
/// Métodos de extensão para <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanExtensions
{
    /// <summary>Retorna mm:ss formatado.</summary>
    public static string ToMinutesDisplay(this TimeSpan value)
        => $"{(int)value.TotalMinutes:D2}:{value.Seconds:D2}";

    /// <summary>Retorna hh:mm:ss formatado.</summary>
    public static string ToFullDisplay(this TimeSpan value)
        => $"{(int)value.TotalHours:D2}:{value.Minutes:D2}:{value.Seconds:D2}";

    /// <summary>Retorna string legível (ex: 1h 30min).</summary>
    public static string ToHumanReadable(this TimeSpan value)
    {
        if (value.TotalHours >= 1) return $"{(int)value.TotalHours}h {value.Minutes}min";
        return $"{(int)value.TotalMinutes}min";
    }

    /// <summary>Retorna string legível com segundos (ex: 1h 30min 5s).</summary>
    public static string ToHumanReadableWithSeconds(this TimeSpan value)
    {
        if (value.TotalHours >= 1) return $"{(int)value.TotalHours}h {value.Minutes}min {value.Seconds}s";
        if (value.TotalMinutes >= 1) return $"{(int)value.TotalMinutes}min {value.Seconds}s";
        return $"{value.Seconds}s";
    }

    /// <summary>Retorna true se <= zero.</summary>
    public static bool IsZeroOrNegative(this TimeSpan value)
        => value <= TimeSpan.Zero;

    /// <summary>Retorna true se > zero.</summary>
    public static bool IsPositive(this TimeSpan value)
        => value > TimeSpan.Zero;

    /// <summary>Retorna o progresso de 0.0 a 1.0 dado um total.</summary>
    public static double ProgressOf(this TimeSpan elapsed, TimeSpan total)
    {
        if (total <= TimeSpan.Zero) return 0;
        return (elapsed.TotalSeconds / total.TotalSeconds).Clamp(0, 1);
    }
}

/// <summary>
/// Métodos de extensão para <see cref="IEnumerable{T}"/>.
/// </summary>
public static class IEnumerableExtensions
{
    /// <summary>Retorna true se nulo ou sem elementos.</summary>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> value)
        => value is null || !value.Any();

    /// <summary>Retorna a coleção ou vazia se nula.</summary>
    public static IEnumerable<T> OrEmpty<T>(this IEnumerable<T> value)
        => value ?? Enumerable.Empty<T>();

    /// <summary>Retorna o único elemento que satisfaz o predicado, ou null se não encontrado.</summary>
    public static T? FirstOrNull<T>(this IEnumerable<T> source, Func<T, bool> predicate) where T : class
        => source.FirstOrDefault(predicate);

    /// <summary>Aplica uma ação a cada elemento da coleção.</summary>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }

    /// <summary>Divide a coleção em grupos de tamanho <paramref name="size"/>.</summary>
    public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int size)
        => source
            .Select((item, index) => (item, index))
            .GroupBy(x => x.index / size)
            .Select(g => g.Select(x => x.item));

    /// <summary>Embaralha a coleção.</summary>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var list = source.ToList();
        var rng = new Random();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }

    /// <summary>Remove duplicatas com base em uma chave.</summary>
    public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        => source.GroupBy(keySelector).Select(g => g.First());

    /// <summary>Retorna os elementos em ordem aleatória (alias de Shuffle).</summary>
    public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        => source.Shuffle();

    /// <summary>Retorna índice e item juntos.</summary>
    public static IEnumerable<(int Index, T Item)> Indexed<T>(this IEnumerable<T> source)
        => source.Select((item, i) => (i, item));
}

/// <summary>
/// Métodos de extensão para <see cref="bool"/>.
/// </summary>
public static class BoolExtensions
{
    /// <summary>Retorna "Sim" ou "Não" em pt-BR.</summary>
    public static string ToYesNo(this bool value)
        => value ? "Sim" : "Não";

    /// <summary>Retorna "Ativo" ou "Inativo".</summary>
    public static string ToActiveInactive(this bool value)
        => value ? "Ativo" : "Inativo";

    /// <summary>Retorna "Concluído" ou "Pendente".</summary>
    public static string ToCompletedPending(this bool value)
        => value ? "Concluído" : "Pendente";

    /// <summary>Executa a ação se o valor for true.</summary>
    public static bool IfTrue(this bool value, Action action)
    {
        if (value) action();
        return value;
    }

    /// <summary>Executa a ação se o valor for false.</summary>
    public static bool IfFalse(this bool value, Action action)
    {
        if (!value) action();
        return value;
    }
}

/// <summary>
/// Métodos de extensão para <see cref="Guid"/>.
/// </summary>
public static class GuidExtensions
{
    /// <summary>Retorna os primeiros 8 caracteres do GUID como identificador curto.</summary>
    public static string ToShortId(this Guid value)
        => value.ToString("N")[..8].ToUpperInvariant();

    /// <summary>Retorna true se o GUID é vazio.</summary>
    public static bool IsEmpty(this Guid value)
        => value == Guid.Empty;
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
        if (value is null) throw new ArgumentNullException(paramName);
        return value;
    }

    /// <summary>Garante que a string não é nula ou vazia.</summary>
    public static string NotNullOrEmpty(string? value, string paramName)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentException("Valor não pode ser nulo ou vazio.", paramName);
        return value;
    }

    /// <summary>Garante que a string não é nula, vazia ou espaços.</summary>
    public static string NotNullOrWhiteSpace(string? value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Valor não pode ser nulo, vazio ou espaço em branco.", paramName);
        return value;
    }

    /// <summary>Garante que o inteiro está no intervalo [min, max].</summary>
    public static int InRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName, $"Valor deve estar entre {min} e {max}. Recebido: {value}");
        return value;
    }

    /// <summary>Garante que o inteiro é positivo (> 0).</summary>
    public static int Positive(int value, string paramName)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(paramName, $"Valor deve ser positivo. Recebido: {value}");
        return value;
    }

    /// <summary>Garante que o inteiro é zero ou positivo (>= 0).</summary>
    public static int NonNegative(int value, string paramName)
    {
        if (value < 0) throw new ArgumentOutOfRangeException(paramName, $"Valor deve ser não-negativo. Recebido: {value}");
        return value;
    }

    /// <summary>Garante que o Guid não é vazio.</summary>
    public static Guid NotEmpty(Guid value, string paramName)
    {
        if (value == Guid.Empty) throw new ArgumentException("Guid não pode ser vazio.", paramName);
        return value;
    }

    /// <summary>Garante que a coleção não é nula ou vazia.</summary>
    public static IReadOnlyCollection<T> NotEmpty<T>(IReadOnlyCollection<T>? value, string paramName)
    {
        if (value is null || value.Count == 0) throw new ArgumentException("Coleção não pode ser nula ou vazia.", paramName);
        return value;
    }

    /// <summary>Garante que o double está no intervalo [min, max].</summary>
    public static double InRange(double value, double min, double max, string paramName)
    {
        if (value < min || value > max) throw new ArgumentOutOfRangeException(paramName, $"Valor deve estar entre {min} e {max}. Recebido: {value}");
        return value;
    }

    /// <summary>Garante que o double é positivo.</summary>
    public static double Positive(double value, string paramName)
    {
        if (value <= 0) throw new ArgumentOutOfRangeException(paramName, $"Valor deve ser positivo. Recebido: {value}");
        return value;
    }

    /// <summary>Garante que a string não excede o comprimento máximo.</summary>
    public static string MaxLength(string value, int maxLength, string paramName)
    {
        if (value is not null && value.Length > maxLength)
            throw new ArgumentException($"Valor não pode exceder {maxLength} caracteres. Recebido: {value.Length}", paramName);
        return value!;
    }

    /// <summary>Garante que o DateTime não é MinValue.</summary>
    public static DateTime NotMinValue(DateTime value, string paramName)
    {
        if (value == DateTime.MinValue) throw new ArgumentException("Data não pode ser DateTime.MinValue.", paramName);
        return value;
    }
}

#endregion

#region EventArgs

/// <summary>
/// Argumentos do evento PomodoroPhaseChanged.
/// </summary>
public sealed class PomodoroPhaseChangedEventArgs : EventArgs
{
    public PomodoroPhase PreviousPhase { get; }
    public PomodoroPhase NewPhase { get; }
    public int CompletedSessions { get; }
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
    public TimeSpan Remaining { get; }
    public TimeSpan Elapsed { get; }
    public bool IsRunning { get; }
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
    public DateTime StartedAt { get; }
    public DateTime CompletedAt { get; }
    public int DurationMinutes { get; }
    public string? TaskTitle { get; }

    /// <summary>Número da sessão concluída no dia atual.</summary>
    public int DailySessionNumber { get; }

    /// <summary>Indica se esta sessão fechou um ciclo (4 sessões antes da pausa longa).</summary>
    public bool ClosedCycle { get; }

    public SessionCompletedEventArgs(DateTime startedAt, DateTime completedAt, int durationMinutes, string? taskTitle, int dailySessionNumber = 0, bool closedCycle = false)
    {
        StartedAt = startedAt;
        CompletedAt = completedAt;
        DurationMinutes = durationMinutes;
        TaskTitle = taskTitle;
        DailySessionNumber = dailySessionNumber;
        ClosedCycle = closedCycle;
    }
}

/// <summary>
/// Argumentos do evento FocusBlockerStatusChanged.
/// </summary>
public sealed class FocusBlockerStatusChangedEventArgs : EventArgs
{
    public FocusBlockerStatus Status { get; }
    public string? Error { get; }
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
    public string Theme { get; }
    public string AccentColor { get; }
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
    public int TaskId { get; }
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
    public int NoteId { get; }
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
    public int NoteId { get; }
    public DateTime Date { get; }
    public string ChangeType { get; }

    public CalendarNoteChangedEventArgs(int noteId, DateTime date, string changeType)
    {
        NoteId = noteId;
        Date = date;
        ChangeType = changeType;
    }
}

/// <summary>
/// Argumentos do evento StreakChanged.
/// </summary>
public sealed class StreakChangedEventArgs : EventArgs
{
    /// <summary>Valor anterior do streak.</summary>
    public int PreviousStreak { get; }

    /// <summary>Valor atual do streak.</summary>
    public int CurrentStreak { get; }

    /// <summary>Indica se o streak foi quebrado.</summary>
    public bool WasBroken { get; }

    /// <summary>Milestone atingido nesta mudança, se houver.</summary>
    public StreakMilestone? MilestoneReached { get; }

    public StreakChangedEventArgs(int previousStreak, int currentStreak, bool wasBroken, StreakMilestone? milestoneReached = null)
    {
        PreviousStreak = previousStreak;
        CurrentStreak = currentStreak;
        WasBroken = wasBroken;
        MilestoneReached = milestoneReached;
    }
}

/// <summary>
/// Argumentos do evento AppPageChanged.
/// </summary>
public sealed class AppPageChangedEventArgs : EventArgs
{
    /// <summary>Página anterior.</summary>
    public AppPage PreviousPage { get; }

    /// <summary>Nova página.</summary>
    public AppPage NewPage { get; }

    public AppPageChangedEventArgs(AppPage previousPage, AppPage newPage)
    {
        PreviousPage = previousPage;
        NewPage = newPage;
    }
}

/// <summary>
/// Argumentos do evento SearchExecuted.
/// </summary>
public sealed class SearchExecutedEventArgs : EventArgs
{
    /// <summary>Query pesquisada.</summary>
    public string Query { get; }

    /// <summary>Número de resultados retornados.</summary>
    public int ResultCount { get; }

    /// <summary>Duração da busca em milissegundos.</summary>
    public long ElapsedMs { get; }

    public SearchExecutedEventArgs(string query, int resultCount, long elapsedMs)
    {
        Query = query;
        ResultCount = resultCount;
        ElapsedMs = elapsedMs;
    }
}

#endregion

#region DisposableBase

/// <summary>
/// Classe base para implementação do padrão Dispose.
/// </summary>
public abstract class DisposableBase : IDisposable
{
    private volatile bool _disposed;

    protected bool IsDisposed => _disposed;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) DisposeManagedResources();
        DisposeUnmanagedResources();
        _disposed = true;
    }

    protected virtual void DisposeManagedResources() { }
    protected virtual void DisposeUnmanagedResources() { }

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
public sealed class SimpleCache<TKey, TValue> : IDisposable where TKey : notnull
{
    private sealed record CacheEntry(TValue Value, DateTime ExpiresAt);

    private readonly ConcurrentDictionary<TKey, CacheEntry> _store = new();
    private readonly Timer _cleanupTimer;
    private readonly TimeSpan _defaultTtl;
    private bool _disposed;

    public SimpleCache(TimeSpan defaultTtl, TimeSpan? cleanupInterval = null)
    {
        _defaultTtl = defaultTtl;
        var interval = cleanupInterval ?? TimeSpan.FromMinutes(5);
        _cleanupTimer = new Timer(Cleanup, null, interval, interval);
    }

    public int Count => _store.Count;

    public void Set(TKey key, TValue value, TimeSpan? ttl = null)
    {
        var expiry = DateTime.UtcNow.Add(ttl ?? _defaultTtl);
        _store[key] = new CacheEntry(value, expiry);
    }

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

    public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory, TimeSpan? ttl = null)
    {
        if (TryGet(key, out var cached) && cached is not null) return cached;
        var value = factory(key);
        Set(key, value, ttl);
        return value;
    }

    /// <summary>Retorna todos os itens não expirados do cache.</summary>
    public IReadOnlyDictionary<TKey, TValue> GetAll()
    {
        var now = DateTime.UtcNow;
        return _store
            .Where(kv => kv.Value.ExpiresAt > now)
            .ToDictionary(kv => kv.Key, kv => kv.Value.Value);
    }

    /// <summary>Retorna true se a chave existe e não expirou.</summary>
    public bool ContainsKey(TKey key)
        => _store.TryGetValue(key, out var entry) && entry.ExpiresAt > DateTime.UtcNow;

    public bool Remove(TKey key) => _store.TryRemove(new KeyValuePair<TKey, CacheEntry>(key, _store.GetValueOrDefault(key)!));
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
/// </summary>
public sealed class ObservableValue<T>
{
    private T _value;

    public event Action<T, T>? Changed;

    public ObservableValue(T initial) => _value = initial;

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

    /// <summary>Força a notificação mesmo que o valor não tenha mudado.</summary>
    public void ForceNotify() => Changed?.Invoke(_value, _value);

    /// <summary>Define o valor silenciosamente, sem disparar o evento Changed.</summary>
    public void SetSilently(T value) => _value = value;

    public static implicit operator T(ObservableValue<T> obs) => obs._value;
    public override string ToString() => _value?.ToString() ?? "null";
}

#endregion

#region Debouncer

/// <summary>
/// Implementa debounce para ações.
/// </summary>
public sealed class Debouncer : IDisposable
{
    private readonly TimeSpan _delay;
    private CancellationTokenSource _cts = new();
    private bool _disposed;

    public Debouncer(TimeSpan delay) => _delay = delay;

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

    public void Debounce(Action action) =>
        Debounce(_ => { action(); return Task.CompletedTask; });

    /// <summary>Cancela qualquer ação pendente sem executá-la.</summary>
    public void Cancel()
    {
        _cts.Cancel();
        _cts.Dispose();
        _cts = new CancellationTokenSource();
    }

    public void Dispose()
    {
        if (_disposed) return;
        _cts.Cancel();
        _cts.Dispose();
        _disposed = true;
    }
}

#endregion

#region Throttler

/// <summary>
/// Implementa throttle para ações: garante que a ação seja executada
/// no máximo uma vez por intervalo de tempo, independentemente da frequência de chamadas.
/// Complementar ao Debouncer.
/// </summary>
public sealed class Throttler : IDisposable
{
    private readonly TimeSpan _interval;
    private DateTime _lastExecution = DateTime.MinValue;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private bool _disposed;

    public Throttler(TimeSpan interval) => _interval = interval;

    /// <summary>
    /// Executa <paramref name="action"/> apenas se o intervalo desde a última execução
    /// já tiver passado.
    /// </summary>
    public async Task ThrottleAsync(Func<Task> action)
    {
        await _lock.WaitAsync();
        try
        {
            if (DateTime.UtcNow - _lastExecution < _interval) return;
            _lastExecution = DateTime.UtcNow;
            await action();
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <summary>Versão síncrona do throttle.</summary>
    public void Throttle(Action action)
    {
        _lock.Wait();
        try
        {
            if (DateTime.UtcNow - _lastExecution < _interval) return;
            _lastExecution = DateTime.UtcNow;
            action();
        }
        finally
        {
            _lock.Release();
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _lock.Dispose();
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

    /// <summary>Retorna a próxima página.</summary>
    public PageRequest Next() => this with { Page = Page + 1 };

    /// <summary>Retorna a página anterior (mínimo 1).</summary>
    public PageRequest Previous() => this with { Page = Math.Max(1, Page - 1) };

    /// <summary>Retorna a primeira página com o mesmo tamanho.</summary>
    public PageRequest First() => this with { Page = 1 };
}

/// <summary>
/// Resultado paginado de uma consulta.
/// </summary>
public sealed class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;

    /// <summary>Indica se o resultado está vazio.</summary>
    public bool IsEmpty => Items.Count == 0;

    /// <summary>Mapeia os itens para outro tipo.</summary>
    public PagedResult<TOut> Map<TOut>(Func<T, TOut> mapper) => new()
    {
        Items = Items.Select(mapper).ToList(),
        TotalCount = TotalCount,
        Page = Page,
        PageSize = PageSize,
    };

    public static PagedResult<T> Empty(int page = 1, int pageSize = 20) => new()
    {
        Items = Array.Empty<T>(),
        TotalCount = 0,
        Page = page,
        PageSize = pageSize,
    };

    /// <summary>Cria um resultado de página única a partir de uma lista completa.</summary>
    public static PagedResult<T> FromList(IReadOnlyList<T> items, int page, int pageSize) => new()
    {
        Items = items,
        TotalCount = items.Count,
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
    public const string License = "MIT";
    public const string IssueTracker = "https://github.com/opedrovisk/FocusVisk/issues";

    public static string FullVersion => $"{Name} v{Version} — {Branch}";

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
        ["License"] = License,
        ["BuildDate"] = BuildDate.ToString("yyyy-MM-dd"),
    };

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

    /// <summary>Retorna a versão como tupla (major, minor, patch).</summary>
    public static (int Major, int Minor, int Patch) ParsedVersion
    {
        get
        {
            var parts = Version.Split('.');
            return (
                int.Parse(parts[0]),
                parts.Length > 1 ? int.Parse(parts[1]) : 0,
                parts.Length > 2 ? int.Parse(parts[2]) : 0
            );
        }
    }
}

#endregion

#region ColorHelper

/// <summary>
/// Utilitários para manipulação de cores hexadecimais.
/// Útil para geração dinâmica de paletas de tema.
/// </summary>
public static class ColorHelper
{
    /// <summary>Converte hex (#RRGGBB) para componentes RGB.</summary>
    public static (byte R, byte G, byte B) HexToRgb(string hex)
    {
        hex = hex.TrimStart('#');
        return (
            Convert.ToByte(hex[..2], 16),
            Convert.ToByte(hex[2..4], 16),
            Convert.ToByte(hex[4..6], 16)
        );
    }

    /// <summary>Converte componentes RGB para hex (#RRGGBB).</summary>
    public static string RgbToHex(byte r, byte g, byte b)
        => $"#{r:X2}{g:X2}{b:X2}";

    /// <summary>Aplica opacidade a uma cor hex, retornando #RRGGBBAA.</summary>
    public static string WithOpacity(string hex, double opacity)
    {
        var (r, g, b) = HexToRgb(hex);
        var a = (byte)(opacity.Clamp(0, 1) * 255);
        return $"#{r:X2}{g:X2}{b:X2}{a:X2}";
    }

    /// <summary>Clareia uma cor hex por um fator [0,1].</summary>
    public static string Lighten(string hex, double factor)
    {
        var (r, g, b) = HexToRgb(hex);
        return RgbToHex(
            (byte)Math.Min(255, r + (255 - r) * factor),
            (byte)Math.Min(255, g + (255 - g) * factor),
            (byte)Math.Min(255, b + (255 - b) * factor)
        );
    }

    /// <summary>Escurece uma cor hex por um fator [0,1].</summary>
    public static string Darken(string hex, double factor)
    {
        var (r, g, b) = HexToRgb(hex);
        return RgbToHex(
            (byte)(r * (1 - factor)),
            (byte)(g * (1 - factor)),
            (byte)(b * (1 - factor))
        );
    }

    /// <summary>Retorna true se a cor é "escura" (para decidir cor do texto).</summary>
    public static bool IsDark(string hex)
    {
        var (r, g, b) = HexToRgb(hex);
        // Luminância relativa (fórmula WCAG)
        var luminance = 0.299 * r + 0.587 * g + 0.114 * b;
        return luminance < 128;
    }

    /// <summary>Retorna branco ou preto, dependendo do contraste com a cor de fundo.</summary>
    public static string ContrastColor(string backgroundHex)
        => IsDark(backgroundHex) ? "#FFFFFF" : "#000000";

    /// <summary>Interpola entre duas cores hex por um fator t [0,1].</summary>
    public static string Lerp(string fromHex, string toHex, double t)
    {
        t = t.Clamp(0, 1);
        var (r1, g1, b1) = HexToRgb(fromHex);
        var (r2, g2, b2) = HexToRgb(toHex);
        return RgbToHex(
            (byte)(r1 + (r2 - r1) * t),
            (byte)(g1 + (g2 - g1) * t),
            (byte)(b1 + (b2 - b1) * t)
        );
    }
}

#endregion

#region StringTokenizer

/// <summary>
/// Utilitário simples para parsing de templates de string com tokens {chave}.
/// Útil para mensagens de notificação e strings localizadas.
/// </summary>
public static class StringTokenizer
{
    private static readonly Regex TokenPattern = new(@"\{(\w+)\}", RegexOptions.Compiled);

    /// <summary>
    /// Substitui tokens do tipo {chave} pelos valores do dicionário.
    /// Tokens sem correspondência são mantidos como estão.
    /// </summary>
    public static string Resolve(string template, IReadOnlyDictionary<string, string> values)
    {
        return TokenPattern.Replace(template, match =>
        {
            var key = match.Groups[1].Value;
            return values.TryGetValue(key, out var value) ? value : match.Value;
        });
    }

    /// <summary>Retorna os tokens encontrados no template.</summary>
    public static IReadOnlyList<string> ExtractTokens(string template)
        => TokenPattern.Matches(template)
            .Select(m => m.Groups[1].Value)
            .Distinct()
            .ToList();

    /// <summary>Retorna true se o template contém pelo menos um token.</summary>
    public static bool HasTokens(string template)
        => TokenPattern.IsMatch(template);
}

#endregion

#region ProductivityCalculator

/// <summary>
/// Cálculos e métricas de produtividade do FocusVisk.
/// Centraliza a lógica de pontuação, metas e estimativas.
/// </summary>
public static class ProductivityCalculator
{
    /// <summary>
    /// Calcula o score de produtividade do dia (0–100) com base em sessões concluídas
    /// e meta diária.
    /// </summary>
    public static int DailyScore(int completedSessions, int dailyGoal)
    {
        if (dailyGoal <= 0) return 0;
        return (int)Math.Min(100, (completedSessions / (double)dailyGoal) * 100);
    }

    /// <summary>
    /// Estima o tempo necessário para concluir N sessões Pomodoro,
    /// incluindo pausas curtas e uma pausa longa ao final do ciclo.
    /// </summary>
    public static TimeSpan EstimateTotalTime(int sessions, int focusMinutes, int shortBreakMinutes, int longBreakMinutes, int sessionsBeforeLongBreak)
    {
        var totalFocus = sessions * focusMinutes;
        var shortBreaks = Math.Max(0, sessions - 1) - (sessions - 1) / sessionsBeforeLongBreak;
        var longBreaks = (sessions - 1) / sessionsBeforeLongBreak;
        return TimeSpan.FromMinutes(totalFocus + shortBreaks * shortBreakMinutes + longBreaks * longBreakMinutes);
    }

    /// <summary>
    /// Calcula a taxa de conclusão de tarefas (0.0 a 1.0).
    /// </summary>
    public static double TaskCompletionRate(int completed, int total)
        => total <= 0 ? 0 : Math.Min(1.0, completed / (double)total);

    /// <summary>
    /// Retorna o milestone de streak correspondente ao valor atual.
    /// </summary>
    public static StreakMilestone GetMilestone(int streak) => streak switch
    {
        >= FocusViskConstants.StreakMilestoneObsidian => StreakMilestone.Obsidian,
        >= FocusViskConstants.StreakMilestoneDiamond => StreakMilestone.Diamond,
        >= FocusViskConstants.StreakMilestoneGold => StreakMilestone.Gold,
        >= FocusViskConstants.StreakMilestoneSilver => StreakMilestone.Silver,
        >= FocusViskConstants.StreakMilestoneBronze => StreakMilestone.Bronze,
        _ => StreakMilestone.None,
    };

    /// <summary>
    /// Retorna quantos dias faltam para o próximo milestone.
    /// </summary>
    public static int DaysToNextMilestone(int streak)
    {
        if (streak < FocusViskConstants.StreakMilestoneBronze) return FocusViskConstants.StreakMilestoneBronze - streak;
        if (streak < FocusViskConstants.StreakMilestoneSilver) return FocusViskConstants.StreakMilestoneSilver - streak;
        if (streak < FocusViskConstants.StreakMilestoneGold) return FocusViskConstants.StreakMilestoneGold - streak;
        if (streak < FocusViskConstants.StreakMilestoneDiamond) return FocusViskConstants.StreakMilestoneDiamond - streak;
        if (streak < FocusViskConstants.StreakMilestoneObsidian) return FocusViskConstants.StreakMilestoneObsidian - streak;
        return 0; // já no máximo
    }
}

#endregion