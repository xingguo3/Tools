// image to pdf
// add image to pdf with proper size

private string mergeImagesToPdf(List<string> images)
{
    Document document = new Document();
    var pdfName = getTempPDFFileName(Path.GetDirectoryName(images.FirstOrDefault()), Path.GetFileNameWithoutExtension(images.FirstOrDefault()));
    using (var stream = new FileStream(pdfName, FileMode.Create, FileAccess.Write, FileShare.None))
    {
        PdfWriter.GetInstance(document, stream);
        document.Open();
        images.ForEach(a =>
        {
            if (File.Exists(a))
            {
                var image = Image.GetInstance(a);
                
                float percentage = 0.0f;
                if (image.Height > image.Width)
                {
                    percentage = 700 / image.Height;
                }
                else
                {
                    percentage = 540 / image.Height;
                }
                image.ScalePercent(percentage * 100);
                image.Border = Rectangle.NO_BORDER;
                document.Add(image);
            }
            else
            {
            }
            
        });
        document.Close();
    }
    if (File.Exists(pdfName))
    {
        return pdfName;
    }
    else
    {
        return "";
    }
    
}
