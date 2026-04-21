echo on
:: Publish everything as a single a self contained file
dotnet publish .\PensionMCP\PensionMCP.csproj -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o ./publish

:: create the mcpb installer file
if exist .\mcpb\server\PensionMCP.exe del /f /q .\mcpb\server\PensionMCP.exe
copy /y .\publish\PensionMCP.exe .\mcpb\server\PensionMCP.exe

pushd .\mcpb
cmd /c mcpb pack
popd

if exist .\mcpb\PensionAssistant.mcpb del /f /q .\mcpb\PensionAssistant.mcpb
ren .\mcpb\mcpb.mcpb PensionAssistant.mcpb
copy /y .\mcpb\PensionAssistant.mcpb .\publish\PensionAssistant.mcpb
echo Done.

