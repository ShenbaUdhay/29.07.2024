using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace Modules.BusinessObjects.SampleManagement.SamplePreparation
{
    [DefaultClassOptions]
    [NonPersistent]
    public class CopyFromSamplePrepBatch : BaseObject
    {
        public CopyFromSamplePrepBatch(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region Reagent
        private bool _Reagent;
        [ImmediatePostData]
        public bool Reagent
        {
            get
            {
                return _Reagent;
            }
            set
            {
                SetPropertyValue<bool>(nameof(Reagent), ref _Reagent, value);
                if (value == false && AllAbove == true)
                {
                    AllAbove = false;
                }
            }
        }
        #endregion

        #region Instrument
        private bool _Instrument;
        [ImmediatePostData]
        public bool Instrument
        {
            get
            {
                return _Instrument;
            }
            set
            {
                SetPropertyValue<bool>(nameof(Instrument), ref _Instrument, value);
                if (value == false && AllAbove == true)
                {
                    AllAbove = false;
                }
            }
        }
        #endregion

        #region Others
        private bool _Others;
        [ImmediatePostData]
        public bool Others
        {
            get
            {
                return _Others;
            }
            set
            {
                SetPropertyValue<bool>(nameof(Others), ref _Others, value);
                if (value == false && AllAbove == true)
                {
                    AllAbove = false;
                }
            }
        }
        #endregion


        #region AllAbove
        private bool _AllAbove;
        [ImmediatePostData]
        public bool AllAbove
        {
            get
            {
                return _AllAbove;
            }
            set
            {
                SetPropertyValue<bool>(nameof(AllAbove), ref _AllAbove, value);
                if (value == true)
                {
                    Reagent = Instrument = Others = true;
                }
                else if (Reagent == true && Instrument == true && Others == true)
                {

                    Reagent = Instrument = Others = false;

                }
            }
        }
        #endregion

        //public IList<SamplePrepBatch> PrepBatches
        //{
        //    get
        //    {
        //        return owners;
        //    }
        //}

    }
}