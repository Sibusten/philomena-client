using System.Text.Json.Serialization;

namespace Philomena.Client.Api.Models
{
    public class ImageErrorsModel
    {
        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image")]
        public string[]? Image { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_aspect_ratio")]
        public string[]? ImageAspectRatio { get; set; }

        /// <summary>
        /// When an image is unsupported (ex. WEBP)
        /// </summary>
        [JsonPropertyName("image_format")]
        public string[]? ImageFormat { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_height")]
        public string[]? ImageHeight { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_width")]
        public string[]? ImageWidth { get; set; }

        /// <summary>
        /// Usually if an image that is too large is uploaded.
        /// </summary>
        [JsonPropertyName("image_size")]
        public string[]? ImageSize { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_is_animated")]
        public string[]? ImageIsAnimated { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_mime_type")]
        public string[]? ImageMimeType { get; set; }

        /// <summary>
        /// Errors in the submitted image. If has already been taken is present, means the image already exists in the database.
        /// </summary>
        [JsonPropertyName("image_orig_sha512_hash")]
        public string[]? ImageOrigSha512Hash { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("image_sha512_hash")]
        public string[]? ImageSha512Hash { get; set; }

        /// <summary>
        /// Errors with the tag metadata.
        /// </summary>
        [JsonPropertyName("tag_input")]
        public string[]? TagInput { get; set; }

        /// <summary>
        /// Errors in the submitted image
        /// </summary>
        [JsonPropertyName("uploaded_image")]
        public string[]? UploadedImage { get; set; }
    }
}
