# Presentation Application

## Sumário

- [Sobre](#sobre)
- [Pré-requisitos](#pré-requisitos)
- [Instalação](#instalação)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Configuração](#configuração)
- [Execução](#execução)
- [Migrações do Entity Framework](#migrações-do-entity-framework)
- [Dependências](#dependências)
- [Uso](#uso)
- [Licença](#licença)

## Sobre

A **Presentation Application** é um projeto ASP.NET Core MVC que utiliza o template AdminLTE para a interface de usuário. Ele inclui funcionalidades de cadastro e consulta de clientes.

## Pré-requisitos

Antes de começar, certifique-se de ter os seguintes itens instalados:

- [.NET 6 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Instalação

### Clonar o Repositório

```bash
git clone https://github.com/wenklesleymendes/TesteUpd8.git
cd repo

Estrutura do Projeto

Presentation/
│
├── 1-Apresentacao
│   ├── Controllers
│   │   ├── ClienteController.cs
│   │   └── HomeController.cs
│   ├── Models
│   │   └── Cliente.cs
│   ├── Views
│   │   ├── Cliente
│   │   │   ├── Cadastro.cshtml
│   │   │   └── Consulta.cshtml
│   │   ├── Home
│   │   │   └── Index.cshtml
│   │   ├── Shared
│   │   │   ├── _Layout.cshtml
│   │   │   └── _ValidationScriptsPartial.cshtml
│   │   └── _ViewImports.cshtml
│   │   └── _ViewStart.cshtml
│   ├── wwwroot
│   │   ├── css
│   │   │   └── site.css
│   │   └── js
│   │       └── site.js
│
├── 2-Aplicacao
│   └── Services
│       └── ClienteService.cs
│
├── 3-Dominio
│   └── Entities
│       └── Cliente.cs
│
└── 4-InfraEstrutura
    └── Data
        └── ClienteContext.cs
