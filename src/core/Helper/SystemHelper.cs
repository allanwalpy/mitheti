using System;

namespace Mitheti.Core.Helper
{
    public static class SystemHelper
    {
        public static string Format(this string template, params object[] args)
            => String.Format(template, args: args);
    }
}
