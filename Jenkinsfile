pipeline {
  agent { label 'linux' }
  stages {
    stage('run unit tests') {
      steps {
        script {
          docker.image('mcr.microsoft.com/dotnet/core/sdk:2.2').inside {
            sh(script: '''
              # Workaround from https://stackoverflow.com/a/57212491/2363767
              export DOTNET_CLI_HOME="/tmp/DOTNET_CLI_HOME"
              cd ntbs-service-tests
              echo "Running unit tests"
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
            ntbsImage = docker.build("ntbs-service:build-${env.BUILD_ID}",  ".")
            echo "Uploading build image build-${env.BUILD_ID}"
            ntbsImage.push()
            echo "Uploading latest imagine"
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
              echo "Deploying to int"
              sh "kubectl apply -f ./ntbs-service/int.yml"
              // This sets the image to current build. Should be the same as "latest", but triggers pull of the image.
              sh "kubectl set image deployment/ntbs-int ntbs-int=ntbscontainerregistry.azurecr.io/ntbs-service:build-${env.BUILD_ID}"
              echo "Deployed!"
            }
          }
        }
      }
    }
  }
  post {
    success {
      notifySlack(":green_heart: Build succeeded. New deployment on int: build-${env.BUILD_ID}")
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
        slackSend teamDomain: "phe-ntbs", channel: "#dev-ci", token: "$SLACKTOKEN", message: "*${message}* - ${env.JOB_NAME} ${env.BUILD_NUMBER} (<${env.BUILD_URL}|Open>)"
    }
}