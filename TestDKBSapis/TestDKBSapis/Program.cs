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
using SP = Microsoft.SharePoint.Client;

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

            //Connected successfully
            getListData odata = new getListData();

            //odata.getBookingerListData(clientContext);

            //odata.GetPatenerKursuspakkeListData(clientContext);

            //odata.getDocumentLib(clientContext);

            //odata.GetPatenerAktiviteterListData(clientContext);

            //odata.GetPatenerInspirationskategorierListData(clientContext);

            //odata.GetPatenerCenterbeskrivelseListData(clientContext);

            //odata.GetPatenerCentretsListData(clientContext);

            //odata.GetPatenerCentretitalListData(clientContext);

            //odata.getImages(clientContext);

            
        }

    }
    public class getListData
    {

        public void getBookingerListData(ClientContext clientContext)
        {
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

        public void getDocumentLib(ClientContext clientContext)
        {
            SP.List oListData = clientContext.Web.Lists.GetByTitle("Aftaler & dokumenter");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);
            clientContext.Load(oListDataItem);
            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine(oItem["ID"]);
                Console.WriteLine(oItem["Title"]);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                if (oItem["RelatedPartnerType"] != null)
                {
                    var childIdField = oItem["RelatedPartnerType"] as FieldLookupValue[];

                    if (childIdField != null)
                    {
                        foreach (var lookupValue in childIdField)
                        {
                            var childId_Value = lookupValue.LookupValue;
                            var childId_Id = lookupValue.LookupId;

                            Console.WriteLine("LookupID: " + childId_Id.ToString());
                            Console.WriteLine("LookupValue: " + childId_Value.ToString());
                        }
                    }
                }
            }
        }

        public void GetPatenerKursuspakkeListData(ClientContext clientContext)
        {
            SP.List oList = clientContext.Web.Lists.GetByTitle("Partnere");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {

                var hyperLink = ((SP.FieldUrlValue)(oListItem["CISite"]));
                if (hyperLink != null)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1} \nSite: {2} \nSiteUrl: {3} ", oListItem.Id, oListItem["Title"], oListItem["CISite"], oListItem["CISiteShortUrl"]);
                    var hLink = ((SP.FieldUrlValue)(oListItem["CISite"])).Url;
                    Console.WriteLine(hLink);


                    ClientContext Context = new ClientContext(hLink);
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
                    Context.ExecuteQuery();
                    SP.List oListData = Context.Web.Lists.GetByTitle("Kursuspakke");

                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    Context.Load(oListDataItem);

                    Context.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine("ID: {0} \nTitle: {1} \nKursuspakkeUK:{2} \nOffered: {3} \nPrice:{4}", oItem.Id, oItem["Title"], oItem["KursuspakkeUK"], oItem["Offered"], oItem["Price"]);
                        Console.WriteLine("IncludedInPriceAdditional: {0} \nIncludedInPriceDefault:{1} \nOptionalPurchases: {2} \nPricePerYear:{3}", oItem["IncludedInPriceAdditional"], oItem["IncludedInPriceDefault"], oItem["OptionalPurchases"], oItem["PricePerYear"]);

                    }

                }

            }
        }

        public void GetPatenerAktiviteterListData(ClientContext clientContext)
        {
            SP.List oList = clientContext.Web.Lists.GetByTitle("Partnere");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {

                var hyperLink = ((SP.FieldUrlValue)(oListItem["CISite"]));
                if (hyperLink != null)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1} \nSite: {2} \nSiteUrl: {3} ", oListItem.Id, oListItem["Title"], oListItem["CISite"], oListItem["CISiteShortUrl"]);
                    var hLink = ((SP.FieldUrlValue)(oListItem["CISite"])).Url;
                    Console.WriteLine(hLink);


                    ClientContext Context = new ClientContext(hLink);
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
                    Context.ExecuteQuery();
                    SP.List oListData = Context.Web.Lists.GetByTitle("Aktiviteter");

                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    Context.Load(oListDataItem);

                    Context.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine("ID: {0} \nHeadline: {1}", oItem["ID"], oItem["Headline"]);
                    }

                }

            }
        }

        public void GetPatenerInspirationskategorierListData(ClientContext clientContext)
        {
            SP.List oList = clientContext.Web.Lists.GetByTitle("Partnere");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {

                var hyperLink = ((SP.FieldUrlValue)(oListItem["CISite"]));
                if (hyperLink != null)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1} \nSite: {2} \nSiteUrl: {3} ", oListItem.Id, oListItem["Title"], oListItem["CISite"], oListItem["CISiteShortUrl"]);
                    var hLink = ((SP.FieldUrlValue)(oListItem["CISite"])).Url;
                    Console.WriteLine(hLink);


                    ClientContext Context = new ClientContext(hLink);
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
                    Context.ExecuteQuery();
                    SP.List oListData = Context.Web.Lists.GetByTitle("Inspirationskategorier (EN)");

                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    Context.Load(oListDataItem);

                    Context.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine("ID: {0} \nHeadline: {1}", oItem["ID"], oItem["Headline"]);
                    }

                }

            }
        }

        public void GetPatenerCenterbeskrivelseListData(ClientContext clientContext)
        {
            SP.List oList = clientContext.Web.Lists.GetByTitle("Partnere");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {

                var hyperLink = ((SP.FieldUrlValue)(oListItem["CISite"]));
                if (hyperLink != null)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1} \nSite: {2} \nSiteUrl: {3} ", oListItem.Id, oListItem["Title"], oListItem["CISite"], oListItem["CISiteShortUrl"]);
                    var hLink = ((SP.FieldUrlValue)(oListItem["CISite"])).Url;
                    Console.WriteLine(hLink);


                    ClientContext Context = new ClientContext(hLink);
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
                    Context.ExecuteQuery();
                    SP.List oListData = Context.Web.Lists.GetByTitle("Centerbeskrivelse");

                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    Context.Load(oListDataItem);

                    Context.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine("ID: {0} \nDescription: {1} \nLanguageType:{2} \nRooms:{3}", oItem["ID"], oItem["Description"], oItem["LanguageType"], oItem["Rooms"]);
                        Console.WriteLine("TraficConnections: {0} \nCapacity: {1} \nFacilities:{2} \nActivities:{3}", oItem["TraficConnections"], oItem["Capacity"], oItem["Facilities"], oItem["Activities"]);
                        Console.WriteLine("TextOffer: {0} \nFurtherIncluded: {1}", oItem["TextOffer"], oItem["FurtherIncluded"]);

                    }

                }

            }
        }

        public void GetPatenerCentretsListData(ClientContext clientContext)
        {
            
                    SP.List oListData = clientContext.Web.Lists.GetByTitle("Centrets lokaler i tal");
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    clientContext.Load(oListDataItem);

                    clientContext.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                    }
        }

        public void GetPatenerCentretitalListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Centret_i_tal");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

            clientContext.Load(oListDataItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
            }
        }

        public void getImages(ClientContext clientContext)
        {
            SP.List oList = clientContext.Web.Lists.GetByTitle("Partnere");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {

                var hyperLink = ((SP.FieldUrlValue)(oListItem["CISite"]));
                if (hyperLink != null)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1} \nSite: {2} \nSiteUrl: {3} ", oListItem.Id, oListItem["Title"], oListItem["CISite"], oListItem["CISiteShortUrl"]);
                    var hLink = ((SP.FieldUrlValue)(oListItem["CISite"])).Url;
                    Console.WriteLine(hLink);


                    ClientContext Context = new ClientContext(hLink);
                    Context.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
                    Context.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
                    Context.ExecuteQuery();
                    SP.List oListData = Context.Web.Lists.GetByTitle("Billeder");

                    camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
                    ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                    Context.Load(oListDataItem);

                    Context.ExecuteQuery();

                    foreach (ListItem oItem in oListDataItem)
                    {
                        Console.WriteLine(oItem["ID"]);
                        Console.WriteLine(oItem["FileLeafRef"]);
                        Console.WriteLine(oItem["FileRef"]);
                        Console.WriteLine(oItem["Title"]);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                        Console.WriteLine(oItem["Created"].ToString());
                        Console.WriteLine(oItem["Modified"].ToString());

                    }

                }

            }
        }
    }

}



