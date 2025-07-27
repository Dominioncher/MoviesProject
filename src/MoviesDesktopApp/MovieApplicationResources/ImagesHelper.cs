using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

using System.Drawing;
using System.IO;
using System.Reflection;

namespace MovieApplicationResources
{
    public static class ImagesHelper
    {
        public enum ImageSize
        {
            Small,
            Large
        }

        public static Image GetImage(string name, ImageSize size)
        {
            var fullName = $"MovieApplicationResources.Resources.singleimages.{name}_{GetImageSizeString(size)}.png";
            var manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fullName);
            if (manifestResourceStream == null)
            {
                return null;
            }

            return Image.FromStream(manifestResourceStream);
        }

        private static string GetImageSizeString(ImageSize size)
        {
            if (size == ImageSize.Small)
            {
                return "16x16";
            }

            return "32x32";
        }
    }
}
