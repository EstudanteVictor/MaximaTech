📘 Documento de Instalação e Arquitetura - MaximaTech
1. Visão Geral
O sistema MaximaTech é composto por dois módulos principais:
- Backend: API desenvolvida em ASP.NET Core integrada ao banco de dados PostgreSQL.
- Frontend: Aplicação desenvolvida em Angular para interação com o usuário.

Este documento descreve o processo de instalação do sistema, incluindo a configuração do banco, execução do backend e frontend, além de apresentar o desenho arquitetural do sistema.
2. Pré-requisitos
Backend:
- .NET 7 SDK ou superior
- PostgreSQL 14+
- Git

Frontend:
- Node.js (>= 18 LTS)
- Angular CLI instalado globalmente (npm install -g @angular/cli)
3. Configuração do Banco de Dados
1. Criar banco de dados no PostgreSQL:
   CREATE DATABASE ecommerce_db;

2. Ajustar a connection string no arquivo Program.cs do backend.
3. Rodar a aplicação para inicializar as tabelas automaticamente.
O sistema criará automaticamente as seguintes tabelas:
•	produtos: Armazena informações dos produtos
•	departamentos: Lista de departamentos disponíveis

4. Instalação e Execução do Backend
cd MaximaTech/MaximaBackend
dotnet restore
dotnet build
dotnet run

A API estará disponível em: https://localhost:7251/index.html
por meio do swagger
Estrutura de pastas da API:
ECommerceAPI/
├── Controllers/
│   ├── ProdutoController.cs
│   └── DepartamentoController.cs
├── Models/
│   ├── Produto.cs
│   └── Departamento.cs
├── Services/
│   ├── IProdutoService.cs
│   ├── ProdutoService.cs
│   ├── IDepartamentoService.cs
│   └── DepartamentoService.cs
├── Data/
│   └── DatabaseInitializer.cs
├── Program.cs
└── appsettings.json
API Endpoints:
Departamento
•	GET - /api/Departamento
Produto
•	GET -   /api/Produto
•	POST - /api/Produto
•	GET - /api/Produto/{id}
•	PUT - /api/Produto/{id}
•	DELETE - /api/Produto/{id}
5. Instalação e Execução do Frontend
cd MaximaTech/frontend
npm install

Configurar o endpoint da API no arquivo environment.ts:
export const environment = {
  production: false,
  apiUrl: 'http://localhost: 7251/api'
};

Rodar a aplicação:
ng serve

Acesse no navegador: http://localhost:4200
Estrutura de pastas frontend:
src/
├── app/
│   ├── components/
│   │   ├── produto/
│   │   │   ├── produto-list/
│   │   │   └── produto-form/
│   │   └── shared/
│   │       └── confirm-dialog/
│   ├── models/
│   │   ├── produto.model.ts
│   │   └── departamento.model.ts
│   ├── services/
│   │   ├── produto.service.ts
│   │   └── departamento.service.ts
│   ├── app.component.ts
│   ├── app.component.html
│   ├── app.component.css
│   ├── app.config.ts
│   └── app.routes.ts
│   ├── app.spec.ts
└── styles.css

6. Testes Automatizados
Backend (xUnit/Moq):
cd MaximaTech/tests
dotnet test

Frontend (Jasmine/Karma):
cd MaximaTech/frontend
ng test
7. Arquitetura do Sistema
O sistema segue uma arquitetura em camadas:

- Frontend (Angular): responsável pela interface com o usuário.
- Backend (ASP.NET Core): expõe APIs REST organizadas em Controllers e Services.
- Banco de Dados (PostgreSQL): armazena as entidades de Produtos e Departamentos.

 

Fluxo: Usuário → Angular(15) → Controllers → Services → Interfaces → PostgreSQL → Resposta ao usuário.

