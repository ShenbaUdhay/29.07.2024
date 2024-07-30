using DevExpress.ExpressApp.DC;

namespace Modules.BusinessObjects.SuboutTracking
{
    public enum SuboutStatus
    {
        PendingForSubout = 0,
        //PendingForSuboutDelivery = 1,
        //SuboutDelivered = 2,
        [XafDisplayName("Pending Signing Off")]
        PendingSigningOff = 1,
        [XafDisplayName("Subout Pending Result Entry")]
        PendingResultEntry = 2,
        SuboutPendingValidation = 3,
        SuboutPendingApproval = 4,
        IsExported = 5
    }
    public enum SuboutNotificationQueueStatus
    {
        Waiting = 0,
        Send = 1,
    }

    public enum DeliveryService
    {
        [XafDisplayName("N/A")]
        NA = 0,
        XXX = 1,
        YYY = 2,
        ZZZ = 3,
    }
    //[DefaultClassOptions]
    ////[ImageName("BO_Contact")]
    ////[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    ////[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    ////[Persistent("DatabaseTableName")]
    //// Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    //public class SubOutEnum : BaseObject
    //{ // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
    //    public SubOutEnum(Session session)
    //        : base(session)
    //    {
    //    }
    //    public override void AfterConstruction()
    //    {
    //        base.AfterConstruction();
    //        // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
    //    }
    //private string _PersistentProperty;
    //[XafDisplayName("My display name"), ToolTip("My hint message")]
    //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
    //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
    //public string PersistentProperty {
    //    get { return _PersistentProperty; }
    //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
    //}

    //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
    //public void ActionMethod() {
    //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
    //    this.PersistentProperty = "Paid";
    //}

}