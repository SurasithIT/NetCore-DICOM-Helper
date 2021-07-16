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

                string filePath = "../../../example_files";

                string inputFileName = "CT2_J2KR";
                string input = Path.Combine(Environment.CurrentDirectory, filePath, inputFileName);

                string outputFileName = "CT2_J2KR.jpg";
                string output = Path.Combine(Environment.CurrentDirectory, filePath, outputFileName);

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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
        }

        static void ConvertDcmToJpg(string inputFilePath, string outputFilePath)
        {
            bool exitsting = File.Exists(inputFilePath);

            if (exitsting)
            {
                using (Stream stream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
                {
                    DicomFile dicom = DicomFile.Open(stream);
                    DicomImage image = new DicomImage(dicom.Dataset);
                    var bitmap = image.RenderImage().AsSharedBitmap();
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
