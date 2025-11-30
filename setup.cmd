@echo off

REM Initialize git repository
git init

REM Create standard project directories
mkdir docs
mkdir docs\user
mkdir requirements
mkdir prompting

REM Symbolic links
mklink /D .\prompting\docs E:\Source\SES.Prompting\docs
mklink /D .\prompting\scripts E:\Source\SES.Prompting\src\scripts
mklink /D .\prompting\templates E:\Source\SES.Prompting\src\templates

REM Get current project name (folder name)
for %%I in (.) do set PROJECT_NAME=%%~nxI

REM Create integration tests test data structure
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\Commands"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\ExpectedLogs"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\ExpectedResults"
mkdir "src\%PROJECT_NAME%.IntegrationTests\TestData\Fixtures"
