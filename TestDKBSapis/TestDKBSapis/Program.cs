using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using Microsoft.SDK.SharePointServices.Samples;

namespace TestDKBSapis
{
    class Program
    {
        static void Main(string[] args)
        {
            RetrieveListItems.TestMethod1();
            string siteUrl = "https://bookon.dkbs.dk/";

            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
            clientContext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
            clientContext.ExecuteQuery();
            // string contentTypeName = "Bookinger";
            Console.WriteLine(" Successfully Connected");
            List oList = clientContext.Web.Lists.GetByTitle("Bookinger");
            ListItemCollectionPosition itemPosition = null;
            ContentTypeCollection ctColl = oList.ContentTypes;
            clientContext.Load(ctColl);
            clientContext.ExecuteQuery();
            foreach (ContentType ct in ctColl)
            {
                if (ct.Name == "Bookinger" || ct.Name == "Gamle sager")
                {
                    Console.WriteLine(ct.Name);
                    Console.WriteLine("---Please wait while list item is displaying.---");
                    try
                    {
                        {
                            string contentTypeName = "Bookinger";
                            var query = new CamlQuery()
                            {

                                ViewXml = String.Format("<View Scope='RecursiveAll'><Query><Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>{0}</Value></Eq></Where></Query></View>", contentTypeName)
                                // ViewXml = String.Format("<View Scope='RecursiveAll'><RowLimit>5000</RowLimit></View>")

                            };

                            ListItemCollection collListItem = oList.GetItems(query);

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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            }
        }

    }

}

