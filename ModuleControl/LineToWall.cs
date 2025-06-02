using System.Text;
using Autodesk.Revit.DB;
using static SVMC.Settings.Constants;

namespace SVMC.ModuleControl
{
    public static class LineToWall
    {
        private static void SetWallCenter(Wall wall)
        {
            Parameter param = wall.get_Parameter(BuiltInParameter.WALL_KEY_REF_PARAM);
            param?.Set((int)WallLocationLine.WallCenterline);

        }

        public static string CreateWalls(Document doc, FamilyInstance instance)
        {
            Options options = new Options { IncludeNonVisibleObjects = true };
            GeometryElement geomElement = instance.get_Geometry(options);

            if (geomElement == null)
                throw new Exception("Geometry를 가져올 수 없습니다.");

            // 레벨
            Level level = new FilteredElementCollector(doc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .FirstOrDefault();

            if (level == null)
                throw new Exception("레벨이 없습니다.");

            // 벽체 타입
            WallType wallType = new FilteredElementCollector(doc)
                .OfClass(typeof(WallType))
                .Cast<WallType>()
                .FirstOrDefault(w => w.Kind == WallKind.Basic);

            if (wallType == null)
                throw new Exception("벽체 타입을 찾을 수 없습니다.");

            StringBuilder sb = new StringBuilder();
            int lineCount = 0;

            foreach (GeometryObject geomObj in geomElement)
            {

                if (geomObj is GeometryInstance geomInst)
                {
                    Transform symbolTransform = geomInst.Transform;

                    GeometryElement instGeom = geomInst.GetSymbolGeometry();
                    foreach (GeometryObject instObj in instGeom)
                    {
                        if (instObj is Curve nestedCurve)
                        {
                            Curve worldCurve = nestedCurve.CreateTransformed(symbolTransform);

                            XYZ p1 = worldCurve.GetEndPoint(0);
                            XYZ p2 = worldCurve.GetEndPoint(1);

                            sb.AppendLine($"▶ Curve {++lineCount}");
                            //sb.AppendLine($"  Start: ({p1.X:F2}, {p1.Y:F2}, {p1.Z:F2})");
                            //sb.AppendLine($"  End  : ({p2.X:F2}, {p2.Y:F2}, {p2.Z:F2})\n");

                            if (Math.Abs(p1.Z - p2.Z) > 0.001)
                            {
                                throw new Exception($"⚠ 경사진 Curve는 벽체로 만들 수 없습니다.\nZ1: {p1.Z:F3}, Z2: {p2.Z:F3}");
                            }
                            else
                            {
                                double z = p1.Z;
                                if (z < 0)
                                {
                                    throw new Exception("벽체 높이 오류");
                                }
                                else if (z == 0)
                                {
                                    Wall wall = Wall.Create(doc, worldCurve, wallType.Id, level.Id, 3000.0 / MF, 0.0, false, false);
                                    SetWallCenter(wall);
                                }
                                else
                                {
                                    Wall wallUnder = Wall.Create(doc, worldCurve, wallType.Id, level.Id, z, 0.0, false, false);
                                    SetWallCenter(wallUnder);

                                    Wall wallUpper = Wall.Create(doc, worldCurve, wallType.Id, level.Id, 600 / MF, 2400 / MF, false, false);
                                    SetWallCenter(wallUpper);
                                }
                            }
                        }
                    }
                }
            }

            if (lineCount == 0)
            {
                return "⚠️ 이 패밀리에서는 모델 선을 찾을 수 없습니다.";
            }

            //return sb.ToString();
            return $"모듈이 생성되었습니다. (벽체 {lineCount}개)";
        }
    }

}
