using System;
using Microsoft.Extensions.Localization;

namespace Mitheti.Wpf.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        // = null можно убрать
        private IStringLocalizer _localizer = null;

        public IStringLocalizer Create(Type resourceSource)
            => GetLocalizer();

        public IStringLocalizer Create(string baseName, string location)
            => GetLocalizer();

        private IStringLocalizer GetLocalizer()
        {
            //есть оператор ??=
            if (_localizer == null)
            {
                _localizer = new JsonStringLocalizer();
            }

            return _localizer;
        }
    }
}
