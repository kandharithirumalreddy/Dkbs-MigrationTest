﻿using Newtonsoft.Json;
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

            odata.getBookingerListData(clientContext);

            //odata.GetPatenerKursuspakkeListData(clientContext);

            //odata.getDocumentLib(clientContext);

            //odata.GetPatenerAktiviteterListData(clientContext);

            //odata.GetPatenerInspirationskategorierListData(clientContext);

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
                            ListItemCollectionPosition position = null;
                            var page = 1;
                            do
                            {
                                var query = new CamlQuery()
                                {
                                    ViewXml = String.Format("<View Scope='Recursive'><Query><Where><Eq><FieldRef Name='ContentType' /><Value Type='Computed'>{0}</Value></Eq></Where></Query><RowLimit>5000</RowLimit></View>", contentTypeName)
                                };
                                query.ListItemCollectionPosition = position;
                                ListItemCollection collListItem = oList.GetItems(query);
                                clientContext.Load(collListItem);
                                clientContext.ExecuteQuery();
                                position = collListItem.ListItemCollectionPosition;
                                Console.WriteLine(position);
                                foreach (ListItem oListItem in collListItem)
                                {
                                    Console.WriteLine("ID: {0} \nTitle: {1} ", oListItem.Id, oListItem["Title"]);
                                }
                                page++;
                            }
                            while (position != null);
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

        //Billeder --Done
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
                if (oItem["MailGroups"] != null)
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

        //Procedures --Done
        public void GetPatenerProceduresListData(ClientContext clientContext)
        {
            SP.List oListData = clientContext.Web.Lists.GetByTitle("Procedures");
            ListItemCollectionPosition position = null;
            var page = 1;
            do
            {
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View Scope='Recursive'><Query></Query><RowLimit>5000</RowLimit></View>";
                camlQuery.ListItemCollectionPosition = position;
                ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                clientContext.Load(oListDataItem);

                clientContext.ExecuteQuery();
                position = oListDataItem.ListItemCollectionPosition;
                foreach (ListItem oItem in oListDataItem)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                    Console.WriteLine(oItem["Created"].ToString());
                    Console.WriteLine(oItem["Modified"].ToString());
                    Console.WriteLine(oItem["Predecessors"]);
                    Console.WriteLine(oItem["ParentSR"]);
                    Console.WriteLine(oItem["ParentIM"]);
                    Console.WriteLine(oItem["ITProcedures"]);
                    Console.WriteLine(oItem["RelevantOrderDetails"]);
                    Console.WriteLine(oItem["RelevantOrderDetailsStaticHTML"]);
                    Console.WriteLine(oItem["DKBSReopenComment"]);
                    Console.WriteLine(oItem["Arrangementtype"]);
                    Console.WriteLine(oItem["Antal_x0020_grupperum"]);
                    Console.WriteLine(oItem["AlternativtServices"]);
                    if (oItem["Reason"] != null)
                    {
                        Console.WriteLine(oItem["Reason"]);
                        var childIdField = oItem["Reason"] as FieldLookupValue[];
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
                    if (oItem["ItProcResponsible"] != null)
                    {
                        Console.WriteLine(oItem["ItProcResponsible"]);
                        var childIdField = oItem["ItProcResponsible"] as FieldLookupValue[];
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
                    if (oItem["RelatedCI"] != null)
                    {
                        Console.WriteLine(oItem["RelatedCI"]);
                        var childIdField = oItem["RelatedCI"] as FieldLookupValue[];
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
                    if (oItem["Communications"] != null)
                    {
                        Console.WriteLine(oItem["Communications"]);
                        var childIdField = oItem["Communications"] as FieldLookupValue[];
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
                    if (oItem["ITProcContactPerson"] != null)
                    {
                        Console.WriteLine(oItem["ITProcContactPerson"]);
                        var childIdField = oItem["ITProcContactPerson"] as FieldLookupValue[];
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
                    if (oItem["ITProcFirma"] != null)
                    {
                        Console.WriteLine(oItem["ITProcFirma"]);
                        var childIdField = oItem["ITProcFirma"] as FieldLookupValue[];
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
                    if (oItem["CauseOFProcedureRemoval"] != null)
                    {
                        Console.WriteLine(oItem["CauseOFProcedureRemoval"]);
                        var childIdField = oItem["CauseOFProcedureRemoval"] as FieldLookupValue[];
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
                    Console.WriteLine(oItem["MainCase"]);
                    Console.WriteLine(oItem["Responsible"]);
                    Console.WriteLine(oItem["CaseType"]);
                    Console.WriteLine(oItem["Status"]);

                    Console.WriteLine(oItem["Outcome"]);
                    Console.WriteLine(oItem["ResponseTime"]);
                    Console.WriteLine(oItem["BeforeReopenCalculatedTime"]);
                    Console.WriteLine(oItem["CloseOrReopenDate"]);
                    Console.WriteLine(oItem["ResponsibleDKBS"]);
                    Console.WriteLine(oItem["ClosedProcedure"]);
                    Console.WriteLine(oItem["ITProcedureStatus"]);
                    Console.WriteLine(oItem["ITProcAnkomst"]);

                    Console.WriteLine(oItem["ITProcAfrejse"]);
                    Console.WriteLine(oItem["NeedReview"]);
                    Console.WriteLine(oItem["Read"]);
                    Console.WriteLine(oItem["SRMID"]);

                    Console.WriteLine(oItem["FirmaBranchekode"]);
                    Console.WriteLine(oItem["ITProcedureCancelReason"]);
                    Console.WriteLine(oItem["PlannedEnd"]);
                    Console.WriteLine(oItem["NotifWithMail"]);
                    Console.WriteLine(oItem["RelevantProcedureOutcome"]);
                    Console.WriteLine(oItem["TurnOffNotification"]);
                    Console.WriteLine(oItem["ExternalPerson"]);
                    Console.WriteLine(oItem["ResponsibleTeam"]);
                    Console.WriteLine(oItem["PlannedStart"]);
                    Console.WriteLine(oItem["UsedInEmailOffer"]);

                }
                page++;
            }
            while (position != null);
        }

        //Provision --Done
        public void GetPatenerProvisionListData(ClientContext clientContext)
        {
         
            SP.List oListData = clientContext.Web.Lists.GetByTitle("Provision");
            ListItemCollectionPosition position = null;
            var page = 1;
            do
            {
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View Scope='Recursive'><Query></Query><RowLimit>5000</RowLimit></View>";
                camlQuery.ListItemCollectionPosition = position;
                ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                clientContext.Load(oListDataItem);

                clientContext.ExecuteQuery();
                position = oListDataItem.ListItemCollectionPosition;
                foreach (ListItem oItem in oListDataItem)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                    Console.WriteLine(oItem["Created"].ToString());
                    Console.WriteLine(oItem["Modified"].ToString());
                    Console.WriteLine(oItem["Afrejse"]);
                    Console.WriteLine(oItem["Ankomst"]);
                    Console.WriteLine(oItem["DatoForAfsendelse"]);
                    Console.WriteLine(oItem["Debtor"]);
                    Console.WriteLine(oItem["Pris"]);
                    Console.WriteLine(oItem["LinkToParentItem"]);
                    Console.WriteLine(oItem["UnitID"]);
                    if (oItem["PartnerLookup"] != null)
                    {
                        Console.WriteLine(oItem["PartnerLookup"]);
                        var childIdField = oItem["PartnerLookup"] as FieldLookupValue[];
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
                    if (oItem["Kunde"] != null)
                    {
                        Console.WriteLine(oItem["Kunde"]);
                        var childIdField = oItem["Kunde"] as FieldLookupValue[];
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
                    if (oItem["BookingerID"] != null)
                    {
                        Console.WriteLine(oItem["BookingerID"]);
                        var childIdField = oItem["BookingerID"] as FieldLookupValue[];
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
                page++;
            }
            while (position != null);
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

        //Service request conversation items --Done
        public void GetPatenerServicerequestconversationitemsListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service request conversation items");
            ListItemCollectionPosition position = null;
            var page = 1;
            do
            {
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View Scope='Recursive'><Query></Query><RowLimit>5000</RowLimit></View>";
                camlQuery.ListItemCollectionPosition = position;
                ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                clientContext.Load(oListDataItem);

                clientContext.ExecuteQuery();
                position = oListDataItem.ListItemCollectionPosition;
                foreach (ListItem oItem in oListDataItem)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                    Console.WriteLine(oItem["Created"].ToString());
                    Console.WriteLine(oItem["Modified"].ToString());
                    Console.WriteLine(oItem["Message"]);
                    Console.WriteLine(oItem["Sender"]);
                    Console.WriteLine(oItem["CcAdresses"]);
                    Console.WriteLine(oItem["MessageId"]);
                    if (oItem["RelatedServiceRequest"] != null)
                    {
                        Console.WriteLine(oItem["RelatedServiceRequest"]);
                        var childIdField = oItem["RelatedServiceRequest"] as FieldLookupValue[];
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
                page++;
            }
            while (position != null);
        }
       
        //Service request notes	--Done	
        public void GetPatenerServicerequestnotesListData(ClientContext clientContext)
        {

            SP.List oListData = clientContext.Web.Lists.GetByTitle("Service request notes");
            ListItemCollectionPosition position = null;
            var page = 1;
            do
            {
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View Scope='Recursive'><Query></Query><RowLimit>5000</RowLimit></View>";
                camlQuery.ListItemCollectionPosition = position;
                ListItemCollection oListDataItem = oListData.GetItems(camlQuery);

                clientContext.Load(oListDataItem);

                clientContext.ExecuteQuery();

                position = oListDataItem.ListItemCollectionPosition;
                foreach (ListItem oItem in oListDataItem)
                {
                    Console.WriteLine("ID: {0} \nTitle: {1}", oItem["ID"], oItem["Title"]);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Author"])).LookupValue);
                    Console.WriteLine(((SP.FieldUserValue)(oItem["Editor"])).LookupValue);
                    Console.WriteLine(oItem["Created"].ToString());
                    Console.WriteLine(oItem["Modified"].ToString());
                    Console.WriteLine(oItem["Action"]);
                    Console.WriteLine(oItem["ScheduleAction"]);
                    Console.WriteLine(oItem["PlannedStart"]);
                    Console.WriteLine(oItem["Notify"]);
                    Console.WriteLine(oItem["PlannedEnd"]);
                    Console.WriteLine(oItem["CopyToCloseRemark"]);
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
                    if (oItem["CloseField"] != null)
                    {
                        Console.WriteLine(oItem["CloseField"]);
                        var childIdField = oItem["CloseField"] as FieldLookupValue[];
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
                page++;
            }
            while (position != null);
        }


    }

}



