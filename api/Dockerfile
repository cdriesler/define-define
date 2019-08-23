FROM cdriesler/rhinocompute:0001

# compile compute
COPY ["src", "src"]
RUN powershell -Command "nuget restore .\src"
RUN powershell -Command "dotnet msbuild /p:Configuration=Release .\src"

EXPOSE 80

CMD .\src\bin\Release\compute.frontend.exe