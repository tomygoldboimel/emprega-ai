# EmpregaAI
## Plataforma de Inclusão Produtiva e Gestão de Currículos
O EmpregaAI é uma aplicação full-stack projetada para converter relatos de experiências informais em currículos profissionais estruturados. O sistema foca em acessibilidade, utilizando tecnologias de voz e inteligência artificial para auxiliar usuários em situação de informalidade a ingressar no mercado formal de trabalho.


## Stack Técnica
- Frontend: Vue.js 3 e TypeScript
- Backend: ASP.NET Core e Entity Framework Core.
- Banco de Dados: PostgreSQL.
- IA e NLP: Groq Cloud API para processamento de linguagem natural e estruturação de seções profissionais.
- Acessibilidade: Web Speech API para funcionalidade de Speech-to-Text.

## Diferenciais Técnicos e Arquitetura
- Interface de Voz: Implementação de entrada de dados via voz para reduzir barreiras de uso por pessoas com baixa literacia.
- Processamento de Dados: Integração com LLM para categorização automática de competências e objetivos otimizados para sistemas de recrutamento (ATS).
- Persistência Segura: Estruturação de dados via EF Core com foco em integridade e segurança das informações do usuário.
- Arquitetura de Software: Organização do backend seguindo separação de responsabilidades entre Controladores, Serviços e Camada de Dados.

## Prints de Execução
<img width="381" height="775" alt="image" src="https://github.com/user-attachments/assets/c83c757f-d16c-4774-bb67-0958e05d1c5e" />
<img width="625" height="774" alt="image" src="https://github.com/user-attachments/assets/18879800-db23-4418-b691-3b255f4d148a" />
<img width="472" height="773" alt="image" src="https://github.com/user-attachments/assets/326a8e10-2bc5-42c8-b9aa-942ea309b4c7" />
<img width="493" height="774" alt="image" src="https://github.com/user-attachments/assets/c3930d5e-fc25-40a5-b919-60e7d2bb1979" />

## Exemplo de Currículo Base Gerado
<img width="555" height="774" alt="image" src="https://github.com/user-attachments/assets/22b157df-3ee2-47a1-9aeb-4c1a2b6d38ec" />

## Instalação e Configuração

### Requisitos
- .NET 8 SDK
- Node.js (v18+)
- PostgreSQL

### Configuração do Backend
I. Navegue até o diretório EmpregaAPI.

II. Execute o comando para restaurar as dependências: dotnet restore.

III. Configure as credenciais de banco de dados e as chaves de autenticação no arquivo appsettings.json.

IV. Atualize o banco de dados com as migrações existentes: dotnet ef database update.

V. Inicie a aplicação: dotnet run.

### Configuração do Frontend
I. Navegue até o diretório EmpregaAIVue.

II. Instale as dependências: npm install.

III. Crie um arquivo .env na raiz deste diretório e insira sua chave de API: VITE_GROQ_API_KEY=sua_chave_aqui.

IV. Inicie o servidor de desenvolvimento: npm run dev.

## Estrutura de Diretórios
- EmpregaAPI: Contém a lógica de negócio, modelos de dados, controladores REST e migrações do banco de dados.
- EmpregaAIVue: Contém a interface do usuário, componentes reutilizáveis, gerenciamento de estado e serviços de integração com a API.
