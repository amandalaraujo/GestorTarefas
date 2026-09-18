# Gestor de Tarefas e Compliance (GRC Lite)

Aplicação web desenvolvida para a gestão e acompanhamento de tarefas, processos internos e checklists de conformidade (compliance). O objetivo do projeto é automatizar a organização de fluxos de trabalho e garantir o cumprimento de prazos.

---

## Tecnologias Utilizadas

### **Backend**
* **C# / .NET 8 (Web API)** — Construção de endpoints RESTful.
* **Entity Framework Core** — ORM para mapeamento e manipulação de dados.
* **SQLite** — Banco de dados relacional leve para armazenamento local.
* **Swagger / OpenAPI** — Documentação e teste interativo da API.

### **Frontend**
* **HTML5 & CSS3** — Interface responsiva e estilizada.
* **JavaScript Vanilla** — Consumo da API via Fetch API e manipulação dinâmica do DOM.

---

## Funcionalidades

- [x] **Cadastro de Tarefas:** Registro com título, descrição, prioridade (Baixa, Normal, Alta) e prazo de conclusão.
- [x] **Listagem Dinâmica:** Exibição visual diferenciada por nível de prioridade.
- [x] **Exclusão de Tarefas:** Remoção de itens com atualização em tempo real.
- [x] **Persistência de Dados:** Integração completa com banco SQLite via Entity Framework Core.
- [x] **CORS Configurado:** Comunicação fluida entre a API e o Frontend.

---

## Como Executar o Projeto Localmente

### **Pré-requisitos**
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download) instalado.
* Extensão **Live Server** no VS Code (opcional, para rodar o frontend).
