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

            odata.GetPatenerInspirationskategorierListData(clientContext);

            //odata.GetPatenerCenterbeskrivelseListData(clientContext);

            //odata.GetPatenerCentretsListData(clientContext);

            //odata.GetPatenerCentretitalListData(clientContext);

            //odata.getImages(clientContext);

            //odata.GetPatenerDiskussionsforumListData(clientContext);

            //odata.GetPatenerNyhederListData(clientContext);

            //odata.GetPatenerProceduresListData(clientContext);

            //odata.GetPatenerProvisionListData(clientContext);

            //odata.GetPatenerServicekatalogListData(clientContext);

            //odata.GetPatenerServicerequestcommunicationsListData(clientContext);

            //odata.GetPatenerServicerequestconversationitemsListData(clientContext);

            //odata.GetPatenerServicerequestnotesListData(clientContext);

        }

    }
    public class getListData
    {
        //Bookinger
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
                            string contentTypeName = ct.Name;
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

        //Aftaler & dokumenter --Done
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
                Console.WriteLine(oItem["URL"]);
                Console.WriteLine(oItem["ContentTypeId"].ToString());
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

        //Kursuspakke --Done
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
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                        Console.WriteLine(oItem["Created"].ToString());
                        Console.WriteLine(oItem["Modified"].ToString());
                        Console.WriteLine(oItem["KursuspakkeID"]);
                        
                    }

                }

            }
        }

        //Aktiviteter --Done
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
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                        Console.WriteLine(oItem["Created"].ToString());
                        Console.WriteLine(oItem["Modified"].ToString());
                        Console.WriteLine(oItem["Title"]);
                        Console.WriteLine(oItem["Description"]);
                        Console.WriteLine(oItem["Picture"]);
                        Console.WriteLine(oItem["Price"]);
                     }

                }

            }
        }

        //Inspirationskategorier(EN) --Done
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
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                        Console.WriteLine(oItem["Created"].ToString());
                        Console.WriteLine(oItem["Modified"].ToString());
                        Console.WriteLine(oItem["Title"]);
                        Console.WriteLine(oItem["Description"]);
                        Console.WriteLine(oItem["Picture"]);
                        Console.WriteLine(oItem["Price"]);
                     }

                }

            }
        }

        //Centerbeskrivelse --Done
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
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                        Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                        Console.WriteLine(oItem["Created"].ToString());
                        Console.WriteLine(oItem["Modified"].ToString());
                    }

                }

            }
        }

        //Centrets lokaler i tal --Done
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
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                Console.WriteLine(oItem["MaxPeopleAtTable"]);
                Console.WriteLine(oItem["MaxPeopleAtUTable"]);
                Console.WriteLine(oItem["MaxPeopleAtSchoolBoard"]);
                Console.WriteLine(oItem["MaxPeopleByIsland"]);
                Console.WriteLine(oItem["MaxPeopleInOneRoom"]);
                Console.WriteLine(oItem["DivideRoom"]);
                Console.WriteLine(oItem["Remarks"]);
            }
        }

        //Centret_i_tal --Done
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
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());

                Console.WriteLine(oItem["TotalNumberOfRooms"]);
                Console.WriteLine(oItem["SingleRooms"]);
                Console.WriteLine(oItem["DoubleRooms"]);
                Console.WriteLine(oItem["Suite"]);
                Console.WriteLine(oItem["PrivateRoom"]);

                Console.WriteLine(oItem["HandicapRooms"]);
                Console.WriteLine(oItem["DistanceToAdditionalAccomodation"]);
                Console.WriteLine(oItem["Plenum"]);
                Console.WriteLine(oItem["TeamRoom"]);
                Console.WriteLine(oItem["DistanceToAirport"]);

                Console.WriteLine(oItem["DistanceToTrainStation"]);
                Console.WriteLine(oItem["DistanceToBusStop"]);
                Console.WriteLine(oItem["DistanceToMotorway"]);
                Console.WriteLine(oItem["NumberOfFreeParkingSpace"]);
                Console.WriteLine(oItem["DistanceToTheFreeParking"]);

                Console.WriteLine(oItem["NumberOfPaidParkingSpace"]);
                Console.WriteLine(oItem["DistanceToThePaidParking"]);
                Console.WriteLine(oItem["MaxDiners"]);
                Console.WriteLine(oItem["MaximumSeatsInAuditorium"]);
                Console.WriteLine(oItem["GreenArea"]);
                Console.WriteLine(oItem["AgreementForEmployees"]);
                Console.WriteLine(oItem["HandicapFriendly"]);
                Console.WriteLine(oItem["StateAgreement"]);
                Console.WriteLine(oItem["Bar"]);
                Console.WriteLine(oItem["Lounge"]);

                Console.WriteLine(oItem["NumberOfPaidParkingSpace"]);
                Console.WriteLine(oItem["Spa"]);
                Console.WriteLine(oItem["Golf"]);
                Console.WriteLine(oItem["Pool"]);
                Console.WriteLine(oItem["AirCon"]);
                Console.WriteLine(oItem["FitnessRoom"]);
                Console.WriteLine(oItem["CookingSchool"]);
                Console.WriteLine(oItem["Casino"]);
                Console.WriteLine(oItem["Lounge"]);

                Console.WriteLine(oItem["AreaRestaurant"]);
                Console.WriteLine(oItem["EnvironmentalCertificate"]);
                Console.WriteLine(oItem["MinimumAttendees"]);
                Console.WriteLine(oItem["MaximumTableSeats"]);
                Console.WriteLine(oItem["MaximumAccommodations"]);
                Console.WriteLine(oItem["MaximumSeats"]);
                Console.WriteLine(oItem["DinningArea"]);
                Console.WriteLine(oItem["NumberOfRooms"]);



            }
        }

        //Billeder
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

        //Diskussionsforum --Done
        public void GetPatenerDiskussionsforumListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Diskussionsforum");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

            clientContext.Load(oListDataItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                Console.WriteLine(oItem["Body"]);
                Console.WriteLine(oItem["IsQuestion"]);
                Console.WriteLine(oItem["ParentItemEditor"]);
                Console.WriteLine(oItem["LastReplyBy"]);
                if (oItem["MailGroups"]!=null)
                {
                    Console.WriteLine(oItem["MailGroups"]);
                    var childIdField = oItem["MailGroups"] as FieldLookupValue[];

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
                if (oItem["RelatedCIs"] != null)
                {
                    Console.WriteLine(oItem["RelatedCIs"]);
                    var childIdField = oItem["RelatedCIs"] as FieldLookupValue[];

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
                if (oItem["RelatedPartnerType"] != null)
                {
                    Console.WriteLine(oItem["RelatedPartnerType"]);
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

        //Nyheder --Done
        public void GetPatenerNyhederListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Nyheder");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

            clientContext.Load(oListDataItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                Console.WriteLine(oItem["PublishAnnouncement"]);
                Console.WriteLine(oItem["ExpirationDate"]);
                Console.WriteLine(oItem["Publish"]);
                Console.WriteLine(oItem["Public"]);
                Console.WriteLine(oItem["DescriptionRichText"]);
                if (oItem["RelatedCIID"] != null)
                {
                    Console.WriteLine(oItem["RelatedCIID"]);
                    var childIdField = oItem["RelatedCIID"] as FieldLookupValue[];

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
                if (oItem["AnnounceOnlyTo"] != null)
                {
                    Console.WriteLine(oItem["AnnounceOnlyTo"]);
                    var childIdField = oItem["AnnounceOnlyTo"] as FieldLookupValue[];

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

        //Procedures
        public void GetPatenerProceduresListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Procedures");
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

        //Provision
        public void GetPatenerProvisionListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Provision");
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

        //Service katalog --Done
        public void GetPatenerServicekatalogListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service katalog");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

            clientContext.Load(oListDataItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                Console.WriteLine(oItem["KursuspakkeUK"]);
                Console.WriteLine(oItem["Offered"]);
                Console.WriteLine(oItem["Price"]);
                Console.WriteLine(oItem["KursuspakkeID"]);
                Console.WriteLine(oItem["IncludedInPriceDefault"]);
                Console.WriteLine(oItem["IncludedInPriceAdditional"]);
                Console.WriteLine(oItem["OptionalPurchases"]);
                if (oItem["KursuspakkeTypen"] != null)
                {
                    Console.WriteLine(oItem["KursuspakkeTypen"]);
                    var childIdField = oItem["KursuspakkeTypen"] as FieldLookupValue[];

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

        //Service request communications --Done
        public void GetPatenerServicerequestcommunicationsListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service request communications");
            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View Scope='RecursiveAll'><Query></Query></View>";
            ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

            clientContext.Load(oListDataItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oItem in oListDataItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                Console.WriteLine(oItem["Created"].ToString());
                Console.WriteLine(oItem["Modified"].ToString());
                Console.WriteLine(oItem["Communications"]);
                Console.WriteLine(oItem["FromMyIT"]);
                Console.WriteLine(oItem["Created"]);
                Console.WriteLine(oItem["CopyToCloseRemark"]);
                Console.WriteLine(oItem["IsPartnerSideCommunication"]);
                Console.WriteLine(oItem["ProcedureInfoCommunication"]);
                if (oItem["ServiceRequestID"] != null)
                {
                    Console.WriteLine(oItem["ServiceRequestID"]);
                    var childIdField = oItem["ServiceRequestID"] as FieldLookupValue[];

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
                if (oItem["ITProcedureID"] != null)
                {
                    Console.WriteLine(oItem["ITProcedureID"]);
                    var childIdField = oItem["ITProcedureID"] as FieldLookupValue[];

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

        //Service request conversation items
        public void GetPatenerServicerequestconversationitemsListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service request conversation items");
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

        //Service request notes		
        public void GetPatenerServicerequestnotesListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service request notes");
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


    }

}



