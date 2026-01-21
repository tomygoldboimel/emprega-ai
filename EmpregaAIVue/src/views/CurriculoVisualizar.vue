<template>
  <div class="wrapper">
    <div v-if="loading" class="loading-container">
      <div class="loading-spinner-large"></div>
      <p>Carregando currículo...</p>
    </div>

    <div v-else-if="curriculo" class="curriculo-container">
      <div class="actions-bar">
        <BackButton @back="prevStep" />
        <div class="action-buttons">
          <LogoutButton @logout="abrirModal" />
        </div>
      </div>

      <div class="curriculo-paper" id="curriculo-print">
        <div class="paper-actions">
           <DownloadButton @download="downloadPDF" />
        </div>
        <header class="cv-header">
          <h1>{{ curriculo.nomeCompleto }}</h1>
          
          <div class="contact-info">
            <span v-if="curriculo.telefone">
              <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M22 16.92v3a2 2 0 0 1-2.18 2 19.79 19.79 0 0 1-8.63-3.07 19.5 19.5 0 0 1-6-6 19.79 19.79 0 0 1-3.07-8.67A2 2 0 0 1 4.11 2h3a2 2 0 0 1 2 1.72 12.84 12.84 0 0 0 .7 2.81 2 2 0 0 1-.45 2.11L8.09 9.91a16 16 0 0 0 6 6l1.27-1.27a2 2 0 0 1 2.11-.45 12.84 12.84 0 0 0 2.81.7A2 2 0 0 1 22 16.92z"></path>
              </svg>
              {{ formatarTelefone(curriculo.telefone) }}
            </span>
            
            <span v-if="curriculo.email">
              <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                <polyline points="22,6 12,13 2,6"></polyline>
              </svg>
              {{ curriculo.email }}
            </span>
            
            <span v-if="curriculo.estado && !curriculo.cidade">
              <img src='@/assets/icons/locationIcon.svg' class="locationIcon"/>
              {{ curriculo.estado }}
            </span>
            <span v-if="curriculo.cidade && curriculo.estado">
              <img src='@/assets/icons/locationIcon.svg' class="locationIcon"/>
              {{ curriculo.cidade }}, {{ curriculo.estado }}
            </span>
          </div>
          
          <div class="social-links" v-if="curriculo.linkedIn || curriculo.gitHub">
            <a v-if="curriculo.linkedIn" :href="curriculo.linkedIn" target="_blank">
              {{ curriculo.linkedIn }}
            </a>
            
            <a v-if="curriculo.gitHub" :href="curriculo.gitHub" target="_blank">
              {{ curriculo.gitHub }}
            </a>
          </div>
        </header>

        <section v-if="curriculo.resumoProfissional" class="cv-section">
          <h2>Resumo Profissional</h2>
          <p>{{ curriculo.resumoProfissional }}</p>
        </section>

        <section v-if="experiencias.length > 0" class="cv-section">
          <h2>Experiência Profissional</h2>
          <div v-for="(exp, index) in experiencias" :key="index" class="cv-item">
            <div class="item-header">
              <div>
                <h3>{{ exp.cargo || "Cargo não Informado" }}</h3>
                <p class="company">{{ exp.empresa || "Empresa não Informada" }}</p>
              </div>
              <span class="period">{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</span>
            </div>
            <p v-if="exp.descricao" class="description">{{ exp.descricao }}</p>
          </div>
        </section>

        <section v-if="formacoes.length > 0" class="cv-section">
          <h2>Formação Acadêmica</h2>
          <div v-for="(form, index) in formacoes" :key="index" class="cv-item">
            <div class="item-header">
              <div>
                <h3>{{ form.curso }}</h3>
                <p class="company">{{ form.instituicao }}</p>
              </div>
              <span class="period">
                {{ form.status === true ? 'Incompleto' : 'Completo' }}
              </span>
            </div>
            <p v-if="form.dataInicio" class="dates">
              {{ formatarData(form.dataInicio) }} - {{ form.dataConclusao ? formatarData(form.dataConclusao) : 'Em andamento' }}
            </p>
          </div>
        </section>
      </div>
    </div>

    <div v-else class="error-container">
      <p>Erro ao carregar currículo</p>
      <button @click="voltar" class="btn-back">Voltar</button>
    </div>

    <ModalEncerramentoSessao
      :isOpen="modalAberto"
      @confirmar="confirmarSair"
      @fechar="fecharModal"
    />
  </div>
</template>

<script>
import jsPDF from 'jspdf';
import ModalEncerramentoSessao from '@/components/ModalEncerramentoSessao.vue';
import curriculoService from '@/services/curriculoService';
import experienciaService from '@/services/experienciaService';
import formacaoService from '@/services/formacaoService';
import usuarioService from '@/services/usuarioService';
import BackButton from '@/components/BackButton.vue';
import LogoutButton from '@/components/LogoutButton.vue';
import DownloadButton from '@/components/DownloadButton.vue';

export default {
  name: 'CurriculoVisualizar',
  components: {
    ModalEncerramentoSessao,
    BackButton,
    LogoutButton,
    DownloadButton
  },
  data() {
    return {
      loading: true,
      curriculo: null,
      modalAberto: false,
      experiencias: [],
      formacoes: []
    }
  },
  async created() {
    const autenticado = await usuarioService.verificarSessao();
    
    if (!autenticado) {
      this.$router.replace('/login');
      return;
    }

    const curriculoId = this.$route.params.id;
    if (curriculoId) {
      await this.carregarCurriculo(curriculoId);
    } else {
      this.$router.push('/');
    }
  },
  methods: {
    abrirModal() {
      this.modalAberto = true;
    },

    prevStep() {
      this.$router.push('/curriculo');
    },

    fecharModal() {
      this.modalAberto = false;
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

    async carregarCurriculo(id) {
      try {
        this.loading = true;
        this.curriculo = await curriculoService.listarCurriculoPorId(id);
        
        const todasExperiencias = await experienciaService.listarExperiencias();
        this.experiencias = todasExperiencias.filter(exp => exp.curriculoId === id);
        
        const todasFormacoes = await formacaoService.listarFormacoes();
        this.formacoes = todasFormacoes.filter(form => form.curriculoId === id);
      } catch (error) {
        this.curriculo = null;
      } finally {
        this.loading = false;
      }
    },
    formatarTelefone(telefone) {
      if (!telefone) return '';
      
      const apenasNumeros = telefone.toString().replace(/\D/g, '');
      
      if (apenasNumeros.length === 11) {
        return `(${apenasNumeros.substring(0, 2)}) ${apenasNumeros.substring(2, 7)}-${apenasNumeros.substring(7)}`;
      } else if (apenasNumeros.length === 10) {
        return `(${apenasNumeros.substring(0, 2)}) ${apenasNumeros.substring(2, 6)}-${apenasNumeros.substring(6)}`;
      }
      
      return telefone;
    },
    formatarPeriodo(inicio, fim, atual) {
      if (!inicio) return 'Não informado';
      
      const dataInicio = this.formatarData(inicio);
      const dataFim = atual ? 'Atual' : (fim ? this.formatarData(fim) : 'Não informado');
      
      return `${dataInicio} - ${dataFim}`;
    },

    formatarData(data) {
      if (!data) return '';
      
      const date = new Date(data);
      const mes = String(date.getMonth() + 1).padStart(2, '0');
      const ano = date.getFullYear();
      
      return `${mes}/${ano}`;
    },

    voltar() {
      this.$router.go(-1);
    },

    editarCurriculo() {
      this.$router.push(`/curriculo/editar/${this.curriculo.id}`);
    },

    downloadPDF() {
      const doc = new jsPDF('p', 'mm', 'a4');
      
      let yPosition = 20;
      const pageWidth = doc.internal.pageSize.getWidth();
      const margin = 15;

      doc.setFontSize(22);
      doc.setFont('helvetica', 'bold');
      doc.text(this.curriculo.nomeCompleto, pageWidth / 2, yPosition, { align: 'center' });
      
      yPosition += 8;
      doc.setFontSize(10);
      doc.setFont('helvetica', 'normal');
      doc.setTextColor(60, 60, 60);
      
      const contato = [
        this.formatarTelefone(this.curriculo.telefone),
        this.curriculo.email,
        `${this.curriculo.cidade}, ${this.curriculo.estado}`
      ].filter(item => item && item.trim() !== '').join(' | ');
      
      doc.text(contato, pageWidth / 2, yPosition, { align: 'center' });
      yPosition += 6;
      
      if (this.curriculo.linkedIn || this.curriculo.gitHub) {
        const social = [
          this.curriculo.linkedIn,
          this.curriculo.gitHub
        ].filter(item => item).join(' | ');
        
        doc.setTextColor(30, 64, 175);
        doc.text(social, pageWidth / 2, yPosition, { align: 'center' });
        yPosition += 10;
      } else {
        yPosition += 5;
      }

      doc.setTextColor(0, 0, 0);

      if (this.experiencias.length > 0) {
        doc.setFontSize(14);
        doc.setFont('helvetica', 'bold');
        doc.text('EXPERIÊNCIAS PROFISSIONAIS', margin, yPosition);
        yPosition += 2;
        
        doc.setDrawColor(0, 0, 0);
        doc.setLineWidth(0.5);
        doc.line(margin, yPosition, pageWidth - margin, yPosition);
        yPosition += 8;

        this.experiencias.forEach((exp) => {
          if (yPosition > 270) {
            doc.addPage();
            yPosition = 20;
          }

          doc.setFontSize(12);
          doc.setFont('helvetica', 'bold');
          doc.text(exp.cargo || 'Cargo não informado', margin, yPosition);
          
          yPosition += 5;
          doc.setFontSize(10);
          doc.setFont('helvetica', 'italic');
          doc.setTextColor(60, 60, 60);
          doc.text(exp.empresa || 'Empresa não informada', margin, yPosition);
          
          yPosition += 5;
          doc.setFont('helvetica', 'normal');
          doc.setTextColor(100, 100, 100);
          doc.text(this.formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual), margin, yPosition);
          
          if (exp.descricao) {
            yPosition += 5;
            doc.setTextColor(40, 40, 40);
            const descricaoLines = doc.splitTextToSize(exp.descricao, pageWidth - 2 * margin);
            doc.text(descricaoLines, margin, yPosition);
            yPosition += (descricaoLines.length * 4);
          }
          
          yPosition += 10;
          doc.setTextColor(0, 0, 0);
        });
      }

      if (this.formacoes.length > 0) {
        if (yPosition > 240) {
          doc.addPage();
          yPosition = 20;
        }

        doc.setFontSize(14);
        doc.setFont('helvetica', 'bold');
        doc.text('FORMAÇÃO ACADÊMICA', margin, yPosition);
        yPosition += 2;
        
        doc.setDrawColor(0, 0, 0);
        doc.setLineWidth(0.5);
        doc.line(margin, yPosition, pageWidth - margin, yPosition);
        yPosition += 8;

        this.formacoes.forEach((form) => {
          if (yPosition > 270) {
            doc.addPage();
            yPosition = 20;
          }

          doc.setFontSize(12);
          doc.setFont('helvetica', 'bold');
          doc.text(form.curso || 'Curso não informado', margin, yPosition);
          
          yPosition += 5;
          doc.setFontSize(10);
          doc.setFont('helvetica', 'italic');
          doc.setTextColor(60, 60, 60);
          doc.text(form.instituicao || 'Instituição não informada', margin, yPosition);
          
          yPosition += 5;
          doc.setFont('helvetica', 'normal');
          doc.setTextColor(100, 100, 100);
          const statusTexto = form.status === true ? 'Incompleto' : 'Completo';
          const nivel = `${form.nivel || ''} • ${statusTexto}`.trim();
          doc.text(nivel, margin, yPosition);
          
          if (form.dataInicio) {
            yPosition += 5;
            const periodo = `${this.formatarData(form.dataInicio)} - ${form.dataConclusao ? this.formatarData(form.dataConclusao) : 'Em andamento'}`;
            doc.text(periodo, margin, yPosition);
          }
          
          yPosition += 10;
          doc.setTextColor(0, 0, 0);
        });
      }

      doc.save(`Curriculo_${this.curriculo.nomeCompleto.replace(/\s/g, '_')}.pdf`);
    }
  }
}
</script>

<style scoped>
.wrapper {
  min-height: 100vh;
  background: #f3f4f6;
  padding: 40px 20px;
}

.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  min-height: 60vh;
  gap: 20px;
}

.loading-spinner-large {
  width: 48px;
  height: 48px;
  border: 4px solid #e5e7eb;
  border-top-color: #000;
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.error-container {
  text-align: center;
  padding: 60px 20px;
}

.curriculo-container {
  max-width: 900px;
  margin: 0 auto;
}

.actions-bar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 24px;
}

.action-buttons {
  display: flex;
  gap: 12px;
}

.locationIcon{
  width: 16px;
  height: 16px;
  margin-top: auto;
}

.btn-back, .btn-logout, .btn-excel, .btn-pdf {
  padding: 10px 20px;
  border-radius: 8px;
  font-size: 14px;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s ease;
  border: none;
}

.btn-back {
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
}

.btn-back:hover {
  background: #f9fafb;
}

.btn-edit-cv {
  background: #3b82f6;
  color: white;
}

.btn-edit-cv:hover {
  background: #2563eb;
}

.btn-excel {
  background: #000000;
  color: white;
}

.btn-excel:hover {
  background: #000000;
}

.btn-pdf {
  background: #000000;
  color: white;
}

.btn-pdf:hover {
  background: #000000;
}

.curriculo-paper {
  position: relative; /* Importante! */
  background: white;
  padding: 60px;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  animation: fadeIn 0.4s ease-out;
}

.paper-actions {
  position: absolute;
  top: 20px;  /* Distância do topo do papel */
  right: 20px; /* Distância da direita do papel */
  z-index: 10; /* Garante que fique sobre o texto se necessário */
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.cv-header {
  text-align: center;
  padding-bottom: 32px;
  border-bottom: 2px solid #e5e7eb;
  margin-bottom: 32px;
}

.cv-header h1 {
  font-size: 32px;
  font-weight: 700;
  color: #111827;
  margin-bottom: 16px;
}

.contact-info {
  display: flex;
  justify-content: center;
  flex-wrap: wrap;
  gap: 20px;
  margin-bottom: 12px;
  color: #6b7280;
  font-size: 14px;
}

.social-links {
  display: flex;
  justify-content: center;
  gap: 16px;
}

.social-links a {
  color: #3b82f6;
  text-decoration: none;
  font-size: 14px;
  transition: color 0.2s;
}

.social-links a:hover {
  color: #2563eb;
  text-decoration: underline;
}

.cv-section {
  margin-bottom: 36px;
}

.cv-section h2 {
  font-size: 21px;
  font-weight: 600;
  color: #111827;
  margin-bottom: 20px;
  padding-bottom: 8px;
  border-bottom: 1px solid #e5e7eb;
}

.cv-item {
  margin-bottom: 24px;
}

.item-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.cv-item h3 {
  font-size: 16px;
  font-weight: 550;
  color: #111827;
  margin-bottom: 4px;
}

.company {
  font-size: 14px;
  color: #6b7280;
  font-weight: 500;
}

.period {
  font-size: 13px;
  color: #9ca3af;
  white-space: nowrap;
  margin-left: 16px;
  margin-top:18px;
}

.dates {
  font-size: 13px;
  color: #9ca3af;
  margin-top: 4px;
}

.description {
  font-size: 14px;
  color: #4b5563;
  line-height: 1.6;
  margin-top: 8px;
}

.cert-item {
  border-left: 3px solid #3b82f6;
  padding-left: 16px;
}

.cert-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.cert-meta {
  display: flex;
  align-items: center;
  gap: 12px;
  font-size: 13px;
  color: #6b7280;
}

.badge {
  background: #dbeafe;
  color: #1e40af;
  padding: 4px 10px;
  border-radius: 12px;
  font-size: 12px;
  font-weight: 600;
}

.cert-link {
  display: inline-block;
  margin-top: 8px;
  color: #3b82f6;
  text-decoration: none;
  font-size: 13px;
  transition: color 0.2s;
}

.cert-link:hover {
  color: #2563eb;
  text-decoration: underline;
}

@media print {
  .cv-header h1,
  .cv-section h2,
  .cv-item h3 {
    color: #000000 !important;
  }

  .company,
  .description,
  .contact-info,
  .contact-info span {
    color: #1f2937 !important;
  }

  .period,
  .dates,
  .cert-meta {
    color: #374151 !important;
  }

  .social-links a,
  .cert-link {
    color: #1e40af !important;
  }
}
</style>