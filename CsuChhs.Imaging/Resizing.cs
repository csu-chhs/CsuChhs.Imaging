using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CsuChhs.Imaging
{
    public static class Resizing
    {

        /// <summary>
        /// Generates a thumbnail that is drawn to the minimum value.  This
        /// ensures that the aspect ratio is respected, and that the image
        /// is not cropped and has no pad bar.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="originalImage"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetThumbnail(int width, int height, 
            byte[] originalImage, string contentType)
        {

            using (MemoryStream inStream = new MemoryStream(originalImage))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (Image<Rgba32> image = Image.Load<Rgba32>(inStream))
                    {
                        ResizeOptions options = new ResizeOptions();
                        options.Mode = ResizeMode.Max;
                        options.Size = new Size(width, height);

                        image.Mutate(x => x.Resize(options));

                        image.Save(outStream, Encoders.GetEncoder(contentType));
                    }

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Generates a thumbnail that is cropped to fit the exact dimensions provided.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="originalImage"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetCroppedThumbnail(int width, int height, 
            byte[] originalImage, string contentType)
        {
            using (MemoryStream inStream = new MemoryStream(originalImage))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (Image<Rgba32> image = Image.Load<Rgba32>(inStream))
                    {
                        ResizeOptions options = new ResizeOptions();
                        options.Mode = ResizeMode.Crop;
                        options.Size = new Size(width, height);

                        image.Mutate(x => x.Resize(options));

                        image.Save(outStream, Encoders.GetEncoder(contentType));
                    }

                    return outStream.ToArray();
                }
            }
        }
    }
}