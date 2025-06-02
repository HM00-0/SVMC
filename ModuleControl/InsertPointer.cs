using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using SVMC.Settings;

namespace SVMC.ModuleControl
{
    public static class InsertPointer
    {
        public static XYZ GetClickPoint(UIDocument uidoc)
        {
            Selection sel = uidoc.Selection;
            return sel.PickPoint("삽입 위치 클릭");
        }

        public static InsertInfo GetInsertInfo(UIDocument uidoc)
        {
            Selection sel = uidoc.Selection;

            XYZ basePoint = sel.PickPoint("모듈의 기준점을 선택하세요.");
            XYZ directionPoint = sel.PickPoint("모듈이 바라보는 방향을 선택하세요.");

            double angleRad = DirectionSnapper.Rotation(basePoint, directionPoint);
            return new InsertInfo(basePoint, angleRad);
        }
    }
}
