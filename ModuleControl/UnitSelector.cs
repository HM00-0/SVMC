using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using SVMC.Settings;

namespace SVMC.ModuleControl
{
    public class UnitSelector
    {
        public static UnitType? SelectUnit()
        {
            if (SelectedUnitType.CurrentUnit.HasValue)
                return SelectedUnitType.CurrentUnit;

            TaskDialog td = new TaskDialog("유닛 선택");
            td.MainInstruction = "Unit 종류를 선택하세요:";
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Unit A");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Unit B");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "Unit C");

            TaskDialogResult result = td.Show();

            switch (result)
            {
                case TaskDialogResult.CommandLink1:
                    SelectedUnitType.CurrentUnit = UnitType.UnitA;
                    break;
                case TaskDialogResult.CommandLink2:
                    SelectedUnitType.CurrentUnit = UnitType.UnitB;
                    break;
                case TaskDialogResult.CommandLink3:
                    SelectedUnitType.CurrentUnit = UnitType.UnitC;
                    break;
                default:
                    return null;
            }

            return SelectedUnitType.CurrentUnit;
        }
    }
}
