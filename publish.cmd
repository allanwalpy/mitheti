:? publish wpf project as self contained;

dotnet publish mitheti.sln --output ./out/publish/self ^
    --configuration Release --runtime win-x64 ^
    --self-contained true ^
    && call publish.copy_files self

dotnet publish ./src/Wpf/Wpf.csproj --output ./out/publish/wpf.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true ^
    && call publish.copy_files wpf.single.optimized

dotnet publish ./src/Worker/Worker.csproj --output ./out/publish/worker.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true ^
    && call publish.copy_files worker.single.optimized