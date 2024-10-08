@echo off

dotnet test ../GoogleLibrary.Test/GoogleLibrary.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover" --verbosity normal /p:CoverletOutput=../GoogleLibrary.Test/coverage.opencover.xml
dotnet test ../GoogleServices.Test/GoogleServices.Test.csproj /p:CollectCoverage=true /p:CoverletOutputFormat="opencover" --verbosity normal /p:CoverletOutput=../GoogleServices.Test/coverage.opencover.xml

dotnet tool install -g dotnet-reportgenerator-globaltool

reportgenerator "-reports:../GoogleLibrary.Test/coverage.opencover.xml;../GoogleServices.Test/coverage.opencover.xml" "-targetdir:../coverage/" "-reporttypes:Badges;Html;TextSummary"

IF [%1]==[-s] GOTO:show 
IF [%1]==[--show] GOTO:show

GOTO:eof

:show

  rundll32 url.dll,FileProtocolHandler ..\coverage\index.html
