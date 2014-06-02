using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using System.Text;

namespace pvspa.SkanDoAnalizySWF.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void SelectBatch_ExecuteCode(object sender, EventArgs e)
        {
            //Generate Batch Name
            string batchName = String.Format("B{0:yyMMdd}", DateTime.Today);

            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {

                    SPList list = web.Lists["Rejestr Batchów"];

                    StringBuilder sb = new StringBuilder(@"<OrderBy><FieldRef Name=""ID"" /></OrderBy><Where><Eq><FieldRef Name=""BatchName"" /><Value Type=""Text"">___BatchName___</Value></Eq></Where>");
                    sb.Replace("___BatchName___", batchName);
                    string camlQuery = sb.ToString();

                    SPQuery query = new SPQuery();
                    query.Query = camlQuery;


                    int batchID;

                    SPListItemCollection items = list.GetItems(query);
                    if (items.Count > 0)
                    {
                        batchID = items[0].ID;
                    }
                    else
                    {
                        SPListItem nitem = list.AddItem();
                        nitem["BatchName"] = batchName;
                        nitem.Update();
                        batchID = nitem.ID;


                        try
                        {
                            //create Batch Library
                            web.Lists.Add(batchName, "", SPListTemplateType.PictureLibrary);
                            web.Update();

                            //set visible new Batch library
                            //SPList createdList = web.Lists[batchName];
                            //createdList.OnQuickLaunch = true;
                            //createdList.Update();
                        }
                        catch (Exception)
                        { }

                    }

                    try
                    {

                        //update Batch.ID in current item
                        SPItem item = workflowProperties.Item;

                        //copy Scan to Batch Library
                        SPList s_list = web.Lists["Skany"];
                        SPListItem s_item = s_list.GetItemById(Convert.ToInt32(item["Skan.ID"]));
                        string tokenName = s_item["NameOrTitle"].ToString();

                        item["Batch.ID"] = batchID;
                        item["Token zgłoszenia"] = tokenName;
                        item.Update();

                        byte[] picFile = null;
                        SPFolder Sourcelibrary = s_item.Web.Folders["Skany"];
                        picFile = s_item.File.OpenBinary();
                        if (picFile != null && picFile.Length > 0)
                        {
                            try
                            {
                                SPFolder DestLibrary = s_item.Web.Folders[batchName]; //batchName
                                s_item.Web.AllowUnsafeUpdates = true;
                                DestLibrary.Files.Add(System.IO.Path.GetFileName(s_item.File.Name), picFile);
                            }
                            catch (Exception)
                            { }

                        }
                    }
                    catch (Exception)
                    { }

                }
            }
        }

        private object CreateNewBatch(string batchName, SPList list)
        {
            SPListItem item = list.AddItem();
            item["BatchName"] = batchName;
            item.Update();

            return item.ID;
        }
    }
}
    

