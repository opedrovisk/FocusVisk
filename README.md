# FocusVisk — Desktop

Aplicação **desktop** de produtividade pessoal desenvolvida com **WPF + Blazor Hybrid (.NET 8)**, com foco em gestão de tarefas, Pomodoro, calendário, notas rápidas e bloqueio de sites distratores durante sessões de foco.

> 🖥️ **Branch atual: `Desktop`** — versão nativa para Windows.
> Uma branch `Web` está planejada e compartilhará as camadas `Core` e `Application` desta mesma solução.

---

## Sumário

- [Apresentação](#apresentação)
- [Branches do Projeto](#branches-do-projeto)
- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
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
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/88dcda39-5b70-4f75-a12e-009ae9bfd80e" />

**Tarefas**
- Cadastro com título, descrição, prioridade (`Low / Medium / High`), tag e prazo
- Filtros por status (todas, pendentes, concluídas) e prioridade
- Toggle de conclusão e exclusão
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/a70c5139-2157-472e-aec2-a01bc07c0d5f" />

**Pomodoro**
- Timer configurável com fases de foco, pausa curta e pausa longa
- Skip e reset de fase
- Persistência automática de sessões concluídas
- Integração com `FocusBlockerService` para bloquear sites durante o foco
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/b91ce9a1-9548-470d-8bfb-f55f0904922e" />

**Calendário**
- Navegação mensal
- Anotações por dia com cor customizável
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/4f08be83-aa27-4dfa-809f-752bb4159586" />

**Notas Rápidas**
- Criação e edição inline
- Fixação de notas ao topo
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/510b1a05-93ca-49f7-9863-8f7f7fca45a5" />

**Configurações**
- Duração das fases do Pomodoro
- Ativar/desativar bloqueio de sites e editar lista de sites bloqueados
- Alternância entre tema claro e escuro
- Customização de cor de destaque e cores de fundo
<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/f6cc96e8-9896-4686-8ba4-0790f7db181a" />

**System Tray**
- Aplicação minimiza para a bandeja do sistema
- Menu de contexto com opções de restauração e encerramento
<img width="336" height="223" alt="image" src="https://github.com/user-attachments/assets/06de44b5-0fa2-4019-b340-ee9a5c179c03" />

---

## Pré-requisitos

- **Windows 10/11** (x64)
- **.NET 8 SDK** — [download](https://dotnet.microsoft.com/download/dotnet/8)
- **Visual Studio 2022** (17.8+) com workload *ASP.NET and web development* e *Windows desktop development*
- **SQL Server** ou **SQL Server Express LocalDB** (incluído com o Visual Studio)

---

## Observações Importantes

- **Bloqueio de sites:** o `FocusBlockerService` manipula o arquivo `hosts` do Windows e requer que o aplicativo seja executado como **Administrador**. Sem permissão elevada, o bloqueio é ignorado com uma exceção tratada.
- **Banco de dados:** o schema é criado/migrado automaticamente na inicialização via `db.Database.Migrate()`. Migrations novas são aplicadas sem perda de dados existentes.
- **WebView2:** o `Microsoft.AspNetCore.Components.WebView.Wpf` inclui o runtime do WebView2 automaticamente via NuGet; não é necessário instalar separadamente.
- **Font Awesome:** os ícones são carregados localmente a partir de `wwwroot/lib/fontawesome`, sem dependência de CDN externo.
- **EM DESENVOLVIMENTO, O PROJETO FOI IDEALIZADO PARA AUXILIAR NO MEU APRENDIZADO, AINDA RECEBERÁ NOVAS IMPLEMENTAÇÕES FUTURAMENTE.
