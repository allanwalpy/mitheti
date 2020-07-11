using System;
using Microsoft.Extensions.Localization;

namespace Mitheti.Wpf.Localization
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private IStringLocalizer _localizer = null;

        public IStringLocalizer Create(Type resourceSource)
            => GetLocalizer();

        public IStringLocalizer Create(string baseName, string location)
            => GetLocalizer();

        private IStringLocalizer GetLocalizer()
        {
            if (_localizer == null)
            {
                _localizer = new JsonStringLocalizer();
            }

            return _localizer;
        }
    }
}