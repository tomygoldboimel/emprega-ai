<template>
  <div class="wrapper">
    <ModalCarregamento :isOpen="loading" />
    <TutorialHand 
      v-model="mostrarTutorial"
      :handSrc="pointerHandIcon"
    />
    <div class="container">
      <div class="top-bar">
        <LogoutButton @logout="abrirModal" />
        <div class="right-actions">
          <BotaoDescricao @toggle="mostrarTutorial = $event"/>
        </div>
      </div>
      <div class="header">
        <div class="logo-circle">
          <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
            <path d="M22 10v6M2 10l10-5 10 5-10 5z"></path>
            <path d="M6 12v5c3 3 9 3 12 0v-5"></path>
          </svg>
        </div>
        <h1>{{ modoEdicao ? 'Editar Currículo' : 'Criar Currículo' }}</h1>
        <p>Preencha seus dados profissionais</p>
      </div>

      <div class="progress-bar">
        <div :class="['progress-step', { active: step >= 1 }]">
          <div class="step-circle">1</div>
          <span>Sobre você</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 2 }"></div>
        <div :class="['progress-step', { active: step >= 2 }]">
          <div class="step-circle">2</div>
          <span>Trabalhos</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 3 }"></div>
        <div :class="['progress-step', { active: step >= 3 }]">
          <div class="step-circle">3</div>
          <span>Estudos</span>
        </div>
      </div>

      <div v-if="step === 1" class="step-content">
        <h2 class="step-title">Sobre você</h2>
        <div class="form-group">
          <label>Nome Completo*</label>
          <div style="position: relative;">
            <input type="text" v-model="curriculo.nomeCompleto" required placeholder="Seu nome todo..."/>
            <BotaoMicrofone 
              :isRecording="camposGravando.nomeCompleto" 
              @toggle="toggleGravacao('nomeCompleto', curriculo)"
            />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Data de Nascimento</label>
            <div style="position: relative;"> 
              <input
              type="date" 
              v-model="curriculo.dataNascimento"
              class="input-com-dois-icones"
              />
              
              <BotaoMicrofone 
              :isRecording="gravandoDataFim" 
              @toggle="toggleGravacaoDataFim"
              />
                
              </div>
          </div>
          <div class="form-group">
            <label>Telefone</label>
            <input 
              type="tel" 
              v-model="curriculo.telefone" 
              @input="formatarTelefone"
              placeholder="(XX) XXXXX-XXXX"
              maxlength="15"
              disabled
            />
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label>Estado</label>
            <div style="position: relative;">
              <EstadoDropdown v-model="curriculo.estado" class="input-com-dois-icones"/>
              <BotaoMicrofone 
                  :isRecording="camposGravando.estado" 
                  @toggle="toggleGravacao('estado', curriculo)"
              />
            </div>
          </div>
          <div class="form-group">
            <label>Cidade</label>
            <div style="position: relative;">
              <CidadeDropdown 
                v-model="curriculo.cidade" 
                :cidades="cidades"
                class="input-com-dois-icones"
              />
              <BotaoMicrofone 
                :isRecording="camposGravando.cidade" 
                @toggle="toggleGravacao('cidade', curriculo)"
              />
            </div>
          </div>
        </div>

        <div class="form-group">
            <label>Objetivo Profissional</label>
              <div style="position: relative;">
                <textarea 
                  v-model="curriculo.objetivo" 
                  rows="6" 
                  required
                  placeholder="No que você quer trabalhar?"
                ></textarea>
                <BotaoMicrofone 
                  :isRecording="camposGravando.objetivo"
                  @toggle="toggleGravacao('objetivo', curriculo)"
                />
                <button type="button" @click="formatarObjetivoComIA" class="btn-ia" :disabled="loadingIA || !curriculo.objetivo">
                  <img v-if="!loadingIA" src="@/assets/icons/BotSparkleIcon.svg" alt="IA" class="icon-ia" />
                  <span v-if="loadingIA" class="loading-spinner-small"></span>
                </button>
              </div>
            
            
            
            <small v-if="iaMessage" :class="['ia-message', iaMessageType]">{{ iaMessage }}</small>
          </div>
        <div class="bottom-bar">
          <BotaoProximo @forward="nextStepPerfil" class="next-btn"/>
        </div>
      </div>

      <div v-if="step === 2" class="step-content">
        <h2 class="step-title">Trabalhos</h2>
        <div class="form-card">
          <div class="form-group">
            <label>Descrição das Atividades*</label>
              <div style="position: relative;">
                <textarea 
                  v-model="novaExperiencia.descricao" 
                  rows="6" 
                  required
                  placeholder="O que você fazia?"
                ></textarea>
                <BotaoMicrofone 
                  :isRecording="camposGravando.descricao"
                  @toggle="toggleGravacao('descricao', novaExperiencia)"
                />
                <button type="button" @click="formatarDescricaoComIA" class="btn-ia" :disabled="loadingIA || !novaExperiencia.descricao">
                  <img v-if="!loadingIA" src="@/assets/icons/BotSparkleIcon.svg" alt="IA" class="icon-ia" />
                  <span v-if="loadingIA" class="loading-spinner-small"></span>
                </button>
              </div>
            
            
            
            <small v-if="iaMessage" :class="['ia-message', iaMessageType]">{{ iaMessage }}</small>
          </div>
          <div class="form-group">
            <label>Empresa</label>
            <div style="position: relative;">
              
              <input 
                type="text" 
                v-model="novaExperiencia.empresa" 
                style="width: 100%;" 
                placeholder="Onde você trabalhou? (opcional)"
              />
              
              <BotaoMicrofone 
                :isRecording="camposGravando.empresa" 
                @toggle="toggleGravacao('empresa', novaExperiencia)"
              />
              
            </div>
          </div>

          <div class="form-group">
            <label>Cargo</label>
            <div style="position: relative;">
              <input type="text" v-model="novaExperiencia.cargo" placeholder="Trabalhou como o quê? (opcional)"/>
              <BotaoMicrofone :isRecording="camposGravando.cargo" @toggle="toggleGravacao('cargo', novaExperiencia)"/>
            </div>
          </div>
          
          <div class="form-row">
            <div class="form-group">
              <label>Data Início</label>
              
              <div style="position: relative;">
                
                <input 
                type="date" 
                v-model="novaExperiencia.dataInicio"
                class="input-com-dois-icones"
                />
                
                <BotaoMicrofone 
                :isRecording="gravandoDataInicioExperiencia" 
                @toggle="toggleGravacaoDataInicioExperiencia"
                />
                
              </div>
            </div>
            <div class="form-group">
              <label>Data Fim</label>
              <div style="position: relative;"> 
                <input v-if="!novaExperiencia.empregoAtual"
                type="date" 
                v-model="novaExperiencia.dataFim"
                class="input-com-dois-icones"
                />
                <input v-else
                type="date" 
                v-model="novaExperiencia.dataFim"
                class="input-com-dois-icones"
                disabled
                />
                
                <BotaoMicrofone 
                :isRecording="gravandoDataFim" 
                @toggle="toggleGravacaoDataFim"
                />
                
              </div>
              <div class="checkbox-wrapper">
                <input type="checkbox" id="empregoAtual" v-model="novaExperiencia.empregoAtual" />
                <label for="empregoAtual" style="margin: 0;">Trabalho aqui atualmente</label>
              </div>
            </div>
          </div>


          <button class="btn-add" @click="adicionarExperiencia">
            <span v-if="!editandoIndexExperiencia">+</span>
            <img v-else src="@/assets/icons/updateIcon.svg" alt="Atualizar" class="icon-update" />
          </button>
        </div>

        <div v-if="curriculo.experiencias.length > 0" class="item-list">
          <div v-for="(exp, index) in curriculo.experiencias" :key="index" class="item-card">
            <button class="btn-edit" @click="editarExperiencia(index)" title="Editar">
                <i class="fas fa-pencil"></i>
              </button>
            <button class="btn-remove" @click="removerExperiencia(index)">×</button>
            <h4>{{ exp.cargo ? exp.cargo : "Não Informado"}}</h4>
            <p class="subtitle">{{ exp.empresa ? exp.empresa : "Não Informado"}}</p>
            <p class="date">{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</p>
            <p 
                v-if="exp.descricao" 
                class="description"
                v-html="formatarDescricao(exp.descricao)"
              ></p>
          </div>
        </div>

        <div class="bottom-bar">
          <BackButton @back="prevStep" />
          <BotaoProximo @forward="nextStepPerfil"/>
        </div>
      </div>

      <div v-if="step === 3" class="step-content">
        <h2 class="step-title">Estudos</h2>

        <div class="form-card">
          <div class="form-group">
            <label>Instituição</label>
            <div style="position: relative;">
              <input type="text" v-model="novaFormacao.instituicao" placeholder="Nome da escola ou universidade..."/>
              <BotaoMicrofone 
                :isRecording="camposGravando.instituicao" 
                @toggle="toggleGravacao('instituicao', novaFormacao)"
              />
            </div>
          </div>
          <div class="form-group">
            <label>Curso</label>
            <div style="position: relative;">
              <input type="text" v-model="novaFormacao.curso" placeholder="Ex: Ensino Médio, Administração..."/>
              <BotaoMicrofone 
                :isRecording="camposGravando.curso" 
                @toggle="toggleGravacao('curso', novaFormacao)"
              />
            </div>
          </div>

          <div class="checkbox-wrapper" style="margin-bottom: 15px;">
            <input type="checkbox" id="naoConcluiu" v-model="novaFormacao.status" />
            <label for="naoConcluiu" style="margin: 0;">Curso incompleto?</label>
          
          </div>

          <button class="btn-add" @click="adicionarFormacao">
            <span>+</span>
          </button>
        </div>

        <div v-if="curriculo.formacoes.length > 0" class="item-list">
          <div v-for="(form, index) in curriculo.formacoes" :key="index" class="item-card">
            <button class="btn-edit" @click="editarFormacao(index)" title="Editar">
              <i class="fas fa-pencil"></i>
            </button>
            <button class="btn-remove" @click="removerFormacao(index)">×</button>
            <h4>{{ form.curso }} • {{ form.status === true ? 'Incompleto' : 'Completo' }}</h4>
            <p class="subtitle">{{ form.instituicao }}</p>
          </div>
        </div>

        <div class="bottom-bar">
          <BackButton @back="prevStep" />
          <SaveButton
            :loading="loading"
            @save="salvarCurriculo"/>
        </div>
      </div>

      <div v-if="successMessage" class="alert alert-success">
        {{ successMessage }}
      </div>

      <div v-if="errorMessage" class="alert alert-error">
        {{ errorMessage }}
      </div>
    </div>

    <ModalExclusao
      :show="showConfirmModal"
      :title="modalConfig.title"
      :message="modalConfig.message"
      :type="modalConfig.type"
      :confirmText="modalConfig.confirmText"
      @confirmar="confirmarRemocao"
      @fechar="showConfirmModal = false"
    />
    <!-- <ModalErro
      :isOpen="modalAberto"
      @confirmar="confirmarSair"
      @fechar="fecharModal"/> -->
    <ModalEncerramentoSessao
      :isOpen="modalAberto"
      @confirmar="confirmarSair"
      @fechar="fecharModal"/>
    <ModalAviso
      :show="modalAvisoAberto"
      title="Atenção"
      message="Ao importar o currículo, a formatação da descrição pode ser alterada. Recomendamos revisar os textos após a importação."
      type="aviso"
      @fechar="fecharModalAviso"
    />
  </div>
</template>

<script>
import EstadoDropdown from '@/components/EstadoDropdown.vue'
import ModalExclusao from '@/components/ModalExclusao.vue'
import curriculoService from '@/services/curriculoService';
import experienciaService from '@/services/experienciaService';
import formacaoService from '@/services/formacaoService';
import usuarioService from '@/services/usuarioService';
import '@fortawesome/fontawesome-free/css/all.css';
import { ref, watch } from 'vue';
import StatusDropdown from '@/components/StatusDropdown.vue';
import NivelDropdown from '@/components/NivelDropdown.vue';
import ResumeUpload from "@/components/ResumeUpload.vue";
import ModalEncerramentoSessao from '@/components/ModalEncerramentoSessao.vue';
import ModalConfirmacao from '@/components/ModalConfirmacao.vue';
import ModalErro from '@/components/ModalErro.vue';
import ModalAviso from '@/components/AvisoDescricao.vue';
import BotaoAudio from '@/components/BotaoAudio.vue';
import LogoutButton from '@/components/LogoutButton.vue';
import BotaoMicrofone from '@/components/BotaoMicrofone.vue';
import BotaoDescricao from '@/components/BotaoDescricao.vue';
import BotaoProximo from '@/components/BotaoProximo.vue';
import { ibgeService } from '@/services/ibgeService';
import BackButton from '@/components/BackButton.vue';
import SaveButton from '@/components/SaveButton.vue';
import CidadeDropdown from '@/components/CidadeDropdown.vue';
import TutorialHand from '@/components/TutorialHand.vue';
import pointerHandIcon from '@/assets/icons/pointerHandIcon.svg';
import BotaoContraste from '@/components/BotaoContraste.vue';
import ModalCarregamento from '@/components/ModalCarregamento.vue';

export default {
  name: 'CurriculoForm',
  
  components: {
    EstadoDropdown,
    StatusDropdown,
    NivelDropdown,
    ModalExclusao,
    ModalEncerramentoSessao,
    ModalConfirmacao,
    ModalCarregamento,
    ModalAviso,
    ModalErro,
    ResumeUpload,
    BotaoAudio,
    LogoutButton,
    BotaoMicrofone,
    BotaoDescricao,
    BotaoProximo,
    BackButton,
    SaveButton,
    CidadeDropdown,
    BotaoContraste,
    TutorialHand,
    ModalCarregamento
  },
  data() {
    return {
      step: 1,
      editandoIndexExperiencia: null,
      editandoExperiencia: false,
      editandoIndexFormacao: null,
      editandoFormacao: null,
      loading: false,
      loadingIA: false,
      successMessage: '',
      errorMessage: '',
      iaMessage: '',
      iaMessageType: '',
      telefoneFormatado: '',
      modoEdicao: false,
      modalAberto: false,
      modalAvisoAberto: false,
      curriculoId: null,
      showConfirmModal: false,
      itemParaRemover: null,
      pointerHandIcon: pointerHandIcon,
      // Sistema genérico de gravação
      camposGravando: {
        descricao: false,
        empresa: false,
        cargo: false,
        instituicao: false,
        curso: false,
        estado: false,
        nomeCompleto: false,
        email: false,
        cidade: false
      },
      // Gravação de datas individuais
      gravandoDataFim: false,
      gravandoDataInicioExperiencia: false,
      gravandoDataInicioFormacao: false,
      gravandoDataConclusaoFormacao: false,
      transcricaoDescricao: '',
      transcricaoAtual: '', // Transcrição em tempo real do que está sendo falado
      erroAudio: null,
      curriculoOriginal: {},
      modalConfig: {
        title: '',
        message: '',
        type: 'aviso',
        confirmText: 'Confirmar',
      },
      curriculo: {
        usuarioId: '',
        nomeCompleto: '',
        dataNascimento: '',
        telefone: '',
        email: '',
        endereco: '',
        cidade: '',
        estado: '',
        linkedin: '',
        github: '',
        resumoProfissional: '',
        experiencias: [],
        formacoes: []
      },
      
      cidades: [],
      carregandoCidades: false,
      mostrarTutorial: false,

      novaExperiencia: {
        empresa: '',
        cargo: '',
        dataInicio: '',
        dataFim: '',
        empregoAtual: false,
        descricao: ''
      },

      novaFormacao: {
        instituicao: '',
        curso: '',
        status: false,
      }
    }
  },
  watch: {
    'novaExperiencia.empregoAtual': function(newValue) {
      if (newValue) {
          this.novaExperiencia.dataFim = '';
      }
    },
    'curriculo.estado': async function(newValue, oldValue) {
      if (newValue) {
        this.carregandoCidades = true;
        this.cidades = [];
        
        // SÓ limpa a cidade se já existia um estado antes (mudança manual)
        // Se oldValue for undefined ou null, significa que é o carregamento inicial
        if (oldValue) {
          this.curriculo.cidade = ''; 
        }
        
        try {
          this.cidades = await ibgeService.listarCidades(newValue);
          console.log(`📍 Cidades carregadas para ${newValue}:`, this.cidades.length);
        } catch (error) {
          console.error('Erro ao carregar cidades:', error);
          this.mostrarErro('Erro ao carregar cidades');
        } finally {
          this.carregandoCidades = false;
        }
      }
    }
  },
  async created() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    const userStr = localStorage.getItem('user');
    if (!userStr) {
      this.mostrarErro('Usuário não encontrado. Faça login novamente.');
      this.$router.push('/login');
      return;
    }

    let user;
    try {
      user = JSON.parse(userStr);
    } catch (e) {
      this.mostrarErro('Erro ao carregar dados do usuário.');
      this.$router.push('/login');
      return;
    }

    // 1. Já garantimos os dados básicos do usuário no objeto local imediatamente
    this.curriculo.usuarioId = user.id;
    if (user.telefone) {
      this.curriculo.telefone = this.formatarTelefoneString(user.telefone);
    }

    try {
      this.loading = true;
      const curriculoExistente = await curriculoService.listarCurriculosPorUsuario(user.id);
      
      if (curriculoExistente) {
        const exps = await experienciaService.listarExperienciaPorIdCurriculo(curriculoExistente.id);
        const forms = await formacaoService.listarFormacoesPorCurriculoId(curriculoExistente.id);
        this.curriculo = {
          ...this.curriculo, // Mantém o que já setamos acima (telefone e userId)
          ...curriculoExistente,
          experiencias: exps || [],
          formacoes: forms || []
        };
        console.log(this.curriculo)
      } else {
        // Se for nulo, garantimos que as listas não sejam "undefined" para o Vue não quebrar
        this.curriculo.experiencias = [];
        this.curriculo.formacoes = [];
      }
    } catch (error) {
      console.error("Erro ao buscar currículo, mas continuaremos com os dados de login:", error);
      // Mesmo com erro na API, as listas devem ser inicializadas
      this.curriculo.experiencias = [];
      this.curriculo.formacoes = [];
    } finally {
      // 2. Desativa o modal independente de sucesso ou erro
      this.loading = false; 
    }

    // 3. Verificação de edição (params ID)
    if (this.$route.params.id) {
      this.modoEdicao = true;
      this.curriculoId = this.$route.params.id;
      // ... sua lógica de carregar experiências por ID
    }

    // 4. Formatação de data (só se existir)
    if (this.curriculo.dataNascimento) {
      this.curriculo.dataNascimento = this.curriculo.dataNascimento.split('T')[0];
    }

    this.curriculoOriginal = JSON.parse(JSON.stringify(this.curriculo));
  },


  methods: {
    getTextoStep1() {
      const campos = [];
      
      if (this.curriculo.nomeCompleto) {
        campos.push(`Nome completo: ${this.curriculo.nomeCompleto}`);
      }
      if (this.curriculo.dataNascimento) {
        campos.push(`Data de nascimento: ${this.formatarDataParaLeitura(this.curriculo.dataNascimento)}`);
      }
      if (this.telefoneFormatado) {
        campos.push(`Telefone: ${this.telefoneFormatado}`);
      }
      if (this.curriculo.email) {
        campos.push(`Email: ${this.curriculo.email}`);
      }
      if (this.curriculo.estado) {
        campos.push(`Estado: ${this.curriculo.estado}`);
      }
      if (this.curriculo.cidade) {
        campos.push(`Cidade: ${this.curriculo.cidade}`);
      }
      if (this.curriculo.endereco) {
        campos.push(`Endereço: ${this.curriculo.endereco}`);
      }
      if (this.curriculo.linkedIn) {
        campos.push(`LinkedIn: ${this.curriculo.linkedIn}`);
      }
      if (this.curriculo.gitHub) {
        campos.push(`GitHub: ${this.curriculo.gitHub}`);
      }

      if (campos.length === 0) {
        return 'Dados pessoais. Preencha os campos do formulário.';
      }

      return 'Dados pessoais preenchidos: ' + campos.join('. ');
    },

    formatarDataParaLeitura(data) {
      if (!data) return '';
      const partes = data.split('-');
      if (partes.length === 3) {
        return `${partes[2]} de ${this.getNomeMes(partes[1])} de ${partes[0]}`;
      }
      return data;
    },

    getNomeMes(mes) {
      const meses = {
        '01': 'janeiro', '02': 'fevereiro', '03': 'março',
        '04': 'abril', '05': 'maio', '06': 'junho',
        '07': 'julho', '08': 'agosto', '09': 'setembro',
        '10': 'outubro', '11': 'novembro', '12': 'dezembro'
      };
      return meses[mes] || mes;
    },

    mostrarErro(mensagem){
      this.errorMessage = mensagem
      this.$nextTick(() => {
          window.scrollTo({
          top: document.body.scrollHeight,
          behavior: 'smooth'
        });
      });
      setTimeout(() => this.errorMessage = '', 3000);
      return;
    },
    handleResumeImport(data) {
      try {
        this.abrirModalAviso();
        if (data.dadosPessoais) {
          this.curriculo.nomeCompleto = data.dadosPessoais.nomeCompleto || '';
          this.curriculo.dataNascimento = data.dadosPessoais.dataNascimento?.split('T')[0] || '';
          this.curriculo.cidade = data.dadosPessoais.cidade || '';
          this.curriculo.estado = data.dadosPessoais.estado || '';
          this.curriculo.endereco = data.dadosPessoais.endereco || '';
          this.curriculo.linkedIn = data.dadosPessoais.linkedIn || '';
          this.curriculo.gitHub = data.dadosPessoais.gitHub || '';

          if (data.dadosPessoais.telefone) {
            this.telefoneFormatado = this.formatarTelefoneString(data.dadosPessoais.telefone);
            this.curriculo.telefone = data.dadosPessoais.telefone.replace(/\D/g, '');
          }
        }

        /* EXPERIÊNCIAS */
        if (Array.isArray(data.experiencias)) {
          this.curriculo.experiencias = data.experiencias.map(exp => ({
            empresa: exp.empresa || '',
            cargo: exp.cargo || '',
            dataInicio: exp.dataInicio ? exp.dataInicio.split('T')[0] : '',
            dataFim: exp.dataFim ? exp.dataFim.split('T')[0] : '',
            empregoAtual: exp.empregoAtual || false,
            descricao: exp.descricao || ''
          }));
          console.log('✅ Experiências importadas:', this.curriculo.experiencias.length);
        }

        /* FORMAÇÕES */
        if (Array.isArray(data.formacoes)) {
          this.curriculo.formacoes = data.formacoes.map(form => ({
            instituicao: form.instituicao || '',
            curso: form.curso || '',
            nivel: form.nivel || '',
            status: form.status || '',
            dataInicio: form.dataInicio ? form.dataInicio.split('T')[0] : '',
            dataConclusao: form.dataConclusao ? form.dataConclusao.split('T')[0] : ''
          }));
          console.log('✅ Formações importadas:', this.curriculo.formacoes.length);
        }

        /* FEEDBACK */
        this.successMessage = 'Currículo importado com sucesso!';
        
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);

        // Scroll para o topo
        window.scrollTo({ top: 0, behavior: 'smooth' });

      } catch (error) {
        console.error('❌ Erro ao importar:', error);
        this.mostrarErro('Erro ao aplicar dados do currículo.');
      }
    },
    toggleGravacaoDescricao() {
      if (this.gravandoDescricao) {
        // Parar gravação
        this.stopRecording();
      } else {
        // Iniciar gravação
        this.startRecordingDescricao();
      }
    },
    
    async startRecordingDescricao() {
      try {
        // Verificar suporte
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        // Solicitar permissão do microfone
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        // Criar reconhecimento
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        // Quando tem resultado
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            } else {
              this.transcricaoDescricao = event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            objeto[fieldName] = this.capitalizeText(transcript);
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoDescricao = true;
          this.erroAudio = null;
          this.transcricaoDescricao = '';
          console.log('🎤 Gravação iniciada');
        };
        
        // Quando termina
        this.recognition.onend = () => {
          this.gravandoDescricao = false;
          this.transcricaoDescricao = '';
          console.log('🛑 Gravação finalizada');
        };
        
        // Quando dá erro
        this.recognition.onerror = (event) => {
          this.gravandoDescricao = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        // Iniciar
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoDescricao = false;
      }
    },

    stopRecording() {
      if (this.recognition) {
        this.recognition.stop();
      }
    },

    toggleGravacao(fieldName, objeto) {
      if (this.camposGravando[fieldName]) {
        this.stopRecording();
      } else {
        this.startRecording(fieldName, objeto);
      }
    },

    async startRecording(fieldName, objeto) {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          let interim = ''; // Texto enquanto está falando
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            const transcriptParcial = event.results[i][0].transcript;
            
            if (event.results[i].isFinal) {
              transcript += transcriptParcial;
            } else {
              interim += transcriptParcial; // Captura o que está sendo falado
            }
          }
          
          // Mostrar em tempo real o que está sendo falado
          this.transcricaoAtual = interim || transcript;
          
          // Console logs para verificação
          if (interim) {
            console.log(`🎙️ OUVINDO (${fieldName}):`, interim);
          }
          if (transcript) {
            console.log(`✅ FINAL (${fieldName}):`, transcript);
          }
          
          if (transcript) {
            // Se for estado, converter para sigla
            if (fieldName === 'estado') {
              const sigla = this.converterEstadoParaSigla(transcript);
              // Só preenchher se a conversão foi bem-sucedida (retornou uma sigla válida)
              if (sigla) {
                objeto[fieldName] = sigla;
                console.log(`✨ Estado convertido para:`, sigla);
              }
            } 
            // Se for cidade, validar se existe no estado selecionado
            else if (fieldName === 'cidade') {
              const cidadeValida = this.validarEConverterCidade(transcript);
              if (cidadeValida) {
                objeto[fieldName] = cidadeValida;
              }
            }
            else if (fieldName === 'nivel') {
              const nivelValido = this.validarNivel(transcript);
              if (nivelValido) {
                objeto[fieldName] = nivelValido;
              }
            }
            else {
              // Para outros campos, adicionar ao existente ou criar novo
              objeto[fieldName] = this.capitalizeText(transcript);
            }
          }
        };
        
        this.recognition.onstart = () => {
          this.camposGravando[fieldName] = true;
          this.erroAudio = null;
          this.transcricaoDescricao = '';
          console.log(`🎤 Gravação ${fieldName} iniciada`);
        };
        
        this.recognition.onend = () => {
          this.camposGravando[fieldName] = false;
          this.transcricaoAtual = ''; // Limpar a transcrição ao terminar
          this.transcricaoDescricao = '';
          console.log(`🛑 Gravação ${fieldName} finalizada`);
        };
        
        this.recognition.onerror = (event) => {
          this.camposGravando[fieldName] = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error(`Erro ao iniciar gravação ${fieldName}:`, error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.camposGravando[fieldName] = false;
      }
    },
    capitalizeText(text) {
      if (!text) return '';
      const trimmed = text.trim();
      return trimmed.charAt(0).toUpperCase() + trimmed.slice(1);
    },

    toggleGravacaoEmpresa() {
      if (this.gravandoEmpresa) {
        this.stopRecording();
      } else {
        this.startRecordingEmpresa();
      }
    },
    
    async startRecordingEmpresa() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            this.novaExperiencia.empresa = transcript;
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoEmpresa = true;
          this.erroAudio = null;
          console.log('🎤 Gravação empresa iniciada');
        };
        
        this.recognition.onend = () => {
          this.gravandoEmpresa = false;
          console.log('🛑 Gravação empresa finalizada');
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoEmpresa = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação empresa:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoEmpresa = false;
      }
    },
    toggleGravacaoCargo() {
      if (this.gravandoCargo) {
        this.stopRecording();
      } else {
        this.startRecordingCargo();
      }
    },
    
    async startRecordingCargo() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            this.novaExperiencia.cargo = transcript;
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoCargo = true;
          this.erroAudio = null;
        };
        
        this.recognition.onend = () => {
          this.gravandoCargo = false;
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoCargo = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação cargo:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoCargo = false;
      }
    },
    
    converterEstadoParaSigla(nomeEstado) {
      const estadosMap = {
        // Nomes completos
        'acre': 'AC',
        'alagoas': 'AL',
        'amapá': 'AP',
        'amazonas': 'AM',
        'bahia': 'BA',
        'ceará': 'CE',
        'distrito federal': 'DF',
        'espírito santo': 'ES',
        'goiás': 'GO',
        'maranhão': 'MA',
        'mato grosso': 'MT',
        'mato grosso do sul': 'MS',
        'minas gerais': 'MG',
        'pará': 'PA',
        'para': 'PA', // Variação para "Pará"
        'parà': 'PA', // Variação para "Pará"
        'parar': 'PA', // Variação para "Pará"
        'paraíba': 'PB',
        'paraná': 'PR',
        'pernambuco': 'PE',
        'piauí': 'PI',
        'rio de janeiro': 'RJ',
        'rio grande do norte': 'RN',
        'rio grande do sul': 'RS',
        'rondônia': 'RO',
        'roraima': 'RR',
        'santa catarina': 'SC',
        'são paulo': 'SP',
        'sergipe': 'SE',
        'tocantins': 'TO'
      };
      
      const estadoLower = nomeEstado.trim().toLowerCase();
      const sigla = estadosMap[estadoLower];
      
      // Retorna null se não encontrar um estado válido
      if (!sigla) {
        return null;
      }
      
      return sigla;
    },
    
    validarEConverterCidade(nomeCidade) {
      if (!this.curriculo.estado) {
        this.erroAudio = 'Selecione um estado antes de selecionar a cidade.';
        console.warn('❌ Estado não selecionado');
        return null;
      }
      
      // Procurar pela cidade nas cidades carregadas do estado
      const cidadeEncontrada = this.cidades.find(c => 
        c.nome.toLowerCase() === nomeCidade.trim().toLowerCase()
      );
      
      if (!cidadeEncontrada) {
        this.erroAudio = `"${nomeCidade}" não é uma cidade válida em ${this.curriculo.estado}. Tente falar o nome de uma cidade do estado.`;
        console.warn(`❌ Cidade inválida para ${this.curriculo.estado}: "${nomeCidade}"`);
        return null;
      }
      
      console.log(`✨ Cidade encontrada: ${cidadeEncontrada.nome}`);
      return cidadeEncontrada.nome;
    },

    validarNivel(nomeNivel) {
      const niveis = [
        'Ensino Fundamental',
        'Ensino Médio',
        'Técnico',
        'Graduação',
        'Pós-graduação',
        'Mestrado',
        'Doutorado'
      ];

      const nivelEncontrado = niveis.find(n =>
        n.toLowerCase() === nomeNivel.trim().toLowerCase()
      );

      if (!nivelEncontrado) {
        this.erroAudio = `"${nomeNivel}" não é um nível válido. Opções: ${niveis.join(', ')}.`;
        console.warn(`❌ Nível inválido: "${nomeNivel}"`);
        return null;
      }

      console.log(`✨ Nível encontrado: ${nivelEncontrado}`);
      return nivelEncontrado;
    },
    
    toggleGravacaoDataInicioExperiencia() {
      if (this.gravandoDataInicioExperiencia) {
        this.stopRecording();
      } else {
        this.startRecordingDataInicioExperiencia();
      }
    },
    
    async startRecordingDataInicioExperiencia() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            // AQUI ESTÁ A MUDANÇA:
            // Convertemos o texto "5 de maio de 1996" para "1996-05-05"
            const dataFormatada = this.converterTextoParaDataISO(transcript);
            
            if (dataFormatada) {
              this.novaExperiencia.dataInicio = dataFormatada;
            } else {
              // Opcional: Avisar usuário que não entendeu a data
              this.erroAudio = `Não entendi a data "${transcript}". Tente falar "05 de maio de 1996"`;
            }
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoDataInicioExperiencia = true;
          this.erroAudio = null;
        };
        
        this.recognition.onend = () => {
          this.gravandoDataInicioExperiencia = false;
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoDataInicioExperiencia = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação data início:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoDataInicioExperiencia = false;
      }
    },

    converterTextoParaDataISO(texto) {
      if (!texto) return '';
      texto = texto.toLowerCase().trim();

      const meses = {
        'janeiro': '01', 'fevereiro': '02', 'março': '03', 'abril': '04',
        'maio': '05', 'junho': '06', 'julho': '07', 'agosto': '08',
        'setembro': '09', 'outubro': '10', 'novembro': '11', 'dezembro': '12',
        '1': '01', '2': '02', '3': '03', '4': '04', '5': '05', '6': '06',
        '7': '07', '8': '08', '9': '09', 
        '01': '01', '02': '02', '03': '03', '04': '04', '05': '05', '06': '06',
        '07': '07', '08': '08', '09': '09', '10': '10', '11': '11', '12': '12',
        'um': '01', 'dois': '02', 'três': '03', 'quatro': '04', 'cinco': '05', 'seis': '06',
        'sete': '07', 'oito': '08', 'nove': '09', 'dez': '10', 'onze': '11', 'doze': '12'
      };

      const regexFalado = /(\d{1,2})\s*(?:d[eo])?\s*([a-zç0-9]+)\s*(?:d[eo])?\s*(\d{4})/i;
      const matchFalado = texto.match(regexFalado);

      if (matchFalado) {
        let dia = matchFalado[1];
        let mesCapturado = matchFalado[2];
        let ano = matchFalado[3];
        if (parseInt(dia) > 31 && /^\d+$/.test(mesCapturado)) {
          const digitoSobrando = dia.slice(-1);
          
          dia = dia.slice(0, -1);
          
          mesCapturado = digitoSobrando + mesCapturado;
        }

        dia = dia.padStart(2, '0');
        let mes = meses[mesCapturado];

        if (mes) {
          return `${ano}-${mes}-${dia}`;
        }
      }

      // Regex Misto (Barra + Texto)
      const regexMisto = /(\d{1,2})[\/-](\d{1,2})\s*(?:d[eo])?\s*(\d{4})/;
      const matchMisto = texto.match(regexMisto);
      if (matchMisto) {
        let dia = matchMisto[1].padStart(2, '0');
        let mes = matchMisto[2].padStart(2, '0');
        let ano = matchMisto[3];
        return `${ano}-${mes}-${dia}`;
      }

      // Regex Numérico Puro
      const regexNumerico = /(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})/;
      const matchNumerico = texto.match(regexNumerico);
      if (matchNumerico) {
        let dia = matchNumerico[1].padStart(2, '0');
        let mes = matchNumerico[2].padStart(2, '0');
        let ano = matchNumerico[3];
        return `${ano}-${mes}-${dia}`;
      }

      console.warn("Não foi possível converter a data:", texto);
      return '';
    },
    toggleGravacaoDataFim() {
      if (this.gravandoDataFim) {
        this.stopRecording();
      } else {
        this.startRecordingDataFim();
      }
    },
    
    async startRecordingDataFim() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            // AQUI ESTÁ A MUDANÇA:
            // Convertemos o texto "5 de maio de 1996" para "1996-05-05"
            const dataFormatada = this.converterTextoParaDataISO(transcript);
            
            if (dataFormatada) {
              this.novaExperiencia.dataFim = dataFormatada;
              console.log(`Convertido: "${transcript}" -> "${dataFormatada}"`);
            } else {
              // Opcional: Avisar usuário que não entendeu a data
              this.erroAudio = `Não entendi a data "${transcript}". Tente falar "05 de maio de 1996"`;
            }
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoDataFim = true;
          this.erroAudio = null;
          console.log('🎤 Gravação data fim iniciada');
        };
        
        this.recognition.onend = () => {
          this.gravandoDataFim = false;
          console.log('🛑 Gravação data fim finalizada');
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoDataFim = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação data fim:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoDataFim = false;
      }
    },
    toggleGravacaoDataInicioFormacao() {
      if (this.gravandoDataInicioFormacao) {
        this.stopRecording();
      } else {
        this.startRecordingDataInicioFormacao();
      }
    },
    
    async startRecordingDataInicioFormacao() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            // AQUI ESTÁ A MUDANÇA:
            // Convertemos o texto "5 de maio de 1996" para "1996-05-05"
            const dataFormatada = this.converterTextoParaDataISO(transcript);
            
            if (dataFormatada) {
              this.novaFormacao.dataInicio = dataFormatada;
              console.log(`Convertido: "${transcript}" -> "${dataFormatada}"`);
            } else {
              // Opcional: Avisar usuário que não entendeu a data
              this.erroAudio = `Não entendi a data "${transcript}". Tente falar "05 de maio de 1996"`;
            }
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoDataInicioFormacao = true;
          this.erroAudio = null;
          console.log('🎤 Gravação data início formação iniciada');
        };
        
        this.recognition.onend = () => {
          this.gravandoDataInicioFormacao = false;
          console.log('🛑 Gravação data início formação finalizada');
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoDataInicioFormacao = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação data início formação:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoDataInicioFormacao = false;
      }
    },
    toggleGravacaoDataConclusaoFormacao() {
      if (this.gravandoDataConclusaoFormacao) {
        this.stopRecording();
      } else {
        this.startRecordingDataConclusaoFormacao();
      }
    },
    
    async startRecordingDataConclusaoFormacao() {
      try {
        const SpeechRecognition = window.SpeechRecognition || window.webkitSpeechRecognition;
        
        if (!SpeechRecognition) {
          this.erroAudio = 'Seu navegador não suporta reconhecimento de voz. Use Chrome, Edge ou Safari.';
          return;
        }
        
        await navigator.mediaDevices.getUserMedia({ audio: true });
        
        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = true;
        
        this.recognition.onresult = (event) => {
          let transcript = '';
          
          for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
              transcript += event.results[i][0].transcript;
            }
          }
          
          if (transcript) {
            // AQUI ESTÁ A MUDANÇA:
            // Convertemos o texto "5 de maio de 1996" para "1996-05-05"
            const dataFormatada = this.converterTextoParaDataISO(transcript);
            
            if (dataFormatada) {
              this.novaFormacao.dataConclusao = dataFormatada;
              console.log(`Convertido: "${transcript}" -> "${dataFormatada}"`);
            } else {
              // Opcional: Avisar usuário que não entendeu a data
              this.erroAudio = `Não entendi a data "${transcript}". Tente falar "05 de maio de 1996"`;
            }
          }
        };
        
        this.recognition.onstart = () => {
          this.gravandoDataConclusaoFormacao = true;
          this.erroAudio = null;
          console.log('🎤 Gravação data conclusão formação iniciada');
        };
        
        this.recognition.onend = () => {
          this.gravandoDataConclusaoFormacao = false;
          console.log('🛑 Gravação data conclusão formação finalizada');
        };
        
        this.recognition.onerror = (event) => {
          this.gravandoDataConclusaoFormacao = false;
          
          const errorMessages = {
            'no-speech': 'Não detectei fala. Tente novamente.',
            'audio-capture': 'Microfone não encontrado.',
            'not-allowed': 'Permissão negada. Permita o microfone.',
            'network': 'Erro de rede.',
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
          console.error('❌ Erro:', event.error);
        };
        
        this.recognition.start();
        
      } catch (error) {
        console.error('Erro ao iniciar gravação data conclusão formação:', error);
        this.erroAudio = 'Erro ao acessar microfone. Verifique as permissões.';
        this.gravandoDataConclusaoFormacao = false;
      }
    },
    resetarFormExperiencia() {
      this.novaExperiencia = {
        empresa: '',
        cargo: '',
        dataInicio: '',
        dataFim: '',
        empregoAtual: false,
        descricao: ''
      };
      this.editandoIndexExperiencia = null;
    },
    resetarFormFormacao() {
      this.novaFormacao = {
        instituicao: '',
        curso: '',
        nivel: '',
        status: '',
        dataInicio: '',
        dataConclusao: ''
      };
      this.editandoIndexFormacao = null;
    },

    editarExperiencia(index) {
      const experiencia = this.curriculo.experiencias[index];

      const isEditingSameItem = this.editandoExperiencia && this.editandoIndexExperiencia === index;

      if (isEditingSameItem) {
            this.novaExperiencia = {
                cargo: '',
                empresa: '',
                dataInicio: '',
                dataFim: '',
                descricao: '',
                atual: false
            };
            this.editandoIndexExperiencia = null;
            this.editandoExperiencia = false;
            
      } else {
        this.novaExperiencia = {
         ...experiencia,
         dataInicio: experiencia.dataInicio?.split('T')[0] || '',
         dataFim: experiencia.dataFim?.split('T')[0] || ''
        };
        
        this.editandoIndexExperiencia = index;
        this.editandoExperiencia = true;
      }
      window.scrollTo({ top: 0, behavior: 'smooth' });
    },

    editarFormacao(index) {
      const formacao = this.curriculo.formacoes[index];

      const isEditingSameItem = this.editandoFormacao && this.editandoIndexFormacao === index;
      
      if(isEditingSameItem){
        this.novaFormacao = {
          instituicao: '',
          curso: '',
          nivel: '',
          status: '',
          dataInicio: '',
          dataConclusao: '',
        }
        this.editandoIndexFormacao = null;
        this.editandoFormacao = null;
      }
      else{
        this.novaFormacao = {
          ...formacao,
          dataInicio: formacao.dataInicio?.split('T')[0] || '',
          dataConclusao: formacao.dataConclusao?.split('T')[0] || ''
        };
        
        this.editandoIndexFormacao = index;
        this.editandoFormacao = true;
      }
      window.scrollTo({ top: 0, behavior: 'smooth' });
    },


    formatarTelefone(event) {
      let valor = event.target.value.replace(/\D/g, '');
      
      if (valor.length <= 11) {
        if (valor.length <= 10) {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        } else {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        }
      }
      this.telefoneFormatado = valor;
      this.user.telefone = valor.replace(/\D/g, ''); 
    },

    formatarTelefoneString(telefone) {
      if (!telefone) return '';
      
      let valor = telefone.toString().replace(/\D/g, '');
      
      if (valor.length <= 11) {
        if (valor.length <= 10) {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        } else {
          valor = valor.replace(/^(\d{2})(\d)/g, '($1) $2');
          valor = valor.replace(/(\d)(\d{4})$/, '$1-$2');
        }
      }
      
      return valor;
    },
    formatarDescricao(descricao) {
      if (!descricao) return '';

      descricao = descricao.trim();
      descricao = descricao.replace(/(?<!\.)([a-z])([A-Z])/g, '$1; $2');

      if (descricao.includes('\n')) {
          return descricao
              .split('\n')
              .map(item => item.trim())
              .filter(item => item.length > 0)
              .map(item => {
                  if (!/^[•\-\*]/.test(item)) {
                      return `• ${item}`;
                  }
                  return item.replace(/^([•\-\*])\s*/, '• ');
              })
              .join('<br>');
      }

      const marcadores = descricao.match(/[•\-\*]\s+/g);

      if (marcadores && marcadores.length >= 2) {
          return descricao
              .split(/[•\-\*]\s+/)
              .map(item => item.trim())
              .filter(item => item.length > 0)
              .map(item => `• ${item}`)
              .join('<br>');
      }

      return descricao;
  },
    formatarPeriodo(dataInicio, dataFim, empregoAtual) {
      const formatarData = (dataString) => {
        if (!dataString) return 'Data não informada';
        
        const dataLimpa = dataString.split('T')[0];
        const data = new Date(dataLimpa + 'T00:00:00');
        
        if (isNaN(data.getTime())) return 'Data inválida';
        
        return data.toLocaleDateString('pt-BR', { day: 'numeric', month: 'numeric', year: 'numeric' });
      };

      const inicio = formatarData(dataInicio);
      let fim;
      
      if (empregoAtual) {
        fim = 'Atual';
      } else {
        fim = dataFim ? formatarData(dataFim) : 'Não Informado';
      }

      return `${inicio} - ${fim}`;
    },
    nextStep() {
      if (this.step < 3) this.step++;
    },
    
    prevStep() {
      if (this.step > 1) {
        this.step--;
        window.scrollTo({
          top: 0,
          behavior: 'smooth' // Faz a subida ser suave
        });
      }
    },
    formatarDataAtual(dataRef) {
      const dataString = dataRef?.value || dataRef;
      
      if (!dataString) return '';
      
      const data = new Date(dataString);
      
      if (isNaN(data.getTime())) {
        return '';
      }
      
      const dia = String(data.getDate()).padStart(2, '0');
      const mes = String(data.getMonth() + 1).padStart(2, '0');
      const ano = data.getFullYear();
      
      return `${dia}/${mes}/${ano}`;
    },
    async adicionarExperiencia() {
      if (!this.novaExperiencia.descricao) {
        return this.mostrarErro('Preencha a descrição da atividade.');
      }
      else if (!this.novaExperiencia.dataInicio) {
        return this.mostrarErro('Preencha a data de início da experiência.');
      }
      else if(!this.novaExperiencia.empregoAtual && this.novaExperiencia.dataFim == ''){
        return this.mostrarErro('Preencha a data fim da experiência.');
      }
      else if(!this.novaExperiencia.empregoAtual && (this.novaExperiencia.dataFim < this.novaExperiencia.dataInicio)){
        return this.mostrarErro('Data fim deve ser posterior à data de início.');
      }

      try {
          let novaExp;
          
          if (this.editandoIndexExperiencia !== null) { 
              const expId = this.curriculo.experiencias[this.editandoIndexExperiencia].id;
              
              if (expId) {
                  await experienciaService.atualizarExperiencia(this.novaExperiencia);
              }
              
              this.curriculo.experiencias.splice(this.editandoIndexExperiencia, 1, { ...this.novaExperiencia });
              this.successMessage = 'Experiência atualizada!';

          } else {
              console.log(this.curriculo.id)
              if (this.curriculo.id) {
                  const experienciaComCurriculo = {
                      ...this.novaExperiencia,
                      dataFim: this.novaExperiencia.empregoAtual || !this.novaExperiencia.dataFim 
                          ? null 
                          : this.novaExperiencia.dataFim,
                      curriculoId: this.curriculo.id
                  };
                  
                  novaExp = await experienciaService.adicionarExperiencia(experienciaComCurriculo);
                  
                  this.curriculo.experiencias.push(novaExp);
              } else {
                  this.curriculo.experiencias.push({ ...this.novaExperiencia });
              }
              
              this.successMessage = 'Experiência adicionada!';
          }
          
          this.resetarFormExperiencia(); 
          
      } catch (error) {
          const apiResponse = error.response;
          if (apiResponse && apiResponse.status === 400) {
              const errorCode = apiResponse.data.code;
              
              if (errorCode === 'DataInicio_Futura'){
                  return this.mostrarErro('A data de início não pode ser posterior à data atual.');
              }
          } else {
              return this.mostrarErro('Erro ao salvar experiência. Verifique a conexão e o servidor.');
          }
          
      }
      
      setTimeout(() => this.successMessage = '', 2000);
    },
    removerExperiencia(index) {
      const experiencia = this.curriculo.experiencias[index];
      
      this.itemParaRemover = { 
        index, 
        tipo: 'experiencia',
        id: experiencia.id
      };
      
      this.modalConfig = {
        title: 'Excluir Experiência',
        message: 'Você tem certeza que deseja excluir esta experiência profissional?',
        type: 'perigo',
        confirmText: 'Sim, excluir'
      };
      
      this.showConfirmModal = true;
    },

    async adicionarFormacao() {
      if (!this.novaFormacao.instituicao) {
        return this.mostrarErro('Preencha a Nome da Instituição');
      } else if (!this.novaFormacao.curso) {
        return this.mostrarErro('Preencha o Curso');
      }

      try {
        const formacaoNormalizada = {
          ...this.novaFormacao,
          status:
            this.novaFormacao.status === true
              ? true
              : this.novaFormacao.status === false
              ? false
              : null
        };

        if (this.editandoIndexFormacao !== null) {
          const formId = this.curriculo.formacoes[this.editandoIndexFormacao].id;

          if (formId) {
            console.log("B")
            await formacaoService.atualizarFormacao(formacaoNormalizada);
          }

          this.curriculo.formacoes.splice(
            this.editandoIndexFormacao,
            1,
            { ...formacaoNormalizada }
          );

          this.successMessage = 'Formação atualizada!';
        } else {
          console.log(this.curriculoId)
          this.curriculoId = this.curriculo.id
          if (this.curriculoId) {
            const formacaoComCurriculo = {
              ...formacaoNormalizada,
              curriculoId: this.curriculoId
            };
            console.log("A")
            const novaForm = await formacaoService.adicionarFormacao(formacaoComCurriculo);
            this.curriculo.formacoes.push(novaForm);
          } else {
            this.curriculo.formacoes.push({ ...formacaoNormalizada });
          }

          this.successMessage = 'Formação adicionada!';
        }

        this.resetarFormFormacao();
      } catch (error) {
        const apiResponse = error.response;

        if (apiResponse && apiResponse.status === 400) {
          const errorCode = apiResponse.data.code;

          if (errorCode === 'DataInicio_Futura') {
            return this.mostrarErro('A data de início não pode ser posterior à data atual.');
          }
        } else {
          return this.mostrarErro('Erro ao salvar formação. Verifique a conexão e o servidor.');
        }
      }

      setTimeout(() => (this.successMessage = ''), 2000);
    },

    removerFormacao(index) {
      const formacao = this.curriculo.formacoes[index];
      
      this.itemParaRemover = { 
        index, 
        tipo: 'formacao',
        id: formacao.id
      };
      
      this.modalConfig = {
        title: 'Excluir Formação',
        message: 'Você tem certeza que deseja excluir esta formação acadêmica?',
        type: 'perigo',
        confirmText: 'Sim, excluir'
      };
      
      this.showConfirmModal = true;
    },

    async confirmarRemocao() {
      if (!this.itemParaRemover) return;

      const { index, tipo, id } = this.itemParaRemover;

      try {
        switch (tipo) {
          case 'experiencia':
            if (id) {
              await experienciaService.excluirExperiencia(id);
            }
            this.curriculo.experiencias.splice(index, 1);
            
            if (this.editandoIndexExperiencia === index) {
              this.resetarFormExperiencia();
            } else if (this.editandoIndexExperiencia !== null && index < this.editandoIndexExperiencia) {
              this.editandoIndexExperiencia--;
            }
            
            this.successMessage = 'Experiência removida!';
            break;
            
          case 'formacao':
            if (id) {
              await formacaoService.excluirFormacao(id);
            }
            this.curriculo.formacoes.splice(index, 1);
            
            if (this.editandoIndexFormacao === index) {
              this.resetarFormFormacao();
            } else if (this.editandoIndexFormacao !== null && index < this.editandoIndexFormacao) {
              this.editandoIndexFormacao--;
            }
            
            this.successMessage = 'Formação removida!';
            break;
        }

        setTimeout(() => this.successMessage = '', 2000);
      } catch (error) {
        return this.mostrarErro(`Erro ao excluir ${tipo}. Tente novamente.`);
      } finally {
        this.itemParaRemover = null;
        this.showConfirmModal = false;
      }
    },

    async formatarDescricaoComIA() {
      if (!this.novaExperiencia.descricao || this.novaExperiencia.descricao.trim().length < 10) {
        this.iaMessage = 'Digite pelo menos 10 caracteres';
        this.iaMessageType = 'info';
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
        return;
      }

      this.loadingIA = true;
      this.iaMessage = 'Melhorando descrição...';
      this.iaMessageType = 'info';

      try {
        const iaFormattingService = (await import('@/services/iaFormattingService')).default;
        
        const descricaoMelhorada = await iaFormattingService.formatarDescricaoProfissional(
          this.novaExperiencia.descricao,
          this.novaExperiencia.cargo,
          this.novaExperiencia.empresa
        );
        
        this.novaExperiencia.descricao = descricaoMelhorada;
        
        this.iaMessage = 'Descrição melhorada!';
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        this.iaMessage = 'Erro ao melhorar. Tente novamente.';
        this.iaMessageType = 'error';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
      } finally {
        this.loadingIA = false;
      }
    },

    async formatarObjetivoComIA() {
      if (!this.curriculo.objetivo || this.curriculo.objetivo.trim().length < 10) {
        this.iaMessage = 'Digite pelo menos 10 caracteres';
        this.iaMessageType = 'info';
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
        return;
      }

      this.loadingIA = true;
      this.iaMessage = 'Melhorando objetivo...';
      this.iaMessageType = 'info';

      try {
        const iaFormattingService = (await import('@/services/iaFormattingService')).default;
        
        const objetivoMelhorado = await iaFormattingService.formatarObjetivo(
          this.curriculo.objetivo
        );
        
        this.curriculo.objetivo = objetivoMelhorado;
        
        this.iaMessage = 'Objetivo melhorado!';
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        this.iaMessage = 'Erro ao melhorar. Tente novamente.';
        this.iaMessageType = 'error';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
      } finally {
        this.loadingIA = false;
      }
    },

    async salvarCurriculo() {
      try {
        const dadosParaEnviar = JSON.parse(JSON.stringify(this.curriculo));

        if (dadosParaEnviar.experiencias) {
          dadosParaEnviar.experiencias.forEach(exp => {
            if (!exp.dataFim || exp.dataFim === "") exp.dataFim = null;
            if (!exp.dataInicio || exp.dataInicio === "") exp.dataInicio = null;
          });
        }

        if (dadosParaEnviar.id) {
          await curriculoService.atualizarCurriculo(dadosParaEnviar);
          this.successMessage = 'Currículo atualizado com sucesso!';
        } else {
          const data = await curriculoService.adicionarCurriculo(dadosParaEnviar);
          this.curriculoId = data.id;
          this.successMessage = 'Currículo salvo com sucesso!';
        }
        this.$router.push(`/curriculo/visualizar/${this.curriculo.id || this.curriculoId}`);
      } catch (error) {
        console.error("Erro detalhado:", error.response?.data);
        return this.mostrarErro('Erro ao salvar currículo');
      }
    },
    async updateCurriculo() {
      try {
        await curriculoService.atualizarCurriculo(this.curriculo);
        this.successMessage = 'Currículo atualizado com sucesso!';
        setTimeout(() => {
          this.$router.push(`/curriculo/visualizar/${this.curriculo.id || this.curriculoId}`);
        }, 2000);
      } catch (error) {
        return this.mostrarErro('Erro ao salvar currículo');
      }
    },
    async continuarPerfil() {
    try {
        if (this.curriculo.id) {
            await curriculoService.atualizarCurriculo(this.curriculo);
            this.successMessage = 'Currículo atualizado com sucesso!';
            setTimeout(() => this.successMessage = '', 3000);
        } else {
            const data = await curriculoService.adicionarCurriculo(this.curriculo);
            this.curriculoId = data.id;
            this.curriculo.id = data.id;
            this.successMessage = 'Currículo salvo com sucesso!';
            setTimeout(() => this.successMessage = '', 3000);
        }
        this.curriculoOriginal = JSON.parse(JSON.stringify(this.curriculo));
        
        return true;
    } catch (error) {
        const apiResponse = error.response;
        if (apiResponse && apiResponse.status === 400) {
            const errorCode = apiResponse.data.code;
            if (errorCode === 'DataNascimento_Invalida'){
                return this.mostrarErro('A data de nascimento é inválida');
            }
        } else {
            return this.mostrarErro('Erro ao salvar experiência. Verifique a conexão e o servidor.');
        }
      }
  },
  nextStepPerfil() {
    this.step++;
    window.scrollTo({
      top: 0,
      behavior: 'smooth' // Faz a subida ser suave
    });
  },
  async voltarLogin() {
    try {
      await usuarioService.logout();
      
      await new Promise(resolve => setTimeout(resolve, 100));
      
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
      
    } catch (error) {
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
    }
  },
  abrirModal() {
    this.modalAberto = true;
  },
  abrirModalAviso() {
    this.modalAvisoAberto = true;
  },
  fecharModal() {
    this.modalAberto = false;
  },
  fecharModalAviso() {
    this.modalAvisoAberto = false;
  },
  async confirmarSair() {
    try {
      await usuarioService.logout();
      this.curriculo = null;
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
    } catch (error) {
      this.$router.replace('/login').then(() => {
        window.location.reload();
      });
    }
  },
  hasChanges() {
    return JSON.stringify(this.curriculo) !== JSON.stringify(this.curriculoOriginal);
  }
}
}
</script>

<style scoped>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

.wrapper {
  min-height: 100vh;
  display: flex;
  align-items: center;
  justify-content: center;
  background: #fafafa;
  padding: 40px 24px;
}

.container {
  background: white;
  border-radius: 16px;
  border: 1px solid #e5e7eb;
  padding: 48px;
  width: 100%;
  max-width: 720px;
  animation: fadeIn 0.4s ease-out;
}

.top-bar { display: flex; justify-content: space-between; align-items: center; margin-bottom: 24px; } .right-actions { display: flex; gap: 12px; align-items: center; }

.bottom-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.next-btn{
  display: flex;
  margin-left: auto;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(8px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.header {
  text-align: center;
  margin-bottom: 40px;
}

.logo-circle {
  width: 56px;
  height: 56px;
  background: #000;
  border-radius: 12px;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto 20px;
  color: white;
}

.header h1 {
  font-size: 24px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 8px;
  letter-spacing: -0.02em;
}

.header p {
  font-size: 14px;
  color: #6b7280;
}

.progress-bar {
  display: flex;
  align-items: center; /* Alinha pelo topo para o cálculo da linha ser fixo */
  justify-content: space-between;
  margin-bottom: 40px;
  padding: 0 10px; 
  box-sizing: border-box;
  width: 100%;
}

.progress-step {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 8px;
  min-width: 80px; /* Garante que cada etapa tenha uma área mínima para não "esmagar" o círculo */
  z-index: 1; /* Garante que o círculo fique acima da linha se eles se sobrepuserem */
}

.step-circle {
  width: 36px;
  height: 36px;
  border-radius: 50%;
  background: #f3f4f6;
  color: #9ca3af;
  display: flex;
  align-items: center;
  justify-content: center;
  font-weight: 600;
  font-size: 14px;
  transition: all 0.2s ease;
}

.progress-step.active .step-circle {
  background: #000;
  color: white;
}

.progress-step span {
  font-size: 11px;
  color: #9ca3af;
  font-weight: 500;
}

.progress-step.active span {
  color: #111827;
  font-weight: 600;
}

.progress-line {
  flex: 1;
  height: 2px;
  background: #e5e7eb;
  margin-bottom: 20px;
  transition: all 0.3s ease;
}

.progress-line.active {
  background: #000;
}

.step-content {
  animation: slideIn 0.3s ease-out;
}

@keyframes slideIn {
  from {
    opacity: 0;
    transform: translateX(8px);
  }
  to {
    opacity: 1;
    transform: translateX(0);
  }
}

.step-title {
  font-size: 20px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 24px;
  letter-spacing: -0.01em;
}

.form-card {
  background: #fafafa;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  padding: 24px;
  margin-bottom: 24px;
}

.form-group {
  margin-bottom: 18px;
}

.form-group div[style*="position: relative"] {
  display: flex;
  align-items: center;
}

.posicao-lateral {
  position: absolute;
  left: 580px;
  top: 50%;
  transform: translateY(-50%);
}

input:disabled { /* Fundo cinza bem claro */
  color: #999999;       /* Muda o cursor para um sinal de "proibido" */
  border: 1px solid #dddddd; /* Borda suave */
  opacity: 0.7;              /* Garante que pareça levemente desbotado */
}

.input-com-dois-icones {
  width: 100%; /* Espaço para Calendário/Dropdown + Microfone */
}


/* 2. O MÁGICO: Empurra o ícone nativo do calendário para a esquerda */
.input-com-dois-icones::-webkit-calendar-picker-indicator {
  /* Move o ícone do calendário para longe da borda direita, 
     dando espaço para o microfone entrar */
  margin-right: 25px; 
  cursor: pointer;
  opacity: 0.3;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
  align-items: start; /* Garante que os labels fiquem alinhados no topo */
}

label {
  display: block;
  font-size: 13px;
  font-weight: 500;
  color: #374151;
  margin-bottom: 8px;
}

input, select {
  width: 100%;
  padding: 10px 12px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  font-family: inherit;
}

textarea {
  width: 100%;
  padding: 10px 50px 10px 12px;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 14px;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  font-family: inherit;
  resize: vertical;
  min-height: 80px;
}

.btn-microfone {
  position: absolute;
  right: 8px; /* Ajuste este valor para afastar da borda direita do input */
  top: 50%;   /* Centraliza verticalmente */
  transform: translateY(-50%); /* Ajuste fino para a centralização vertical */
  
  width: 36px;
  height: 36px;
  background: transparent;
  border: none;
  border-radius: 0;
  font-size: 1.1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
  padding: 0;
  opacity: 0.6;
}

.btn-microfone img {
  width: 20px;
  height: 20px;
  display: block;
}

.btn-microfone:hover {
  opacity: 1;
  transform: translateY(-50%) scale(1.1); /* Mantém o alinhamento vertical com o scale */
}

.btn-microfone:hover svg {
  color: #475569;
}

.btn-microfone.gravando {
  opacity: 1;
  animation: pulse-icon 1.5s ease-in-out infinite;
}

.btn-microfone.gravando svg {
  color: #ef4444; /* ← Vermelho quando grava */
}

@keyframes pulse-icon {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.15);
  }
}

.btn-microfone:focus {
  outline: none;
}

.btn-microfone:active {
  transform: scale(0.95);
}

/* Transcrição em tempo real */
.transcricao-tempo-real {
  margin-top: 10px;
  padding: 10px 12px;
  background: #e3f2fd;
  border: 1px dashed #1976d2;
  border-radius: 8px;
  animation: fadeInAudio 0.3s ease;
}

.transcricao-tempo-real small {
  color: #1565c0;
  font-style: italic;
  font-size: 13px;
  display: block;
}

/* Erro de áudio */
.erro-audio {
  margin-top: 10px;
  padding: 10px 12px;
  background: #ffebee;
  border: 1px solid #ef5350;
  border-radius: 8px;
  animation: fadeInAudio 0.3s ease;
}

.erro-audio small {
  color: #c62828;
  font-size: 13px;
  display: block;
}

@keyframes fadeInAudio {
  from {
    opacity: 0;
    transform: translateY(-4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

/* Responsivo */
@media (max-width: 768px) {
  .btn-microfone {
    width: 32px;
    height: 32px;
    font-size: 1rem;
  }
  
  textarea {
    padding-right: 45px;
  }
}

input:focus, select:focus, textarea:focus {
  outline: none;
  border-color: #000;
  box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.05);
}

input::placeholder, textarea::placeholder {
  color: #9ca3af;
}

.checkbox-wrapper {
  display: flex;
  align-items: center;
  gap: 8px;
  margin-top: 10px;
}

.checkbox-wrapper input[type="checkbox"] {
  width: 18px;
  height: 18px;
  cursor: pointer;
}

.checkbox-wrapper label {
  margin: 0;
  cursor: pointer;
  font-size: 13px;
}

.button-container {
  position: relative;
  width: 100%;
}

.btn-primary {
  width: 100%;
  padding: 12px 60px 12px 12px; /* Padding extra à direita para o botão de áudio */
  background: #000;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 50px;
  position: relative;
}

.btn-text {
  flex: 1;
  margin-left: 50px;
}

.btn-audio-inline {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  z-index: 2;
}

.btn-audio {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  background: transparent;
  border: none;
  cursor: pointer;
  padding: 8px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 50%;
  color: #ffffff;
  transition: all 0.2s;
  z-index: 2;
  min-width: 40px;
  min-height: 40px;
}

.btn-audio:hover {
  background: rgba(255, 255, 255, 0.15);
}

.btn-audio:active {
  transform: translateY(-50%) scale(0.95);
}

.btn-audio:active {
  transform: translateY(-50%) scale(0.95);
}

.btn-primary:hover:not(:disabled) {
  background: #1f2937;
}

.btn-primary:active:not(:disabled) {
  transform: scale(0.98);
}

.btn-primary:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-secondary {
  padding: 12px;
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 15px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
}

.btn-secondary:hover {
  background: #fafafa;
}

.btn-add {
  width: 100%;
  padding: 10px;
  background: #000;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  transition: all 0.2s ease;
}

.btn-add:hover {
  background: #1f2937;
}

.btn-back{
  padding: 10px 20px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
  left: auto;
}

.btn-back:hover {
  background: #f9fafb;
}

.btn-ia {
  position: absolute;
  right: 5px; /* Ajuste este valor para afastar da borda direita do input */
  top: 20%;   /* Centraliza verticalmente */
  transform: translateY(-50%); /* Ajuste fino para a centralização vertical */
  
  width: 36px;
  height: 36px;
  background: transparent;
  border: none;
  border-radius: 0;
  font-size: 1.1rem;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 10;
  padding: 0;
  opacity: 0.6;
}

.icon-update {
  filter: brightness(0) invert(1);
}

.icon-update {
  width: 16px;
  height: 16px;
}

.icon-ia{
  opacity: 0.2;
}

.btn-ia:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.loading-spinner-small {
  width: 16px;
  height: 16px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

.ia-message {
  display: block;
  margin-top: 10px;
  font-size: 12px;
  font-weight: 500;
  padding: 8px 12px;
  border-radius: 6px;
}

.ia-message.success {
  background: #f0fdf4;
  color: #166534;
}

.ia-message.error {
  background: #fef2f2;
  color: #991b1b;
}

.ia-message.info {
  background: #eff6ff;
  color: #1e40af;
}

.button-group {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 12px;
  margin-top: 24px;
}

.item-list {
  margin-top: 20px;
  margin-bottom: 20px;
  display: flex;
  flex-direction: column;
  gap: 12px;
}

.item-card {
  background: white;
  border: 1px solid #e5e7eb;
  border-radius: 10px;
  padding: 16px;
  position: relative;
  animation: fadeIn 0.3s ease;
}

.item-card h4 {
  color: #111827;
  font-size: 15px;
  font-weight: 600;
  margin-bottom: 6px;
  padding-right: 70px;
}

.item-card .subtitle {
  color: #6b7280;
  font-size: 14px;
  margin-bottom: 4px;
}

.status{
  color: #6b7280;
  font-size: 12px;
  margin-bottom: 4px;
}

.item-card .date {
  color: #9ca3af;
  font-size: 13px;
  margin-bottom: 8px;
}

.item-card .description {
  color: #6b7280;
  font-size: 13px;
  line-height: 1.5;
}

.btn-remove {
  position: absolute;
  top: 12px;
  right: 12px;
  width: 28px;
  height: 28px;
  background: #fef2f2;
  color: #991b1b;
  border: none;
  border-radius: 6px;
  font-size: 20px;
  line-height: 1;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-edit {
  position: absolute;
  top: 12px;
  right: 50px;
  width: 28px;
  height: 28px;
  background: #f9f9f9;
  color: #000000;
  border: none;
  border-radius: 6px;
  font-size: 12px;
  line-height: 1;
  cursor: pointer;
  transition: all 0.2s;
  display: flex;
  align-items: center;
  justify-content: center;
}

.btn-remove:hover {
  background: #fecaca;
}

.btn-edit:hover {
  background: #e5e7eb;
}

.loading-spinner {
  width: 18px;
  height: 18px;
  border: 2px solid rgba(255, 255, 255, 0.3);
  border-top-color: white;
  border-radius: 50%;
  animation: spin 0.6s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.alert {
  padding: 12px 14px;
  border-radius: 8px;
  font-size: 14px;
  margin-top: 16px;
  animation: slideDown 0.3s ease;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translateY(-4px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.alert-success {
  background: #f0fdf4;
  color: #166534;
  border: 1px solid #bbf7d0;
}

.alert-error {
  background: #fef2f2;
  color: #991b1b;
  border: 1px solid #fecaca;
}

@media (max-width: 728px) {
  .wrapper {
    padding: 0; /* Remove o respiro externo para o card encostar na lateral se necessário */
    background: rgb(255, 255, 255); /* Opcional: tira o fundo cinza para parecer um app nativo */
    align-items: flex-start; /* Alinha o conteúdo no topo */
  }

  .container {
    padding: 20px; /* Reduz drasticamente o padding interno */
    border-radius: 0; /* Remove arredondamento para ocupar os cantos da tela */
    border: none; /* Remove a borda para um visual mais limpo */
    max-width: 100vw; /* Garante 100% da largura da visualização */
    min-height: 100vh; /* Faz o branco ocupar a altura toda do celular */
  }
  
  /* Ajuste para que os inputs fiquem confortáveis */
  input, select, textarea {
    font-size: 16px; /* Evita que o iOS dê zoom automático ao clicar */
  }

  .header h1 {
    font-size: 22px;
  }

  .form-row {
    grid-template-columns: 1fr; /* Um campo por linha no celular */
    gap: 0; /* O margin-bottom do form-group já cuidará do espaço */
  }

  .button-group {
    grid-template-columns: 1fr;
  }

  .progress-bar {
    /* No mobile, talvez precise de um pouco mais de respiro */
    padding: 0 25px;
  }

  .progress-step span {
    font-size: 10px;
  }

  .step-circle {
    width: 32px;
    height: 32px;
    font-size: 13px;
  }
}
</style>