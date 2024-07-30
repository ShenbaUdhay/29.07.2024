using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;

namespace Modules.BusinessObjects.PLM_Control
{
    public partial class PLMQualityControlController : ViewController
    {
        public PLMQualityControlController()
        {
            InitializeComponent();
            TargetViewId = "PLMQualityControl_DetailView_QueryView";
            SelectMode.TargetViewId = "PLMQualityControl_DetailView_QueryView;";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
            if (SelectMode.SelectedItem == null)
            {
                SelectMode.SelectedItem = SelectMode.Items[0];
            }
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            //if (e.PropertyName == "Analyst")
            //{
            //    PLMQualityControl objTask = (PLMQualityControl)e.Object;
            //    if (!string.IsNullOrEmpty(objTask.Analyst))
            //    {
            //        IObjectSpace os = Application.CreateObjectSpace();
            //        HttpContext.Current.Session["Test"] = objTask.Analyst;
            //        if (HttpContext.Current.Session["Test"] != null)
            //        {
            //            List<Employee> lstVM = new List<Employee>();
            //            foreach (string strMatrix in objTask.Analyst.Split(';'))
            //            {
            //                Employee objSM = ObjectSpace.GetObjectByKey<Employee>(new Guid(strMatrix));
            //                if (objSM != null)
            //                {
            //                    lstVM.Add(objSM);
            //                }
            //            }
            //        }
            //    }
            //}
        }
    }
}
