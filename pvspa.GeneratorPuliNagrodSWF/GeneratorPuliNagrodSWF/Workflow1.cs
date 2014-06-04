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

namespace pvspa.GeneratorPuliNagrodSWF.Workflow1
{
    public sealed partial class Workflow1 : SequentialWorkflowActivity
    {
        public Workflow1()
        {
            InitializeComponent();
        }

        public Guid workflowId = default(System.Guid);
        public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

        private int intZgloszenieID;
        const string _PULA_M = "Pula nagród - M";
        const string _PULA_R = "Pula nagród - R";
        const string _PULA_Z = "Pula nagród - Z";
        const string _LST_ZGLOSZENIA = "Zgłoszenia";
        const string _LST_NAGRODY = "Nagrody";
        const string _LST_KODYWYDANE = "Kody wydane";
        private SPListItemCollection cNagrody;
        private bool isAllClear = true;

        private void onWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
        {
            intZgloszenieID = Convert.ToInt32(workflowProperties.Item["Zgłoszenie.ID"]);

            Load_cNagrody();

            Clear_ContentEntries();
        }

        private void Clear_ContentEntries()
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList lstZgloszenia = web.Lists[_LST_ZGLOSZENIA];
                        SPListItem item = lstZgloszenia.GetItemById(intZgloszenieID);
                        if (Convert.ToBoolean(item["Package_Completed"]) == false)
                        {
                            item["M_Cont"] = false;
                            item["R_Cont"] = false;
                            item["Z_Cont"] = false;

                            item["M_Component"] = string.Empty;
                            item["R_Component"] = string.Empty;
                            item["Z_Component"] = string.Empty;
                            item["ALL_Component"] = string.Empty;

                            item.Update();

                        }
                    }
                    catch (Exception)
                    { isAllClear = false; }
                }
            }

        }


        private void isMSelected(object sender, ConditionalEventArgs e)
        {
            bool result = false;

            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Myjka"))
                    {
                        result = true;
                        break;
                    }
                }
            }

            e.Result = result;
        }

        private void isRSelected(object sender, ConditionalEventArgs e)
        {
            bool result = false;

            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Ręcznik"))
                    {
                        result = true;
                        break;
                    }
                }
            }

            e.Result = result;
        }

        private void isZSelected(object sender, ConditionalEventArgs e)
        {
            bool result = false;

            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Zestaw"))
                    {
                        result = true;
                        break;
                    }
                }
            }

            e.Result = result;

        }

        private void CompleteM_ExecuteCode(object sender, EventArgs e)
        {
            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Myjka"))
                    {
                        int myCounter = Convert.ToInt32(item["Ilość"]);
                        while (myCounter > 0)
                        {
                            GetPromoNumber(_PULA_M, "Myjka");

                            myCounter--;
                        }
                    }
                }
            }

        }

        private void CompleteR_ExecuteCode(object sender, EventArgs e)
        {
            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Ręcznik"))
                    {
                        int myCounter = Convert.ToInt32(item["Ilość"]);
                        while (myCounter > 0)
                        {
                            GetPromoNumber(_PULA_R, "Ręcznik");

                            myCounter--;
                        }
                    }
                }
            }

        }

        private void CompleteZ_ExecuteCode(object sender, EventArgs e)
        {
            if (cNagrody != null)
            {
                foreach (SPListItem item in cNagrody)
                {
                    if (item["Typ nagrody"].Equals("Zestaw"))
                    {
                        int myCounter = Convert.ToInt32(item["Ilość"]);
                        while (myCounter > 0)
                        {
                            GetPromoNumber(_PULA_Z, "Zestaw");

                            myCounter--;
                        }
                    }
                }
            }

        }

        #region Helpers

        private void Load_cNagrody()
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList lstNagrody = web.Lists[_LST_NAGRODY];

                        StringBuilder sb = new StringBuilder(@"<OrderBy><FieldRef Name=""ID"" Ascending=""FALSE"" /></OrderBy><Where><Eq><FieldRef Name=""Zg_x0142_oszenie_x002e_ID"" /><Value Type=""Number"">___ZgloszenieID___</Value></Eq></Where>");
                        sb.Replace("___ZgloszenieID___", intZgloszenieID.ToString());
                        string camlQuery = sb.ToString();

                        SPQuery query = new SPQuery();
                        query.Query = camlQuery;

                        cNagrody = lstNagrody.GetItems(query);
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private void GetPromoNumber(string listName, string promoType)
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList lstNumery = web.Lists[listName];

                        string camlQuery = @"<OrderBy><FieldRef Name=""ID"" /></OrderBy><Where><IsNull><FieldRef Name=""Zg_x0142_oszenie_x002e_ID"" /></IsNull></Where>";

                        SPQuery query = new SPQuery();
                        query.Query = camlQuery;
                        query.RowLimit = 30;

                        SPListItemCollection promoNumbers = lstNumery.GetItems(query);

                        string promoCode = string.Empty;

                        if (promoNumbers.Count > 0)
                        {
                            bool success = false;
                            do
                            {
                                foreach (SPListItem promoItem in promoNumbers)
                                {


                                    try
                                    {
                                        promoItem["Zgłoszenie.ID"] = intZgloszenieID;
                                        promoItem.Update();
                                        success = true;

                                        promoCode = promoItem["Kod nagrody"].ToString();

                                        break;
                                    }
                                    catch (Exception)
                                    { }

                                }
                            } while (!success);


                            if (!string.IsNullOrEmpty(promoCode))
                            {
                                //register promocode release
                                RegisterPromoCodeRelease(promoCode, promoType);

                                UpdatePromoInfoComponent(promoCode, promoType);
                            }
                        }

                    }
                    catch (Exception)
                    {
                        isAllClear = false;
                    }

                }
            }
        }

        private void UpdatePromoInfoComponent(string promoCode, string promoType)
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList lstZgloszenia = web.Lists[_LST_ZGLOSZENIA];
                        SPListItem item = lstZgloszenia.GetItemById(intZgloszenieID);

                        string tempLine = string.Empty;
                        string myPromoType;

                        switch (promoType)
                        {
                            case "Myjka":
                                myPromoType = "Myjka do kąpieli";
                                tempLine = string.Format(@"<li><b>{0}</b> ({1})</li>", promoCode, myPromoType);
                                if (string.IsNullOrEmpty((string)item["M_Component"]))
                                {
                                    item["M_Component"] = tempLine;
                                }
                                else
                                {
                                    item["M_Component"] = item["M_Component"].ToString() + tempLine;
                                }

                                item["M_Cont"] = true;

                                break;

                            case "Ręcznik":
                                myPromoType = "Ręcznik plażowy";
                                tempLine = string.Format(@"<li><b>{0}</b> ({1})</li>", promoCode, myPromoType);
                                if (string.IsNullOrEmpty((string)item["R_Component"]))
                                {
                                    item["R_Component"] = tempLine;
                                }
                                else
                                {
                                    item["R_Component"] = item["R_Component"].ToString() + tempLine;
                                }

                                item["R_Cont"] = true;

                                break;

                            case "Zestaw":
                                myPromoType = "Zestaw akcesoriów SPA";
                                tempLine = string.Format(@"<li><b>{0}</b> ({1})</li>", promoCode, myPromoType);
                                if (string.IsNullOrEmpty((string)item["Z_Component"]))
                                {
                                    item["Z_Component"] = tempLine;
                                }
                                else
                                {
                                    item["Z_Component"] = item["Z_Component"].ToString() + tempLine;
                                }

                                item["Z_Cont"] = true;

                                break;

                            default:
                                break;
                        }

                        //update tempLine
                        StringBuilder sb = new StringBuilder(tempLine);
                        sb.Replace(@"<li><b>", @"<li>Twój osobisty kod to <b>");
                        tempLine = sb.ToString();

                        //update ALL_Component
                        if (string.IsNullOrEmpty((string)item["ALL_Component"]))
                        {
                            item["ALL_Component"] = tempLine;
                        }
                        else
                        {
                            item["ALL_Component"] = item["ALL_Component"].ToString() + tempLine;
                        }

                        item.Update();

                    }
                    catch (Exception)
                    {
                        isAllClear = false;
                    }
                }
            }
        }

        private void RegisterPromoCodeRelease(string promoCode, string promoType)
        {
            using (SPSite site = new SPSite(workflowProperties.SiteId))
            {
                using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                {
                    try
                    {
                        SPList lstAllocatedCodes = web.Lists[_LST_KODYWYDANE];
                        SPListItem newItem = lstAllocatedCodes.AddItem();
                        newItem["Zgłoszenie.ID"] = intZgloszenieID.ToString();
                        newItem["Typ nagrody"] = promoType;
                        newItem["Kod nagrody"] = promoCode;
                        newItem.Update();
                    }
                    catch (Exception)
                    {
                        isAllClear = false;
                    }
                }
            }
        }

        #endregion

        private void MarkPackage_Completed_ExecuteCode(object sender, EventArgs e)
        {

            if (isAllClear)
            {
                using (SPSite site = new SPSite(workflowProperties.SiteId))
                {
                    using (SPWeb web = site.AllWebs[workflowProperties.WebId])
                    {
                        try
                        {
                            SPList lstZgloszenia = web.Lists[_LST_ZGLOSZENIA];
                            SPListItem item = lstZgloszenia.GetItemById(intZgloszenieID);

                            item["Package_Completed"] = true;
                            item.Update();

                        }
                        catch (Exception)
                        { }
                    }

                }
            }
        }
    }
}
