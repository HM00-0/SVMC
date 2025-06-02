using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using static SVMC.Settings.Constants;

namespace SVMC.Structure
{
    public class PanelContext
    {
        public WallType WallType { get; set; }
        public Level Level { get; set; }
        public double Height { get; set; }
        public XYZ Direction { get; set; }
        public XYZ Start { get; set; }
        public double TotalLength { get; set; }
    }

    public class InsertedPanels
    {
        public string WallTypeName { get; set; }
        public string LevelName { get; set; }
        public double Height { get; set; }
        public XYZWrapper StartPoint { get; set; }
        public XYZWrapper EndPoint { get; set; }
        public int PanelCount { get; set; }
        public List<double> SegmentLengths { get; set; }
        public double TotalLength { get; set; }
    }

}
