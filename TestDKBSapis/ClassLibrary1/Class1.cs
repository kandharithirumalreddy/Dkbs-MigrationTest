using System;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;

namespace Microsoft.SDK.SharePointServices.Samples
{
    class RetrieveListItems
    {
        static void Main()
        {
            string siteUrl = "http://MyServer/sites/MySiteCollection";

            ClientContext clientContext = new ClientContext(siteUrl);
            SP.List oList = clientContext.Web.Lists.GetByTitle("Announcements");

            //get partner listitem using this line, but you can use caml query to get many partner items

            //ListItem partnerItem = lstCIs.GetItemById(lv.LookupId);

            // get subsite url

            //if (partnerItem["CISite"] != null)
            //    {
            //        FieldUrlValue subSiteUrl = partnerItem["CISite"] as FieldUrlValue;
            //        using (ClientContext subsiteContext = new ClientContext(subSiteUrl.Url))
            //        { }
            //    }

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = "<View><Query><Where><Geq><FieldRef Name='ID'/>" +
                "<Value Type='Number'>10</Value></Geq></Where></Query><RowLimit>100</RowLimit></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            clientContext.Load(collListItem);

            clientContext.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {
                Console.WriteLine("ID: {0} \nTitle: {1} \nBody: {2}", oListItem.Id, oListItem["Title"], oListItem["Body"]);
            }
        }
    }
}





//using System;
//using Microsoft.SharePoint.Client;

//namespace ClassLibrary1
//{
//    public class Class1
//    {
//        //public void testMethod()
//        {
//            // Web oWebsite = spctx.Web;

//            // List _lst = oWebsite.Lists.GetByTitle("List Name");
//            // Microsoft.SharePoint.Client.ListItemCollection _lstCol = _lst.GetItems(CamlQuery.CreateAllItemsQuery());

//            // spctx.Load(_lst);
//            // spctx.Load(_lstCol);
//            // spctx.ExecuteQuery();
//            //foreach (Microsoft.SharePoint.Client.ListItem _item in _lstCol)
//            // {
//            //  string ID = _item.FieldValues["ID"].ToString();
//            // string Title = _item.FieldValues["Title"].ToString();
//            //  }

//            // Starting with ClientContext, the constructor requires a URL to the 
//            // server running SharePoint. 
//            ClientContext context = new ClientContext("http://SiteUrl");

//            // Assume the web has a list named "Announcements". 
//            List announcementsList = context.Web.Lists.GetByTitle("Announcements");

//            // This creates a CamlQuery that has a RowLimit of 100, and also specifies Scope="RecursiveAll" 
//            // so that it grabs all list items, regardless of the folder they are in. 
//            CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
//            ListItemCollection items = announcementsList.GetItems(query);

//            // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
//            context.Load(items);
//            context.ExecuteQuery();
//            foreach (ListItem listItem in items)
//            {
//                // We have all the list item data. For example, Title. 
//                // label1.Text = label1.Text + ", " + listItem["Title"]; 
//                Console.WriteLine("Title: " + listItem["Title"]);
//            }



// }


