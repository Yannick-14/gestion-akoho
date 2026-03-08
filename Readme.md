<!-- compilation du projet -->
<!-- Repertoire actuelle -->
>> msbuild

<!-- installation nuget -->
>> Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "C:\nuget.exe"

<!-- apres succes de build on fait -->
>> iisexpress /path:"Y:\MY PROJECT\Akoho\Akoho ASPX" /port:8080
