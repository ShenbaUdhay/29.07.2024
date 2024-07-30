using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Controls;
using DevExpress.ExpressApp.Web.Templates;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.Web;
using Modules.BusinessObjects.InfoClass;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Web.UI;

namespace LDM.Web
{
    public partial class ClassicDefaultVerticalTemplateContent1 : TemplateContent, IHeaderImageControlContainer, IXafPopupWindowControlContainer
    {
        string strSearchItem;
        curlanguage objLanguage = new curlanguage();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender += new EventHandler(CurrentRequestWindow_PagePreRender);
            }

            //SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            //conn.Open();

            //SqlCommand cmd = new SqlCommand("SELECT [dbo].[getCurrentLanguage] ()", conn);

            //string value = cmd.ExecuteScalar().ToString();
            if (objLanguage.strcurlanguage == "En")
            {
                ResourceManager rm = new ResourceManager("Resources.LocalizeResourcesEnglish", Assembly.Load("App_GlobalResources"));
                btnSearchBar.Text = rm.GetString(btnSearchBar.Text);
                txtSearch.Attributes.Add("placeholder", rm.GetString("Search Navigation Item"));
            }
            else
            {
                ResourceManager rm = new ResourceManager("Resources.LocalizeResourcesChinese", Assembly.Load("App_GlobalResources"));
                btnSearchBar.Text = rm.GetString(btnSearchBar.Text);
                txtSearch.Attributes.Add("placeholder", rm.GetString("Search Navigation Item"));
            }

            //conn.Close();
        }
        protected override void OnUnload(EventArgs e)
        {
            if (WebWindow.CurrentRequestWindow != null)
            {
                WebWindow.CurrentRequestWindow.PagePreRender -= new EventHandler(CurrentRequestWindow_PagePreRender);
            }
            base.OnUnload(e);
        }
        private void CurrentRequestWindow_PagePreRender(object sender, EventArgs e)
        {
            WebWindow.CurrentRequestWindow.RegisterStartupScript("OnLoadCore", string.Format(@"Init(""{0}"", ""VerticalCS"");OnLoadCore(""LPcell"", ""separatorCell"", ""separatorImage"", true, true);", BaseXafPage.CurrentTheme), true);
            UpdateTRPVisibility();
        }
        private void UpdateTRPVisibility()
        {
            bool isVisible = false;
            foreach (Control control in TRP.Controls)
            {
                if (control is ActionContainerHolder)
                {
                    if (((ActionContainerHolder)control).HasActiveActions())
                    {
                        isVisible = true;
                        break;
                    }
                }
            }
            TRP.Visible = isVisible;
        }
        public override IActionContainer DefaultContainer
        {
            get
            {
                if (TB != null)
                {
                    return TB.FindActionContainerById("View");
                }
                return null;
            }
        }
        public override void SetStatus(ICollection<string> statusMessages)
        {
            //InfoMessagesPanel.Text = string.Join("<br>", new List<string>(statusMessages).ToArray());
        }
        public override object ViewSiteControl
        {
            get
            {
                return VSC;
            }
        }
        public ActionContainerHolder LeftToolsActionContainer { get { return VTC; } }
        public ActionContainerHolder DiagnosticActionContainer { get { return DAC; } }
        public ActionContainerHolder MainToolBarActionContainer { get { return TB; } }
        public ActionContainerHolder SecurityActionContainer { get { return SAC; } }
        public ActionContainerHolder TopToolsActionContainer { get { return SHC; } }

        public XafPopupWindowControl XafPopupWindowControl
        {
            get { return PopupWindowControl; }
        }
        public ThemedImageControl HeaderImageControl { get { return TIC; } }

        protected void btnSearchBar_Click(object sender, EventArgs e)
        {
            string strText = txtSearch.Text.ToUpper();
            ASPxTreeView treeView = (ASPxTreeView)NC.NavigationControl;
            TreeViewNodeCollection collection = treeView.Nodes;

            foreach (TreeViewNode node in collection)
            {
                string strNodeText = node.Text.ToUpper();
                //Matching Node
                if (strNodeText.Contains(strText))
                {
                    //validate whether node has any child node
                    if (node.Nodes.Count != 0)
                    {
                        foreach (TreeViewNode childNode in node.Nodes)
                        {
                            string strChildNodeText = childNode.Text.ToUpper();

                            //Matching Node matching child
                            if (strChildNodeText.Contains(strText))
                            {
                                if (strText == "")
                                {
                                    node.Expanded = false;
                                }
                                else
                                {
                                    node.Expanded = true;
                                }

                                childNode.Visible = true;

                                //validate whether child node has any child
                                if (childNode.Nodes.Count != 0)
                                {
                                    foreach (TreeViewNode subChildNode in childNode.Nodes)
                                    {
                                        string strSubChildNode = subChildNode.Text.ToUpper();

                                        //Matching Node matching child matching subchild node
                                        if (strSubChildNode.Contains(strText))
                                        {
                                            subChildNode.Visible = true;

                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Matching Node matching child matching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                    }
                                                    //Matching Node matching child matching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Matching Node matching child unmatching subchild node
                                        else
                                        {
                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                int superSubChildNodeCount = 0;
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Matching Node matching child unmatching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                    }
                                                    //Matching Node matching child unmatching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                        superSubChildNodeCount += 1;
                                                    }
                                                }

                                                //hide unmatched subchildnode with no matching child node
                                                if (superSubChildNodeCount == subChildNode.Nodes.Count)
                                                {
                                                    subChildNode.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                subChildNode.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                            //Matching Node unmatching child
                            else
                            {
                                //childNode.Visible = false;

                                //validate whether childnode has any child nodes
                                if (childNode.Nodes.Count != 0)
                                {
                                    int subChildCount = 0;
                                    foreach (TreeViewNode subChildNode in childNode.Nodes)
                                    {
                                        string strSubChildNode = subChildNode.Text.ToUpper();

                                        //Matching Node unmatching child matching subchild node
                                        if (strSubChildNode.Contains(strText))
                                        {
                                            subChildNode.Visible = true;

                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {

                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Matching Node unmatching child matching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                        childNode.Expanded = true;
                                                        node.Expanded = true;
                                                    }
                                                    //Matching Node unmatching child matching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Matching Node unmatching child unmatching subchild node
                                        else
                                        {
                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                int superSubChildNodeCount = 0;
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Matching Node unmatching child unmatching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                        childNode.Expanded = true;
                                                        node.Expanded = true;
                                                    }
                                                    //Matching Node unmatching child unmatching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                        superSubChildNodeCount += 1;
                                                    }
                                                }

                                                //hide unmatching sub child node with no matching super child node
                                                if (superSubChildNodeCount == subChildNode.Nodes.Count)
                                                {
                                                    subChildNode.Visible = false;
                                                    subChildCount += 1;
                                                }
                                            }
                                            else
                                            {
                                                subChildNode.Visible = false;
                                                subChildCount += 1;
                                            }
                                        }
                                    }

                                    //hide unmatching childnode if there are no matching subchild
                                    if (subChildCount == childNode.Nodes.Count)
                                    {
                                        childNode.Visible = false;
                                    }
                                }
                                //hide umatching child node with no subchild node
                                else
                                {
                                    childNode.Visible = false;
                                }
                            }
                        }
                    }
                }
                //Unmatching Node
                else
                {
                    //validate whether node has any child nodes
                    if (node.Nodes.Count != 0)
                    {
                        int count = 0;
                        foreach (TreeViewNode childNode in node.Nodes)
                        {
                            string strChildNodeText = childNode.Text.ToUpper();

                            //Unmatching Node matching child node
                            if (strChildNodeText.Contains(strText))
                            {
                                node.Expanded = true;
                                childNode.Visible = true;

                                //validate whether childnode has any child nodes
                                if (childNode.Nodes.Count != 0)
                                {
                                    foreach (TreeViewNode subChildNode in childNode.Nodes)
                                    {
                                        string strSubChildNode = subChildNode.Text.ToUpper();

                                        //Unmatching Node matching child node matching subchild node
                                        if (strSubChildNode.Contains(strText))
                                        {
                                            subChildNode.Visible = true;

                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Unmatching Node matching child node matching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                    }
                                                    //Unmatching Node matching child node matching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Unmatching Node matching child node unmatching subchild node
                                        else
                                        {
                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                int superSubChildCount = 0;
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Unmatching Node matching child node unmatching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                        childNode.Expanded = true;
                                                        node.Expanded = true;
                                                    }
                                                    //Unmatching Node matching child node unmatching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                        superSubChildCount += 1;
                                                    }
                                                }

                                                //hide unmatching child node with no matching child nodes
                                                if (superSubChildCount == subChildNode.Nodes.Count)
                                                {
                                                    subChildNode.Visible = false;
                                                }
                                            }
                                            else
                                            {
                                                subChildNode.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                            //Unmatching Node unmatching child node
                            else
                            {
                                //count += 1;
                                //childNode.Visible = false;

                                //validate whether childnode has any child nodes
                                if (childNode.Nodes.Count != 0)
                                {
                                    int subChildCount = 0;
                                    foreach (TreeViewNode subChildNode in childNode.Nodes)
                                    {
                                        string strSubChildNodeText = subChildNode.Text.ToUpper();

                                        //Unmatching Node matching child node matching subchild node
                                        if (strSubChildNodeText.Contains(strText))
                                        {
                                            node.Expanded = true;
                                            childNode.Expanded = true;
                                            subChildNode.Visible = true;

                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Unmatching Node matching child node matching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                    }
                                                    //Unmatching Node matching child node matching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                        //Unmatching Node matching child node unmatching subchild node
                                        else
                                        {
                                            //validate whether subchild has any child nodes
                                            if (subChildNode.Nodes.Count != 0)
                                            {
                                                int superSubChildCount = 0;
                                                foreach (TreeViewNode superSubChildNode in subChildNode.Nodes)
                                                {
                                                    string strSuperSubChildNode = superSubChildNode.Text.ToUpper();

                                                    //Unmatching Node matching child node unmatching subchild node matching supersubchildnode
                                                    if (strSuperSubChildNode.Contains(strText))
                                                    {
                                                        superSubChildNode.Visible = true;
                                                        subChildNode.Expanded = true;
                                                        childNode.Expanded = true;
                                                        node.Expanded = true;
                                                    }
                                                    //Unmatching Node matching child node unmatching subchild node unmatching supersubchildnode
                                                    else
                                                    {
                                                        superSubChildNode.Visible = false;
                                                        superSubChildCount += 1;
                                                    }
                                                }

                                                if (superSubChildCount == subChildNode.Nodes.Count)
                                                {
                                                    subChildNode.Visible = false;
                                                    subChildCount += 1;
                                                }
                                            }
                                            else
                                            {
                                                subChildNode.Visible = false;
                                                subChildCount += 1;
                                            }
                                        }
                                    }

                                    //hide unmatching childnode if it consists of unmatching sub child nodes
                                    if (subChildCount == childNode.Nodes.Count)
                                    {
                                        childNode.Visible = false;
                                        count += 1;
                                    }
                                }
                                //hiding unmatching child node with no matching subchild node
                                else
                                {
                                    childNode.Visible = false;
                                    count += 1;
                                }
                            }
                        }

                        //hide unmatching node if it consists of unmatching child nodes
                        if (count == node.Nodes.Count)
                        {
                            node.Visible = false;
                        }
                    }
                    //hiding unmatching node with no child node
                    else
                    {
                        node.Visible = false;
                    }
                }
            }
        }
    }
}
