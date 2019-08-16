pipeline {
  agent { label 'windows' }
  stages {
    stage('run unit tests') {
      steps {
        powershell(script: '''
          Push-Location ntbs-service-tests
          Write-Output "Running unit tests"
          dotnet test
        ''')
      }
    }
    stage('build and publish') {
      steps {
        powershell(script: '''
          Push-Location ntbs-service
          Write-Output "Building ntbs .net core application"
          dotnet publish -c Release
        ''')
      }
    }
  }
}