%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\msbuild build.proj /target:Release /Property:Configuration=Release /Property:Platform="Any CPU"
if errorlevel 0 exit else pause