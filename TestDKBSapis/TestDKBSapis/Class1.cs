using System;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

namespace Microsoft.SDK.SharePointServices.Samples
{
    class RetrieveListItems
    {
       public static void TestMethod1()
        {
            string siteUrl = "http://MyServer/sites/MySiteCollection";

            ClientContext clientContext = new ClientContext(siteUrl);
            clientContext.AuthenticationMode = ClientAuthenticationMode.FormsAuthentication;
            clientContext.FormsAuthenticationLoginInfo = new FormsAuthenticationLoginInfo("CRM Automation", "9LEkTny4");
            clientContext.ExecuteQuery();
            SP.List oList = clientContext.Web.Lists.GetByTitle("Service katalog");

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View><Query></Query></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            // get partner listitem using this line, but you can use caml query to get many partner items

            //ListItem partnerItem = lstCIs.GetItemById(lv.LookupId);

            // get subsite url

            //if (partnerItem["CISite"] != null)
            //    {
            //        FieldUrlValue subSiteUrl = partnerItem["CISite"] as FieldUrlValue;
            //        using (ClientContext subsiteContext = new ClientContext(subSiteUrl.Url))
            //        { }
            //    }

            foreach (ListItem oListItem in collListItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1} ", oListItem.Id, oListItem["Title"]);
            }
        }
    }
}