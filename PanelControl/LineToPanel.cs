using Autodesk.Revit.DB;
using SVMC.Structure;
using static SVMC.Settings.Constants;

namespace SVMC.PanelControl
{
    public static class LineToPanel
    {
        public static PanelContext GetPanelContext(Document doc)
        {
            WallType wallType = new FilteredElementCollector(doc)
         .OfClass(typeof(WallType))
         .Cast<WallType>()
         .FirstOrDefault(w => w.Kind == WallKind.Basic);
            if (wallType == null)
                throw new Exception("기본 벽 타입을 찾을 수 없습니다.");

            Level level = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault();
            if (level == null)
                throw new Exception("레벨(Level)을 찾을 수 없습니다.");

            double height = 3000.0 / MF;

            return new PanelContext
            {
                WallType = wallType,
                Level = level,
                Height = height,
                // Direction, Start, Length 는 선마다 따로 처리
            };
        }
    }
}
