<template>
  <div class="wrapper">
    <ModalCarregamento :isOpen="loading" />

    <div v-if="!loading && curriculo" class="curriculo-container">
      <div class="top-bar">
        <BackButton @back="prevStep" />
        <div class="right-actions">
          <BotaoDescricao @toggle="handleTutorialToggle" :active="mostrarTutorial" class="pr-20"/>
          <LogoutButton @logout="abrirModal" />
        </div>
      </div>

      <div class="curriculo-paper" id="curriculo-print">
        <div class="paper-actions">
           <DownloadButton @download="downloadPDF" />
        </div>

        <header class="cv-header">
          <h1 @click="falarElemento">{{ curriculo.nomeCompleto }}</h1>
          <div class="contact-info" @click="falarElemento"> 
            {{ formatarInfoContato }}
          </div>
        </header>

        <section v-if="curriculo.objetivo" class="cv-section">
          <h2 @click="falarElemento">OBJETIVO</h2>
          <p class="description-text" @click="falarElemento">{{ curriculo.objetivo }}</p>
        </section>

        <section v-if="experiencias.length > 0" class="cv-section">
          <h2 @click="falarElemento">EXPERIÊNCIAS PROFISSIONAIS</h2>
          <div v-for="(exp, index) in experiencias" :key="index" class="cv-item">
            <h3 class="item-title" @click="falarElemento">{{ exp.empresa || "Empresa não Informada" }}</h3>
            <p class="item-subtitle" @click="falarElemento">{{ exp.cargo || "Cargo não Informado" }}</p>
            <span class="period-text" @click="falarElemento">{{ formatarPeriodo(exp.dataInicio, exp.dataFim, exp.empregoAtual) }}</span>
            <p v-if="exp.descricao" class="description-text" @click="falarElemento">{{ exp.descricao }}</p>
          </div>
        </section>

        <section v-if="formacoes.length > 0" class="cv-section">
          <h2 @click="falarElemento">FORMAÇÃO ACADÊMICA</h2>
          <div v-for="(form, index) in formacoes" :key="index" class="cv-item">
            <h3 class="college-title" @click="falarElemento">{{ form.instituicao || "Instituição não informada" }}</h3>
            <p class="item-subtitle" @click="falarElemento">{{ form.curso || "Curso não informado" }}</p>
            <span class="period-text" @click="falarElemento">
              {{ form.nivel }} • {{ form.status === true ? 'Incompleto' : 'Completo' }}
              <br>
            </span>
          </div>
        </section>
      </div>
    </div>

    <div v-if="!loading && !curriculo" class="error-container">
      <p>Erro ao carregar currículo</p>
      <button @click="voltar" class="btn-back">Voltar</button>
    </div>

    <ModalEncerramentoSessao
    :isOpen="modalAberto"
    @confirmar="confirmarSair"
    @fechar="fecharModal"
    @falar="falarTexto"
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
import ModalCarregamento from '@/components/ModalCarregamento.vue';
import BotaoDescricao from '@/components/BotaoDescricao.vue';

export default {
  name: 'CurriculoVisualizar',
  components: {
    ModalEncerramentoSessao,
    BackButton,
    LogoutButton,
    DownloadButton,
    ModalCarregamento,
    BotaoDescricao
  },
  data() {
    return {
      loading: true,
      curriculo: null,
      modalAberto: false,
      experiencias: [],
      formacoes: [],
      mostrarTutorial: false,
    }
  },
  mounted() {
    const estadoSalvo = localStorage.getItem('audioDescricaoAtiva');

    if (estadoSalvo !== null) {
      this.mostrarTutorial = estadoSalvo === 'true';
    }
  },
  computed: {
    formatarInfoContato() {
      if (!this.curriculo) return '';
      const tel = this.formatarTelefone(this.curriculo.telefone);
      const email = this.curriculo.email || '';
      const local = [this.curriculo.cidade, this.curriculo.estado]
        .filter(p => p && p.trim() !== '' && p !== 'null')
        .join(', ');

      return [tel, email, local].filter(i => i).join(' | ');
    }
  },
  async created() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
    const autenticado = await usuarioService.verificarSessao();
    
    const curriculoId = this.$route.params.id;
    if (curriculoId) {
      await this.carregarCurriculo(curriculoId);
    } else {
      this.$router.push('/');
    }
  },
  methods: {
    handleTutorialToggle(ativo) {
      this.mostrarTutorial = ativo;
      localStorage.setItem('audioDescricaoAtiva', ativo);
      if (ativo) {
        this.executarBoasVindasNativo();
      } else {
        this.pararAudioTutorial();
      }
    },
    extensoMes(mesAno) {
      if (!mesAno || !mesAno.includes('/')) return mesAno;

      const [mes, ano] = mesAno.split('/');
      const meses = {
        '01': 'janeiro', '02': 'fevereiro', '03': 'março', 
        '04': 'abril', '05': 'maio', '06': 'junho',
        '07': 'julho', '08': 'agosto', '09': 'setembro', 
        '10': 'outubro', '11': 'novembro', '12': 'dezembro'
      };

      const nomeMes = meses[mes];
      return nomeMes ? `${nomeMes} de ${ano}` : mesAno;
    },
    falarElemento(event) {
      let texto = event.target.innerText;

      const regexData = /(\d{2})\/(\d{4})/g;

      texto = texto.replace(regexData, (match) => {
        return this.extensoMes(match);
      });

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
    abrirModal() { 
      this.modalAberto = true; 
    },

    fecharModal() { 
      this.modalAberto = false; 
    },

    prevStep() {
      this.$router.push('/curriculo');
    },

    voltar() {
      this.$router.go(-1);
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

    formatarTelefone(telefone) {
      if (!telefone) return '';
      const apenasNumeros = telefone.toString().replace(/\D/g, '');
      if (apenasNumeros.length === 11) {
        return `(${apenasNumeros.substring(0, 2)}) ${apenasNumeros.substring(2, 7)}-${apenasNumeros.substring(7)}`;
      }
      return telefone;
    },

    formatarData(data) {
      if (!data) return '';
      const date = new Date(data);
      const mes = String(date.getMonth() + 1).padStart(2, '0');
      const ano = date.getFullYear();
      return `${mes}/${ano}`;
    },

    formatarPeriodo(inicio, fim, atual) {
      const dataInicio = this.formatarData(inicio);
      const dataFim = atual ? 'Atual' : (fim ? this.formatarData(fim) : 'Não informado');
      return `${dataInicio} - ${dataFim}`;
    },

    downloadPDF() {
      const doc = new jsPDF('p', 'mm', 'a4');

      let yPosition = 20;
      const pageWidth = doc.internal.pageSize.getWidth();
      const margin = 15;

      doc.setFontSize(22);
      doc.setFont('helvetica', 'bold');
      const nomeLines = doc.splitTextToSize(
        this.curriculo.nomeCompleto,
        pageWidth - 2 * margin
      );

      doc.text(nomeLines, pageWidth / 2, yPosition, { align: 'center' });
      yPosition += nomeLines.length * 8;

      yPosition += 8;
      doc.setFontSize(10);
      doc.setFont('helvetica', 'normal');
      doc.setTextColor(60, 60, 60);

      const localizacao = [this.curriculo.cidade, this.curriculo.estado]
        .filter(part => part && part.trim() !== '')
        .join(', ');

      const contato = [
        this.formatarTelefone(this.curriculo.telefone),
        this.curriculo.email,
        localizacao
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
      if (this.curriculo.objetivo) {
          doc.setFontSize(14);
          doc.setFont('helvetica', 'bold');
          doc.text('OBJETIVO', margin, yPosition);
          yPosition += 2;

          doc.setDrawColor(0, 0, 0);
          doc.setLineWidth(0.5);
          doc.line(margin, yPosition, pageWidth - margin, yPosition);
          yPosition += 7;

          doc.setFontSize(11);
          doc.setFont('helvetica', 'normal');
          doc.setTextColor(40, 40, 40);
          const objetivoLines = doc.splitTextToSize(this.curriculo.objetivo, pageWidth - 2 * margin);
          doc.text(objetivoLines, margin, yPosition);
          yPosition += (objetivoLines.length * 5) + 10;
      }

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
            yPosition = 10;
          }

          doc.setFontSize(12);
          doc.setFont('helvetica', 'bold');
          doc.text(exp.empresa || 'Empresa não informada', margin, yPosition);

          yPosition += 5;
          doc.setFontSize(10);
          doc.setFont('helvetica', 'italic');
          doc.setTextColor(60, 60, 60);
          doc.text(exp.cargo || 'Cargo não informado', margin, yPosition);

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
          yPosition += 5;
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
          doc.text(form.instituicao || 'Instituição não informada', margin, yPosition);

          yPosition += 5;
          doc.setFontSize(10);
          doc.setFont('helvetica', 'italic');
          doc.setTextColor(60, 60, 60);
          const statusTexto = form.status === true ? 'Incompleto' : 'Completo';
          const text = `${form.curso || 'Curso não informado'} • ${statusTexto}`.trim();
          doc.text(text, margin, yPosition);

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
  background: #f4f4f5;
  padding: 40px 20px;
  min-height: 100vh;
}

.curriculo-container {
  max-width: 850px;
  margin: 0 auto;
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

.btn-back, .btn-logout {
  background: white;
  color: #111827;
  border: 1px solid #e5e7eb;
}

.btn-back:hover {
  background: #f9fafb;
}

.curriculo-paper {
  background: white;
  padding: 60px;
  min-height: 297mm;
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
  color: #000;
  font-family: 'Arial', sans-serif;
  position: relative;
}

.cv-header {
  text-align: center;
  margin-bottom: 40px;
}

.cv-header h1 {
  font-size: 32px;
  margin-bottom: 8px;
  font-weight: bold;
  word-break: break-word;
  overflow-wrap: break-word;
  hyphens: auto;
  text-align: center;
}

.contact-info {
  font-size: 14px;
  color: #333;
}

.cv-section {
  margin-bottom: 30px;
}

.cv-section h2 {
  font-size: 16px;
  font-weight: bold;
  border-bottom: 2px solid #000;
  padding-bottom: 4px;
  margin-bottom: 12px;
}

.item-title {
  font-size: 16px;
  font-weight: bold;
  margin: 0;
}

.college-title {
  font-size: 16px;
  font-weight: bold;
  margin: 12px 0 0 0;
}

.item-subtitle {
  font-size: 14px;
  font-style: italic;
  margin: 0 0 2px;
}

.period-text {
  font-size: 13px;
  color: #555;
  display: block;
}

.description-text {
  font-size: 14px;
  line-height: 1.5;
  margin-top: 2px;
  text-align: justify;
}

.paper-actions {
  position: absolute;
  top: 20px;
  right: 20px;
}

.top-bar { 
  display: flex;
  justify-content: space-between; 
  align-items: center; 
  margin-bottom: 24px; 
}
.right-actions { display: flex; gap: 12px; align-items: center; }
</style>