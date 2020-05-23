using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace CodeSpy
{
    class JDoodle
    {
        private const string endPoint =
            "https://api.jdoodle.com/v1/execute";
        private const string endPointCreditsCheck =
            "https://api.jdoodle.com/v1/credit-spent";

        MessageToCompile infoToCompileMessage;

        MessageToCheck creditsInfoMessage;

        HttpWebRequest request;

        CompilerOutputInfo compilerResult;

        private string json;
        private string result;
        private string code = "#include <stdio.h> \nint main() { int x = 10; int y = 25; int z = x + y; printf(\"Sum of x+y = %i\", z); }";

        public JDoodle()
        {
            infoToCompileMessage = new MessageToCompile
            {
                clientId = "6b4813b8a8bcaa306cf200cb1614b862",
                clientSecret =
                    "648bd55518cc25ec785b70ecf614298a1868679fe653517eace4567cb44bf86e",
                script = code,
                stdin = "",
                language = "c",
                versionIndex = "0"
            };

            creditsInfoMessage = new MessageToCheck
            {
                clientId = "6b4813b8a8bcaa306cf200cb1614b862",
                clientSecret =
                    "648bd55518cc25ec785b70ecf614298a1868679fe653517eace4567cb44bf86e"
            };

            compilerResult = new CompilerOutputInfo();
        }

        public async void SendMessage()
        {
            json = JsonConvert.SerializeObject(infoToCompileMessage);

            request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.ContentType = "application/json";
            request.Method = "POST";

            var stream = await request.GetRequestStreamAsync();

            using (var writer = new StreamWriter(stream))
            {
                writer.Write(json);
                writer.Flush();
                writer.Dispose();
            }
            
            var response = await request.GetResponseAsync();

            var respStream = response.GetResponseStream();

            using (StreamReader sr = new StreamReader(respStream))
            {
                result = sr.ReadToEnd();

                var resultExample = JsonConvert.DeserializeObject<CompilerOutputInfo>(result);   

                compilerResult.output = resultExample.output;
                compilerResult.statusCode = resultExample.statusCode;
                compilerResult.memory = resultExample.memory;
                compilerResult.cpuTime = resultExample.cpuTime;
            }
        }
    }
}