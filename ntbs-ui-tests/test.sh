#!/bin/bash
set -ex

trap "kill 0" EXIT

./node_modules/.bin/selenium-standalone start &
dotnet test

kill 0
