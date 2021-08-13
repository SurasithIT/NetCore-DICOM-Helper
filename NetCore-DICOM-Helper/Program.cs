using System;
using System.Drawing;
using System.IO;
using Dicom;
using Dicom.Imaging;

namespace NetCore_DICOM_Helper
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Start convert DICOM file to Jpg image");
                ImageManager.SetImplementation(new WinFormsImageManager());

                string filePath = "../../../example_files";

                string inputFileName = "E1.dcm";
                string input = Path.Combine(Environment.CurrentDirectory, filePath, inputFileName);

                string outputFileName = "E1.jpg";
                string output = Path.Combine(Environment.CurrentDirectory, filePath, outputFileName);

                ConvertDcmToJpg(input, output, true, 200);
                if (File.Exists(output))
                {
                    Console.WriteLine("Convert success");
                }
                else
                {
                    throw new Exception("Convert fail");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
        }

        static void ConvertDcmToJpg(string inputFilePath, string outputFilePath, bool resize = false, decimal maxSize = 0)
        {
            bool exitsting = File.Exists(inputFilePath);

            if (exitsting)
            {
                using (Stream stream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    DicomFile dicom = DicomFile.Open(stream);
                    var ob = dicom.Dataset.GetDicomItem<DicomOtherByteFragment>(DicomTag.PixelData);
                    if (ob != null)
                    {
                        byte[] bytes = ob.Fragments[0].Data;
                        MemoryStream mstream = new MemoryStream(bytes);
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(mstream))
                        {
                            decimal width = image.Width;
                            decimal height = image.Height;
                            if (resize == true) CalculateNewImageSize(maxSize, image.Width, image.Height, out width, out height);
                            var bitmap = new Bitmap(image, new Size(Decimal.ToInt32(width), Decimal.ToInt32(height)));
                            bitmap.Save(outputFilePath);
                        }
                    }
                    else
                    {
                        DicomImage image = new DicomImage(dicom.Dataset);
                        var bitmap = image.RenderImage().AsSharedBitmap();

                        decimal width = image.Width;
                        decimal height = image.Height;
                        if (resize == true) CalculateNewImageSize(maxSize, image.Width, image.Height, out width, out height);
                        var _bitmap = new Bitmap(bitmap, new Size(Decimal.ToInt32(width), Decimal.ToInt32(height)));

                        _bitmap.Save(outputFilePath);
                    }
                }
            }
            else
            {
                throw new Exception("Original file not found");
            }
        }

        static void CalculateNewImageSize(decimal maxSize, decimal imageWidth, decimal imageHeight, out decimal newWidth, out decimal newHeight)
        {
            decimal width = 0;
            decimal ratio = 0;
            decimal height = 0;

            if (imageWidth > imageHeight)
            {
                width = maxSize;
                ratio = Decimal.Divide(width, imageWidth);
                height = imageHeight * ratio;
            }
            if (imageWidth < imageHeight)
            {
                height = maxSize;
                ratio = Decimal.Divide(height, imageHeight);
                width = imageWidth * ratio;
            }
            if (imageWidth == imageHeight)
            {
                height = maxSize;
                width = maxSize;
            }

            newWidth = width;
            newHeight = height;
        }
    }
}
