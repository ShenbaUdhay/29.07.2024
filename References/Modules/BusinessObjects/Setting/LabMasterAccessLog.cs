using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using System;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]

    public class LoginLog : BaseObject
    {
        public LoginLog(Session session)
            : base(session)
        {
        }


        public override void AfterConstruction()
        {
            base.AfterConstruction();

        }


        protected override void OnSaving()
        {
            base.OnSaving();
            //if (LogOffDateTime == null && LoginDateTime != null)
            //{
            //    LoginDateTime = DateTime.Now;
            //}
            //else if (LogOffDateTime != null && LoginDateTime != null)
            //{
            //    LogOffDateTime = DateTime.Now;
            //    Duration = LoginDateTime - LogOffDateTime;
            //}

        }


        private DateTime _LoginDateTime;
        private DateTime _LogOffDateTime;
        private TimeSpan _Duration;
        private string _VersionNumber;
        private Employee _UserID;
        //private IPermissionPolicyUser _UserName;
        private string _GeoLocation;
        private string _DeviceType;
        private string _DeviceName;
        private string _DeviceID;
        private string _IPAddress;
        private bool _Active;


        public DateTime LoginDateTime
        {
            get { return _LoginDateTime; }
            set { SetPropertyValue(nameof(LoginDateTime), ref _LoginDateTime, value); }
        }

        public DateTime LogOffDateTime
        {
            get { return _LogOffDateTime; }
            set { SetPropertyValue(nameof(LogOffDateTime), ref _LogOffDateTime, value); }
        }

        public TimeSpan Duration
        {
            get { return _Duration; }
            set { SetPropertyValue(nameof(Duration), ref _Duration, value); }
        }

        public string VersionNumber
        {
            get { return _VersionNumber; }
            set { SetPropertyValue(nameof(VersionNumber), ref _VersionNumber, value); }
        }

        public Employee UserID
        {
            get { return _UserID; }
            set { SetPropertyValue(nameof(UserID), ref _UserID, value); }
        }
        public string GeoLocation
        {
            get { return _GeoLocation; }
            set { SetPropertyValue(nameof(GeoLocation), ref _GeoLocation, value); }
        }

        public string DeviceType
        {
            get { return _DeviceType; }
            set { SetPropertyValue(nameof(DeviceType), ref _DeviceType, value); }
        }
        public string DeviceName
        {
            get { return _DeviceName; }
            set { SetPropertyValue(nameof(DeviceName), ref _DeviceName, value); }
        }
        public string DeviceID
        {
            get { return _DeviceID; }
            set { SetPropertyValue(nameof(DeviceID), ref _DeviceID, value); }
        }
        public string IPAddress
        {
            get { return _IPAddress; }
            set { SetPropertyValue(nameof(IPAddress), ref _IPAddress, value); }
        }
        public bool Active
        {
            get { return _Active; }
            set { SetPropertyValue(nameof(Active), ref _Active, value); }
        }
    }
}