:? publish wpf project as self contained;

dotnet publish ./src/wpf/wpf.csproj ^
    --output ./out/publish/ ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    -p:PublishSingleFile=true

:? copy other files to publish folder;
copy src\core\*.json out\publish
xcopy src\web\Properties out\publish\Properties /C/I/Y
xcopy src\wpf\Resources out\publish\Resources /C/I/Y
