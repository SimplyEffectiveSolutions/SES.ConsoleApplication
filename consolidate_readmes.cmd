@echo off
setlocal enabledelayedexpansion

REM Delete READMES directory if it exists, then create it fresh
if exist READMES (
    echo Removing existing READMES folder...
    rmdir /s /q READMES
)
echo Creating new READMES folder...
mkdir READMES

REM Find README*.md and README*.txt files and copy them with path in filename the 
echo Searching for README files with .md or .txt extensions...
for /f "delims=" %%F in ('dir /s /b README*.md README*.txt') do (
    REM Get the relative path
    set "fullpath=%%F"
    set "relpath=!fullpath:%CD%\=!"
    
    REM Replace path separators with underscores
    set "newname=!relpath:\=_!"
    
    REM Copy file to READMES folder
    echo Copying "!relpath!" to "READMES\!newname!"
    copy "%%F" "READMES\!newname!"
)

echo Done. All README.md and README.txt files have been copied to the READMES folder.
pause