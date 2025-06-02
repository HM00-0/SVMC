using Autodesk.Revit.DB;
using SVMC.Structure;

namespace SVMC.PanelControl
{
    public static class PanelCreator
    {
        public static void CreateWalls(Document doc, Line line, WallType wallType, Level level, double height, List<double> segmentLengths)
        {
            XYZ start = line.GetEndPoint(0);
            XYZ dir = (line.GetEndPoint(1) - start).Normalize();

            double offset = 0;
            foreach (var len in segmentLengths)
            {
                XYZ p1 = start + dir.Multiply(offset);
                XYZ p2 = start + dir.Multiply(offset + len);
                Line segment = Line.CreateBound(p1, p2);

                Wall.Create(doc, segment, wallType.Id, level.Id, height, 0, false, false);

                offset += len;
            }
        }
    }
}
