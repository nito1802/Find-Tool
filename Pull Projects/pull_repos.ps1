param (
    [string]$baseDir
)

# Przechodzenie do folderu głównego
Set-Location -Path $baseDir

# Pullowanie wszystkich repozytoriów
Get-ChildItem -Directory | ForEach-Object {
    $repoDir = $_.FullName
    if (Test-Path "$repoDir\.git") {
        Write-Host "Pulling changes in $repoDir"
        Set-Location -Path $repoDir
        git pull
        Set-Location -Path $baseDir
    }
}
