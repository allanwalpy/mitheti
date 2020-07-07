:? builds projects as self contained;

dotnet publish ^
    --output ./out/publish_try2/ ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    -p:PublishReadyToRun=true ^
    -p:PublishSingleFile=true
