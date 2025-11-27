namespace Backend.services.Submissions
{
    public interface IDatabaseOperations
    {
        Task<int> GetPiiCount();
        Task InsertPiiClassifications(List<string> piis, Dictionary<string, string> piiToToken);
    }
}
