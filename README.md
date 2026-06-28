# FocusVisk — Desktop

Aplicação **desktop** de produtividade pessoal desenvolvida com **WPF + Blazor Hybrid (.NET 8)**, com foco em gestão de tarefas, Pomodoro, calendário, notas rápidas e bloqueio de sites distratores durante sessões de foco.

> 🖥️ **Branch atual: `Desktop`** — versão nativa para Windows.
> Uma branch `Web` está planejada e compartilhará as camadas `Core` e `Application` desta mesma solução.

![Preview do FocusVisk](./screenshot.png)

---

## Sumário

- [Apresentação](#apresentação)
- [Branches do Projeto](#branches-do-projeto)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Modelos de Dados](#modelos-de-dados)
- [Serviços](#serviços)
- [Funcionalidades](#funcionalidades)
- [Pré-requisitos](#pré-requisitos)
- [Configuração e Execução](#configuração-e-execução)
- [Observações Importantes](#observações-importantes)

---

## Apresentação

O **FocusVisk** é uma ferramenta de produtividade pessoal para Windows que centraliza tudo o que o usuário precisa para manter o foco: um timer Pomodoro com bloqueio automático de sites, gerenciamento de tarefas com prioridades, calendário com anotações, notas rápidas fixáveis e um dashboard com estatísticas de desempenho da semana.

A interface é construída em **Blazor Hybrid** renderizada dentro de um `WebView2` no WPF, o que permite um front-end web moderno (HTML/CSS) rodando como aplicação nativa sem depender de navegador externo.

---

## Branches do Projeto

| Branch | Plataforma | Status | Stack principal |
|--------|------------|--------|-----------------|
| `Desktop` | Windows (WPF) | ✅ Em desenvolvimento | WPF + Blazor Hybrid + SQL Server |
| `Web` *(planejado)* | Navegador | 🔜 Futuro | ASP.NET Core MVC + Razor + Vue.js 3 |

As camadas `Core` e `Application` serão compartilhadas entre as duas versões, garantindo que a lógica de negócio não seja duplicada.

---

## Tecnologias

| Camada | Tecnologia |
|--------|-----------|
| Framework | .NET 8 (WPF) |
| UI | Blazor Hybrid (`Microsoft.AspNetCore.Components.WebView.Wpf`) |
| ORM | Entity Framework Core 8 (Code First + Migrations) |
| Banco de dados | SQL Server / LocalDB |
| System Tray | H.NotifyIcon.Wpf |
| MVVM | CommunityToolkit.Mvvm |
| Ícones | Font Awesome 6 (local, offline) |
| Arquitetura | Layered (Core · Application · Infrastructure · WPF) |

---

## Estrutura do Projeto

```
FocusVisk/
├── FocusVisk.Core/               # Entidades e interfaces (futura extração)
│   ├── Models/
│   ├── Services/
│   └── Data/
├── FocusVisk.UI/                 # Componentes Blazor reutilizáveis (futuro)
│   ├── Components/
│   ├── Pages/
│   ├── Shared/
│   └── wwwroot/
└── FocusVisk.WPF/                # Projeto principal — aplicação desktop
    ├── Data/
    │   ├── AppDbContext.cs        # DbContext (EF Core); seed de AppSettings
    │   └── AppDbContextFactory.cs # Factory para design-time (migrations)
    ├── Models/
    │   ├── AppModels.cs           # CalendarNote, PomodoroSession, QuickNote, AppSettings
    │   └── TodoItem.cs            # Tarefa com Priority enum
    ├── Pages/
    │   ├── Dashboard.razor        # Estatísticas, gráfico semanal, tarefas pendentes
    │   ├── TodoPage.razor         # Lista de tarefas com filtros e prioridades
    │   ├── PomodoroPage.razor     # Timer Pomodoro com controle de fase
    │   ├── CalendarPage.razor     # Calendário mensal com anotações por dia
    │   ├── NotesPage.razor        # Notas rápidas fixáveis
    │   └── SettingsPage.razor     # Configurações de Pomodoro, tema e bloqueio
    ├── Services/
    │   ├── TaskService.cs         # CRUD de tarefas
    │   ├── PomodoroService.cs     # Timer com fases e persistência de sessões
    │   ├── CalendarService.cs     # CRUD de anotações de calendário
    │   ├── NotesService.cs        # CRUD de notas rápidas
    │   ├── FocusBlockerService.cs # Bloqueio de sites via arquivo hosts do Windows
    │   ├── StatsService.cs        # Estatísticas e streak do dashboard
    │   └── ThemeService.cs        # Persistência e aplicação de tema/cores
    ├── Shared/
    │   └── Sidebar.razor          # Navegação lateral
    ├── wwwroot/
    │   ├── css/app.css            # Estilos globais e variáveis CSS
    │   ├── js/app.js              # Interoperabilidade JS
    │   ├── index.html             # Host da aplicação Blazor
    │   └── lib/fontawesome/       # Font Awesome 6 (offline)
    ├── App.xaml / App.xaml.cs     # Bootstrap, DI e configuração do DbContext
    ├── MainWindow.xaml            # Janela principal com BlazorWebView
    └── AppComponent.razor         # Componente raiz Blazor (Router)
```

---

## Modelos de Dados

### `TodoItem`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | `int` | Chave primária |
| `Title` | `string` | Título da tarefa |
| `Description` | `string?` | Descrição opcional |
| `IsCompleted` | `bool` | Status de conclusão |
| `Priority` | `Priority` | `Low`, `Medium`, `High` |
| `Tag` | `string?` | Etiqueta livre |
| `CreatedAt` | `DateTime` | Data de criação |
| `CompletedAt` | `DateTime?` | Data de conclusão |
| `DueDate` | `DateTime?` | Prazo |

### `PomodoroSession`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | `int` | Chave primária |
| `StartedAt` | `DateTime` | Início da sessão |
| `CompletedAt` | `DateTime?` | Fim da sessão |
| `DurationMinutes` | `int` | Duração configurada |
| `WasCompleted` | `bool` | Sessão completada sem interrupção |
| `TaskTitle` | `string?` | Tarefa associada à sessão |

### `CalendarNote`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | `int` | Chave primária |
| `Date` | `DateTime` | Data da anotação |
| `Content` | `string` | Conteúdo |
| `Color` | `string?` | Cor de destaque (hex, default `#7C6AF7`) |
| `CreatedAt` | `DateTime` | Data de criação |

### `QuickNote`

| Campo | Tipo | Descrição |
|-------|------|-----------|
| `Id` | `int` | Chave primária |
| `Title` | `string` | Título da nota |
| `Content` | `string` | Conteúdo |
| `IsPinned` | `bool` | Nota fixada ao topo |
| `CreatedAt` | `DateTime` | Data de criação |
| `UpdatedAt` | `DateTime` | Última atualização |

### `AppSettings`

Registro único (Id = 1) com todas as preferências persistidas do usuário.

| Campo | Tipo | Padrão |
|-------|------|--------|
| `PomodoroDurationMinutes` | `int` | `25` |
| `ShortBreakMinutes` | `int` | `5` |
| `LongBreakMinutes` | `int` | `15` |
| `SessionsBeforeLongBreak` | `int` | `4` |
| `PlaySounds` | `bool` | `true` |
| `ShowNotifications` | `bool` | `true` |
| `FocusBlockEnabled` | `bool` | `false` |
| `BlockedSites` | `string` | youtube.com, x.com, instagram.com, reddit.com |
| `Theme` | `string` | `"dark"` |
| `AccentColor` | `string` | `"#7C6AF7"` |

---

## Serviços

### `TaskService`
CRUD completo de tarefas (`TodoItem`). Expõe o evento `OnChanged` para atualização reativa da UI. Cria um novo scope de `DbContext` por operação, compatível com o ciclo de vida `Transient` do contexto.

### `PomodoroService`
Timer baseado em `System.Timers.Timer` com controle de fases (Focus → ShortBreak → LongBreak). Ao concluir uma sessão de foco, persiste automaticamente um `PomodoroSession` no banco. Expõe eventos `OnTick` e `OnPhaseChanged` para o componente Blazor.

### `CalendarService`
CRUD de `CalendarNote` com queries por mês e por dia. Expõe `OnChanged` para reatividade.

### `NotesService`
CRUD de `QuickNote` com suporte a fixação (`IsPinned`). Ordenação: fixadas primeiro, depois por `UpdatedAt` decrescente.

### `FocusBlockerService`
Bloqueia e desbloqueia sites distratores manipulando o arquivo `C:\Windows\System32\drivers\etc\hosts`. Insere um bloco demarcado com comentários (`# === FOCUS FocusVisk START/END ===`) para isolamento seguro. Requer execução como **Administrador** para escrever no arquivo.

### `StatsService`
Agrega estatísticas para o dashboard: sessões do dia, sessões da semana, tarefas concluídas hoje, tarefas pendentes, streak de dias consecutivos (até 365 dias) e gráfico de barras dos últimos 7 dias.

### `ThemeService`
Persiste e aplica preferências visuais (tema claro/escuro/customizado, cor de destaque, cores de fundo). Gera as variáveis CSS dinâmicas injetadas na aplicação Blazor via `BuildCssVariables()`.

---

## Funcionalidades

**Dashboard**
- Cards com sessões do dia, streak de dias seguidos, tarefas concluídas hoje e tarefas pendentes
- Gráfico de barras das sessões Pomodoro dos últimos 7 dias
- Listagem rápida de tarefas pendentes

**Tarefas**
- Cadastro com título, descrição, prioridade (`Low / Medium / High`), tag e prazo
- Filtros por status (todas, pendentes, concluídas) e prioridade
- Toggle de conclusão e exclusão

**Pomodoro**
- Timer configurável com fases de foco, pausa curta e pausa longa
- Skip e reset de fase
- Persistência automática de sessões concluídas
- Integração com `FocusBlockerService` para bloquear sites durante o foco

**Calendário**
- Navegação mensal
- Anotações por dia com cor customizável

**Notas Rápidas**
- Criação e edição inline
- Fixação de notas ao topo

**Configurações**
- Duração das fases do Pomodoro
- Ativar/desativar bloqueio de sites e editar lista de sites bloqueados
- Alternância entre tema claro e escuro
- Customização de cor de destaque e cores de fundo

**System Tray**
- Aplicação minimiza para a bandeja do sistema
- Menu de contexto com opções de restauração e encerramento

---

## Pré-requisitos

- **Windows 10/11** (x64)
- **.NET 8 SDK** — [download](https://dotnet.microsoft.com/download/dotnet/8)
- **Visual Studio 2022** (17.8+) com workload *ASP.NET and web development* e *Windows desktop development*
- **SQL Server** ou **SQL Server Express LocalDB** (incluído com o Visual Studio)

---

## Configuração e Execução

### 1. Clone o repositório e selecione a branch Desktop

```bash
git clone https://github.com/opedrovisk/FocusVisk.git
cd FocusVisk
git checkout Desktop
```

### 2. Configure a connection string

Abra `FocusVisk.WPF/App.xaml.cs` e ajuste a connection string conforme seu ambiente:

```csharp
// LocalDB (padrão — zero configuração com Visual Studio)
options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=FocusViskDb;Trusted_Connection=True;");

// SQL Server Express
options.UseSqlServer("Server=.\\SQLEXPRESS;Database=FocusViskDb;Trusted_Connection=True;");
```

### 3. Aplique as migrations

No **Package Manager Console** do Visual Studio (com `FocusVisk.WPF` como Default Project):

```powershell
Add-Migration InitialCreate -Project FocusVisk.WPF
Update-Database -Project FocusVisk.WPF
```

Ou via CLI:

```bash
dotnet ef migrations add InitialCreate --project FocusVisk.WPF
dotnet ef database update --project FocusVisk.WPF
```

### 4. Execute o projeto

Defina `FocusVisk.WPF` como projeto de inicialização e pressione **F5** no Visual Studio.

---

## Observações Importantes

- **Bloqueio de sites:** o `FocusBlockerService` manipula o arquivo `hosts` do Windows e requer que o aplicativo seja executado como **Administrador**. Sem permissão elevada, o bloqueio é ignorado com uma exceção tratada.
- **Banco de dados:** o schema é criado/migrado automaticamente na inicialização via `db.Database.Migrate()`. Migrations novas são aplicadas sem perda de dados existentes.
- **WebView2:** o `Microsoft.AspNetCore.Components.WebView.Wpf` inclui o runtime do WebView2 automaticamente via NuGet; não é necessário instalar separadamente.
- **Font Awesome:** os ícones são carregados localmente a partir de `wwwroot/lib/fontawesome`, sem dependência de CDN externo.
