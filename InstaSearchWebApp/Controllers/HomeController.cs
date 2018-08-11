using InstaSearchWebApp.Common;
using InstaSearchWebApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;



namespace InstaSearchWebApp.Controllers
{
    public class HomeController : Controller
    {
        private MyDataBaseContext db = new MyDataBaseContext();
        private BingSearcher instagramSearch = new BingSearcher();
        private ViewModelSearch model = new ViewModelSearch {
            StringResults = "No Results.",            
            ErrorMsg = string.Empty
        };

        public ActionResult Index()
        {
            model.ErrorMsg = string.Empty;
            try
            {
                model.SearchHistory = db.Searches.OrderByDescending(st => st.SearchTime).ToList();
            }
            catch (Exception e)
            {
                model.ErrorMsg = e.Message;                
            }
            return View(model);
        }

        [HttpPost]      
        public ActionResult Index(string query)
        {
            model.ErrorMsg = string.Empty;
            model.StringResults = string.Empty;
                 
            if (query != null)
            {
                Search modelSearch = new Search() { Term = query, SearchTime = DateTime.Now };
                db.Searches.Add(modelSearch);
                db.SaveChanges();                
                try
                {
                    model.Result = instagramSearch.BingWebSearch(query);
                    model.SearchHistory = db.Searches.OrderByDescending(st => st.SearchTime).ToList();
                }
                catch (Exception e)
                {
                    model.StringResults = "No Results.";
                    model.ErrorMsg = "error accured: " + e.Message;
                    return View(model);                    
                }
                
                return View(model);              
            }
            model.ErrorMsg = "Query term is empty";
            return View(model);
            
        }

    }    

    public class BingSearcher {

        public const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        // Replace the accessKey string value with your valid access key.
        public const string accessKey = "9ee256727e7844fc83955c1cdac849bf";

        /// <summary>
        /// Performs a Bing Web search and return the results as a SearchResult.
        /// </summary>
        public SearchResult BingWebSearch(string searchQuery)
        {
            // Construct the URI of the search request
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery) + "+site:instagram.com";

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response;
            SearchResult searchResult = new SearchResult();
            try
            {
                response = (HttpWebResponse)request.GetResponseAsync().Result;
                string json = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var data = (JObject)JsonConvert.DeserializeObject(json);
                var pages = data["webPages"];
                var firstPage = pages["value"].First;

                searchResult.Name = firstPage["name"].ToString();
                searchResult.Url = firstPage["url"].ToString();
            }
            catch (Exception e)
            {

                throw e;
            }
            return searchResult;
        }

    }
}