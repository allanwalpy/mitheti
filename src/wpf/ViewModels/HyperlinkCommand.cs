using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Mitheti.Wpf.ViewModels
{
    public class HyperlinkCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => parameter is string;

        public void Execute(object parameter)
        {
            var url = new Uri(parameter as string ?? throw new NullReferenceException(nameof(parameter)));
            var info = new ProcessStartInfo
            {
                //? see why on https://github.com/dotnet/runtime/issues/28005#issuecomment-442214248 ;
                FileName = url.AbsoluteUri,
                UseShellExecute = true
            };

            Process.Start(info);
        }
    }
}