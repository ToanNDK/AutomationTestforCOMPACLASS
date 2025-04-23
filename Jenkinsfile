pipeline {
    agent any

    stages {
        stage('Clean entire workspace') {
            steps {
                deleteDir()
            }
        }

        stage('Checkout code') {
            steps {
                checkout scm
            }
        }

        stage('Clean LoginTest') {
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
                    bat 'dotnet test --no-build --configuration Release --verbosity normal --logger "trx;LogFileName=test_results.trx"'
                }
            }
        }
    }

    post {
        always {
            
            junit allowEmptyResults: true, testResults: '**/LoginTest/TestResults/*.trx'
        }
    }
}
