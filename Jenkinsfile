pipeline {
    agent any

    stages {
        stage('Clean entire workspace') {    // ✨ Thêm bước clean tổng trước checkout
            steps {
                deleteDir()
            }
        }

        stage('Checkout code') {
            steps {
                checkout scm
            }
        }

        stage('Clean LoginTest') {    // ✨ Clean riêng thư mục LoginTest
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
