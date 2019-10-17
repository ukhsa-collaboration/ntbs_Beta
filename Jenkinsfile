pipeline {
  environemnt {
    NTBS_BUILD = "${env.BUILD_ID}:${env.GIT_COMMIT.substring(0, 6)}"
  }
  agent { label 'linux' }
  stages {
    stage('run unit tests') {
      steps {
        script {
          docker.image('mcr.microsoft.com/dotnet/core/sdk:2.2').inside {
            sh(script: '''
              # Workaround from https://stackoverflow.com/a/57212491/2363767
              export DOTNET_CLI_HOME="/tmp/DOTNET_CLI_HOME"
              cd ntbs-service-unit-tests
              echo "Running service unit tests"
              dotnet test
              cd ../EFAuditer-tests
              echo "Running EFAuditer unit tests"
              dotnet test
            ''')
          }
        }
      }
    }
    stage('run integration tests') {
      steps {
        script {
          docker.image('mcr.microsoft.com/dotnet/core/sdk:2.2').inside {
            sh(script: '''
              # Workaround from https://stackoverflow.com/a/57212491/2363767
              export DOTNET_CLI_HOME="/tmp/DOTNET_CLI_HOME"
              cd ntbs-integration-tests
              echo "Running integration tests"
              dotnet test
            ''')
          }
        }
      }
    }
    stage('build and publish image') {
      steps {
        script {
          docker.withRegistry('https://ntbscontainerregistry.azurecr.io', 'ntbs-registery-credentials') {
            ntbsImage = docker.build("ntbs-service:build-${NTBS_BUILD}",  ".")
            echo "Uploading build image build-${NTBS_BUILD}"
            ntbsImage.push()
            echo "Uploading latest image"
            ntbsImage.push("latest")
          }
        }
      }
    }  
    stage('deploy to int') {
      steps {
        script {
          kubectl = docker.image('bitnami/kubectl')
          kubectl.inside("--entrypoint=''") {
            withCredentials([[$class: 'FileBinding', credentialsId: 'kubeconfig', variable: 'KUBECONFIG']]) {
              sh "./scriptss/release.sh int ${NTBS_BUILD}"
            }
          }
        }
      }
    }
  }
  post {
    success {
      notifySlack(":green_heart: Build succeeded. New deployment on int: build-${NTBS_BUILD}")
    }
    failure {
      notifySlack(":red_circle: Build failed")
    }
    unstable {
      notifySlack(":large_orange_diamond: Build unstable")
    }
  }
}


def notifySlack(message) {
    withCredentials([string(credentialsId: 'slack-token', variable: 'SLACKTOKEN')]) {
        slackSend teamDomain: "phe-ntbs", channel: "#dev-ci", token: "$SLACKTOKEN", message: "*${message}* - ${env.JOB_NAME} ${NTBS_BUILD} (<${env.BUILD_URL}|Open>)"
    }
}