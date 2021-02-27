using System;
using System.IO;
using Sibusten.Philomena.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace Sibusten.Philomena.Client.Images
{
    public class PhilomenaImage : IPhilomenaImage
    {
        public ImageModel Model { get; private init; }
        private readonly int _id;
        public bool IsSvgVersion { get; init; } = false;

        public bool IsSvgImage => Model.Format == "svg";

        public PhilomenaImage(ImageModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (model.Id is null)
            {
                throw new ArgumentNullException("Image is missing an ID", nameof(model.Id));
            }

            Model = model;
            _id = model.Id.Value;
        }

        public int Id => _id;

        public string? Name
        {
            get
            {
                if (Model.ViewUrl is null)
                {
                    return null;
                }

                string localPath = new Uri(Model.ViewUrl).LocalPath;
                return Path.GetFileNameWithoutExtension(localPath);
            }
        }

        public string? OriginalName
        {
            get
            {
                if (Model.Name is null)
                {
                    return null;
                }

                return Path.GetFileNameWithoutExtension(Model.Name);
            }
        }

        public string? ShortViewUrl
        {
            get
            {
                string? shortViewUrl = Model.Representations?.Full;

                if (shortViewUrl is null)
                {
                    return null;
                }

                if (IsSvgVersion)
                {
                    // Modify the URL to point to the SVG image
                    string urlWithoutExtension = shortViewUrl.Substring(0, shortViewUrl.LastIndexOf('.'));
                    return urlWithoutExtension + ".svg";
                }

                // Return the normal URL
                return shortViewUrl;
            }
        }

        public string? ViewUrl
        {
            get
            {
                string? viewUrl = Model.ViewUrl;
                if (viewUrl is null)
                {
                    return null;
                }

                if (IsSvgVersion)
                {
                    // Modify the URL to point to the SVG image
                    string urlWithoutExtension = viewUrl.Substring(0, viewUrl.LastIndexOf('.'));
                    return urlWithoutExtension + ".svg";
                }

                // Return the normal URL
                return viewUrl;
            }
        }

        public string? Format
        {
            get
            {
                if (IsSvgImage)
                {
                    // The image is an SVG image, which has two possible formats
                    // Assume rasters are always png
                    return IsSvgVersion ? "svg" : "png";
                }

                return Model.Format;
            }
        }

        public int? FileSize
        {
            get
            {
                if (IsSvgVersion)
                {
                    // The size of the SVG image is not known
                    return null;
                }

                return Model.Size;
            }
        }

        public string? Hash => Model.Sha512Hash;
        public string? OriginalHash => Model.OrigSha512Hash;
        public List<string> TagNames => Model.Tags?.ToList() ?? new List<string>();  // .ToList to prevent editing the original model list
        public List<int> TagIds => Model.TagIds?.ToList() ?? new List<int>();  // .ToList to prevent editing the original model list
        public int? Score => Model.Score;
        public string? SourceUrl => Model.SourceUrl;
        public bool? IsSpoilered => Model.IsSpoilered;
        public int? TagCount => Model.TagCount;
        public bool? ThumbnailsGenerated => Model.ThumbnailsGenerated;
        public DateTime? UpdatedAt => Model.UpdatedAt;
        public string? Uploader => Model.Uploader;
        public int? UploaderId => Model.UploaderId;
        public int? Upvotes => Model.Upvotes;
        public bool? Processed => Model.Processed;
        public string? MimeType => Model.MimeType;
        public bool? IsAnimated => Model.IsAnimated;
        public double? AspectRatio => Model.AspectRatio;
        public int? CommentCount => Model.CommentCount;
        public DateTime? CreatedAt => Model.CreatedAt;
        public string? DeletionReason => Model.DeletionReason;
        public string? Description => Model.Description;
        public int? Downvotes => Model.Downvotes;
        public int? Width => Model.Width;
        public int? DuplicateOf => Model.DuplicateOf;
        public int? Faves => Model.Faves;
        public DateTime? FirstSeenAt => Model.FirstSeenAt;
        public int? Height => Model.Height;
        public bool? IsHiddenFromUsers => Model.IsHiddenFromUsers;
        public double? Duration => Model.Duration;
        public double? WilsonScore => Model.WilsonScore;
    }
}
