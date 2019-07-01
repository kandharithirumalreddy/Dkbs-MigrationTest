using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace TestDKBSapis
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteUrl = "https://bookon.dkbs.dk/";

            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
            clientContext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
            clientContext.ExecuteQuery();
            string contentTypeName = "Bookinger";
            Console.WriteLine(" Successfully Connected");
            List oList = clientContext.Web.Lists.GetByTitle("Bookinger");
            ListItemCollectionPosition itemPosition = null;
            ContentTypeCollection ctColl = oList.ContentTypes;
            clientContext.Load(ctColl);
            clientContext.ExecuteQuery();
            foreach (ContentType ct in ctColl)
            {
                if (ct.Name == contentTypeName && contentTypeName == "Bookinger")
                {
                    //string query = $@"<View><ViewFields><FieldRef Name='Title'/></ViewFields>       
                    //                      <Query>
                    //                           <Where><Lt><FieldRef Name='ID' /><Value Type='Integer'>100</Value></Lt>
                    //                            </Where>
                    //                      <OrderBy><FieldRef Name='ID' /></OrderBy>
                    //                       </Query><RowLimit>100</RowLimit></View>";

                    string query = $@"<View>                            
                                            <Query>
                                                <Where>
                                                    <And>
                                                        <Geq>
                                                            <FieldRef Name='ID' /><Value Type='Integer'>1</Value>
                                                        </Geq>
                                                        <Leq>
                                                            <FieldRef Name='ID' /><Value Type='Integer'>1000</Value>
                                                        </Leq>                                        
                                                    </And>
                                                </Where>
                                             </Query></View>";

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ListItemCollectionPosition = itemPosition;
                    // camlQuery.ViewXml = @"<View><Query><RowLimit>500</RowLimit></Query></View>";
                   // camlQuery.ViewXml = @"<View Scope='RecursiveAll'><RowLimit>5000</RowLimit></View>";
                      camlQuery.ViewXml = query;
                     ListItemCollection collListItem = oList.GetItems(camlQuery);

                    clientContext.Load(collListItem);

                    clientContext.ExecuteQuery();
                    itemPosition = collListItem.ListItemCollectionPosition;
                    Console.WriteLine(itemPosition);

                    foreach (ListItem oListItem in collListItem)
                    {
                        Console.WriteLine("ID: {0} \nTitle: {1} ", oListItem.Id, oListItem["Title"]);
                    }
                }
            }

            //public static List<ListItem> GetAllListItemsInaList1()
            //{
            //    List<ListItem> items = new List<ListItem>();
            //    string sitrUrl = "https://spotenant.sharepoint.com/sites/yoursite";
            //    using (var ctx = new ClientContext(sitrUrl))
            //    {
            //        //ctx.Credentials = Your Credentials
            //        ctx.Load(ctx.Web, a => a.Lists);
            //        ctx.ExecuteQuery();

            //        List list = ctx.Web.Lists.GetByTitle("Documents");
            //        ListItemCollectionPosition position = null;
            //        // Page Size: 50
            //        int rowLimit = 50;
            //        var camlQuery = new CamlQuery();
            //        camlQuery.ViewXml = @"<View Scope='RecursiveAll'>
            //<Query>
            //    <OrderBy Override='TRUE'><FieldRef Name='ID'/></OrderBy>
            //</Query>
            //<ViewFields>
            //    <FieldRef Name='Title'/><FieldRef Name='Modified' /><FieldRef Name='Editor' />
            //</ViewFields>
            //<RowLimit Paged='TRUE'>" + rowLimit + "</RowLimit></View>";
            //        do
            //        {
            //            ListItemCollection listItems = null;
            //            camlQuery.ListItemCollectionPosition = position;
            //            listItems = list.GetItems(camlQuery);
            //            ctx.Load(listItems);
            //            ctx.ExecuteQuery();
            //            position = listItems.ListItemCollectionPosition;
            //            items.AddRange(listItems.ToList());
            //        }
            //        while (position != null);
            //    }
            //    return items;
            //}


            Console.WriteLine("Starting program...");
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create("https://login.microsoftonline.com/d3d8e52c-b3c6-4d7a-9857-34c82389369c/oauth2/token");
            tokenRequest.Method = "POST";
            string postData = "grant_type=+client_credentials&resource=+3f79188f-f9af-4203-96c1-4bcd76fd4fbc&client_id=+3f79188f-f9af-4203-96c1-4bcd76fd4fbc&client_secret=+ZB-bc%3FvCKBRt5F8dY2WEz0%5DDMRfOFrX%2B";
            //string postData = "grant_type=+client_credentials&resource=https://dkbsdev.crm4.dynamics.com&client_id=9d575b22-612f-4d84-a8c1-38de9e26846a&client_secret=9d575b22-612f-4d84-a8c1-38de9e26846a";
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] byte1 = encoding.GetBytes(postData);

            tokenRequest.ContentType = "application/x-www-form-urlencoded";

            tokenRequest.ContentLength = byte1.Length;
            Stream newStream = tokenRequest.GetRequestStream();
            newStream.Write(byte1, 0, byte1.Length);

            string jsonString = null;
            HttpWebResponse response = tokenRequest.GetResponse() as HttpWebResponse;
            using (Stream responseStream1 = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream1, Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            if (!string.IsNullOrEmpty(jsonString))
            {
                AuthenticationResponse authenticationResponse = JsonConvert.DeserializeObject<AuthenticationResponse>(jsonString);
                string accessToken = authenticationResponse.access_token;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    try
                    {
                        //for (int i = 1; i <= 1; i++)
                        {
                            string data = "{ \"firstName\": \"TestName\"," +
                            "\"lastName\": \"Røgind Jørgensen\"," +
                            "\"jobTitle\": \"Direktør\"," +
                            "\"telePhoneNumber\": \"123456\"," +
                            "\"email\": \"ega@itsmcompany.net\"," +
                            "\"partner\": \"214\"," +
                            "\"mailGroup\": \"string\"," +
                            "\"peSharePointId\": \"testSpecialCharacters27\"," +
                            "\"createdOn\": \"2019-05-18T08:33:33.723Z\"," +
                            "\"createdBy\": \"System account\"," +
                            "\"lastModified\": \"2019-05-18T08:33:33.723Z\"," +
                            "\"lastModifiedBY\": \"Eimantas\"," +
                            "\"modifiedOn\": \"2019-05-18T08:33:33.723Z\"," +
                            "\"modifiedBY\": \"Eimantas\"," +
                            "\"emailNotification\": true," +
                            "\"smsNotification\": true," +
                            "\"identifier\": \"test\"," +
                            "\"deactivatedUser\": false }";

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://dkbs-api-dev.azurewebsites.net/api/partneremployee");
                            request.ContentType = "application/json; charset=utf-8";
                            request.Headers.Add("Authorization", "Bearer " + accessToken);
                            request.Method = "POST";
                            /////////////////////////////////////////////////////
                            //request.Headers.Add("cache-control", "no-cache");
                            ////request.Headers.Add("accept-encoding", "gzip, deflate");
                            //request.Headers.Add("Cache-Control", "no-cache");
                            //request.KeepAlive = true;
                            //request.Host = "dkbs-api-dev.azurewebsites.net";
                            //request.Accept = "*/*";
                            //request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;


                            Encoding utfencoding = new UTF8Encoding();
                            byte[] byte2 = utfencoding.GetBytes(data);

                            request.ContentLength = byte2.Length;
                            Stream newStream2 = request.GetRequestStream();
                            newStream2.Write(byte2, 0, byte2.Length);

                            string responseString = null;
                            using (HttpWebResponse dataResponse = request.GetResponse() as HttpWebResponse)
                            {
                                StreamReader reader = new StreamReader(dataResponse.GetResponseStream());
                                responseString = reader.ReadToEnd();
                                Console.WriteLine("Post request result: " + responseString);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    Console.ReadLine();
                }
            }
        }
    }
    public class AuthenticationResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string ext_expires_in { get; set; }
        public string expires_on { get; set; }
        public string not_before { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
    }
}
