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

namespace pvspa.BatchSWF.pvspa
{
    public sealed partial class pvspa : SequentialWorkflowActivity
    {
        public pvspa()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();


        #region Helpers

        private void ShowBatch_In_Menu()
        {
            string batchName = workflowProperties.Item["BatchName"].ToString();
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList list = web.Lists[batchName];
                        list.OnQuickLaunch = true;
                        list.Update();
                    }
                    catch (Exception)
                    { }
                    
                }
            }

        }

        private void HideBatch_From_Menu()
        {

            string batchName = workflowProperties.Item["BatchName"].ToString();
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList list = web.Lists[batchName];
                        list.OnQuickLaunch = false;
                        list.Update();
                    }
                    catch (Exception)
                    { }
                    
                }
            }
        }

        #endregion

        private void IsOtwarty(object sender, ConditionalEventArgs e)
        {
            string batchStatus = workflowProperties.Item["Batch Status"].ToString();
            if (batchStatus == "Otwarty")
            {
                e.Result = true;
            }
            else
            {
                e.Result = false;
            }
        }

        private void IsWeryfikacja(object sender, ConditionalEventArgs e)
        {
            string batchStatus = workflowProperties.Item["Batch Status"].ToString();
            if (batchStatus == "Weryfikacja")
            {
                e.Result = true;
            }
            else
            {
                e.Result = false;
            }
        }

        private void IsZweryfikowany(object sender, ConditionalEventArgs e)
        {
            string batchStatus = workflowProperties.Item["Batch Status"].ToString();
            if (batchStatus == "Zweryfikowany")
            {
                e.Result = true;
            }
            else
            {
                e.Result = false;
            }
        }

        private void HideFromMenu_ExecuteCode(object sender, EventArgs e)
        {
            HideBatch_From_Menu();
        }

        private void ShowInMenu_ExecuteCode(object sender, EventArgs e)
        {
            ShowBatch_In_Menu();
        }

        private void UpdateSkanDoAnalizy_ExecuteCode(object sender, EventArgs e)
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    string batchID = workflowProperties.Item.ID.ToString();

                    SPList list = web.Lists["SkanDoAnalizy"];

                    StringBuilder sb = new StringBuilder(@"<OrderBy><FieldRef Name=""ID"" /></OrderBy><Where><And><Eq><FieldRef Name=""Batch_x002e_ID0"" /><Value Type=""Number"">1</Value></Eq><Neq><FieldRef Name=""Batch_Completed"" /><Value Type=""Boolean"">___BatchID___</Value></Neq></And></Where>");
                    sb.Replace("___BatchID___", batchID);
                    string camlQuery = sb.ToString();

                    SPQuery query = new SPQuery();
                    query.Query = camlQuery;

                    SPListItemCollection items = list.GetItems(query);
                    foreach (SPListItem myItem in items)
                    {
                        bool fileExist = true;

                        if (!string.IsNullOrEmpty((string)myItem["Token zgłoszenia"]))
                        {

                            string fileName = myItem["Token zgłoszenia"].ToString();

                            bool itemFound = false;
                            
                            // lookup in relevant BatchLib
                            SPList batchRegistry = web.Lists["Rejestr Batchów"];
                            SPListItem batchRegistryItem = batchRegistry.GetItemById(Convert.ToInt32(myItem["Batch.ID"]));
                            string batchLibName = batchRegistryItem["BatchName"].ToString();

                            try
                            {
                                SPList wlist = web.Lists[batchLibName];
                                foreach (SPListItem witem in wlist.Items)
                                {
                                    string tempName = witem["Nazwa"].ToString();
                                    if (tempName == fileName)
                                    {
                                        itemFound = true;
                                        break;
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                throw;
                            }


                            fileExist = itemFound;
                        }

                        myItem["Batch_Completed"] = true;
                        myItem["IsDeleted"] = !Convert.ToBoolean(fileExist);
                        myItem.Update();
                    }
                }
            }
        }


        private void SetStatus_Weryfikacja_ExecuteCode(object sender, EventArgs e)
        {
            SPItem item = workflowProperties.Item;
            item["Batch Status"] = "Weryfikacja";
            item.Update();
        }

        private void RemoveWorkLib_ExecuteCode(object sender, EventArgs e)
        {
            string batchName = workflowProperties.Item["BatchName"].ToString();
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    SPList list = web.Lists[batchName];
                    list.Delete();
                }
            }
        }

        private void RemoveListItem_ExecuteCode(object sender, EventArgs e)
        {
            workflowProperties.Item.Delete();
        }


    }
}
