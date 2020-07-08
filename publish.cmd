:? publish wpf project as self contained;

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.self ^
    --configuration Release --runtime win-x64 ^
    --self-contained true ^
    && call publish.copy_files wpf.self

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true ^
    && call publish.copy_files wpf.single.optimized
