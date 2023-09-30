using System.Text;

internal record WebFetchHandlerConfig(string Name, string URL, TimeSpan BackoffTime, string FileSinkDir, List<string> Symbols)
{
    public const string WebFetchHandlerConfigName = "WebFetchHandlerConfig";
    public WebFetchHandlerConfig() : this("", "", TimeSpan.Zero, "", new List<string>()) { }

    // override the TOString method to make it easier to debug
    public override string ToString()
    {
        var str = new StringBuilder();
        str.Append($"Name: {Name}\n");
        str.Append($"URL: {URL}\n");
        str.Append($"BackoffTime: {BackoffTime}\n");
        str.Append($"FileSinkDir: {FileSinkDir}\n");
        str.Append("Symbols:\n");
        foreach (var symbol in Symbols)
        {
            str.Append($"- {symbol}\n");
        }
        return str.ToString();
    }
}







