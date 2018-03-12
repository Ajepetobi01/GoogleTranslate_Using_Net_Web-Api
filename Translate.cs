using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace GoogleCloudTranslate.Controllers
{
    public class Translate : ApiController
    {


        [HttpGet, Route("api/Translate/Text")]
        public IHttpActionResult TransText(string q, string target)
        {


            try
            {

                //url for detection
                string url = "https://translation.googleapis.com/language/translate/v2/detect?key=[YOUR_GOOGLE_API_KEY]";
                //the text to be detected as q
                url += "&q=" + q;

                WebClient client = new WebClient();

                //Bring the detected result
                string json = client.DownloadString(url);
                JsonData jsondata = (new JavaScriptSerializer()).Deserialize<JsonData>(json);
                //set result here
                var langdet = jsondata.data.detections[0];
                var detectedlang = langdet[0].language;

                //url for translation
                string Url = "https://translation.googleapis.com/language/translate/v2?key=[YOUR_GOOGLE_API_KEY]";
                //attach source language, target language and the text
                Url += "&source=" + detectedlang;
                Url += "&target=" + target;
                Url += "&q=" + q;
                WebClient clienttwo = new WebClient();
                string json2 = clienttwo.DownloadString(Url);
                JsonData jsondata2 = (new JavaScriptSerializer()).Deserialize<JsonData>(json2);

                //set result here!
                var finaltext = jsondata2.data.Translations[0].TranslatedText;

                //return result
                return Ok(finaltext);

            }
            catch (Exception ex)
            {

                return BadRequest("Error" + ex);
            }

        }



        public class detection
        {

            public string confidence { get; set; }

            public string isReliable { get; set; }

            public string language { get; set; }
        }




        public class Translation
        {
            public string TranslatedText { get; set; }

        }
        public class Data
        {


            public List<Translation> Translations { get; set; }

            public List<List<detection>> detections { get; set; }


        }

        public class JsonData
        {
            public Data data { get; set; }
        }


    }
}
