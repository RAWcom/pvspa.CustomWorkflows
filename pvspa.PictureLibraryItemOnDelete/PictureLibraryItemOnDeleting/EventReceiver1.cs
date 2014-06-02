using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Text;

namespace pvspa.PictureLibraryItemOnDelete.EventReceiver1
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
       /// <summary>
       /// An item is being deleted.
       /// </summary>
       public override void ItemDeleting(SPItemEventProperties properties)
       {
           base.ItemDeleting(properties);

           if (properties.ListTitle != "Skany")
	        {
                try
                {
                    SPListItem item = properties.ListItem;
                    string tokenName = item["Nazwa"].ToString();

                    //update SkanDoAnalizy

                    using (SPSite site = new SPSite(properties.SiteId))
                    {
                        using (SPWeb web = site.AllWebs[properties.Web.ID])
                        {
                            SPList list = web.Lists["SkanDoAnalizy"];

                            StringBuilder sb = new StringBuilder(@"<OrderBy><FieldRef Name=""ID"" /></OrderBy><Where><And><And><Eq><FieldRef Name=""Token_x0020_zg_x0142_oszenia"" /><Value Type=""Text"">___TokenZgloszenia___</Value></Eq><Neq><FieldRef Name=""Batch_Completed"" /><Value Type=""Boolean"">1</Value></Neq></And><Neq><FieldRef Name=""IsDeleted"" /><Value Type=""Boolean"">1</Value></Neq></And></Where>");
                            sb.Replace("___TokenZgloszenia___", tokenName);
                            string camlQuery = sb.ToString();

                            SPQuery query = new SPQuery();
                            query.Query = camlQuery;

                            SPListItemCollection items = list.GetItems(query);
                            foreach (SPListItem myItem in items)
                            {
                                myItem["IsDeleted"] = true;
                                myItem.Update();
                            }                            
                        }   
                    }
                }
                catch (Exception)
                {
                    
                    throw;
                } 


	        }

           
       }


    }
}
