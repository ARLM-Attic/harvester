%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\msbuild build.proj /target:Release /Property:Configuration=Release /Property:Platform="Any CPU"
if errorlevel 1 pause else exit