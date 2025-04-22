pipeline {
    agent any

    stages {
        stage('Checkout code') {
            steps {
                checkout scm
            }
        }

        stage('Clean workspace') {    // ✨ Thêm bước clean ở đây
            steps {
                dir('LoginTest') {
                    bat '''
                    if exist bin rmdir /s /q bin
                    if exist obj rmdir /s /q obj
                    '''
                }
            }
        }

        stage('Restore Packages') {
            steps {
                dir('LoginTest') {
                    bat 'dotnet restore'
                }
            }
        }

        stage('Build Project') {
            steps {
                dir('LoginTest') {
                    bat 'dotnet build --configuration Release'
                }
            }
        }

        stage('Run Tests') {
            steps {
                dir('LoginTest') {
                    bat 'dotnet test --no-build --verbosity normal'
                }
            }
        }
    }
}
