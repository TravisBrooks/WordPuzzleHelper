using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace WordPuzzleHelperConsole
{
    public static class ConfigValues
    {
        private static readonly IConfigurationRoot _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

        private static string _ReadStr(string key)
        {
            var rawVal = _config[key] ?? string.Empty;
            return rawVal.Trim();
        }

        public static string WordFileName => _ReadStr("word-file");

        public static string[] DefaultAlphabet
        {
            get
            {
                var alphaStr = _ReadStr("default-alphabet");
                var alphas = alphaStr
                    .Where(c => char.IsWhiteSpace(c) == false)
                    .Select(c => c.ToString().ToLower())
                    .ToArray();
                return alphas;
            }
        }
    }
}
