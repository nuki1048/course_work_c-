using System;

namespace MVCFrame
{
    internal abstract class View: IDisposable
    {
        public View(Model model, Controller controller)
        {
            this.Model = model;
            this.Controller = controller;
        }

        public readonly  Model Model;
        public Controller Controller
        {
            private get;
            set;            
        }
        public void ReactToUserActions(ModelOperations modelOperation, Settings settings = null)
		{
            Controller.Execute(modelOperation, Model);
        }
        public void Dispose()
        {
            DataUnbind();
        }
        public abstract void DataBind();
        public abstract void DataUnbind();
    }
}
