pipeline {
  agent { label 'windows' }
  stages {
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