using Newtonsoft.Json;
using search.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace search.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("Filter");
        }
        [HttpPost]
        public JsonResult GetCars(string Model,string Make)
        {
            string urlAddress = "http://localhost:8983/solr/cars/select?fl=ID,Make,Model,Year,HorsePower,TopSpeed,Price,ImageUrl&rows=7267&indent=on&q=Make:\"" + Make+"\" AND Model:\""+Model+"\"&wt=json";
            string data = GetData(urlAddress);
                int l1 = data.IndexOf("docs") + 6;
                int l2 = data.LastIndexOf("}}");
                data = data.Substring(l1, (l2 - l1));
            var jarray = JsonConvert.DeserializeObject<List<Car>>(data);
            return Json(jarray, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchCars(string searchKey="")
        {
            string urlAddress = "http://localhost:8983/solr/cars/select?fl=ID,Make,Model,Year,HorsePower,TopSpeed,Price,ImageUrl&rows=7267&indent=on&q=Make:" + searchKey + "* OR Model:" + searchKey + "*&wt=json";
            string data = GetData(urlAddress);
                int l1 = data.IndexOf("docs") + 6;
                int l2 = data.LastIndexOf("}}");
                data = data.Substring(l1, (l2 - l1));
            var jarray = JsonConvert.DeserializeObject<List<Car>>(data);
            return Json(jarray, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetModel(string MakeName)
        {
            string urlAddress = "http://localhost:8983/solr/cars/select?facet.field=Model&rows=7267&facet=on&fl=Model&q=Make:" + MakeName + "&wt=json";

            List<string> SList = new List<string>();
            string data = GetData(urlAddress);

            int l1 = data.IndexOf("\"facet_fields\":{\"Model\":") + 24;
            int l2 = data.IndexOf(",\"facet_ranges");
            data = data.Substring(l1, (l2 - l1));
            foreach (Match match in Regex.Matches(data, "\".*?\""))
                SList.Add(match.ToString().Split('"')[1]);
        List<string> stl = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(SList));
            return Json(stl, JsonRequestBehavior.AllowGet);
    }


    [HttpPost]
        public JsonResult GetMake()
        {
            string urlAddress = "http://localhost:8983/solr/cars/select?facet.field=Make&rows=7267&facet=on&fl=Make&q=Make:*&wt=json";
            string data = GetData(urlAddress);
            List<string> SList = new List<string>();
                int l1 = data.IndexOf("\"facet_fields\":{\"Make\":") + 23;
                int l2 = data.IndexOf(",\"facet_ranges");
                data = data.Substring(l1, (l2 - l1));
                foreach (Match match in Regex.Matches(data, "\".*?\""))
                    SList.Add(match.ToString().Split('"')[1]);
            List<string> stl = JsonConvert.DeserializeObject<List<string>>(JsonConvert.SerializeObject(SList));
            return Json(stl,JsonRequestBehavior.AllowGet); 
            
        }


        public string GetData(string inputURL)
        {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputURL);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string data = "";
            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                data = readStream.ReadToEnd();
                readStream.Close();
            }
            response.Close();
            return data;
        }
    }
}