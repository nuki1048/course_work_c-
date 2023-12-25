using System;

namespace MVCFrame
{
    internal class Controller        
    {        
        public static void Execute(ModelOperations operation, Model model, Settings settings = null)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Empty model");
            }
            switch (operation)
            {
                case ModelOperations.SaveSettings:                    
                    model.SaveSettings();
                    break;
                case ModelOperations.WorkingCycle:
                    model.WorkingCycle();
                    break;
                case ModelOperations.Clear:
                    model.Clear();
                    break;
                default:
                    throw new ArgumentException("Unknown operation: " + operation, "operation");
            }
        }
    }
}
