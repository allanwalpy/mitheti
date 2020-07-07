:? builds projects as self contained;

dotnet publish ^
    --output ./out/publish ^
    --configuration Release ^
    --runtime win-x64 ^
    --self-contained true ^
    -p:PublishTrimmed=true ^
    -p:PublishReadyToRun=true

:TODO: figure out why `-p:PublishSingleFile=true` fails on web project;
