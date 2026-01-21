// Este arquivo contém exemplos da estrutura de dados esperada
// pelo handleResumeImport quando um currículo é processado

// Exemplo completo de dados processados de um currículo
export const exemploCurriculoProcessado = {
  dadosPessoais: {
    nomeCompleto: "João Silva Santos",
    dataNascimento: "1990-05-15T00:00:00.000Z", // ISO string
    telefone: "(11) 99999-9999",
    email: "joao.silva@email.com",
    endereco: "Rua das Flores, 123",
    cidade: "São Paulo",
    estado: "SP",
    linkedIn: "https://linkedin.com/in/joaosilvasantos",
    gitHub: "https://github.com/joaosilva"
  },
  experiencias: [
    {
      empresa: "Empresa Tech Ltda",
      cargo: "Desenvolvedor Full Stack",
      dataInicio: "2020-01-15T00:00:00.000Z",
      dataFim: "2023-12-31T00:00:00.000Z",
      empregoAtual: false,
      descricao: "Desenvolvimento de aplicações web usando Vue.js, Node.js e PostgreSQL. Liderança de equipe de 3 desenvolvedores."
    },
    {
      empresa: "Startup Inovadora",
      cargo: "Desenvolvedor Frontend",
      dataInicio: "2018-03-01T00:00:00.000Z",
      empregoAtual: true, // Quando empregoAtual é true, dataFim pode ser omitida
      descricao: "Criação de interfaces responsivas com React e TypeScript. Implementação de testes automatizados."
    }
  ],
  formacoes: [
    {
      instituicao: "Universidade Federal de São Paulo",
      curso: "Ciência da Computação",
      nivel: "Superior",
      status: "Concluído",
      dataInicio: "2014-02-01T00:00:00.000Z",
      dataConclusao: "2018-12-20T00:00:00.000Z"
    },
    {
      instituicao: "Centro Universitário SENAC",
      curso: "Especialização em Desenvolvimento Web",
      nivel: "Pós-graduação",
      status: "Cursando",
      dataInicio: "2023-08-01T00:00:00.000Z"
      // dataConclusao pode ser omitida se ainda não concluiu
    }
  ],
  certificacoes: [
    {
      nome: "AWS Certified Solutions Architect",
      instituicao: "Amazon Web Services",
      dataConclusao: "2023-06-15T00:00:00.000Z",
      cargaHoraria: 40,
      urlCertificado: "https://aws.amazon.com/certification/verify/123456"
    },
    {
      nome: "Scrum Master Certification",
      instituicao: "Scrum.org",
      dataConclusao: "2022-09-10T00:00:00.000Z",
      cargaHoraria: 16,
      urlCertificado: "https://scrum.org/certificates/789012"
    }
  ]
};

// Exemplo mínimo (apenas dados pessoais)
export const exemploMinimo = {
  dadosPessoais: {
    nomeCompleto: "Maria Oliveira",
    email: "maria@email.com"
  }
};

// Exemplo com apenas experiências
export const exemploApenasExperiencias = {
  experiencias: [
    {
      empresa: "Empresa ABC",
      cargo: "Analista de Sistemas",
      dataInicio: "2020-01-01T00:00:00.000Z",
      empregoAtual: true,
      descricao: "Análise e desenvolvimento de sistemas corporativos."
    }
  ]
};

// NOTAS IMPORTANTES:
// 1. Todos os campos são opcionais - o processamento pode extrair apenas parte da informação
// 2. Datas devem estar no formato ISO string (com T e Z)
// 3. Campos vazios ou undefined serão tratados como strings vazias no frontend
// 4. O campo empregoAtual=true significa que dataFim deve ser ignorada
// 5. Arrays vazios são permitidos (serão simplesmente ignorados)