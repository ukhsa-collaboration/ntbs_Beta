#!/usr/local/bin/powershell

$SeleniumProcess = Start-Process "node_modules/.bin/selenium-standalone" -ArgumentList "start" -PassThru
dotnet test
Stop-Process -Id $SeleniumProcess.Id