using System;
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

                string input = @"/Users/surasith/Downloads/example";
                string output = @"/Users/surasith/Downloads/example.jpg";

                ConvertDcmToJpg(input, output);
                if (File.Exists(output))
                {
                    Console.WriteLine("Convert success");
                }
                else
                {
                    throw new Exception("Convert fail");
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            
            
           
        }

        static void ConvertDcmToJpg(string inputFilePath, string outputFilePath)
        {
            bool exitsting = File.Exists(inputFilePath);

            if (exitsting)
            {
                using (Stream stream = new FileStream(inputFilePath, FileMode.Open))
                {
                    DicomFile dicom = DicomFile.Open(stream);
                    DicomImage image = new DicomImage(dicom.Dataset);
                    var bitmap = image.RenderImage().AsBitmap();
                    bitmap.Save(outputFilePath);
                }
            }
            else
            {
                throw new Exception("Original file not found");
            }
        }
    }
}
