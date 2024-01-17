using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;

namespace CsuChhs.Imaging
{
    public static class Encoders
    {
        /// <summary>
        /// Given a content type of a byte array,
        /// return the proper encoder to use for
        /// thumbnail generation.
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static IImageEncoder GetEncoder(string contentType)
        {
            switch (contentType)
            {
                case "image/jpeg":
                    return new JpegEncoder();

                case "image/png":
                    return new PngEncoder();
            }
            throw new NotSupportedException($"The image content type {contentType} is not supported.");
        }
    }
}