byte[] oldBytes = File.ReadAllBytes(imagePath);
byte[] newBytes;
using (MemoryStream sourceMS = new MemoryStream(oldBytes))
{
    using (System.Drawing.Image oldImage = System.Drawing.Bitmap.FromStream(sourceMS))
    {
        using (System.Drawing.Image newImage = ShrinkImage(oldImage, 0.4f))
        {
            newBytes = ConvertImageToBytes(newImage, 85);
        }
    }
}
image = iTextSharp.text.Image.GetInstance(newBytes);


private static byte[] ConvertImageToBytes(System.Drawing.Image image, long compressionLevel)
{
    if (compressionLevel < 0)
    {
        compressionLevel = 0;
    }
    else if (compressionLevel > 100)
    {
        compressionLevel = 100;
    }
    System.Drawing.Imaging.ImageCodecInfo jgpEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Png);

    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
    System.Drawing.Imaging.EncoderParameters myEncoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
    System.Drawing.Imaging.EncoderParameter myEncoderParameter = new System.Drawing.Imaging.EncoderParameter(myEncoder, compressionLevel);
    myEncoderParameters.Param[0] = myEncoderParameter;
    using (MemoryStream ms = new MemoryStream())
    {
        image.Save(ms, jgpEncoder, myEncoderParameters);
        return ms.ToArray();
    }

}
private static System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
{
    System.Drawing.Imaging.ImageCodecInfo[] codecs = System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
    foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
    {
        if (codec.FormatID == format.Guid)
        {
            return codec;
        }
    }
    return null;
}

private static System.Drawing.Image ShrinkImage(System.Drawing.Image sourceImage, float scaleFactor)
{
    int newWidth = Convert.ToInt32(sourceImage.Width * scaleFactor);
    int newHeight = Convert.ToInt32(sourceImage.Height * scaleFactor);

    var thumbnailBitmap = new System.Drawing.Bitmap(newWidth, newHeight);
    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbnailBitmap))
    {
        g.CompositingQuality = CompositingQuality.HighSpeed;
        g.SmoothingMode = SmoothingMode.HighSpeed;
        g.InterpolationMode = InterpolationMode.Bicubic;
        System.Drawing.Rectangle imageRectangle = new System.Drawing.Rectangle(0, 0, newWidth, newHeight);
        g.DrawImage(sourceImage, imageRectangle);
    }
    return thumbnailBitmap;
}
