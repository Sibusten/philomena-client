using Flurl;
using Flurl.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Philomena.Client.Api.Models;
using System.Threading;

namespace Philomena.Client
{
    public class Image : IImage
    {
        public ImageModel Model { get; init; }

        public Image(ImageModel model)
        {
            Model = model;
        }

        public Url DownloadUrl
        {
            get
            {
                if (Model.Representations is null)
                {
                    throw new InvalidOperationException("The image has no representations");
                }

                if (Model.Representations.Full is null)
                {
                    throw new InvalidOperationException("The image is missing a 'full' representation");
                }

                return new Url(Model.Representations.Full);
            }
        }

        public async Task<byte[]> DownloadAsync(CancellationToken cancellationToken = default)
        {
            return await DownloadUrl.GetBytesAsync(cancellationToken);
        }

        public async Task DownloadToAsync(Stream stream, CancellationToken cancellationToken = default)
        {
            Stream downloadStream = await DownloadUrl.GetStreamAsync(cancellationToken);
            await downloadStream.CopyToAsync(stream, cancellationToken);
        }

        public async Task DownloadToFileAsync(FileInfo file, CancellationToken cancellationToken = default)
        {
            await DownloadUrl.DownloadFileAsync(file.DirectoryName, file.Name, cancellationToken: cancellationToken);
        }
    }
}
