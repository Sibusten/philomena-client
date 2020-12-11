using System.IO;
using System.Threading.Tasks;

namespace Philomena.Client
{
    public interface IImage
    {
        /// <summary>
        /// Downloads the image
        /// </summary>
        /// <returns>The image data</returns>
        Task<byte[]> DownloadAsync();

        /// <summary>
        /// Downloads the image to a stream
        /// </summary>
        /// <param name="stream">The stream to write the image data to</param>
        Task DownloadToAsync(Stream stream);

        /// <summary>
        /// Downloads the image data into a file
        /// </summary>
        /// <param name="filename">The file to save the image as</param>
        Task DownloadToFileAsync(string filename);
    }
}
