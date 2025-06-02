using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace SVMC.ModuleControl
{
    public static class UnitEditor
    {
        public static (int, string)? Expansion()
        {
            TaskDialog td = new TaskDialog("모듈 확장");
            td.MainInstruction = "모듈 확장 방향을 선택하세요";
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "우");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "좌");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink3, "상");
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink4, "하");

            td.CommonButtons = TaskDialogCommonButtons.Close; // No expansion = 닫기

            TaskDialogResult result = td.Show();

            switch (result)
            {
                case TaskDialogResult.CommandLink1:
                    return (2, "Right");
                case TaskDialogResult.CommandLink2:
                    return (2, "Left");
                case TaskDialogResult.CommandLink3:
                    return (2, "Up");
                case TaskDialogResult.CommandLink4:
                    return (2, "Down");
                case TaskDialogResult.Close:
                    return (1, "No expansion");
                default:
                    return null;
            }
        }
    }
}
