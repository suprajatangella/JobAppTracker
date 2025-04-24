//using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.Drawing.Imaging;

namespace JobAppTracker.Web.Helpers
{
    public static class AvatarGenerator
    {
        public static string GenerateAvatar(string fullName, string wwwRootPath)
        {
            string initials = string.Join("", fullName
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w[0])).ToUpper();

            int width = 200;
            int height = 200;
            var image = new Bitmap(width, height);
            var graphics = Graphics.FromImage(image);

            // Background color
            graphics.Clear(Color.LightGray);

            // Draw initials
            var font = new Font("Arial", 60, FontStyle.Bold, GraphicsUnit.Pixel);
            var brush = Brushes.DarkSlateGray;
            var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString(initials, font, brush, new Rectangle(0, 0, width, height), format);

            string fileName = $"{Guid.NewGuid()}.png";
            string filePath = Path.Combine(wwwRootPath, "profileimg", fileName);
            image.Save(filePath, ImageFormat.Png);

            return "/profileimg/" + fileName;
        }
        }
}
