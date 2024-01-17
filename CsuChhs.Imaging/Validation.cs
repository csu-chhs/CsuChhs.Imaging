using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

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
        public static bool IsValidThumbnailSize(int width, int height, 
            byte[] originalImage, string contentType)
        {
            if (!IsValidThumbnailContentType(contentType))
            {
                return false;
            }

            using (MemoryStream inStream = new MemoryStream(originalImage))
            {
                using (Image<Rgba32> image = Image.Load<Rgba32>(inStream))
                {
                    return image.Width == width && image.Height == height;
                }
            }
        }
    }
}