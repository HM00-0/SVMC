using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;

namespace SVMC.PanelControl
{
    public class LineSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem) =>
            elem is CurveElement;

        public bool AllowReference(Reference reference, XYZ position) => true;
    }


}
