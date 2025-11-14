# EmpregaAI
## Sobre o Projeto

EmpregaAI é uma aplicação full-stack moderna projetada para simplificar e aprimorar a criação, edição e gerenciamento de currículos profissionais. A plataforma foi desenvolvida especialmente para auxiliar pessoas em situação de subemprego ou inseridas na informalidade do mercado de trabalho, ajudando-as a formatar seus currículos de forma profissional e informativa para os contratantes, aumentando suas chances de recolocação no mercado formal.


## Instalação

#### 1. Clone o repositório
```bash
git clone https://github.com/tomygoldboimel/emprega-ai.git
cd emprega-ai

```
#### 2. Configure o BackEnd (ASP.NET Core)
```bash
cd EmpregaAPI
dotnet restore
```
Crie o appsettings.json no diretório do backend
```bash
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=empregaai;Username=usuarioExemplo;Password=senhaExemplo"
  },
  "JwtSettings": {
    "SecretKey": "chaveSecretaExemplo",
    "Issuer": "EmpregaAI",
    "Audience": "EmpregaAI-Users",
    "ExpirationMinutes": 60
  }
}
```
Execute as migrations no terminal para criar o banco de dados
```bash
dotnet ef database update
```
Rode a aplicação
```bash
dotnet run
```
O backend estará rodando no http://localhost:5000

#### 3. Configure o FrontEnd (Vue.js + Typescript)
```bash
cd ../EmpregaAIVue
npm install
```
Crie o .env no diretório EmpregaAIVue com sua chave do grok para funcionamento do sistema de auxilio de descrição
```bash
VITE_GROQ_API_KEY=suaChaveGrok
```
OBS: Sua chave do groq não pode estar entre aspas


Inicie o servidor
```bash
npm run dev
```
O frontend estará rodando no http://localhost:5173

### Estrutura do Projeto
```bash
emprega-ai/
├── EmpregaAPI/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   ├── Data/
│   └── Migrations/
│
├── EmpregaAIVue/
│   ├── src/
│   │   ├── components/
│   │   ├── views/
│   │   ├── router/
│   │   ├── services/
│   │   └── store/
│   ├── .env
│   └── public/
│
└── README.md
```
