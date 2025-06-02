using Autodesk.Revit.DB;

namespace SVMC.ModuleControl
{
    public static class FamilyLoader
    {

        public static string GetFamilyPath(string name)
        {
            return Path.Combine(@"C:\Users\hmryu\SVMC\SVMC\bin\Debug\net8.0\ModuleDB\", $"{name}.rfa");
        }


        public static FamilySymbol LoadOrReuseFamily(Document doc, string path)
        {
            Family family;

            if (!doc.LoadFamily(path, out family))
            {
                string familyName = Path.GetFileNameWithoutExtension(path);
                family = new FilteredElementCollector(doc)
                    .OfClass(typeof(Family))
                    .Cast<Family>()
                    .FirstOrDefault(f => f.Name == familyName);

                if (family == null)
                    throw new Exception($"패밀리 '{familyName}' 를 찾을 수 없습니다.");
            }

            var symbolId = family.GetFamilySymbolIds().FirstOrDefault();
            if (symbolId == ElementId.InvalidElementId)
                throw new Exception("FamilySymbol 없음");

            FamilySymbol symbol = doc.GetElement(symbolId) as FamilySymbol;

            if (!symbol.IsActive)
            {
                symbol.Activate();
                doc.Regenerate();
            }

            return symbol;
        }
    }
}