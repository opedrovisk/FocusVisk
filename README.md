# Focus Pedrovisk 🎯

App de produtividade pessoal para Windows — .NET 8 + Blazor Hybrid + WPF.

## Funcionalidades

- ✅ **To-Do List** — tarefas com prioridade, tags, data limite e filtros
- 🍅 **Pomodoro** — timer com anel animado, fases automáticas, histórico de sessões
- 📅 **Calendário** — anotações por dia com indicadores visuais
- 📝 **Notas rápidas** — notas com fixar/desafixar e grid de cards
- 📊 **Dashboard** — streak, gráfico semanal, tarefas pendentes
- 🚫 **Bloqueio de foco** — bloqueia sites pelo arquivo hosts durante o Pomodoro
- 🗕 **System Tray** — minimiza para a bandeja, menu de acesso rápido

## Pré-requisitos

- Windows 10 ou 11 (64-bit)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio 2022 Community (gratuito) com as workloads:
  - ✅ **Desenvolvimento de área de trabalho com .NET**
  - ✅ **Desenvolvimento Web e ASP.NET**

## Como rodar

### Visual Studio (recomendado)

1. Abra `FocusPedrovisk.sln`
2. Aguarde o NuGet restaurar os pacotes automaticamente
3. Pressione **F5** para compilar e rodar

### Linha de comando

```bash
cd FocusPedrovisk.WPF
dotnet restore
dotnet run
```

## Estrutura

```
FocusPedrovisk/
├── FocusPedrovisk.sln
├── README.md
└── FocusPedrovisk.WPF/
    ├── App.xaml / App.xaml.cs       ← Entrada do app, injeção de dependência
    ├── AppComponent.razor            ← Root Blazor: navegação entre páginas
    ├── MainWindow.xaml / .cs        ← Janela WPF + System Tray
    ├── _Imports.razor               ← Usings globais Blazor
    ├── app.manifest                 ← DPI awareness e permissões Windows
    │
    ├── Models/
    │   ├── TodoItem.cs              ← Tarefa com prioridade e tags
    │   └── AppModels.cs            ← CalendarNote, PomodoroSession, QuickNote, AppSettings
    │
    ├── Data/
    │   └── AppDbContext.cs          ← Entity Framework Core + SQLite
    │
    ├── Services/
    │   ├── PomodoroService.cs       ← Timer, fases (focus/short/long), eventos
    │   └── AppServices.cs          ← TaskService, CalendarService, NotesService,
    │                                  StatsService, FocusBlockerService
    ├── Pages/
    │   ├── Dashboard.razor          ← Estatísticas e visão geral
    │   ├── TodoPage.razor           ← Lista de tarefas CRUD
    │   ├── PomodoroPage.razor       ← Timer com anel SVG
    │   ├── CalendarPage.razor       ← Calendário + anotações por dia
    │   ├── NotesPage.razor          ← Notas rápidas com grid
    │   └── SettingsPage.razor       ← Configurações do app
    │
    ├── Shared/
    │   └── Sidebar.razor            ← Navegação lateral
    │
    └── wwwroot/
        ├── index.html               ← Host page do Blazor
        ├── css/app.css             ← Design system completo (dark theme)
        └── js/app.js              ← Helpers JavaScript
```

## Banco de dados

Criado automaticamente em:
```
%AppData%\FocusPedrovisk\focus.db
```
Zero configuração. Funciona 100% offline e local.

## Bloqueio de sites

Para usar, execute o `.exe` como **Administrador** (botão direito → "Executar como administrador").
O app edita o arquivo `hosts` do Windows para redirecionar os sites durante o Pomodoro.

## Expandindo o projeto

Sugestões de próximos passos:
- Notificações toast do Windows 10/11 ao fim de cada sessão
- Atalho global de teclado para iniciar/pausar o Pomodoro
- Export/backup do banco de dados
- Tema claro (light mode)
- Sincronização via arquivo JSON compartilhado

## Stack

| Camada | Tecnologia |
|--------|-----------|
| Framework | .NET 8 WPF |
| UI | Blazor Hybrid (Razor Components) |
| Estilo | CSS puro (dark design system próprio) |
| Banco | SQLite + Entity Framework Core 8 |
| System Tray | H.NotifyIcon.Wpf 2.1 |
| DI | Microsoft.Extensions.DependencyInjection |
