pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                git branch: 'main',
                    url: 'https://github.com/Dheeraj8601/AksharaMane.git'
            }
        }

        stage('Restore') {
            steps {
                bat 'dotnet restore AksharaMane-Backend\\AksharaMane.sln'
            }
        }

        stage('Build') {
            steps {
                bat 'dotnet build AksharaMane-Backend\\AksharaMane.sln -c Release --no-restore'
            }
        }

        stage('Test') {
            steps {
                bat 'dotnet test AksharaMane-Backend\\AksharaMane.sln --no-build -c Release'
            }
        }

        stage('Publish') {
            steps {
                bat 'dotnet publish AksharaMane-Backend\\src\\AksharaMane.API\\AksharaMane.API.csproj -c Release --no-build -o publish'
            }
        }

        stage('Archive Artifact') {
            steps {
                archiveArtifacts artifacts: 'publish/**',
                                 fingerprint: true
            }
        }
    }

    post {
        success {
            echo 'AksharaMane CI succeeded'
        }

        failure {
            echo 'AksharaMane CI failed'
        }

        always {
            echo 'AksharaMane pipeline finished'
        }
    }
}
