<template>
  <div class="wrapper">
    <ModalCarregamento :isOpen="loading" />
    <div class="container">
      <div class="top-bar">
        <LogoutButton @logout="abrirModal" />
        <div class="right-actions">
          <BotaoDescricao @toggle="handleTutorialToggle" :active="mostrarTutorial"/>
        </div>
      </div>
      <div class="header">
        <div class="logo-circle">
          <img src="@/assets/icons/educationIcon.svg" alt="Atualizar" class="icon-education" />
        </div>
        <h1 @click="falarElemento">{{ modoEdicao ? 'Editar Currículo' : 'Criar Currículo' }}</h1>
        <p @click="falarElemento">Preencha seus dados profissionais</p>
      </div>

      <div class="progress-bar">
        <div :class="['progress-step', { active: step >= 1 }]">
          <div class="step-circle" @click="falarTexto('Sobre você')">1</div>
          <span @click="falarElemento">Sobre você</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 2 }"></div>
        <div :class="['progress-step', { active: step >= 2 }]">
          <div class="step-circle" @click="falarTexto('Trabalhos')">2</div>
          <span @click="falarElemento">Trabalhos</span>
        </div>
        <div class="progress-line" :class="{ active: step >= 3 }"></div>
        <div :class="['progress-step', { active: step >= 3 }]">
          <div class="step-circle" @click="falarTexto('Estudos')">3</div>
          <span @click="falarElemento">Estudos</span>
        </div>
      </div>

      <div v-if="step === 1" class="step-content">
        <h2 class="step-title" @click="falarElemento">Sobre você</h2>
        <div class="form-group">
          <label @click="falarElemento">Nome Completo</label>
          <form @submit.prevent="salvarCurriculo">
            <div style="position: relative;">
              <input type="text" v-model="curriculo.nomeCompleto" :class="{'input-erro': erroNome}" required placeholder="Seu nome todo..." @click="garantirVisibilidade"/>
              <span v-if="erroNome" class="msg-erro">O nome é obrigatório!</span>
              <BotaoMicrofone 
                :isRecording="camposGravando.nomeCompleto" 
                @toggle="toggleGravacao('nomeCompleto', curriculo)"
              />
            </div>
          </form>
        </div>

        <div class="form-group">
          <label @click="falarElemento">Telefone</label>
          <input 
            type="tel" 
            v-model="curriculo.telefone" 
            @input="formatarTelefone"
            placeholder="(XX) XXXXX-XXXX"
            maxlength="15"
            disabled
          />
        </div>

        <div class="form-row">
          <div class="form-group">
            <label @click="falarElemento">Estado (opcional)</label>
            <div style="position: relative;">
              <EstadoDropdown v-model="curriculo.estado" class="input-com-dois-icones"/>
              <BotaoMicrofone 
                  :isRecording="camposGravando.estado" 
                  @toggle="toggleGravacao('estado', curriculo)"
              />
            </div>
          </div>
          <div class="form-group">
            <label @click="falarElemento">Cidade (opcional)</label>
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
            <label @click="falarElemento">Objetivo Profissional (opcional)</label>
              <div style="position: relative;">
                <textarea 
                  v-model="curriculo.objetivo" 
                  rows="6" 
                  required
                  placeholder="No que você quer trabalhar?"
                  @click="garantirVisibilidade"
                  ></textarea>
                <BotaoMicrofone 
                  :isRecording="camposGravando.objetivo"
                  @toggle="toggleGravacao('objetivo', curriculo)"
                />
                <button type="button" @click="formatarObjetivoComIA" class="btn-ia" :disabled="loadingIA || !curriculo.objetivo">
                  <img v-if="!loadingIA" src="@/assets/icons/botSparkleIcon.svg" alt="IA" class="icon-ia" />
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
        <h2 class="step-title" @click="falarElemento">Trabalhos (opcional)</h2>
        <div class="form-card">
          <div class="form-group">
            <label @click="falarElemento">Descrição das Atividades</label>
              <div style="position: relative;">
                <textarea 
                  v-model="novaExperiencia.descricao" 
                  rows="6" 
                  required
                  placeholder="O que você fazia?"
                  @click="garantirVisibilidade"
                ></textarea>
                <BotaoMicrofone 
                  :isRecording="camposGravando.descricao"
                  @toggle="toggleGravacao('descricao', novaExperiencia)"
                />
                <button type="button" @click="formatarDescricaoComIA" class="btn-ia" :disabled="loadingIA || !novaExperiencia.descricao">
                  <img v-if="!loadingIA" src="@/assets/icons/botSparkleIcon.svg" alt="IA" class="icon-ia" />
                  <span v-if="loadingIA" class="loading-spinner-small"></span>
                </button>
              </div>
            
            <small v-if="iaMessage" :class="['ia-message', iaMessageType]">{{ iaMessage }}</small>
          </div>
          <div class="form-group">
            <label @click="falarElemento">Empresa (opcional)</label>
            <div style="position: relative;">
              
              <input 
                type="text" 
                v-model="novaExperiencia.empresa" 
                style="width: 100%;" 
                placeholder="Onde você trabalhou?"
                @click="garantirVisibilidade"
              />
              
              <BotaoMicrofone 
                :isRecording="camposGravando.empresa" 
                @toggle="toggleGravacao('empresa', novaExperiencia)"
              />
              
            </div>
          </div>

          <div class="form-group">
            <label @click="falarElemento">Cargo (opcional)</label>
            <div style="position: relative;">
              <input type="text" v-model="novaExperiencia.cargo" placeholder="Trabalhou como o quê?" @click="garantirVisibilidade"/>
              <BotaoMicrofone :isRecording="camposGravando.cargo" @toggle="toggleGravacao('cargo', novaExperiencia)"/>
            </div>
          </div>
          
          <div class="form-row">
            <div class="form-group">
              <label @click="falarElemento">Data Início</label>
              
              <div style="position: relative;">
                <input 
                  type="text"
                  inputmode="numeric"
                  :value="formatarDataParaBR(novaExperiencia.dataInicio)"
                  @input="handleInputDataInicio"
                  @focus="handleFocus"
                  @blur="validarDataInicio"
                  placeholder="DD/MM/AAAA"
                  maxlength="10"
                  class="input-com-dois-icones"
                />
                <span class="icone-calendario"> 
                  <img src="@/assets/icons/calendarIcon.svg" alt="Calendar"/>
                </span>
                <BotaoMicrofone 
                  :isRecording="camposGravando.dataInicio" 
                  @toggle="toggleGravacao('dataInicio', novaExperiencia)"
                />
              </div>
              <span v-if="erroDataInicio" class="msg-erro">{{ mensagemErroDataInicio }}</span>
            </div>
            <div class="form-group">
              <label @click="falarElemento">Data Fim</label>
              <div style="position: relative;"> 
                <input 
                  v-if="!novaExperiencia.empregoAtual"
                  type="text"
                  inputmode="numeric"
                  :value="formatarDataParaBR(novaExperiencia.dataFim)"
                  @input="handleInputDataFim"
                  @focus="handleFocus"
                  @blur="validarDataFim"
                  placeholder="DD/MM/AAAA"
                  maxlength="10"
                  class="input-com-dois-icones"
                />
                <input 
                  v-else
                  type="text" 
                  value="Trabalho atual"
                  class="input-com-dois-icones"
                  disabled
                />
                <span class="icone-calendario"> 
                  <img src="@/assets/icons/calendarIcon.svg" alt="Calendar" />
                </span>
                <BotaoMicrofone 
                  :isRecording="camposGravando.dataFim" 
                  @toggle="toggleGravacao('dataFim', novaExperiencia)"
                />
              </div>
              <span v-if="erroDataFim" class="msg-erro">{{ mensagemErroDataFim }}</span>
              
              <div class="checkbox-wrapper">
                <input type="checkbox" id="empregoAtual" v-model="novaExperiencia.empregoAtual" />
                <label for="empregoAtual" style="margin: 0;" @click="falarElemento">Trabalho aqui atualmente</label>
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
            <h4 @click="falarElemento">{{ exp.cargo ? exp.cargo : "Não Informado"}}</h4>
            <p class="subtitle" @click="falarElemento">{{ exp.empresa ? exp.empresa : "Não Informado"}}</p>
            <p class="date" @click="falarElemento">{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</p>
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
        <h2 class="step-title" @click="falarElemento">Estudos (opcional)</h2>

        <div class="form-card">
          <div class="form-group">
            <label @click="falarElemento">Instituição</label>
            <div style="position: relative;">
              <input type="text" v-model="novaFormacao.instituicao" placeholder="Nome da escola ou universidade..." @click="garantirVisibilidade"/>
              <BotaoMicrofone 
                :isRecording="camposGravando.instituicao" 
                @toggle="toggleGravacao('instituicao', novaFormacao)"
              />
            </div>
          </div>
          <div class="form-group">
            <label @click="falarElemento">Curso</label>
            <div style="position: relative;">
              <input type="text" v-model="novaFormacao.curso" placeholder="Ex: Ensino Médio, Administração..." @click="garantirVisibilidade"/>
              <BotaoMicrofone 
                :isRecording="camposGravando.curso" 
                @toggle="toggleGravacao('curso', novaFormacao)"
              />
            </div>
          </div>

          <div class="checkbox-wrapper" style="margin-bottom: 15px;">
            <input type="checkbox" id="naoConcluiu" v-model="novaFormacao.status" />
            <label for="naoConcluiu" style="margin: 0;" @click="falarElemento">Curso incompleto?</label>
          
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
            <h4 @click="falarElemento">{{ form.curso }} • {{ form.status === true ? 'Incompleto' : 'Completo' }}</h4>
            <p class="subtitle" @click="falarElemento">{{ form.instituicao }}</p>
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
      @falar="falarTexto"
    />
    <ModalEncerramentoSessao
      :isOpen="modalAberto"
      @confirmar="confirmarSair"
      @fechar="fecharModal"
      @falar="falarTexto"/>
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
import ModalEncerramentoSessao from '@/components/ModalEncerramentoSessao.vue';
import LogoutButton from '@/components/LogoutButton.vue';
import BotaoMicrofone from '@/components/BotaoMicrofone.vue';
import BotaoDescricao from '@/components/BotaoDescricao.vue';
import BotaoProximo from '@/components/BotaoProximo.vue';
import { ibgeService } from '@/services/ibgeService';
import BackButton from '@/components/BackButton.vue';
import SaveButton from '@/components/SaveButton.vue';
import CidadeDropdown from '@/components/CidadeDropdown.vue';
import ModalCarregamento from '@/components/ModalCarregamento.vue';

export default {
  name: 'CurriculoForm',
  
  components: {
    EstadoDropdown,
    ModalExclusao,
    ModalEncerramentoSessao,
    ModalCarregamento,
    LogoutButton,
    BotaoMicrofone,
    BotaoDescricao,
    BotaoProximo,
    BackButton,
    SaveButton,
    CidadeDropdown,
  },
  data() {
    return {
      step: 1,
      erroDataInicio: false,
      mensagemErroDataInicio: '',
      erroDataFim: false,
      mensagemErroDataFim: '',
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
      erroNome: false,
      itemParaRemover: null,
      mostrarTutorial: false,
      audioTutorial: null,
      exibirModalPermissao: false,
      funcaoPendente: null,
      camposGravando: {
        descricao: false,
        empresa: false,
        cargo: false,
        instituicao: false,
        curso: false,
        estado: false,
        nomeCompleto: false,
        email: false,
        cidade: false,
        dataInicio: false,
        dataFim: false,
        objetivo: false
      },
      gravandoDataFim: false,
      gravandoDataInicioExperiencia: false,
      transcricaoAtual: '',
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
        objetivo: '',
        experiencias: [],
        formacoes: []
      },
      
      cidades: [],
      carregandoCidades: false,

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
  mounted() {
    if (this.mostrarTutorial) {
      this.executarBoasVindasNativo();
    }
    const estadoSalvo = localStorage.getItem('audioDescricaoAtiva');

    if (estadoSalvo !== null) {
      this.mostrarTutorial = estadoSalvo === 'true';
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
        
        if (oldValue) {
          this.curriculo.cidade = ''; 
        }
        
        try {
          this.cidades = await ibgeService.listarCidades(newValue);
        } catch (error) {
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
          ...this.curriculo,
          ...curriculoExistente,
          experiencias: exps || [],
          formacoes: forms || []
        };
      } else {
        this.curriculo.experiencias = [];
        this.curriculo.formacoes = [];
      }
    } catch (error) {
      this.curriculo.experiencias = [];
      this.curriculo.formacoes = [];
    } finally {
      this.loading = false; 
    }

    if (this.$route.params.id) {
      this.modoEdicao = true;
      this.curriculoId = this.$route.params.id;
    }

    this.curriculoOriginal = JSON.parse(JSON.stringify(this.curriculo));
  },
  methods: {
    formatarDataParaBR(dataISO) {
      if (!dataISO) return '';
      
      if (dataISO.includes('/')) return dataISO;
      
      if (dataISO.includes('-')) {
        const [ano, mes, dia] = dataISO.split('-');
        return `${dia}/${mes}/${ano}`;
      }
      
      return dataISO;
    },
    
    handleInputDataInicio(event) {
      let v = event.target.value.replace(/\D/g, '');
      let finalValue = '';

      if (v.length >= 1) {
        let dia = v.substring(0, 2);
        if (dia.length === 1 && parseInt(dia) > 3) {
          dia = '0' + dia;
          v = dia + v.substring(1);
        } else if (dia.length === 2) {
          if (parseInt(dia) > 31) dia = '31';
          if (parseInt(dia) === 0) dia = '01';
        }
        finalValue = dia;
      }

      if (v.length >= 3) {
        let mes = v.substring(2, 4);
        if (mes.length === 1 && parseInt(mes) > 1) {
          mes = '0' + mes;
          v = v.substring(0, 2) + mes + v.substring(3);
        } else if (mes.length === 2) {
          if (parseInt(mes) > 12) mes = '12';
          if (parseInt(mes) === 0) mes = '01';
        }
        finalValue += '/' + mes;
      }

      if (v.length >= 5) {
        let ano = v.substring(4, 8);
        finalValue += '/' + ano;
      }

      event.target.value = finalValue;

      if (v.length >= 4) {
        const d = parseInt(v.substring(0, 2));
        const m = parseInt(v.substring(2, 4));
        
        const diasNoMes = new Date(2024, m, 0).getDate();
        if (d > diasNoMes && m !== 0) {
          const diaCorrigido = String(diasNoMes).padStart(2, '0');
          event.target.value = diaCorrigido + finalValue.substring(2);
          v = diaCorrigido + v.substring(2);
        }
      }

      if (v.length === 8) {
        const dia = v.slice(0, 2);
        const mes = v.slice(2, 4);
        const ano = v.slice(4, 8);
        this.novaExperiencia.dataInicio = `${ano}-${mes}-${dia}`;
        this.erroDataInicio = false;
      } else {
        this.novaExperiencia.dataInicio = '';
      }
    },
    
    validarDataInicio() {
      const valorAtual = this.formatarDataParaBR(this.novaExperiencia.dataInicio);
      
      if (valorAtual && valorAtual.length > 0 && valorAtual.length < 10) {
        this.erroDataInicio = true;
        this.mensagemErroDataInicio = 'Data incompleta';
      }
    },
    
    handleInputDataFim(event) {
      let v = event.target.value.replace(/\D/g, '');
      let finalValue = '';

      if (v.length >= 1) {
        let dia = v.substring(0, 2);
        if (dia.length === 1 && parseInt(dia) > 3) {
          dia = '0' + dia;
          v = dia + v.substring(1);
        } else if (dia.length === 2) {
          if (parseInt(dia) > 31) dia = '31';
          if (parseInt(dia) === 0) dia = '01';
        }
        finalValue = dia;
      }

      if (v.length >= 3) {
        let mes = v.substring(2, 4);
        if (mes.length === 1 && parseInt(mes) > 1) {
          mes = '0' + mes;
          v = v.substring(0, 2) + mes + v.substring(3);
        } else if (mes.length === 2) {
          if (parseInt(mes) > 12) mes = '12';
          if (parseInt(mes) === 0) mes = '01';
        }
        finalValue += '/' + mes;
      }

      if (v.length >= 5) {
        let ano = v.substring(4, 8);
        finalValue += '/' + ano;
      }

      event.target.value = finalValue;

      if (v.length >= 4) {
        const d = parseInt(v.substring(0, 2));
        const m = parseInt(v.substring(2, 4));
        
        const diasNoMes = new Date(2024, m, 0).getDate();
        if (d > diasNoMes && m !== 0) {
          const diaCorrigido = String(diasNoMes).padStart(2, '0');
          event.target.value = diaCorrigido + finalValue.substring(2);
          v = diaCorrigido + v.substring(2);
        }
      }

      if (v.length === 8) {
        const dia = v.slice(0, 2);
        const mes = v.slice(2, 4);
        const ano = v.slice(4, 8);
        this.novaExperiencia.dataFim = `${ano}-${mes}-${dia}`;
        this.erroDataFim = false;
      } else {
        this.novaExperiencia.dataInicio = '';
      }
    },
    
    validarDataFim() {
      const valorAtual = this.formatarDataParaBR(this.novaExperiencia.dataFim);
      
      if (valorAtual && valorAtual.length > 0 && valorAtual.length < 10) {
        this.erroDataFim = true;
        this.mensagemErroDataFim = 'Data incompleta';
      }
    },
    
    validarDataFim() {
      const valorAtual = this.formatarDataParaBR(this.novaExperiencia.dataFim);
      
      if (valorAtual && valorAtual.length > 0 && valorAtual.length < 10) {
        this.erroDataFim = true;
        this.mensagemErroDataFim = 'Data incompleta';
      }
    },
    
    handleFocus(event) {
      setTimeout(() => {
        event.target.scrollIntoView({
          behavior: 'smooth',
          block: 'center'
        });
      }, 300);
    },
    
    validarData(dia, mes, ano) {
      const diaNum = parseInt(dia);
      const mesNum = parseInt(mes);
      const anoNum = parseInt(ano);
      
      if (diaNum < 1 || diaNum > 31) return false;
      if (mesNum < 1 || mesNum > 12) return false;
      if (anoNum < 1900 || anoNum > new Date().getFullYear() + 10) return false;
      
      const diasPorMes = [31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
      if (diaNum > diasPorMes[mesNum - 1]) return false;
      
      if (mesNum === 2 && diaNum === 29) {
        const bissexto = (anoNum % 4 === 0 && anoNum % 100 !== 0) || (anoNum % 400 === 0);
        if (!bissexto) return false;
      }
      
      return true;
    },

    handleTutorialToggle(ativo) {
      this.mostrarTutorial = ativo;
      localStorage.setItem('audioDescricaoAtiva', ativo);
      if (ativo) {
        this.executarBoasVindasNativo();
      } else {
        this.pararAudioTutorial();
      }
    },
    falarElemento(event) {
      const texto = event.target.innerText;
      this.falarTexto(texto);
    },
    falarTexto(texto) {
      if (!this.mostrarTutorial) return;

      if (!window.speechSynthesis) return;

      window.speechSynthesis.cancel();

      const utterance = new SpeechSynthesisUtterance(texto);
      utterance.lang = 'pt-BR';
      utterance.rate = 1.1;

      const voices = window.speechSynthesis.getVoices();
      const googleVoice = voices.find(v => v.lang === 'pt-BR' && v.name.includes('Google'));
      if (googleVoice) utterance.voice = googleVoice;

      window.speechSynthesis.speak(utterance);
    },

    executarBoasVindasNativo() {
      if (!window.speechSynthesis) return;
      window.speechSynthesis.cancel();

      const texto = "Descrição por áudio habilitada. Clique nos títulos para ouví-los";
      this.audioTutorial = new SpeechSynthesisUtterance(texto);
      this.audioTutorial.lang = 'pt-BR';
      
      this.audioTutorial.rate = 1.1;
      this.audioTutorial.pitch = 1.0;

      const selecionarMelhorVoz = () => {
        const vozes = window.speechSynthesis.getVoices();
        
        const melhorVoz = vozes.find(v => 
          v.lang === 'pt-BR' && 
          (v.name.includes('Google') || v.name.includes('Neural') || v.name.includes('Natural'))
        );

        if (melhorVoz) {
          this.audioTutorial.voice = melhorVoz;
        }
        
        window.speechSynthesis.speak(this.audioTutorial);
      };

      if (window.speechSynthesis.getVoices().length === 0) {
        window.speechSynthesis.onvoiceschanged = selecionarMelhorVoz;
      } else {
        selecionarMelhorVoz();
      }
    },

    pararAudioTutorial() {
      if (window.speechSynthesis) {
        window.speechSynthesis.cancel();
      }
    },

    beforeUnmount() {
      this.pararAudioTutorial();
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
          this.erroAudio = 'Navegador incompatível. Use Chrome ou Safari atualizados.';
          return;
        }

        if (this.recognition) {
          this.recognition.abort();
        }

        this.recognition = new SpeechRecognition();
        
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = false;
        this.recognition.maxAlternatives = 1;

        this.recognition.onstart = () => {
          this.$set(this.camposGravando, fieldName, true);
          this.erroAudio = null;
        };

        this.recognition.onresult = (event) => {
          const transcript = event.results[0][0].transcript.trim();
          
          if (transcript) {
            this.processarTranscricao(fieldName, objeto, transcript);
          }
        };

        this.recognition.onerror = (event) => {
          this.$set(this.camposGravando, fieldName, false);
          
          const errorMessages = {
            'no-speech': 'Não ouvi nada. Fale mais perto do microfone.',
            'not-allowed': 'Acesse as configurações do celular e permita o microfone.',
            'service-not-allowed': 'Serviço de voz do Google/Apple indisponível.',
            'network': 'Sinal de internet fraco para reconhecimento.'
          };
          
          this.erroAudio = errorMessages[event.error] || `Erro: ${event.error}`;
        };

        this.recognition.onend = () => {
          this.$set(this.camposGravando, fieldName, false);
        };
        setTimeout(() => {
          this.recognition.start();
        }, 100);

      } catch (error) {
        this.erroAudio = 'Erro ao iniciar microfone.';
        this.$set(this.camposGravando, fieldName, false);
      }
    },

    processarTranscricao(fieldName, objeto, transcript) {
      if (['dataInicio', 'dataFim'].includes(fieldName)) {
        const dataFormatada = this.interpretarDataFalada(transcript);
        
        if (dataFormatada) {
          objeto[fieldName] = dataFormatada;
          this.falarTexto("Data gravada");
        } else {
          this.mostrarErro(`Não entendi a data. Fale o dia, mês e ano.`);
          this.falarTexto("Não entendi a data. Tente falar o dia, o mês e o ano.");
        }
      } 
      else if (fieldName === 'estado') {
        const sigla = this.converterEstadoParaSigla(transcript);
        if (sigla) objeto[fieldName] = sigla;
      } 
      // Caso 3: Cidade
      else if (fieldName === 'cidade') {
        const cidadeValida = this.validarEConverterCidade(transcript);
        if (cidadeValida) objeto[fieldName] = cidadeValida;
      }
      else {
        objeto[fieldName] = this.capitalizeText(transcript);
      }
    },
    capitalizeText(text) {
      if (!text) return '';
      const trimmed = text.trim();
      return trimmed.charAt(0).toUpperCase() + trimmed.slice(1);
    },
    
    converterEstadoParaSigla(nomeEstado) {
      const estadosMap = {
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
        'para': 'PA',
        'parà': 'PA',
        'parar': 'PA',
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
      
      if (!sigla) {
        return null;
      }
      
      return sigla;
    },
    
    validarEConverterCidade(nomeCidade) {
      if (!this.curriculo.estado) {
        this.erroAudio = 'Selecione um estado antes de selecionar a cidade.';
        return null;
      }
      
      const cidadeEncontrada = this.cidades.find(c => 
        c.nome.toLowerCase() === nomeCidade.trim().toLowerCase()
      );
      
      if (!cidadeEncontrada) {
        this.erroAudio = `"${nomeCidade}" não é uma cidade válida em ${this.curriculo.estado}. Tente falar o nome de uma cidade do estado.`;
        return null;
      }
      
      return cidadeEncontrada.nome;
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
          this.erroAudio = 'Navegador incompatível. Use Chrome ou Safari.';
          return;
        }

        await navigator.mediaDevices.getUserMedia({ audio: true });

        if (this.recognition) this.recognition.abort();

        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = false;

        this.recognition.onstart = () => {
          this.gravandoDataInicioExperiencia = true;
          this.erroAudio = null;
          if (navigator.vibrate) navigator.vibrate(50);
        };

        this.recognition.onresult = (event) => {
          const transcript = event.results[0][0].transcript.toLowerCase().trim();
          
          const dataFormatada = this.interpretarDataFalada(transcript);

          if (dataFormatada) {
            this.novaExperiencia.dataInicio = dataFormatada;
            const [ano, mes, dia] = dataFormatada.split('-');
            this.valorVisualDataInicio = `${dia}/${mes}/${ano}`;
          } else {
            this.erroAudio = `Não entendi "${transcript}". Fale algo como "10 de agosto de 2010".`;
          }
        };

        this.recognition.onerror = (event) => {
          this.gravandoDataInicioExperiencia = false;
          const mensagens = {
            'no-speech': 'Não ouvi nada. Tente falar mais alto.',
            'not-allowed': 'Permissão do microfone negada.',
            'network': 'Erro de conexão.'
          };
          this.erroAudio = mensagens[event.error] || 'Erro ao gravar.';
        };

        this.recognition.onend = () => {
          this.gravandoDataInicioExperiencia = false;
        };

        this.recognition.start();

      } catch (error) {
        this.erroAudio = 'Ligue o microfone nas configurações do navegador.';
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

      const regexMisto = /(\d{1,2})[\/-](\d{1,2})\s*(?:d[eo])?\s*(\d{4})/;
      const matchMisto = texto.match(regexMisto);
      if (matchMisto) {
        let dia = matchMisto[1].padStart(2, '0');
        let mes = matchMisto[2].padStart(2, '0');
        let ano = matchMisto[3];
        return `${ano}-${mes}-${dia}`;
      }

      const regexNumerico = /(\d{1,2})[\/-](\d{1,2})[\/-](\d{4})/;
      const matchNumerico = texto.match(regexNumerico);
      if (matchNumerico) {
        let dia = matchNumerico[1].padStart(2, '0');
        let mes = matchNumerico[2].padStart(2, '0');
        let ano = matchNumerico[3];
        return `${ano}-${mes}-${dia}`;
      }

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
          this.erroAudio = 'Navegador incompatível. Use Chrome ou Safari.';
          return;
        }

        await navigator.mediaDevices.getUserMedia({ audio: true });

        if (this.recognition) this.recognition.abort();

        this.recognition = new SpeechRecognition();
        this.recognition.lang = 'pt-BR';
        this.recognition.continuous = false;
        this.recognition.interimResults = false;

        this.recognition.onstart = () => {
          this.gravandoDataFim = true;
          this.erroAudio = null;
          if (navigator.vibrate) navigator.vibrate(50);
        };

        this.recognition.onresult = (event) => {
          const transcript = event.results[0][0].transcript.toLowerCase().trim();
          
          const dataFormatada = this.interpretarDataFalada(transcript);

          if (dataFormatada) {
            this.novaExperiencia.dataFim = dataFormatada;
            const [ano, mes, dia] = dataFormatada.split('-');
            this.valorVisualDataFim = `${dia}/${mes}/${ano}`;
          } else {
            this.erroAudio = `Não entendi "${transcript}". Fale algo como "10 de agosto de 2010".`;
          }
        };

        this.recognition.onerror = (event) => {
          this.gravandoDataFim = false;
          const mensagens = {
            'no-speech': 'Não ouvi nada. Tente falar mais alto.',
            'not-allowed': 'Permissão do microfone negada.',
            'network': 'Erro de conexão.'
          };
          this.erroAudio = mensagens[event.error] || 'Erro ao gravar.';
        };

        this.recognition.onend = () => {
          this.gravandoDataFim = false;
        };

        this.recognition.start();

      } catch (error) {
        this.erroAudio = 'Ligue o microfone nas configurações do navegador.';
        this.gravandoDataFim = false;
      }
    },

    interpretarDataFalada(transcript) {
      const meses = {
        'janeiro': '01', 'fevereiro': '02', 'março': '03', 'abril': '04',
        'maio': '05', 'junho': '06', 'julho': '07', 'agosto': '08',
        'setembro': '09', 'outubro': '10', 'novembro': '11', 'dezembro': '12'
      };

      let texto = transcript.toLowerCase().replace(/ de /g, ' ').replace(/\./g, '').trim();

      const regex = /(\d{1,2})\s+([a-zç0-9]+)\s+(\d{4})/;
      const match = texto.match(regex);

      if (match) {
        let dia = match[1].padStart(2, '0');
        let mesCapturado = match[2];
        let ano = match[3];
        let mes = meses[mesCapturado] || mesCapturado.padStart(2, '0');

        if (parseInt(mes) > 12 || parseInt(dia) > 31) return null;

        return `${ano}-${mes}-${dia}`;
      }

      return null;
    },

    garantirVisibilidade(event) {
      const elemento = event.target;

      setTimeout(() => {
        if (elemento && elemento.scrollIntoView) {
          elemento.scrollIntoView({
            behavior: 'smooth',
            block: 'center'
          });
        }
      }, 300);
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
        status: false,
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
          status: '',
        }
        this.editandoIndexFormacao = null;
        this.editandoFormacao = null;
      }
      else{
        this.novaFormacao = {
          ...formacao
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
    
    prevStep() {
      if (this.step > 1) {
        this.step--;
        window.scrollTo({
          top: 0,
          behavior: 'smooth'
        });
      }
    },

    async adicionarExperiencia() {
     if (!this.novaExperiencia.descricao || this.novaExperiencia.descricao.trim() === '') {
        
        const mensagem = 'Preencha a descrição da atividade.';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }

        return this.mostrarErro(mensagem);
      }
      else if (!this.novaExperiencia.dataInicio) {
        const mensagem = 'Preencha a data de início da experiência.';
        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }

        return this.mostrarErro(mensagem);
      }
      else if(!this.novaExperiencia.empregoAtual && this.novaExperiencia.dataFim == ''){
        const mensagem = 'Preencha a data fim da experiência.';
        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }

        return this.mostrarErro(mensagem);
      }
      else if(!this.novaExperiencia.empregoAtual && (this.novaExperiencia.dataFim < this.novaExperiencia.dataInicio)){
        const mensagem = 'A data fim não pode ser anterior à data de início.';
        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        return this.mostrarErro(mensagem);
      }

      try {
          let novaExp;
          
          if (this.editandoIndexExperiencia !== null) { 
              const expId = this.curriculo.experiencias[this.editandoIndexExperiencia].id;
              
              if (expId) {
                  await experienciaService.atualizarExperiencia(this.novaExperiencia);
              }
              
              this.curriculo.experiencias.splice(this.editandoIndexExperiencia, 1, { ...this.novaExperiencia });
              const mensagem = 'Experiência atualizada!';

              if (this.mostrarTutorial) {
                this.falarTexto(mensagem);
              }
              this.successMessage = mensagem;

          } else {
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
              const mensagem = 'Experiência adicionada!';

              if (this.mostrarTutorial) {
                this.falarTexto(mensagem);
              }
              this.successMessage = mensagem;
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
            await formacaoService.atualizarFormacao(formacaoNormalizada);
          }

          this.curriculo.formacoes.splice(
            this.editandoIndexFormacao,
            1,
            { ...formacaoNormalizada }
          );
          const mensagem = 'Formação atualizada!';

          if (this.mostrarTutorial) {
            this.falarTexto(mensagem);
          }
          this.successMessage = mensagem;
        } else {
          this.curriculoId = this.curriculo.id
          if (this.curriculoId) {
            const formacaoComCurriculo = {
              ...formacaoNormalizada,
              curriculoId: this.curriculoId
            };
            const novaForm = await formacaoService.adicionarFormacao(formacaoComCurriculo);
            this.curriculo.formacoes.push(novaForm);
          } else {
            this.curriculo.formacoes.push({ ...formacaoNormalizada });
          }
          const mensagem = 'Formação adicionada!';

          if (this.mostrarTutorial) {
            this.falarTexto(mensagem);
          }
          this.successMessage = mensagem;
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
            const mensagemExp = 'Experiência removida com sucesso!';

            if (this.mostrarTutorial) {
              this.falarTexto(mensagemExp);
            }
            this.successMessage = mensagemExp;
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
            const mensagemForm = 'Formação removida com sucesso!';

            if (this.mostrarTutorial) {
              this.falarTexto(mensagemForm);
            }
            this.successMessage = mensagemForm;
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
        const mensagem = 'Digite pelo menos 10 caracteres';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
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
        const mensagem = 'Descrição melhorada com sucesso!';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        const mensagem = 'Erro ao melhorar. Tente novamente mais tarde.';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
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
        const mensagem = 'Digite pelo menos 10 caracteres';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
        this.iaMessageType = 'info';
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);
        return;
      }

      this.loadingIA = true;
      const mensagem = 'Melhorando objetivo...';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
      this.iaMessage = mensagem;
      this.iaMessageType = 'info';

      try {
        const iaFormattingService = (await import('@/services/iaFormattingService')).default;
        
        const objetivoMelhorado = await iaFormattingService.formatarObjetivo(
          this.curriculo.objetivo
        );
        
        this.curriculo.objetivo = objetivoMelhorado;
        const mensagem = 'Objetivo melhorado com sucesso!';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
        this.iaMessageType = 'success';
        
        setTimeout(() => {
          this.iaMessage = '';
          this.iaMessageType = '';
        }, 3000);

      } catch (error) {
        const mensagem = 'Erro ao melhorar. Tente novamente mais tarde.';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        this.iaMessage = mensagem;
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
        console.log(dadosParaEnviar);
        if (dadosParaEnviar.experiencias) {
          dadosParaEnviar.experiencias.forEach(exp => {
            if (!exp.dataFim || exp.dataFim === "") exp.dataFim = null;
            if (!exp.dataInicio || exp.dataInicio === "") exp.dataInicio = null;
          });
        }
        if (dadosParaEnviar.id) {
          await curriculoService.atualizarCurriculo(dadosParaEnviar);
          const mensagem = 'Currículo atualizado com sucesso!';

          if (this.mostrarTutorial) {
            this.falarTexto(mensagem);
          }
          this.successMessage = mensagem;
        } else {
          const data = await curriculoService.adicionarCurriculo(dadosParaEnviar);
          this.curriculoId = data.id;
          const mensagem = 'Currículo salvo com sucesso!';

          if (this.mostrarTutorial) {
            this.falarTexto(mensagem);
          }
          this.successMessage = mensagem;
        }
        this.$router.push(`/curriculo/visualizar/${this.curriculo.id || this.curriculoId}`);
      } catch (error) {
        const mensagem = 'Erro ao salvar currículo';

        if (this.mostrarTutorial) {
          this.falarTexto(mensagem);
        }
        return this.mostrarErro(mensagem);
      }
    },

  nextStepPerfil() {
    if (this.curriculo.nomeCompleto == '' || !this.curriculo.nomeCompleto) {
      
      const mensagem = 'Preencha o nome completo.';

      if (this.mostrarTutorial) {
        this.falarTexto(mensagem);
      }

      return this.mostrarErro(mensagem);
    }
    this.step++;
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
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
  background: #000000;
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
  align-items: center;
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
  min-width: 80px;
  z-index: 1;
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
  width: 100%;
}

.posicao-lateral {
  position: absolute;
  left: 580px;
  top: 50%;
  transform: translateY(-50%);
}

input:disabled {
  color: #999999;
  border: 1px solid #dddddd;
  opacity: 0.7;
}

.input-com-dois-icones {
  width: 100% !important;
  min-width: 100%;
  box-sizing: border-box;
}

.icone-calendario{
  opacity: 0;
}

.input-com-dois-icones::-webkit-calendar-picker-indicator {
  margin-right: 25px; 
  cursor: pointer;
  opacity: 0.3;
}

.form-row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 14px;
  align-items: start;
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
  right: 8px;
  top: 50%; 
  transform: translateY(-50%); 
  
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
}

.btn-microfone:hover svg {
  color: #475569;
}

.btn-microfone.gravando {
  margin-top: 17px;
  opacity: 1;
}

.btn-microfone.gravando svg {
  color: #ef4444;
}

@keyframes pulse-icon {
  0%, 100% {
    transform: scale(1);
  }
  50% {
    transform: scale(1.15);
  }
}

.btn-microfone:click {
  outline: none;
}


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
    background: transparent;
    border: none;
    cursor: pointer;
    padding: 20px; 
    
    display: flex;
    align-items: center;
    justify-content: center;
    right: 4px;
  } 
  
  textarea {
    padding-right: 45px;
  }
}

input:click, select:click, textarea:click {
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
  padding: 12px 60px 12px 12px; 
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
  right: 5px;
  top: 20%;
  transform: translateY(-50%);
  
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
.icon-education {
  filter: brightness(0) invert(1);
  height: 25px;
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
  position: fixed;
  top: 20px;
  left: 50%;
  transform: translateX(-50%);
  z-index: 9999;
  width: 90%;
  max-width: 400px;
  padding: 16px;
  border-radius: 12px;
  font-size: 14px;
  font-weight: 500;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  
  animation: slideDown 0.3s ease;
  text-align: center;
}

@keyframes slideDown {
  from {
    opacity: 0;
    transform: translate(-50%, -20px);
  }
  to {
    opacity: 1;
    transform: translate(-50%, 0);
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
    padding: 0;
    background: rgb(255, 255, 255); 
    align-items: flex-start;
  }

  .container {
    padding: 20px;
    border-radius: 0;
    border: none;
    max-width: 100vw;
    min-height: 100vh;
  }
  input, select, textarea {
    font-size: 16px;
  }

  input::placeholder {
    font-size: 14px;
    color: #b4b4b4;
  }

  textarea {
    resize: none;
  }

  input[type="date"]::-webkit-inner-spin-button,
  input[type="date"]::-webkit-calendar-picker-indicator {
      display: none;
      -webkit-appearance: none;
  }

  input[type="date"] {
      appearance: none;
      -moz-appearance: none;
      -webkit-appearance: none;
      width: 100%;
  }
  
  input[type="date"]::-webkit-calendar-picker-indicator {
    display: block;
    cursor: pointer;
    margin-right: 35px; 
    filter: invert(0.5);
  }

  .header h1 {
    font-size: 22px;
  }

  .form-row {
    grid-template-columns: 1fr;
    gap: 0;
  }

  .button-group {
    grid-template-columns: 1fr;
  }

  .progress-bar {
    padding: 0 25px;
  }

  .icone-calendario {
    position: absolute;
    top: 8px;
    right: 40px;
    pointer-events: none;
    font-size: 1rem;
    z-index: 1;
    color:#000000;
    opacity: 0.23;
  }

  html, body {
    height: 100%;
    overflow-y: auto;
    -webkit-overflow-scrolling: touch;
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