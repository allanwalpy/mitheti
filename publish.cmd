:? publish wpf project as self contained;

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf ^
    --configuration Release --runtime win-x64 ^
    && call publish.copy_files wpf

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.self ^
    --configuration Release --runtime win-x64 ^
    --self-contained true ^
    && call publish.copy_files wpf.self

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.single ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true ^
    && call publish.copy_files wpf.single

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.single.optimized ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true ^
    && call publish.copy_files wpf.single.optimized

dotnet publish ./src/wpf/wpf.csproj --output ./out/publish/wpf.single.optimized.trimmed ^
    --configuration Release --runtime win-x64 ^
    --self-contained true -p:PublishSingleFile=true -p:PublishReadyToRun=true -p:PublishTrimmed=true ^
    && call publish.copy_files wpf.single.optimized.trimmed
