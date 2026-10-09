
using SkiaSharp;

namespace CsuChhs.Imaging
{
    public static class Validation
    {
        /// <summary>
        /// Ensure that the passed in content type is valid
        /// for making thumbnails.
        /// </summary>
        /// <param name="contentType"></param>
        public static bool IsValidThumbnailContentType(string contentType)
        {
            return ValidContentTypes().Contains(contentType);
        }

        private static List<String> ValidContentTypes()
        {
            List<String> contentTypes = ["image/png", "image/jpeg"];
            return contentTypes;
        }

        /// <summary>
        /// Validates that the size of an image correctly 
        /// matches the given width and height in pixels.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="originalImage"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool IsValidThumbnailSize(
            int width,
            int height,
            byte[] originalImage,
            string contentType)
        {
            if (!IsValidThumbnailContentType(contentType))
            {
                return false;
            }

            using var stream = new MemoryStream(originalImage);

            using var codec = SKCodec.Create(stream);

            if (codec == null)
            {
                return false;
            }

            return codec.Info.Width == width &&
                   codec.Info.Height == height;
        }
    }
}