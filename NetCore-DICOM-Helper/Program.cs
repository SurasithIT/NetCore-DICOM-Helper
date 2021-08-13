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

                string inputFileName = "E3.dcm";
                string input = Path.Combine(Environment.CurrentDirectory, filePath, inputFileName);

                string outputFileName = "E1.jpg";
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
                    var ob = dicom.Dataset.GetDicomItem<DicomOtherByteFragment>(DicomTag.PixelData);
                    if (ob != null)
                    {
                        byte[] bytes = ob.Fragments[0].Data;
                        MemoryStream mstream = new MemoryStream(bytes);
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(mstream))
                        {
                            image.Save(outputFilePath);
                        }
                    }
                    else
                    {
                        DicomImage image = new DicomImage(dicom.Dataset);
                        var bitmap = image.RenderImage().AsSharedBitmap();
                        bitmap.Save(outputFilePath);
                    }
                }
            }
            else
            {
                throw new Exception("Original file not found");
            }
        }
    }
}
