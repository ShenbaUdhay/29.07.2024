using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Libraries;
using System;

namespace Modules.BusinessObjects.Setting
{
    public class VersionPosts : BaseObject
    {
        public VersionPosts(Session session)
            : base(session)
        {
        }

        private VersionControl _VersionLog;
        [VisibleInListView(false),VisibleInDetailView(false),VisibleInLookupListView(false)]
        [Association("VersionLog-Posts")]
        public VersionControl VersionLog
        {
            get
            {
                return _VersionLog;
            }
            set
            {
                SetPropertyValue<VersionControl>(nameof(VersionLog),ref _VersionLog, value);
            }
        }

        private string _Comment;
        [Size(SizeAttribute.Unlimited)]
        public string Comment
        {
            get
            {
                return _Comment;
            }
            set
            {
                SetPropertyValue<string>(nameof(Comment), ref _Comment, value);
            }
        }
        private string _RTFComment;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        [Size(SizeAttribute.Unlimited)]
        public string RTFComment
        {
            get
            {
                return _RTFComment;
            }
            set
            {
                SetPropertyValue<string>(nameof(RTFComment), ref _RTFComment, value);
            }
        }

        private FileData _File;
        public FileData File
        {
            get
            {
                return _File;
            }
            set
            {
                SetPropertyValue<FileData>(nameof(File), ref _File, value);
            }
        }

        private DateTime _PostedDateTime;
        public  DateTime PostedDateTime
        {
            get
            {
                return _PostedDateTime;
            }
            set
            {
                SetPropertyValue<DateTime>(nameof(PostedDateTime), ref _PostedDateTime, value);
            }
        }

        #region Posted By
        private Employee _PostedBy;
        public Employee PostedBy
        {
            get
            {
                return _PostedBy;
            }
            set
            {
                SetPropertyValue<Employee>(nameof(PostedBy), ref _PostedBy, value);
            }
        }
        #endregion

        #region PostedBy
        private string _PostedByUserName;
        public string PostedByUserName
        {
            get
            {
                return _PostedByUserName;
            }
            set
            {
                SetPropertyValue<string>(nameof(PostedByUserName), ref _PostedByUserName, value);
            }
        }
        #endregion

        //#region Description
        //public string Description
        //{
        //    get
        //    {
        //        if (PostedBy != null)
        //        {
        //            return "Posted by" + PostedBy.UserName + " on " + PostedDateTime;
        //        }
        //        else
        //        {
        //            return "Posted on " + PostedDateTime;
        //        }
        //    }
        //}
        //#endregion
        #region SupportPostID
        private int _SupportPostID;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int SupportPostID
        {
            get
            {
                return _SupportPostID;
            }
            set
            {
                SetPropertyValue("SupportPostID", ref _SupportPostID, value);
            }
        }
        #endregion
    }
}
