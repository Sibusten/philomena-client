namespace Sibusten.Philomena.Client
{
    public record DownloadProgressInfo
    {
        public string Action { get; init; } = "";
        public long Current { get; init; }
        public long? Total { get; init; }
    }
}
