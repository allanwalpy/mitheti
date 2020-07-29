using System;
using System.Collections.Generic;
using Rasyidf.Localization;

namespace Mitheti.Wpf
{
    public static class Extensions
    {
        //TODO:TMP:tmp solution;
        public static string Translate(this string key, string uid = App.LangUid, string @default = "")
            => LocalizationService.GetString(uid, key, @default);

        //TODO:TMP:tmp solution;
        public static List<string> TranslateAsList(this string key, string uid = App.LangUid)
        {
            var isContinue = true;
            var result = new List<string>();

            for (var i = 0; isContinue; i++)
            {
                try
                {
                    var value = $"{key}:{i}".Translate(uid, null);

                    if (string.IsNullOrEmpty(value))
                    {
                        isContinue = false;
                    }
                    else
                    {
                        result.Add(value);
                    }
                }
                catch (Exception)
                {
                    isContinue = false;
                }
            }

            return result;
        }

        //TODO:TMP:tmp solution;
        public static List<int> TranslateAsIntList(this string key, string uid = App.LangUid)
            => key.TranslateAsList(uid).ConvertAll(int.Parse);
    }
}