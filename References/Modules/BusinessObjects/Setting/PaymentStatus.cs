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

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]
    [DefaultProperty("Status")]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class PaymentStatus : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public PaymentStatus(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Session.CollectReferencingObjects(this).Count > 0)
            {
                foreach (var obj in Session.CollectReferencingObjects(this))
                {
                    if (obj.GetType() != typeof(DevExpress.Xpo.Metadata.Helpers.IntermediateObject))
                    {
                        Exception ex = new Exception("Unable to delete since the data already used in another form");
                        throw ex;
                        break;

                    }
                }
            }
        }
        #region Status
        private string _Status;
        [RuleRequiredField("paymentStatus", DefaultContexts.Save, "'Status' must not be empty.")]
        [RuleUniqueValue]
        public string Status
        {
            get { return _Status; }
            set { SetPropertyValue(nameof(Status), ref _Status, value.Trim()); }
        }
        #endregion
        #region IsInvoiceQueue
        private bool _IsInvoiceQueue;
        public bool IsInvoiceQueue
        {
            get { return _IsInvoiceQueue; }
            set { SetPropertyValue(nameof(IsInvoiceQueue), ref _IsInvoiceQueue, value); }
        }
        #endregion
        #region Default
        private bool _Default;
        [ImmediatePostData]
        public bool Default
        {
            get { return _Default; }
            set { SetPropertyValue(nameof(Default), ref _Default, value); }
        }
        #endregion
       
    }
}