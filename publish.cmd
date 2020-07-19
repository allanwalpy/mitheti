:? publish wpf project as self contained;

dotnet publish ./src/Wpf/Wpf.csproj --output ./out/publish/wpf.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true