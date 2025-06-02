using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;
using SVMC.Settings;
using static SVMC.Settings.Constants;
using static SVMC.Commands.Module_System;

namespace SVMC.ModuleControl
{
    public static class InstanceCreator
    {
        public static FamilyInstance Create(Document doc, XYZ point, string familyPath, UnitType unit, int expConst, string expDir)
        {
            FamilySymbol symbol = FamilyLoader.LoadOrReuseFamily(doc, familyPath);
            FamilyInstance instance = doc.Create.NewFamilyInstance(point, symbol, StructuralType.NonStructural);

            (double UnitShort, double UnitLong) = UnitTypeParameters.GetParameterValues(unit);

            if (expDir == "Right" || expDir == "Left")
            {
                UnitLong *= expConst;
            }
            else if (expDir == "Up" || expDir == "Down")
            {
                UnitShort *= expConst;
            }

            instance.LookupParameter("Unit_Short")?.Set(UnitShort / MF);
            instance.LookupParameter("Unit_Long")?.Set(UnitLong / MF);

            return instance;
        }
    }
}
