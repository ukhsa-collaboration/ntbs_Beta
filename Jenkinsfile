pipeline {
  environment {
    NTBS_BUILD = "build-${env.BUILD_ID}-${env.GIT_COMMIT.substring(0, 6)}"
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
    stage('run ui tests') {
      steps {
        script {
          docker.build("ntbs-service-ui-tests:${NTBS_BUILD}",  "-f Dockerfile-uitests --build-arg CACHEBUST=${NTBS_BUILD} .").inside {
            sh(script: '''
              # We would like to run the actual tests in here, but an issue with jenkins means we do that within the docker build
              # See Dockerfile-uitests for details.
              echo "ui tests complete"
            ''')
          }
        }
      }
    }
    stage('build and publish image') {
      steps {
        script {
          docker.withRegistry('https://ntbscontainerregistry.azurecr.io', 'ntbs-registery-credentials') {
            withCredentials([string(credentialsId: 'sentry-auth-token', variable: 'SENTRY_AUTH_TOKEN')]) {
              ntbsImage = docker.build("ntbs-service:${NTBS_BUILD}",  "--build-arg RELEASE=${NTBS_BUILD} --build-arg SENTRY_AUTH_TOKEN=${SENTRY_AUTH_TOKEN} .")
              echo "Uploading build image ${NTBS_BUILD}"
              ntbsImage.push()
              echo "Uploading latest image"
              ntbsImage.push("latest")
            }
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
              sh "chmod +x ./scripts/release.sh"
              sh "./scripts/release.sh int ${NTBS_BUILD}"
            }
          }
        }
      }
    }
  }
  post {
    success {
      notifySlack(":green_heart: :computer: Build succeeded. New deployment on int: ${NTBS_BUILD}")
    }
    failure {
      notifySlack(":red_circle: :computer: Build failed")
    }
    unstable {
      notifySlack(":large_orange_diamond: :computer: Build unstable")
    }
  }
}


def notifySlack(message) {
    withCredentials([string(credentialsId: 'slack-token', variable: 'SLACKTOKEN')]) {
        slackSend teamDomain: "phe-ntbs", channel: "#dev-ci", token: "$SLACKTOKEN", message: "*${message}* - ${env.JOB_NAME} ${NTBS_BUILD} (<${env.BUILD_URL}|Open>)"
    }
}