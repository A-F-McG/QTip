namespace Backend.services.PiiDetection
{
    public interface IPiiDetector
    {
        string Type { get; }
        List<string> DetectDistinct(string text);
    }
}
