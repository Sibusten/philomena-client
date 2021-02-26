namespace Sibusten.Philomena.Client.Options
{
    public record ImageDownloadOptions
    {
        /// <summary>
        /// The maximum threads to use when downloading images. Defaults to 1.
        /// </summary>
        public int MaxDownloadThreads { get; init; } = 1;
    }
}
