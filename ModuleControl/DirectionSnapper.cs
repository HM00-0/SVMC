using Autodesk.Revit.DB;

namespace SVMC.ModuleControl
{
    public static class DirectionSnapper
    {
        public static double Rotation(XYZ from, XYZ to)
        {
            XYZ dir = to - from;

            XYZ snappedDir;
            if (Math.Abs(dir.X) >= Math.Abs(dir.Y))
            {
                if (dir.X >= 0)
                {
                    snappedDir = new XYZ(1, 0, 0); // East
                }
                else
                {
                    snappedDir = new XYZ(-1, 0, 0); // West
                }
            }
            else
            {
                if (dir.Y >= 0)
                {
                    snappedDir = new XYZ(0, 1, 0); // North
                }
                else
                {
                    snappedDir = new XYZ(0, -1, 0); // South
                }
            }

            XYZ baseDir = new XYZ(1, 0, 0); // 동쪽
            double angle = baseDir.AngleTo(snappedDir);

            double crossZ = baseDir.CrossProduct(snappedDir).Z;
            if (crossZ < 0)
                angle = -angle;

            return angle;
        }
    }
}
