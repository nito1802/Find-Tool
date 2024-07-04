@echo off
setlocal enabledelayedexpansion

:: Ustawienie zmiennej baseDir
set "baseDir=C:\Users\dante\Desktop\Istotne\MojeDane\2024\lipiec\04_07_2024\Inne"

:: Przechodzenie do folderu głównego
cd /d "%baseDir%"

:: Pullowanie wszystkich repozytoriów
for /d %%d in (*) do (
    if exist "%%d\.git" (
        pushd "%%d"

        :: Usuwanie lokalnych branży poza "dev" i "master"
        for /f "tokens=*" %%b in ('git branch ^| findstr /v "dev master"') do (
            echo Deleting local branch %%b
            git branch -d %%b
        )

        popd
    )
)

endlocal
pause
