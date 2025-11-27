using System.Text.RegularExpressions;

namespace QTipBackend.PiiDetection
{
    public class EmailDetector: IPiiDetector
    {
        private static readonly Regex EmailRegex;

        static EmailDetector()
        {
            const string wordBoundary = @"\b";
            const string local = @"[A-Za-z\d.\-_]+"; // allow letters, numbers, periods, hyphens and underscores
            const string domainName = @"[A-Za-z\d]+"; // allow letters and numbers
            const string topLevelDomain = @"[A-Za-z]+"; // allow letters

            string pattern = $"{wordBoundary}{local}@{domainName}\\.{topLevelDomain}{wordBoundary}";

            EmailRegex = new Regex(pattern, RegexOptions.Compiled);
        }

        public string Type => "pii.email";

        public List<string> Detect(string text)
        {
            return [.. EmailRegex.Matches(text).Select(m => m.Value)];
        }
    }
}
