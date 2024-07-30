using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.SampleManagement;
using System.Collections.Generic;
using System.ComponentModel;

namespace Modules.BusinessObjects.Setting
{
    [DefaultClassOptions]

    public class DOCDetails : BaseObject
    {
        public DOCDetails(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        #region IListforSampleParameter
        [Association, Browsable(false)]
        public IList<SampleParameter> SampleParameter
        {
            get
            {
                return GetList<SampleParameter>("SampleParameter");
            }
        }
        #endregion IListforSampleParameter

        #region DOC        
        private DOC _DOC;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public DOC DOC
        {
            get { return _DOC; }
            set { SetPropertyValue<DOC>("DOC", ref _DOC, value); }
        }
        #endregion
        #region Result1        
        private string _Result1;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result1
        {
            get { return _Result1; }
            set { SetPropertyValue<string>("Result1", ref _Result1, value); }
        }
        #endregion

        #region Result2        
        private string _Result2;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result2
        {
            get { return _Result2; }
            set { SetPropertyValue<string>("Result2", ref _Result2, value); }
        }
        #endregion

        #region Result3        
        private string _Result3;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result3
        {
            get { return _Result3; }
            set { SetPropertyValue<string>("Result3", ref _Result3, value); }
        }
        #endregion

        #region Result4        
        private string _Result4;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result4
        {
            get { return _Result4; }
            set { SetPropertyValue<string>("Result4", ref _Result4, value); }
        }
        #endregion

        #region Result5        
        private string _Result5;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result5
        {
            get { return _Result5; }
            set { SetPropertyValue<string>("Result5", ref _Result5, value); }
        }
        #endregion

        #region Result6        
        private string _Result6;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result6
        {
            get { return _Result6; }
            set { SetPropertyValue<string>("Result6", ref _Result6, value); }
        }
        #endregion

        #region Result7        
        private string _Result7;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result7
        {
            get { return _Result7; }
            set { SetPropertyValue<string>("Result7", ref _Result7, value); }
        }
        #endregion

        #region Result8        
        private string _Result8;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Result8
        {
            get { return _Result8; }
            set { SetPropertyValue<string>("Result8", ref _Result8, value); }
        }
        #endregion

        #region Rec1        
        private string _Rec1;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec1
        {
            get { return _Rec1; }
            set { SetPropertyValue<string>("Rec1", ref _Rec1, value); }
        }
        #endregion

        #region Rec2        
        private string _Rec2;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec2
        {
            get { return _Rec2; }
            set { SetPropertyValue<string>("Rec2", ref _Rec2, value); }
        }
        #endregion

        #region Rec3        
        private string _Rec3;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec3
        {
            get { return _Rec3; }
            set { SetPropertyValue<string>("Rec3", ref _Rec3, value); }
        }
        #endregion

        #region Rec4        
        private string _Rec4;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec4
        {
            get { return _Rec4; }
            set { SetPropertyValue<string>("Rec4", ref _Rec4, value); }
        }
        #endregion

        #region Rec5        
        private string _Rec5;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec5
        {
            get { return _Rec5; }
            set { SetPropertyValue<string>("Rec5", ref _Rec5, value); }
        }
        #endregion

        #region Rec6        
        private string _Rec6;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec6
        {
            get { return _Rec6; }
            set { SetPropertyValue<string>("Rec6", ref _Rec6, value); }
        }
        #endregion

        #region Rec7        
        private string _Rec7;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec7
        {
            get { return _Rec7; }
            set { SetPropertyValue<string>("Rec7", ref _Rec7, value); }
        }
        #endregion

        #region Rec8        
        private string _Rec8;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false), VisibleInDashboards(false)]
        public string Rec8
        {
            get { return _Rec8; }
            set { SetPropertyValue<string>("Rec8", ref _Rec8, value); }
        }
        #endregion
    }
}