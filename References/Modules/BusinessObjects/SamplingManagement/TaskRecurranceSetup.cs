using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using Modules.BusinessObjects.Setting;

namespace Modules.BusinessObjects.SamplingManagement
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TaskRecurranceSetup : Event
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TaskRecurranceSetup(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        #region COCSettings
        private COCSettings _COCSettings;
        [ImmediatePostData(true)]
        [Association("COCSettings-CopyTo")]
        public COCSettings COCSettings
        {
            get
            {
                return _COCSettings;
            }
            set
            {
                SetPropertyValue<COCSettings>("COCSettings", ref _COCSettings, value);
            }
        }
        #endregion

        #region COCID
        private string _COCID;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string COCID
        {
            get
            {
                if (COCSettings != null)
                {
                    if (COCSettings.COC_ID != null)
                    {
                        _COCID = COCSettings.COC_ID.ToString();
                    }
                    else
                    {
                        _COCID = string.Empty;
                    }

                }
                else
                {
                    _COCID = string.Empty;
                }
                return _COCID;
            }
            set { SetPropertyValue<string>("COCID", ref _COCID, value); }

        }
        #endregion

        #region ClientName
        private string _ClientName;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ClientName
        {
            get
            {
                if (COCSettings != null)
                {
                    _ClientName = COCSettings.ClientName.CustomerName.ToString();
                }
                else
                {
                    _ClientName = string.Empty;
                }
                return _ClientName;
            }
            set
            {
                SetPropertyValue("ClientName", ref _ClientName, value);
            }
        }
        #endregion

        #region ClientAddress
        private string _ClientAddress;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ClientAddress
        {
            get
            {
                if (COCSettings != null)
                {
                    if (COCSettings.ClientAddress != null)
                    {
                        _ClientAddress = COCSettings.ClientAddress;
                    }
                    else
                    {
                        _ClientAddress = string.Empty;
                    }

                }
                else
                {
                    _ClientAddress = string.Empty;
                }
                return _ClientAddress;
            }
            set { SetPropertyValue<string>("ClientAddress", ref _ClientAddress, value); }

        }
        #endregion

        #region ProjectID
        private string _ProjectID;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ProjectID
        {
            get
            {
                if (COCSettings != null)
                {
                    if(COCSettings.ProjectID!= null)
                    {
                        _ProjectID = COCSettings.ProjectID.ProjectId.ToString();
                    }
                    else
                    {
                        _ProjectID = string.Empty;
                    }

                }
                else
                {
                    _ProjectID = string.Empty;
                }
                return _ProjectID;
            }
            set { SetPropertyValue<string>("ProjectID", ref _ProjectID, value); }

        }
        #endregion


        #region ProjectName
        private string _ProjectName;
        [ImmediatePostData(true)]
        [NonPersistent]
        public string ProjectName
        {
            get
            {
                if (COCSettings != null)
                {
                    if(COCSettings.ProjectName!=null)
                    {
                        _ProjectName = COCSettings.ProjectName.ToString();
                    }
                    else
                    {
                        _ProjectName = string.Empty;
                    }

                }
                else
                {
                    _ProjectName = string.Empty;
                }
                return _ProjectName;
            }
            set { SetPropertyValue<string>("ProjectName", ref _ProjectName, value); }

        }
        #endregion

        #region Remark
        private string _Remark;
        [Size(SizeAttribute.Unlimited)]
        public string Remark
        {
            get { return _Remark; }
            set { SetPropertyValue<string>(nameof(Remark), ref _Remark, value); }
        }
        #endregion




    }
}