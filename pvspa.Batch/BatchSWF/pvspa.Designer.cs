using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace pvspa.BatchSWF.pvspa
{
    public sealed partial class pvspa
    {
        #region Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
        private void InitializeComponent()
        {
            this.CanModifyActivities = true;
            System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
            System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
            System.Workflow.Runtime.CorrelationToken correlationtoken1 = new System.Workflow.Runtime.CorrelationToken();
            System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
            this.RemoveListItem = new System.Workflow.Activities.CodeActivity();
            this.RemoveWorkLib = new System.Workflow.Activities.CodeActivity();
            this.UpdateSkanDoAnalizy = new System.Workflow.Activities.CodeActivity();
            this.HideFromMenu = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity5 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ShowInMenu = new System.Workflow.Activities.CodeActivity();
            this.logToHistoryListActivity4 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.Else = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifZweryfikowany = new System.Workflow.Activities.IfElseBranchActivity();
            this.ifWeryfikacja = new System.Workflow.Activities.IfElseBranchActivity();
            this.logToHistoryListActivity2 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
            this.logToHistoryListActivity1 = new Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity();
            this.onWorkflowActivated1 = new Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated();
            // 
            // RemoveListItem
            // 
            this.RemoveListItem.Name = "RemoveListItem";
            this.RemoveListItem.ExecuteCode += new System.EventHandler(this.RemoveListItem_ExecuteCode);
            // 
            // RemoveWorkLib
            // 
            this.RemoveWorkLib.Name = "RemoveWorkLib";
            this.RemoveWorkLib.ExecuteCode += new System.EventHandler(this.RemoveWorkLib_ExecuteCode);
            // 
            // UpdateSkanDoAnalizy
            // 
            this.UpdateSkanDoAnalizy.Name = "UpdateSkanDoAnalizy";
            this.UpdateSkanDoAnalizy.ExecuteCode += new System.EventHandler(this.UpdateSkanDoAnalizy_ExecuteCode);
            // 
            // HideFromMenu
            // 
            this.HideFromMenu.Name = "HideFromMenu";
            this.HideFromMenu.ExecuteCode += new System.EventHandler(this.HideFromMenu_ExecuteCode);
            // 
            // logToHistoryListActivity5
            // 
            this.logToHistoryListActivity5.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity5.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity5.HistoryDescription = "";
            this.logToHistoryListActivity5.HistoryOutcome = "Case: Zweryfikowany";
            this.logToHistoryListActivity5.Name = "logToHistoryListActivity5";
            this.logToHistoryListActivity5.OtherData = "";
            this.logToHistoryListActivity5.UserId = -1;
            // 
            // ShowInMenu
            // 
            this.ShowInMenu.Name = "ShowInMenu";
            this.ShowInMenu.ExecuteCode += new System.EventHandler(this.ShowInMenu_ExecuteCode);
            // 
            // logToHistoryListActivity4
            // 
            this.logToHistoryListActivity4.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity4.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity4.HistoryDescription = "";
            this.logToHistoryListActivity4.HistoryOutcome = "Case: Weryfikacja";
            this.logToHistoryListActivity4.Name = "logToHistoryListActivity4";
            this.logToHistoryListActivity4.OtherData = "";
            this.logToHistoryListActivity4.UserId = -1;
            // 
            // Else
            // 
            this.Else.Name = "Else";
            // 
            // ifZweryfikowany
            // 
            this.ifZweryfikowany.Activities.Add(this.logToHistoryListActivity5);
            this.ifZweryfikowany.Activities.Add(this.HideFromMenu);
            this.ifZweryfikowany.Activities.Add(this.UpdateSkanDoAnalizy);
            this.ifZweryfikowany.Activities.Add(this.RemoveWorkLib);
            this.ifZweryfikowany.Activities.Add(this.RemoveListItem);
            codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsZweryfikowany);
            this.ifZweryfikowany.Condition = codecondition1;
            this.ifZweryfikowany.Name = "ifZweryfikowany";
            // 
            // ifWeryfikacja
            // 
            this.ifWeryfikacja.Activities.Add(this.logToHistoryListActivity4);
            this.ifWeryfikacja.Activities.Add(this.ShowInMenu);
            codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsWeryfikacja);
            this.ifWeryfikacja.Condition = codecondition2;
            this.ifWeryfikacja.Name = "ifWeryfikacja";
            // 
            // logToHistoryListActivity2
            // 
            this.logToHistoryListActivity2.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity2.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity2.HistoryDescription = "";
            this.logToHistoryListActivity2.HistoryOutcome = "Completed";
            this.logToHistoryListActivity2.Name = "logToHistoryListActivity2";
            this.logToHistoryListActivity2.OtherData = "";
            this.logToHistoryListActivity2.UserId = -1;
            // 
            // ifElseActivity1
            // 
            this.ifElseActivity1.Activities.Add(this.ifWeryfikacja);
            this.ifElseActivity1.Activities.Add(this.ifZweryfikowany);
            this.ifElseActivity1.Activities.Add(this.Else);
            this.ifElseActivity1.Name = "ifElseActivity1";
            // 
            // logToHistoryListActivity1
            // 
            this.logToHistoryListActivity1.Duration = System.TimeSpan.Parse("-10675199.02:48:05.4775808");
            this.logToHistoryListActivity1.EventId = Microsoft.SharePoint.Workflow.SPWorkflowHistoryEventType.WorkflowComment;
            this.logToHistoryListActivity1.HistoryDescription = "";
            this.logToHistoryListActivity1.HistoryOutcome = "Initiated";
            this.logToHistoryListActivity1.Name = "logToHistoryListActivity1";
            this.logToHistoryListActivity1.OtherData = "";
            this.logToHistoryListActivity1.UserId = -1;
            activitybind2.Name = "pvspa";
            activitybind2.Path = "workflowId";
            // 
            // onWorkflowActivated1
            // 
            correlationtoken1.Name = "workflowToken";
            correlationtoken1.OwnerActivityName = "pvspa";
            this.onWorkflowActivated1.CorrelationToken = correlationtoken1;
            this.onWorkflowActivated1.EventName = "OnWorkflowActivated";
            this.onWorkflowActivated1.Name = "onWorkflowActivated1";
            activitybind1.Name = "pvspa";
            activitybind1.Path = "workflowProperties";
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowIdProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
            this.onWorkflowActivated1.SetBinding(Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated.WorkflowPropertiesProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
            // 
            // pvspa
            // 
            this.Activities.Add(this.onWorkflowActivated1);
            this.Activities.Add(this.logToHistoryListActivity1);
            this.Activities.Add(this.ifElseActivity1);
            this.Activities.Add(this.logToHistoryListActivity2);
            this.Name = "pvspa";
            this.CanModifyActivities = false;

        }

        #endregion

        private CodeActivity RemoveListItem;

        private CodeActivity RemoveWorkLib;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity5;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity4;

        private IfElseBranchActivity Else;

        private CodeActivity UpdateSkanDoAnalizy;

        private CodeActivity ShowInMenu;

        private CodeActivity HideFromMenu;

        private IfElseBranchActivity ifZweryfikowany;

        private IfElseBranchActivity ifWeryfikacja;

        private IfElseActivity ifElseActivity1;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity2;

        private Microsoft.SharePoint.WorkflowActions.LogToHistoryListActivity logToHistoryListActivity1;

        private Microsoft.SharePoint.WorkflowActions.OnWorkflowActivated onWorkflowActivated1;
























    }
}
