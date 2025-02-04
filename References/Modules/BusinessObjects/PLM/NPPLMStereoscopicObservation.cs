﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Modules.BusinessObjects.InfoClass;
using Modules.BusinessObjects.Setting;
using System;
using System.Collections.Generic;

namespace Modules.BusinessObjects.PLM
{
    [DefaultClassOptions]
    [NonPersistent]
    public class NPPLMStereoscopicObservation : BaseObject, ICheckedListBoxItemsProvider
    {
        PLMInfo plmInfo = new PLMInfo();
        public NPPLMStereoscopicObservation(Session session) : base(session) { }
        #region Material
        private string _Material;
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.CheckedListBoxEditor)]
        public string Material
        {
            get { return _Material; }
            set { SetPropertyValue("Material", ref _Material, value); }
        }
        #endregion
        #region PositiveStop
        private bool _PositiveStop;
        [ImmediatePostData(true)]
        public bool PositiveStop
        {
            get { return _PositiveStop; }
            set { SetPropertyValue(nameof(PositiveStop), ref _PositiveStop, value); }
        }
        #endregion
        #region Fibrous
        private bool _Fibrous;
        public bool Fibrous
        {
            get { return _Fibrous; }
            set { SetPropertyValue(nameof(Fibrous), ref _Fibrous, value); }
        }
        #endregion
        #region NonFibrous
        private bool _NonFibrous;
        public bool NonFibrous
        {
            get { return _NonFibrous; }
            set { SetPropertyValue(nameof(NonFibrous), ref _NonFibrous, value); }
        }
        #endregion
        #region Homogeneous
        private bool _Homogeneous;
        public bool Homogeneous
        {
            get { return _Homogeneous; }
            set { SetPropertyValue(nameof(Homogeneous), ref _Homogeneous, value); }
        }
        #endregion
        #region Non-Homogenerous
        private bool _NonHomogenerous;
        public bool NonHomogenerous
        {
            get { return _NonHomogenerous; }
            set { SetPropertyValue(nameof(NonHomogenerous), ref _NonHomogenerous, value); }
        }
        #endregion
        #region Layered
        private bool _Layered;
        public bool Layered
        {
            get { return _Layered; }
            set { SetPropertyValue(nameof(Layered), ref _Layered, value); }
        }
        #endregion
        #region OtherHomogenity
        private bool _OtherHomogenity;
        public bool OtherHomogenity
        {
            get { return _OtherHomogenity; }
            set { SetPropertyValue(nameof(OtherHomogenity), ref _OtherHomogenity, value); }
        }
        #endregion
        #region HomogenityText
        private string _HomogenityText;
        public string HomogenityText
        {
            get { return _HomogenityText; }
            set { SetPropertyValue(nameof(HomogenityText), ref _HomogenityText, value); }
        }
        #endregion
        #region Color_Beige
        private bool _Color_Beige;
        public bool Color_Beige
        {
            get { return _Color_Beige; }
            set { SetPropertyValue(nameof(Color_Beige), ref _Color_Beige, value); }
        }
        #endregion
        #region Color_Black
        private bool _Color_Black;
        public bool Color_Black
        {
            get { return _Color_Black; }
            set { SetPropertyValue(nameof(Color_Black), ref _Color_Black, value); }
        }
        #endregion
        #region Color_Blue
        private bool _Color_Blue;
        public bool Color_Blue
        {
            get { return _Color_Blue; }
            set { SetPropertyValue(nameof(Color_Blue), ref _Color_Blue, value); }
        }
        #endregion
        #region Color_Brown
        private bool _Color_Brown;
        public bool Color_Brown
        {
            get { return _Color_Brown; }
            set { SetPropertyValue(nameof(Color_Brown), ref _Color_Brown, value); }
        }
        #endregion
        #region Color_Clear
        private bool _Color_Clear;
        public bool Color_Clear
        {
            get { return _Color_Clear; }
            set { SetPropertyValue(nameof(Color_Clear), ref _Color_Clear, value); }
        }
        #endregion
        #region Color_Gray
        private bool _Color_Gray;
        public bool Color_Gray
        {
            get { return _Color_Gray; }
            set { SetPropertyValue(nameof(Color_Gray), ref _Color_Gray, value); }
        }
        #endregion
        #region Color_Green
        private bool _Color_Green;
        public bool Color_Green
        {
            get { return _Color_Green; }
            set { SetPropertyValue(nameof(Color_Green), ref _Color_Green, value); }
        }
        #endregion
        #region Color_Orange
        private bool _Color_Orange;
        public bool Color_Orange
        {
            get { return _Color_Orange; }
            set { SetPropertyValue(nameof(Color_Orange), ref _Color_Orange, value); }
        }
        #endregion
        #region Color_Pink
        private bool _Color_Pink;
        public bool Color_Pink
        {
            get { return _Color_Pink; }
            set { SetPropertyValue(nameof(Color_Pink), ref _Color_Pink, value); }
        }
        #endregion
        #region Color_Red
        private bool _Color_Red;
        public bool Color_Red
        {
            get { return _Color_Red; }
            set { SetPropertyValue(nameof(Color_Red), ref _Color_Red, value); }
        }
        #endregion
        #region Color_Silver
        private bool _Color_Silver;
        public bool Color_Silver
        {
            get { return _Color_Silver; }
            set { SetPropertyValue(nameof(Color_Silver), ref _Color_Silver, value); }
        }
        #endregion
        #region Color_Tan
        private bool _Color_Tan;
        public bool Color_Tan
        {
            get { return _Color_Tan; }
            set { SetPropertyValue(nameof(Color_Tan), ref _Color_Tan, value); }
        }
        #endregion
        #region Color_Violet
        private bool _Color_Violet;
        public bool Color_Violet
        {
            get { return _Color_Violet; }
            set { SetPropertyValue(nameof(Color_Violet), ref _Color_Violet, value); }
        }
        #endregion
        #region Color_White
        private bool _Color_White;
        public bool Color_White
        {
            get { return _Color_White; }
            set { SetPropertyValue(nameof(Color_White), ref _Color_White, value); }
        }
        #endregion
        #region Color_Yellow
        private bool _Color_Yellow;
        public bool Color_Yellow
        {
            get { return _Color_Yellow; }
            set { SetPropertyValue(nameof(Color_Yellow), ref _Color_Yellow, value); }
        }
        #endregion
        #region Color_Other
        private bool _Color_Other;
        public bool Color_Other
        {
            get { return _Color_Other; }
            set { SetPropertyValue(nameof(Color_Other), ref _Color_Other, value); }
        }
        #endregion
        #region Color_Text
        private string _Color_Text;
        public string Color_Text
        {
            get { return _Color_Text; }
            set { SetPropertyValue(nameof(Color_Text), ref _Color_Text, value); }
        }
        #endregion
        #region FibersPresent
        private bool _FibersPresent;
        public bool FibersPresent
        {
            get { return _FibersPresent; }
            set { SetPropertyValue(nameof(FibersPresent), ref _FibersPresent, value); }
        }
        #endregion
        #region Friability
        private Friability? _Friability;
        public Friability? Friability
        {
            get { return _Friability; }
            set { SetPropertyValue(nameof(Friability), ref _Friability, value); }
        }
        #endregion
        #region SampleTreatment_AcidTreated
        private bool _SampleTreatment_AcidTreated;
        public bool SampleTreatment_AcidTreated
        {
            get { return _SampleTreatment_AcidTreated; }
            set { SetPropertyValue(nameof(SampleTreatment_AcidTreated), ref _SampleTreatment_AcidTreated, value); }
        }
        #endregion
        #region SampleTreatment_Ashed
        private bool _SampleTreatment_Ashed;
        public bool SampleTreatment_Ashed
        {
            get { return _SampleTreatment_Ashed; }
            set { SetPropertyValue(nameof(SampleTreatment_Ashed), ref _SampleTreatment_Ashed, value); }
        }
        #endregion
        #region SampleTreatment_Ground
        private bool _SampleTreatment_Ground;
        public bool SampleTreatment_Ground
        {
            get { return _SampleTreatment_Ground; }
            set { SetPropertyValue(nameof(SampleTreatment_Ground), ref _SampleTreatment_Ground, value); }
        }
        #endregion
        #region SampleTreatment_HCL
        private bool _SampleTreatment_HCL;
        public bool SampleTreatment_HCL
        {
            get { return _SampleTreatment_HCL; }
            set { SetPropertyValue(nameof(SampleTreatment_HCL), ref _SampleTreatment_HCL, value); }
        }
        #endregion
        #region SampleTreatment_Matrix
        private bool _SampleTreatment_Matrix;
        public bool SampleTreatment_Matrix
        {
            get { return _SampleTreatment_Matrix; }
            set { SetPropertyValue(nameof(SampleTreatment_Matrix), ref _SampleTreatment_Matrix, value); }
        }
        #endregion
        #region SampleTreatment_NonFriable
        private bool _SampleTreatment_NonFriable;
        public bool SampleTreatment_NonFriable
        {
            get { return _SampleTreatment_NonFriable; }
            set { SetPropertyValue(nameof(SampleTreatment_NonFriable), ref _SampleTreatment_NonFriable, value); }
        }
        #endregion
        #region SampleTreatment_SolventTreated
        private bool _SampleTreatment_SolventTreated;
        public bool SampleTreatment_SolventTreated
        {
            get { return _SampleTreatment_SolventTreated; }
            set { SetPropertyValue(nameof(SampleTreatment_SolventTreated), ref _SampleTreatment_SolventTreated, value); }
        }
        #endregion
        #region SampleTreatment_Teased
        private bool _SampleTreatment_Teased;
        public bool SampleTreatment_Teased
        {
            get { return _SampleTreatment_Teased; }
            set { SetPropertyValue(nameof(SampleTreatment_Teased), ref _SampleTreatment_Teased, value); }
        }
        #endregion
        #region SampleTreatment_Other
        private bool _SampleTreatment_Other;
        public bool SampleTreatment_Other
        {
            get { return _SampleTreatment_Other; }
            set { SetPropertyValue(nameof(SampleTreatment_Other), ref _SampleTreatment_Other, value); }
        }
        #endregion
        #region FiberObservation
        private string _FiberObservation;
        public string FiberObservation
        {
            get { return _FiberObservation; }
            set { SetPropertyValue(nameof(FiberObservation), ref _FiberObservation, value); }
        }
        #endregion
        #region NoAsbestosDetected
        private bool _NoAsbestosDetected;
        public bool NoAsbestosDetected
        {
            get { return _NoAsbestosDetected; }
            set { SetPropertyValue(nameof(NoAsbestosDetected), ref _NoAsbestosDetected, value); }
        }
        #endregion
        #region ICheckedListBoxItemsProvider Members
        public Dictionary<object, string> GetCheckedListBoxItems(string targetMemberName)
        {
            if (targetMemberName == "Material")
            {
                if (plmInfo.Materials == null)
                {
                    plmInfo.Materials = new Dictionary<object, string>();
                    XPCollection<Materials> Materials = new XPCollection<Materials>(Session, CriteriaOperator.Parse(""), new SortProperty("MaterialName", SortingDirection.Ascending));
                    foreach (Materials obj in Materials)
                    {
                        if (!plmInfo.Materials.ContainsKey(obj.Oid) && !string.IsNullOrEmpty(obj.MaterialName))
                        {
                            plmInfo.Materials.Add(obj.Oid, obj.MaterialName);
                        }
                    }
                }
                return plmInfo.Materials;
            }
            else
            {
                return null;
            }
        }

        public event EventHandler ItemsChanged;
        protected void OnItemsChanged()
        {
            if (ItemsChanged != null)
            {
                ItemsChanged(this, new EventArgs());
            }
        }
        #endregion
    }
}