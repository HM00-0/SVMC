using Autodesk.Revit.UI;
using System.Reflection;

namespace SVMC.Commands
{
    public class App : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            RibbonPanel panel = app.CreateRibbonPanel("SVMC");

            string assemblyPath = Assembly.GetExecutingAssembly().Location;

            SplitButtonData splitButtonData = new SplitButtonData("InsertUnit", "Insert Unit");
            SplitButton splitButton = panel.AddItem(splitButtonData) as SplitButton;

            PushButtonData O_O_W_O = new PushButtonData("O_O_W_O", "O_O_W_O", assemblyPath, "SVMC.Commands.Module_O_O_W_O");
            PushButtonData O_O_W_FC = new PushButtonData("O_O_W_FC", "O_O_W_FC", assemblyPath, "SVMC.Commands.Module_O_O_W_FC");
            PushButtonData O_SC_W_FC = new PushButtonData("O_SC_W_FC", "O_SC_W_FC", assemblyPath, "SVMC.Commands.Module_O_SC_W_FC");

            splitButton.AddPushButton(O_O_W_O);
            splitButton.AddPushButton(O_O_W_FC);
            splitButton.AddPushButton(O_SC_W_FC);


            // Wall Creation Button
            PushButtonData wallButtonData = new PushButtonData(
                "DrawAndSplitWall",
                "벽체 생성",
                assemblyPath,
                "SVMC.Commands.Panel_System"
            );

            panel.AddItem(wallButtonData);

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }
    }
}