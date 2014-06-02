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

namespace pvspa.Batch.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private void UpdateSkanDoAnalizy_ExecuteCode(object sender, EventArgs e)
        {
            string batchStatus = workflowProperties.Item["Batch Status"].ToString();
            switch (batchStatus)
            {
                case "Otwarty":
                    HideBatch_From_Menu();
                    break;
                case "Weryfikacja":
                    ShowBatch_In_Menu();
                    break;
                case "Zweryfikowany":
                    Manage_Zweryfikowany();
                    HideBatch_From_Menu();
                    break;
            }
        }

        private void ShowBatch_In_Menu()
        {
            string batchName = workflowProperties.Item["BatchName"].ToString();
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    SPList list = web.Lists[batchName];
                    list.OnQuickLaunch = true;
                    list.Update();
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
                    SPList list = web.Lists[batchName];
                    list.OnQuickLaunch = false;
                    list.Update();
                }
            }
        }

        private void Manage_Zweryfikowany()
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    string batchID = workflowProperties.Item.ID.ToString();

                    SPList list = web.Lists["SkanDoAnalizy"];

                    StringBuilder sb = new StringBuilder(@"<OrderBy><FieldRef Name=""ID"" /></OrderBy><Where><Eq><FieldRef Name=""Batch_x002e_ID"" /><Value Type=""Number"">___BatchID___</Value></Eq></Where>");
                    sb.Replace("___BatchID___", batchID);
                    string camlQuery = sb.ToString();

                    SPQuery query = new SPQuery();
                    query.Query = camlQuery;

                    SPListItemCollection items = list.GetItems(query);
                    foreach (SPListItem myItem in items)
                    {
                        myItem["Batch_Completed"] = true;
                        myItem.Update();
                    }
                }
            }
        }
    }
}
