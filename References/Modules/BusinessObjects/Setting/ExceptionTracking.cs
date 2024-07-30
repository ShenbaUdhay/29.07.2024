using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Setting
{
    public class ExceptionTracking : BaseObject
    {
        public ExceptionTracking(Session session) : base(session) { }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        private string fErrorMessage;
        [Size(SizeAttribute.Unlimited)]
        public string ErrorMessage
        {
            get
            {
                return fErrorMessage;
            }
            set
            {
                SetPropertyValue("ErrorMessage", ref fErrorMessage, value);
            }
        }

        private string fStackTrace;
        [Size(SizeAttribute.Unlimited)]
        public string StackTrace
        {
            get
            {
                return fStackTrace;
            }
            set
            {
                SetPropertyValue("StackTrace", ref fStackTrace, value);
            }
        }

        private string fControllerName;
        public string ControllerName
        {
            get
            {
                return fControllerName;
            }
            set
            {
                SetPropertyValue("ControllerName", ref fControllerName, value);
            }
        }

        private string fFunctionName;
        public string FunctionName
        {
            get
            {
                return fFunctionName;
            }
            set
            {
                SetPropertyValue("FunctionName", ref fFunctionName, value);
            }
        }

        private string fViewID;
        public string ViewID
        {
            get
            {
                return fViewID;
            }
            set
            {
                SetPropertyValue("ViewID", ref fViewID, value);
            }
        }

        private Employee fLoginBy;
        public Employee LoginBy
        {
            get { return fLoginBy; }
            set { SetPropertyValue("LoginBy", ref fLoginBy, value); }
        }

        private DateTime fLoginDate;
        public DateTime LoginDate
        {
            get { return fLoginDate; }
            set { SetPropertyValue("LoginDate", ref fLoginDate, value); }
        }
    }
}