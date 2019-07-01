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
           // string contentTypeName = "Bookinger";
            Console.WriteLine(" Successfully Connected");
            List oList = clientContext.Web.Lists.GetByTitle("Bookinger");
            ListItemCollectionPosition itemPosition = null;
            ContentTypeCollection ctColl = oList.ContentTypes;
            clientContext.Load(ctColl);
            clientContext.ExecuteQuery();
            foreach (ContentType ct in ctColl)
            {
               // if (ct.Name == contentTypeName && contentTypeName == "Bookinger")
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
        }

    }       
}
