@echo off

REM Initialize git repository
git init

REM Create standard project directories
mkdir doc
mkdir doc\assets
mkdir img
mkdir requirements

REM Get current project name (folder name)
for %%I in (.) do set PROJECT_NAME=%%~nxI

REM Create integration tests test data structure
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\Commands"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\ExpectedLogs"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\ExpectedResults"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\Fixtures"
