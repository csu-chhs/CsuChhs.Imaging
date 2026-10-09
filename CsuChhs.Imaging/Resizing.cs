using SkiaSharp;

namespace CsuChhs.Imaging
{
    public static class Resizing
    {
        /// <summary>
        /// Generates a thumbnail that is drawn to the minimum value.  This
        /// ensures that the aspect ratio is respected, and that the image
        /// is not cropped and has no pad bar.
        /// </summary>
        /// <param name="maxHeight"></param>
        /// <param name="originalImage"></param>
        /// <param name="contentType"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        public static byte[] GetThumbnail(
            int maxWidth,
            int maxHeight,
            byte[] originalImage,
            string contentType)
        {
            using var inputStream = new MemoryStream(originalImage);

            using var originalBitmap = SKBitmap.Decode(inputStream);

            // Calculate scale preserving aspect ratio (ResizeMode.Max)
            float scale = Math.Min(
                (float)maxWidth / originalBitmap.Width,
                (float)maxHeight / originalBitmap.Height);

            // Prevent upsizing
            scale = Math.Min(scale, 1.0f);

            int newWidth = (int)(originalBitmap.Width * scale);
            int newHeight = (int)(originalBitmap.Height * scale);

            using var resizedBitmap = new SKBitmap(newWidth, newHeight);

            using (var canvas = new SKCanvas(resizedBitmap))
            {
                canvas.Clear(SKColors.Transparent);

                canvas.DrawBitmap(
                    originalBitmap,
                    new SKRect(0, 0, newWidth, newHeight),
                    new SKSamplingOptions(
                        SKFilterMode.Linear,
                        SKMipmapMode.Linear));
            }

            using var image = SKImage.FromBitmap(resizedBitmap);

            SKEncodedImageFormat format = contentType.ToLower() switch
            {
                "image/png" => SKEncodedImageFormat.Png,
                "image/webp" => SKEncodedImageFormat.Webp,
                _ => SKEncodedImageFormat.Jpeg
            };

            using var data = image.Encode(format, 90);

            return data.ToArray();
        }

        /// <summary>
        /// Generates a thumbnail that is cropped to fit the exact dimensions provided.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="originalImage"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static byte[] GetCroppedThumbnail(
            int width,
            int height,
            byte[] originalImage,
            string contentType)
        {
            using var inputStream = new MemoryStream(originalImage);
            using var originalBitmap = SKBitmap.Decode(inputStream);

            // Scale the image so that it completely fills the target area.
            // This matches ImageSharp's ResizeMode.Crop behavior.
            float scale = Math.Max(
                (float)width / originalBitmap.Width,
                (float)height / originalBitmap.Height);

            float scaledWidth = originalBitmap.Width * scale;
            float scaledHeight = originalBitmap.Height * scale;

            // Center the image within the target area.
            float offsetX = (width - scaledWidth) / 2f;
            float offsetY = (height - scaledHeight) / 2f;

            using var outputBitmap = new SKBitmap(width, height);

            using (var canvas = new SKCanvas(outputBitmap))
            {
                canvas.Clear(SKColors.Transparent);

                using var paint = new SKPaint
                {
                    IsAntialias = true
                };

                canvas.DrawBitmap(
                    originalBitmap,
                    new SKRect(
                        offsetX,
                        offsetY,
                        offsetX + scaledWidth,
                        offsetY + scaledHeight),
                    new SKSamplingOptions(SKFilterMode.Linear),
                    paint);
            }

            using var image = SKImage.FromBitmap(outputBitmap);

            SKEncodedImageFormat format = contentType.ToLowerInvariant() switch
            {
                "image/png" => SKEncodedImageFormat.Png,
                "image/webp" => SKEncodedImageFormat.Webp,
                "image/gif" => SKEncodedImageFormat.Gif,
                _ => SKEncodedImageFormat.Jpeg
            };

            using var data = image.Encode(format, 90);

            return data.ToArray();
        }
    }
}