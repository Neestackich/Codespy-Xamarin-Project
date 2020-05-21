using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace CodeSpy
{
    public static class Ocr
    {
<<<<<<< HEAD

=======
>>>>>>> 620d01f4a74be0f48deae5e1c76ed54b3aeda375
        private const string SubscriptionKey = 
            "";

        private const string UriBase = 
<<<<<<< HEAD
            "https://ocrcodespy.cognitiveservices.azure.com/";

=======
            "";
>>>>>>> 620d01f4a74be0f48deae5e1c76ed54b3aeda375

        public static async Task<string> GetTextAsync(Stream imageFilePath)
        {
            try
            {
                using (var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(SubscriptionKey))
                { Endpoint = UriBase })
                {
                    var result =
                        await client.RecognizePrintedTextInStreamWithHttpMessagesAsync(true, imageFilePath);
                    return result.Body.GetText();
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        private static string GetText(this OcrResult result)
        {
            var builder = new StringBuilder();

            foreach (var region in result.Regions)
            {
                foreach (var line in region.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        builder.Append(word.Text);
                        builder.Append(" ");
                    }

                    builder.AppendLine();
                }

                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
