using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Modules.BusinessObjects.Hr;
using Modules.BusinessObjects.ICM;
using Modules.BusinessObjects.Libraries;
using Modules.BusinessObjects.Setting;
using System;

namespace ALPACpre.Module.BusinessObjects.ICM
{
    [DefaultClassOptions]

    public class ItemDepletion : BaseObject
    {
        public ItemDepletion(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            if (SecuritySystem.CurrentUserId == null || string.IsNullOrWhiteSpace(SecuritySystem.CurrentUserId.ToString())) return;
            CreatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            CreatedDate = DateTime.Now;
            ModifiedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedDate = DateTime.Now;
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            Date = DateTime.Now;
            TakenBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ConsumedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            AmountTaken = 1;
            //AmountRemain = Distribution.Item.StockQty - AmountTaken;

        }

        protected override void OnSaving()
        {
            base.OnSaving();
            TakenBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ConsumedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            ModifiedBy = Session.GetObjectByKey<CustomSystemUser>(SecuritySystem.CurrentUserId);
            ModifiedDate = Library.GetServerTime(Session);
            LastUpdatedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            LastUpdatedDate = Library.GetServerTime(Session);
            //if (AmountRemain == 0 && Distribution != null)
            //{
            //    Distribution.IsDeplete = true;
            //    Distribution.DepletedBy = Session.GetObjectByKey<Employee>(SecuritySystem.CurrentUserId);
            //    Distribution.DateDepleted = Library.GetServerTime(Session);
            //    Distribution.Status = Distribution.LTStatus.Consumed;
            //}
        }

        private DateTime _Date;
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue<DateTime>(nameof(Date), ref _Date, value); }
        }

        private Employee _ConsumedBy;
        public Employee ConsumedBy
        {
            get { return _ConsumedBy; }
            set { SetPropertyValue<Employee>(nameof(ConsumedBy), ref _ConsumedBy, value); }
        }


        private Distribution _Distribution;
        [Association("Distribution-ItemDepletion")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Distribution Distribution
        {
            get { return _Distribution; }
            set { SetPropertyValue<Distribution>(nameof(Distribution), ref _Distribution, value); }
        }
        private Unit _AmountUnits;
        public Unit AmountUnits
        {
            get { return _AmountUnits; }
            set { SetPropertyValue<Unit>(nameof(AmountUnits), ref _AmountUnits, value); }
        }

        private int _AmountTaken;
        [ImmediatePostData]
        public int AmountTaken
        {
            get { return _AmountTaken; }
            set { SetPropertyValue<int>(nameof(AmountTaken), ref _AmountTaken, value); }
        }
        private int _StockAmount;
        public int StockAmount
        {
            get { return _StockAmount; }
            set { SetPropertyValue<int>(nameof(StockAmount), ref _StockAmount, value); }
        }
        private int _AmountRemain;
        [ImmediatePostData]
        public int AmountRemain
        {
            get { return _AmountRemain; }
            set { SetPropertyValue<int>(nameof(AmountRemain), ref _AmountRemain, value); }
        }


        private Employee _TakenBy;
        public Employee TakenBy
        {
            get { return _TakenBy; }
            set { SetPropertyValue<Employee>(nameof(TakenBy), ref _TakenBy, value); }
        }

        private string _Comment;
        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }
        private Employee _LastUpdatedBy;
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public Employee LastUpdatedBy
        {
            get
            {
                return _LastUpdatedBy;
            }
            set
            {
                SetPropertyValue("LastUpdatedBy", ref _LastUpdatedBy, value);
            }
        }
        private DateTime _LastUpdatedDate;

        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public DateTime LastUpdatedDate
        {
            get
            {
                return _LastUpdatedDate;
            }
            set
            {
                SetPropertyValue("LastUpdatedDate", ref _LastUpdatedDate, value);
            }
        }

        private CustomSystemUser fModifiedBy;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MB9", Enabled = false, Context = "DetailView")]
        public CustomSystemUser ModifiedBy
        {
            get
            {
                return fModifiedBy;
            }
            set
            {
                SetPropertyValue("ModifiedBy", ref fModifiedBy, value);
            }
        }
        private DateTime fModifiedDate;
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        //[Browsable(false)]
        //[Appearance("MD9", Enabled = false, Context = "DetailView")]       
        public DateTime ModifiedDate
        {
            get
            {
                return fModifiedDate;
            }
            set
            {
                SetPropertyValue("ModifiedDate", ref fModifiedDate, value);
            }
        }

        private Employee _CreatedBy;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public Employee CreatedBy
        {
            get { return _CreatedBy; }
            set { SetPropertyValue("CreatedBy", ref _CreatedBy, value); }
        }
        private DateTime _CreatedDate;
        [VisibleInListView(false), VisibleInDetailView(false)]
        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { SetPropertyValue("CreatedDate", ref _CreatedDate, value); }
        }

        private int _Sort;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public int Sort
        {
            get
            {
                return _Sort;
            }
            set
            {
                SetPropertyValue<int>(nameof(Sort), ref _Sort, value);
            }
        }

        private string _ConsumedLT;
        [VisibleInListView(false), VisibleInDetailView(false), VisibleInLookupListView(false)]
        public string ConsumedLT
        {
            get
            {
                return _ConsumedLT;
            }
            set
            {
                SetPropertyValue<string>(nameof(ConsumedLT), ref _ConsumedLT, value);
            }
        }

    }

}
