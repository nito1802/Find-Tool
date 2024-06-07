@echo off
setlocal enabledelayedexpansion

:: Ustawienie zmiennej baseDir
set "baseDir=C:\Users\dante\Desktop\Istotne\source\Visual Studio\Main"

:: Przechodzenie do folderu głównego
cd /d "%baseDir%"

:: Pullowanie wszystkich repozytoriów
for /d %%d in (*) do (
    if exist "%%d\.git" (
        echo Pulling changes in %%d
        pushd %%d
        git pull
        popd
    )
)

endlocal
pause