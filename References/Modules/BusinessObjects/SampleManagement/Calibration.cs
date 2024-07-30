using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.SDMS;
using System;
using System.Linq;

namespace Modules.BusinessObjects.SampleManagement
{
    [DefaultClassOptions]
    //[NonPersistent]
    [Persistent("SpreadSheetEntry_Calibration")]
    public class Calibration : BaseObject
    {
        public Calibration(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString()))
            {
                return;
            }
            else
            {
                CreatedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                CreatedDate = Library.GetServerTime(Session);
            }
        }
        protected override void OnSaving()
        {
            base.OnSaving();
            ////if (uqID == 0)
            ////{
            ////    uqID = Convert.ToInt32(Session.Evaluate(typeof(Calibration), CriteriaOperator.Parse("Max(uqID)"), null)) + 1;
            ////}
            //if (string.IsNullOrEmpty(CalibrationID))//
            //{
            //    string tempID = (Convert.ToInt32(Session.Evaluate(typeof(Calibration), CriteriaOperator.Parse("Max(SUBSTRING(CalibrationID, 2, 8))"), null)) + 1).ToString();
            //    var curdate = DateTime.Now.ToString("yyMMdd");
            //    if (tempID != "1")
            //    {
            //        var predate = tempID.Substring(0, 6);
            //        if (predate == curdate)
            //        {
            //            tempID = "CB" + tempID;
            //        }
            //        else
            //        {
            //            tempID = "CB" + curdate + "01";
            //        }
            //    }
            //    else
            //    {
            //        tempID = "CB" + curdate + "01";
            //    }
            //    CalibrationID = tempID + "_" + SecuritySystem.CurrentUserName;
            //}

            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString()))
            {
                return;
            }
            else
            {
                ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
                ModifiedDate = Library.GetServerTime(Session);
            }
        }

        #region uqID
        int fuqID;
        public int uqID
        {
            get { return fuqID; }
            set { SetPropertyValue<int>(nameof(uqID), ref fuqID, value); }
        }
        #endregion

        #region CalibrationID
        string fCalibrationID;
        [Size(50)]
        public string CalibrationID
        {
            get { return fCalibrationID; }
            set { SetPropertyValue<string>(nameof(CalibrationID), ref fCalibrationID, value); }
        }
        #endregion

        CustomSystemUser fCreatedBy;
        public CustomSystemUser CreatedBy
        {
            get { return fCreatedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(CreatedBy), ref fCreatedBy, value); }
        }
        DateTime fCreatedDate;
        public DateTime CreatedDate
        {
            get { return fCreatedDate; }
            set { SetPropertyValue<DateTime>(nameof(CreatedDate), ref fCreatedDate, value); }
        }
        CustomSystemUser fModifiedBy;
        public CustomSystemUser ModifiedBy
        {
            get { return fModifiedBy; }
            set { SetPropertyValue<CustomSystemUser>(nameof(ModifiedBy), ref fModifiedBy, value); }
        }
        DateTime fModifiedDate;
        public DateTime ModifiedDate
        {
            get { return fModifiedDate; }
            set { SetPropertyValue<DateTime>(nameof(ModifiedDate), ref fModifiedDate, value); }
        }
        SpreadSheetBuilder_TemplateInfo fTemplateID;
        public SpreadSheetBuilder_TemplateInfo TemplateID
        {
            get { return fTemplateID; }
            set { SetPropertyValue<SpreadSheetBuilder_TemplateInfo>(nameof(TemplateID), ref fTemplateID, value); }
        }

        [Association("Calibration-CalibrationInfo")]
        public XPCollection<CalibrationInfo> CalibrationInfos
        {
            get
            {
                return GetCollection<CalibrationInfo>(nameof(CalibrationInfos));
            }
        }

        string _RegressionCurveConstant;
        //[Size(SizeAttribute.Unlimited)]
        //[NonPersistent]
        [ImmediatePostData]
        public string RegressionCurveConstant
        {
            get
            {
                if (CalibrationInfos != null && CalibrationInfos.Count > 0)
                {
                    CalibrationInfo info = CalibrationInfos.FirstOrDefault<CalibrationInfo>(i => i.LevelNo == 1);
                    if (info != null)
                    {

                        _RegressionCurveConstant = @"y=" + info.Slope + "x+" + info.Intercept + Environment.NewLine + "SLOPE:" + info.Slope + Environment.NewLine +
                            @"INTERCEPT:" + info.Intercept + Environment.NewLine + "R ^2:" + info.RCAP2 + Environment.NewLine + "CALIBRATEDDATE:" + CreatedDate + Environment.NewLine +
                            "CALIBRATEDBy:" + CreatedBy.UserName;
                    }
                    else
                    {
                        _RegressionCurveConstant = @"y=x+" + Environment.NewLine + "SLOPE:" + Environment.NewLine +
                            @"INTERCEPT:" + Environment.NewLine + "R ^2:" + Environment.NewLine + "CALIBRATEDDATE:" + Environment.NewLine +
                            "CALIBRATEDBy:";
                    }
                }
                else
                {
                    _RegressionCurveConstant = @"y=x+" + Environment.NewLine + "SLOPE:" + Environment.NewLine +
                        @"INTERCEPT:" + Environment.NewLine + "R ^2:" + Environment.NewLine + "CALIBRATEDDATE:" + Environment.NewLine +
                        "CALIBRATEDBy:";
                }
                return _RegressionCurveConstant;
            }
        }


    }
}