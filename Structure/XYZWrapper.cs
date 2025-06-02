using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace SVMC.Structure
{
    public class XYZWrapper
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public XYZWrapper() { }

        public XYZWrapper(XYZ xyz)
        {
            x = Math.Round(xyz.X, 2);
            y = Math.Round(xyz.Y, 2);
            z = Math.Round(xyz.Z, 2);
        }

        public XYZ ToXYZ()
        {
            return new XYZ(x, y, z);
        }
    }
}
