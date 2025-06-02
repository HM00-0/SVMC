using Autodesk.Revit.DB;

namespace SVMC.Settings
{
    public struct InsertInfo
    {
        public XYZ Point { get; }
        public double RotationAngle { get; }

        public InsertInfo(XYZ point, double rotationAngle)
        {
            Point = point;
            RotationAngle = rotationAngle;
        }
    }
}