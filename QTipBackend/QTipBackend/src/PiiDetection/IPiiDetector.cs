namespace QTipBackend.PiiDetection
{
    public interface IPiiDetector
    {
        string Type { get;  }
        List<string> Detect(string text);
    }
}
