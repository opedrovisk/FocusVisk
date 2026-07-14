# FocusVisk — Desktop

Aplicação **desktop** de produtividade pessoal desenvolvida com **WPF + Blazor Hybrid (.NET 8)**, com foco em gestão de tarefas, hábitos, Pomodoro, calendário, notas rápidas, controle financeiro e bloqueio de sites distratores durante sessões de foco.

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
- [Observações Importantes](#observações-importantes)

---

## Apresentação

O **FocusVisk** é uma ferramenta de produtividade pessoal para Windows que centraliza tudo o que o usuário precisa para manter o foco: um timer Pomodoro com bloqueio automático de sites, gerenciamento de tarefas com prioridades, subtarefas e recorrência, um rastreador de hábitos com streaks, calendário com anotações, notas rápidas fixáveis, controle financeiro com metas de economia e um dashboard com estatísticas de desempenho da semana.

A interface é construída em **Blazor Hybrid** renderizada dentro de um `WebView2` no WPF, o que permite um front-end web moderno (HTML/CSS) rodando como aplicação nativa sem depender de navegador externo. A aplicação também roda minimizada na bandeja do sistema, com notificações nativas do Windows para lembretes de tarefas agendadas.

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
| Notificações | Windows App Notifications (Toast nativo) |
| MVVM | CommunityToolkit.Mvvm |
| Ícones | Font Awesome 6 (local, offline) |
| Gráficos | Chart.js (linha e rosca na aba Finanças) |
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
    │   └── AppDbContext.cs        # DbContext (EF Core) + factory de design-time; seed de AppSettings
    ├── Models/
    │   ├── AppModels.cs           # CalendarNote, PomodoroSession, QuickNote, AppSettings, Habit, HabitLog
    │   ├── FinanceTransaction.cs  # FinancaTransaction, SavingGoal e enums de tipo/categoria
    │   └── TodoItem.cs            # Tarefa com Priority, recorrência, subtarefas e horário agendado
    ├── Native/
    │   └── ToastRegistration.cs   # Registro do AppUserModelID para notificações nativas do Windows
    ├── Pages/
    │   ├── Dashboard.razor        # Estatísticas, gráfico semanal, tarefas pendentes
    │   ├── TodoPage.razor         # Lista de tarefas com filtros, prioridades e subtarefas
    │   ├── PomodoroPage.razor     # Timer Pomodoro com controle de fase
    │   ├── CalendarPage.razor     # Calendário mensal com anotações por dia
    │   ├── NotesPage.razor        # Notas rápidas fixáveis
    │   ├── HabitsPage.razor       # Rastreador de hábitos com streaks e histórico mensal
    │   ├── FinancasPage.razor     # Transações, metas de economia, gráficos e calculadora
    │   └── SettingsPage.razor     # Configurações de Pomodoro, tema, bloqueio e inicialização
    ├── Services/
    │   ├── TaskService.cs         # CRUD de tarefas, subtarefas, recorrência e reset diário
    │   ├── PomodoroService.cs     # Timer com fases e persistência de sessões
    │   ├── CalendarService.cs     # CRUD de anotações de calendário
    │   ├── NotesService.cs        # CRUD de notas rápidas
    │   ├── HabitService.cs        # CRUD de hábitos, logs diários e cálculo de streak
    │   ├── FinancasService.cs     # CRUD de transações e metas de economia
    │   ├── FocusBlockerService.cs # Bloqueio de sites via arquivo hosts do Windows
    │   ├── AlertService.cs        # Verificação periódica e disparo de notificações de tarefas agendadas
    │   ├── StartupService.cs      # Ativa/desativa inicialização automática com o Windows
    │   ├── StatsService.cs        # Estatísticas e streak do dashboard
    │   └── ThemeService.cs        # Persistência e aplicação de tema/cores
    ├── Shared/
    │   └── Sidebar.razor          # Navegação lateral
    ├── wwwroot/
    │   ├── css/app.css            # Estilos globais e variáveis CSS
    │   ├── js/app.js              # Interoperabilidade JS e gráficos (Chart.js) da aba Finanças
    │   ├── index.html             # Host da aplicação Blazor
    │   └── lib/fontawesome/       # Font Awesome 6 (offline)
    ├── App.xaml / App.xaml.cs     # Bootstrap, DI, configuração do DbContext e inicialização em segundo plano
    ├── MainWindow.xaml            # Janela principal com BlazorWebView e ícone de bandeja
    └── AppComponent.razor         # Componente raiz Blazor (Router)
```
---

## Serviços

### `TaskService`
CRUD completo de tarefas (`TodoItem`), incluindo subtarefas e tarefas recorrentes (diária, dias úteis, semanal ou mensal). Calcula em quais dias uma tarefa recorrente deve aparecer no calendário e reseta automaticamente as tarefas recorrentes concluídas quando chega uma nova ocorrência. Expõe o evento `OnChanged` para atualização reativa da UI. Cria um novo scope de `DbContext` por operação, compatível com o ciclo de vida `Transient` do contexto.

### `PomodoroService`
Timer baseado em `System.Timers.Timer` com controle de fases (Focus → ShortBreak → LongBreak). Ao concluir uma sessão de foco, persiste automaticamente um `PomodoroSession` no banco. Expõe eventos `OnTick` e `OnPhaseChanged` para o componente Blazor.

### `CalendarService`
CRUD de `CalendarNote` com queries por mês e por dia. Expõe `OnChanged` para reatividade.

### `NotesService`
CRUD de `QuickNote` com suporte a fixação (`IsPinned`). Ordenação: fixadas primeiro, depois por `UpdatedAt` decrescente.

### `HabitService`
CRUD de hábitos (`Habit`) e seus registros diários (`HabitLog`). Calcula streak de dias consecutivos, total de conclusões nos últimos 30 dias e histórico de um hábito para um mês específico.

### `FinancasService`
CRUD completo de transações (`FinancaTransaction`) e metas de economia (`SavingGoal`), com consultas por mês e cálculo do saldo total e do saldo do período. Uma meta salva para um mês que já possui meta cadastrada atualiza o registro existente em vez de duplicá-lo. Expõe `OnChanged` para reatividade da UI, seguindo o mesmo padrão de scope por operação dos demais serviços.

### `FocusBlockerService`
Bloqueia e desbloqueia sites distratores manipulando o arquivo `C:\Windows\System32\drivers\etc\hosts`. Insere um bloco demarcado com comentários (`# === FOCUS FocusVisk START/END ===`) para isolamento seguro. Requer execução como **Administrador** para escrever no arquivo.

### `AlertService`
Roda em segundo plano verificando periodicamente tarefas com horário agendado (`ScheduledTime`) e dispara notificações nativas (toast) do Windows quando chega o horário, incluindo tarefas recorrentes.

### `StartupService`
Ativa ou desativa a inicialização automática do FocusVisk junto com o Windows, gravando/removendo a entrada correspondente no registro (`HKCU...Run`). Quando ativado, o app abre direto minimizado na bandeja do sistema, sem exibir a janela.

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
- Cadastro com título, descrição, prioridade (`Low / Medium / High`), tag, prazo e subtarefas
- Recorrência diária, em dias úteis, semanal ou mensal, com exibição opcional no calendário
- Horário agendado com notificação nativa do Windows no momento configurado
- Filtros por status (todas, pendentes, concluídas) e prioridade
- Toggle de conclusão (inclusive de subtarefas) e exclusão
- Reset automático de tarefas recorrentes concluídas ao chegar uma nova ocorrência

**Hábitos**
- Cadastro de hábitos com ícone e cor customizáveis
- Marcação diária de conclusão, com cálculo de streak de dias consecutivos
- Histórico visual por mês e contagem de conclusões nos últimos 30 dias
- Arquivamento e exclusão de hábitos

**Pomodoro**
- Timer configurável com fases de foco, pausa curta e pausa longa
- Skip e reset de fase
- Persistência automática de sessões concluídas
- Integração com `FocusBlockerService` para bloquear sites durante o foco

**Calendário**
- Navegação mensal
- Anotações por dia com cor customizável
- Exibição de tarefas (inclusive recorrentes) nos dias em que ocorrem

**Notas Rápidas**
- Criação e edição inline
- Fixação de notas ao topo

**Finanças**
- Lançamento de entradas e saídas com título, valor, categoria e descrição opcional
- Cards de saldo total, entradas do mês, saídas do mês e saldo líquido do mês
- Meta de economia mensal com barra de progresso e aviso ao atingir o valor definido
- Gráficos de evolução do saldo (últimos 7 dias) e de gastos por categoria no mês
- Histórico de transações com filtros (tudo, entradas, saídas, mês atual) e exclusão
- Calculadora rápida embutida (botão flutuante) para conferência de valores

**Configurações**
- Duração das fases do Pomodoro
- Ativar/desativar bloqueio de sites e editar lista de sites bloqueados
- Alternância entre tema claro e escuro
- Customização de cor de destaque e cores de fundo
- Ativar/desativar inicialização automática com o Windows (em segundo plano, direto na bandeja)

**System Tray**
- Aplicação minimiza para a bandeja do sistema
- Menu de contexto com opções de restauração e encerramento
- Ao iniciar com o Windows, abre direto minimizado, sem exibir a janela

**Notificações**
- Toasts nativos do Windows para tarefas com horário agendado, com som e suporte a recorrência

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
- **Finanças:** valores monetários são armazenados como `decimal(18,2)` e exibidos no formato brasileiro (R$, cultura `pt-BR`). A meta de economia é única por mês/ano — salvar uma nova meta em um mês que já possui uma existente atualiza o valor em vez de criar um novo registro.
- **Notificações:** exigem que o app esteja registrado como fonte de notificações do Windows (feito automaticamente na inicialização via `ToastRegistration`); em alguns casos pode ser necessário permitir notificações do FocusVisk nas configurações do Windows.
- **Inicialização automática:** ao ativar "Iniciar com o Windows" nas configurações, o app é registrado no `Run` do registro com um argumento que faz com que ele abra direto em segundo plano, sem exibir a janela principal.
- **WebView2:** o `Microsoft.AspNetCore.Components.WebView.Wpf` inclui o runtime do WebView2 automaticamente via NuGet; não é necessário instalar separadamente.
- **Font Awesome:** os ícones são carregados localmente a partir de `wwwroot/lib/fontawesome`, sem dependência de CDN externo.
- **EM DESENVOLVIMENTO, O PROJETO FOI IDEALIZADO PARA AUXILIAR NO MEU APRENDIZADO, AINDA RECEBERÁ NOVAS IMPLEMENTAÇÕES FUTURAMENTE.**
