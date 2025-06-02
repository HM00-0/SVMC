using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SVMC.Settings;

namespace SVMC.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Module_O_O_W_O : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Module_System.Run(commandData, ModuleType.O_O_W_O, ref message);
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Module_O_O_W_FC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Module_System.Run(commandData, ModuleType.O_O_W_FC, ref message);
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Module_O_SC_W_FC : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Module_System.Run(commandData, ModuleType.O_SC_W_FC, ref message);
        }
    }
}
