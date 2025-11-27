namespace Backend.services.PiiTokenisation
{
    public interface ITokenisation
    {
        Dictionary<string, string> PiiToToken(List<string> piis);
        string BuildTokenisedText(string plainText, Dictionary<string, string> piiToToken);

    }
}
