:? copy other files to publish folder;
copy src\core\*.json out\publish\%1
xcopy src\web\Properties out\publish\%1\Properties /C/I/Y
xcopy src\wpf\Resources out\publish\%1\Resources /C/I/Y
