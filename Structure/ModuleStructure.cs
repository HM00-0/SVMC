 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace SVMC.Structure
{
    public class InsertedModules
    {
        public string UnitType { get; set; }
        public string ModuleType { get; set; }
        public int ExpansionConstant { get; set; }
        public string ExpansionDirection { get; set; }
        public XYZWrapper InsertPoint { get; set; }
        public double RotationAngle { get; set; }
    }

    
}
