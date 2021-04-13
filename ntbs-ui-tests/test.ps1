#!/usr/local/bin/powershell

.\node_modules\.bin\selenium-standalone.ps1 install --drivers.chrome.version=89.0.4389.23

$SeleniumProcess = Start-Process powershell -Argument "node_modules/.bin/selenium-standalone.ps1 start --drivers.chrome.version=89.0.4389.23" -PassThru

Get-Process -Id $SeleniumProcess.Id

Start-Sleep -s 10

try {
    dotnet test
}
finally {
    taskkill /pid $SeleniumProcess.Id
}
