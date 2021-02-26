using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Sibusten.Philomena.Api;
using Sibusten.Philomena.Api.Models;
using Sibusten.Philomena.Client.Options;

namespace Sibusten.Philomena.Client.Images
{
    public class PagedPhilomenaImageSearch : IPhilomenaImageSearch
    {
        private const int _perPage = 50;

        private PhilomenaApi _api;
        private string _query;
        private ImageSearchOptions _options;

        public PagedPhilomenaImageSearch(PhilomenaApi api, string query, ImageSearchOptions? options)
        {
            _api = api;
            _query = query;
            _options = options ?? new ImageSearchOptions();
        }

        public async IAsyncEnumerable<IPhilomenaImage> BeginSearch([EnumeratorCancellation] CancellationToken cancellationToken = default, IProgress<MetadataDownloadProgressInfo>? progress = null)
        {
            // TODO: Optimize this process and make use of multiple threads
            // TODO: Enumerate using id.gt/id.lt when possible

            // Start on the first page
            int page = 1;

            // Track images processed
            int imagesProcessed = 0;

            // Set the random seed if needed
            int? _randomSeed = _options.SortOptions?.RandomSeed;
            if (_options.SortOptions?.SortField == SortField.Random && _options.SortOptions?.RandomSeed is null)
            {
                _randomSeed = _api.GetRandomSearchSeed();  // TODO: Expose this method in the API interface
            }

            // Enumerate images
            ImageSearchModel search;
            do
            {
                // Get the current page of images
                search = await _api.SearchImagesAsync(_query, page, _perPage, _options.SortOptions?.SortField, _options.SortOptions?.SortDirection, _options.FilterId, _options.ApiKey, _randomSeed);

                if (search.Images is null)
                {
                    throw new InvalidOperationException("The search query did not provide a list of images");
                }

                // Update metadata progress if given
                if (progress is not null)
                {
                    if (search.Total is null)
                    {
                        throw new InvalidOperationException("The search query did not provide a total image count");
                    }

                    // Determine how much metadata has been downloaded
                    int metadataDownloaded = imagesProcessed + search.Images.Count;

                    // Cap the total number of images downloaded at the limit
                    int totalImagesToDownload = Math.Min(search.Total.Value, _options.MaxImages);

                    // Avoid reporting more metadata downloaded than total images to download
                    metadataDownloaded = Math.Min(metadataDownloaded, totalImagesToDownload);

                    // Update progress
                    MetadataDownloadProgressInfo progressInfo = new MetadataDownloadProgressInfo()
                    {
                        Downloaded = metadataDownloaded,
                        Total = totalImagesToDownload
                    };
                    progress.Report(progressInfo);
                }

                // Yield the images
                foreach (ImageModel imageModel in search.Images)
                {
                    IPhilomenaImage image = new PhilomenaImage(imageModel);

                    if (image.IsSvgImage)
                    {
                        if (_options.SvgMode is SvgMode.RasterOnly or SvgMode.Both)
                        {
                            // Provide the original image which represents the raster version
                            yield return image;
                        }

                        if (_options.SvgMode is SvgMode.SvgOnly or SvgMode.Both)
                        {
                            // Provide a new image which represents the SVG version
                            IPhilomenaImage svgImage = new PhilomenaImage(imageModel)
                            {
                                IsSvgVersion = true
                            };
                            yield return svgImage;
                        }
                    }
                    else
                    {
                        yield return image;
                    }

                    imagesProcessed++;
                    if (imagesProcessed >= _options.MaxImages)
                    {
                        yield break;
                    }
                }

                // Move to the next page
                page++;
            }
            while (search.Images.Any());  // Stop when there are no more images
        }
    }
}
