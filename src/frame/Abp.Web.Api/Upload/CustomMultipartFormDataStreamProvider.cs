using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Abp.Helper;

namespace Abp.WebApi.Upload
{
    /// <summary>
    /// Represents an System.Net.Http.IMultipartStreamProvider suited for use with HTML
    /// file uploads for writing file content to a System.IO.FileStream.
    /// </summary>
    public class CustomMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        /// <summary>
        /// Gets or sets the root path where the content of MIME multipart body parts are written to.
        /// </summary>
        public string Root { get; set; }

        /// <summary>
        /// Initializes a new instance of the System.Net.Http.CustomMultipartFormDataStreamProvider class
        /// </summary>
        /// <param name="root">The root path where the content of MIME multipart body parts are written to.</param>
        public CustomMultipartFormDataStreamProvider(string root)
            : base(root)
        {
            Root = root;
        }

        /// <summary>
        /// Gets the name of the local file which will be combined with the root path to
        /// create an absolute file name where the contents of the current MIME body part
        /// will be stored.
        /// </summary>
        /// <param name="headers">The headers for the current MIME body part.</param>
        /// <returns>A relative filename with no path component.</returns>
        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string filePath = headers.ContentDisposition.FileName;

            // Multipart requests with the file name seem to always include quotes.
            if (filePath.StartsWith(@"""") && filePath.EndsWith(@""""))
                filePath = filePath.Substring(1, filePath.Length - 2);
            
            var extension = Path.GetExtension(filePath);

            return CombHelper.NewComb() + extension;
        }

    }
}
