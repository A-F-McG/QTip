using Backend.services.PiiDetection;
using System.Text.RegularExpressions;

namespace Backend.Services.piiDetection
{
    public class PhoneDetector: IPiiDetector
    {
        private static readonly Regex PhoneRegex;

        static PhoneDetector()
        {
            PhoneRegex = new Regex(@"\b\d{10}\b");
        }

        public string Type => "pii.phone";

        public List<string> DetectDistinct(string text)
        {
            return [.. PhoneRegex.Matches(text).Select(m => m.Value).Distinct()];
        }
    }
}
