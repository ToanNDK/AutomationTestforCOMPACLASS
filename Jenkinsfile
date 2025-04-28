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
                    if exist TestResults rmdir /s /q TestResults
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

        stage('Run Tests by Category') {
            steps {
                dir('LoginTest') {
                    script {
                        def categories = ["Login", "Register", "Quiz", "Overview", "Video"]
                        categories.each { cat ->
                            bat """
                            dotnet test --no-build --configuration Release --verbosity normal --filter "Category=${cat}" --logger "trx;LogFileName=test_results_${cat}.trx" --results-directory TestResults
                            """
                        }
                    }
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
