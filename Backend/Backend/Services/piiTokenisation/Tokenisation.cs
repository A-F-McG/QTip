namespace Backend.services.PiiTokenisation
{
    public class Tokenisation: ITokenisation
    {
        public Dictionary<string, string> PiiToToken(List<string> piis)
        {
            return piis.ToDictionary(
                pii => pii,
                pii => Guid.NewGuid().ToString()
            );
        }

        public string BuildTokenisedText(string plainText, Dictionary<string, string> piiToToken)
        {
            var tokenisedText = plainText;

            foreach (var p in piiToToken)
            {
                tokenisedText = tokenisedText.Replace(p.Key, p.Value);
            }

            return tokenisedText;
        }
    }
}
