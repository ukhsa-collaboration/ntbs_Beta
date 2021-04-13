#!/usr/local/bin/powershell

ping 127.0.0.1

telnet host 4444
netstat -an

.\node_modules\.bin\selenium-standalone.ps1 install --drivers.chrome.version=89.0.4389.23

netstat -an

try {
    .\node_modules\.bin\selenium-standalone.ps1 start --drivers.chrome.version=89.0.4389.23
}
finally {
    netstat -an
}
