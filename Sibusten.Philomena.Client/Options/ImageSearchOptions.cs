using System;

namespace Sibusten.Philomena.Client.Options
{
    public enum SvgMode
    {
        RasterOnly,
        SvgOnly,
        Both
    }

    public record ImageSearchOptions
    {
        /// <summary>
        /// The API Key to use when querying
        /// </summary>
        /// <value></value>
        public string? ApiKey { get; init; }

        /// <summary>
        /// The filter for the query
        /// </summary>
        public int? FilterId { get; init; }

        /// <summary>
        /// The sort order for the query
        /// </summary>
        public SortOptions? SortOptions { get; init; }

        /// <summary>
        /// Limits the number of images queried. Defaults to querying all images.
        /// </summary>
        public int MaxImages
        {
            get => _maxImages;
            init
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Image limit must be greater than 0", nameof(value));
                }
                _maxImages = value;
            }
        }
        private int _maxImages = int.MaxValue;

        /// <summary>
        /// The behavior for SVG images. Defaults to downloading only raster images.
        /// </summary>
        public SvgMode SvgMode { get; init; } = SvgMode.RasterOnly;
    }
}
