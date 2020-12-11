using Nullforce.Api.Derpibooru.JsonModels;

namespace Philomena.Client.Api
{
    public interface IPhilomenaApi
    {
        /// <summary>
        /// Fetches a comment response for the comment ID
        /// </summary>
        /// <param name="commentId">The comment ID to fetch</param>
        /// <returns>The comment response</returns>
        CommentJson GetComment(int commentId);

        /// <summary>
        /// Fetches an image response for the image ID
        /// </summary>
        /// <param name="imageId">The image ID to fetch</param>
        /// <returns>The image response</returns>
        ImageJson GetImage(int imageId, string? apiKey = null);

        /// <summary>
        /// Submits a new image. Both key and url are required. Errors will result in an {"errors":image-errors-response}. (TODO)
        /// </summary>
        /// <param name="apiKey">The API key of the user submitting the image</param>
        /// <param name="imageUrl">The direct URL to the image file</param>
        /// <returns></returns>
        ImageJson SubmitImage(string apiKey, string imageUrl);

        /// <summary>
        /// Fetches an image response for the for the current featured image.
        /// </summary>
        /// <returns>The featured image</returns>
        ImageJson GetFeaturedImage();

        /// <summary>
        /// Fetches a tag response for the tag slug.
        /// </summary>
        /// <param name="tagSlug">The tag slug to fetch</param>
        /// <returns>The tag response</returns>
        TagJson GetTag(string tagSlug);

        /// <summary>
        /// Fetches a post response for the post ID
        /// </summary>
        /// <param name="postId">The post ID to fetch</param>
        /// <returns>The post response</returns>
        PostJson GetPost(int postId);
    }
}
