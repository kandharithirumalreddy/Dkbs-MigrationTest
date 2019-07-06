using System;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

namespace Microsoft.SDK.SharePointServices.Samples
{
    class RetrieveListItems
    {
        public static void TestMethod1()
        {
            string siteUrl = "https://bookon.dkbs.dk/";

            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
            clientContext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
            clientContext.ExecuteQuery();
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
                        Console.WriteLine("ID: {0} \nTitle: {1}", oItem.Id, oItem["Title"]);
                    }

                }

            }
        }

    }
}