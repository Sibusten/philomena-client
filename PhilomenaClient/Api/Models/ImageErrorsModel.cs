using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class ImageErrorsModel
    {
        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image")]
        public List<string>? Image { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_aspect_ratio")]
        public List<string>? ImageAspectRatio { get; set; }

        /// <summary>
        /// When an image is unsupported (ex. WEBP)
        /// </summary>
        [JsonPropertyName("image_format")]
        public List<string>? ImageFormat { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_height")]
        public List<string>? ImageHeight { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_width")]
        public List<string>? ImageWidth { get; set; }

        /// <summary>
        /// Usually if an image that is too large is uploaded.
        /// </summary>
        [JsonPropertyName("image_size")]
        public List<string>? ImageSize { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_is_animated")]
        public List<string>? ImageIsAnimated { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_mime_type")]
        public List<string>? ImageMimeType { get; set; }

        /// <summary>
        /// Errors in the submitted image. If has already been taken is present, means the image already exists in the database.
        /// </summary>
        [JsonPropertyName("image_orig_sha512_hash")]
        public List<string>? ImageOrigSha512Hash { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_sha512_hash")]
        public List<string>? ImageSha512Hash { get; set; }

        /// <summary>
        /// Errors with the tag metadata.
        /// </summary>
        [JsonPropertyName("tag_input")]
        public List<string>? TagInput { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("uploaded_image")]
        public List<string>? UploadedImage { get; set; }
    }
}
