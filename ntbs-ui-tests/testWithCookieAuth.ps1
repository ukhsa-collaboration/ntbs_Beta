#!/usr/local/bin/powershell

.\node_modules\.bin\selenium-standalone.ps1 install

$SeleniumProcess = Start-Process powershell -Argument "node_modules/.bin/selenium-standalone.ps1 start" -PassThru

try {
    dotnet test --filter Category=CookieOverride
}
finally {
    taskkill /pid $SeleniumProcess.Id
}
