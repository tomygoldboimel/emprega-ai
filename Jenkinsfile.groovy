pipeline {
  agent {
    docker {
      image 'mcr.microsoft.com/dotnet/sdk:8.0'
      args '-u root' // permite instalar pacotes no container
    }
  }

  environment {
    // Ajuste estes caminhos para o seu repo:
    VUE_DIR        = 'frontend'          // pasta que contém package.json (ex.: 'web' ou '.')
    DOTNET_SOLUTION= 'backend/*.sln'     // solução/projeto .NET (ex.: 'src/App.sln' ou '.')
    BACKEND_PUBLISH= 'backend/publish'   // destino do publish .NET
    // Se o backend serve o build do Vue (SPA) diretamente por wwwroot:
    BACKEND_WWWROOT= 'backend/Api/wwwroot' // ajuste ou deixe vazio se não usar

    // Caches (acelera builds)
    NPM_CONFIG_CACHE = "${WORKSPACE}/.npm"
    DOTNET_CLI_HOME  = "${WORKSPACE}"
    NUGET_PACKAGES   = "${WORKSPACE}/.nuget"

    ASPNETCORE_ENVIRONMENT = 'Production'
  }

  options {
    timestamps()
    durabilityHint('PERFORMANCE_OPTIMIZED')
  }

  stages {
    stage('Checkout') {
      steps {
        checkout scm
        sh 'ls -la'
      }
    }

    stage('Prepare (Node + tools)') {
      steps {
        sh '''
          apt-get update -y
          apt-get install -y curl ca-certificates git
          # Node LTS
          curl -fsSL https://deb.nodesource.com/setup_lts.x | bash -
          apt-get install -y nodejs
          node -v && npm -v
          dotnet --info
        '''
      }
    }

    stage('Frontend - Install & Build (Vue)') {
      steps {
        dir(env.VUE_DIR) {
          sh '''
            if [ -f package-lock.json ]; then npm ci; else npm install; fi
            # Ajuste caso seu script seja outro (ex.: build:prod)
            npm run build
          '''
        }
      }
    }

    stage('Backend - Restore, Build & Publish (.NET)') {
      steps {
        sh '''
          set -e
          if ls ${DOTNET_SOLUTION} >/dev/null 2>&1; then
            dotnet restore ${DOTNET_SOLUTION}
            dotnet build   ${DOTNET_SOLUTION} -c Release --no-restore
            mkdir -p ${BACKEND_PUBLISH}
            dotnet publish ${DOTNET_SOLUTION} -c Release -o ${BACKEND_PUBLISH} --no-build
          else
            # fallback se não houver .sln (usa csproj na raiz atual)
            dotnet restore
            dotnet build -c Release --no-restore
            mkdir -p ${BACKEND_PUBLISH}
            dotnet publish -c Release -o ${BACKEND_PUBLISH} --no-build
          fi
        '''
      }
    }

    stage('(Opcional) Copiar Vue para wwwroot') {
      when {
        expression { return env.BACKEND_WWWROOT?.trim() }
      }
      steps {
        sh '''
          if [ -d "${BACKEND_WWWROOT}" ] && [ -d "${VUE_DIR}/dist" ]; then
            echo "Copiando build do Vue para wwwroot..."
            rsync -av --delete "${VUE_DIR}/dist/" "${BACKEND_WWWROOT}/"
          else
            echo "Pasta wwwroot ou dist não encontrada; pulando cópia."
          fi
        '''
      }
    }

    stage('Artefatos') {
      steps {
        script {
          def vueDist = "${env.VUE_DIR}/dist/**"
          def dotnetPub = "${env.BACKEND_PUBLISH}/**"
          archiveArtifacts artifacts: "${vueDist}, ${dotnetPub}", fingerprint: true, onlyIfSuccessful: true
        }
      }
    }

    stage('(Opcional) Testes') {
      when { expression { return false } } // troque para true quando tiver testes
      steps {
        sh '''
          # Exemplos (ajuste caminhos):
          # dotnet test tests/SeuProjeto.Tests/SeuProjeto.Tests.csproj -c Release --no-build
          # npm test --prefix ${VUE_DIR}
          echo "Ative este stage quando configurar seus testes."
        '''
      }
    }

    stage('(Opcional) Deploy AWS') {
      when { expression { return false } } // habilite quando quiser
      steps {
        sh '''
          # Exemplos de deploy:
          # 1) EC2 por SSH/SCP:
          # scp -r ${BACKEND_PUBLISH}/* ec2-user@SEU_EC2:/var/www/app/
          #
          # 2) S3 + CloudFront (requer credenciais AWS):
          # aws s3 sync ${VUE_DIR}/dist s3://SEU-BUCKET-STATIC/ --delete
          # aws s3 sync ${BACKEND_PUBLISH} s3://SEU-BUCKET-API/ --delete
        '''
      }
    }
  }

  post {
    success {
      echo 'Build finalizado com sucesso.'
    }
    failure {
      echo 'Build falhou — confira o Console Output.'
    }
    always {
      cleanWs(deleteDirs: true, notFailBuild: true)
    }
  }
}
