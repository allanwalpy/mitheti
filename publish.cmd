:? publish wpf project as self contained;

dotnet publish mitheti.sln --output ./out/publish/self ^
    --configuration Release --runtime win-x64 ^
    --self-contained true

dotnet publish ./src/Wpf/Wpf.csproj --output ./out/publish/wpf.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true

dotnet publish ./src/Worker/Worker.csproj --output ./out/publish/worker.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true