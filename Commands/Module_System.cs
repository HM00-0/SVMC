using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using SVMC.Settings;
using SVMC.Structure;
using SVMC.ModuleControl;

namespace SVMC.Commands
{
    [Transaction(TransactionMode.Manual)]
    public static class Module_System
    {
        public static Result Run(ExternalCommandData commandData, ModuleType module, ref string message)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            int expConst = 1;
            string expDir = "None";

            var selectedUnit = UnitSelector.SelectUnit();
            if (selectedUnit == null)
            {
                TaskDialog.Show("취소됨", "Unit을 선택하지 않았습니다.");
                return Result.Cancelled;
            }
            SelectedTypes.SelectedUnit = selectedUnit;


            string selectedModule = module.ToString();
            string familyPath = FamilyLoader.GetFamilyPath(selectedModule);

            var expanstionDir = UnitEditor.Expansion();
            if (expanstionDir.HasValue)
            {
                expConst = expanstionDir.Value.Item1;
                expDir = expanstionDir.Value.Item2;

                TaskDialog.Show("확장 방향 선택됨", $"코드: {expConst}\n이름: {expDir}");
            }
            else
            {
                TaskDialog.Show("선택 없음", "확장 방향이 선택되지 않았습니다.");
            }


            //모듈 삽입 위치 및 방향 선택
            InsertInfo insertInfo;
            try
            {
                insertInfo = InsertPointer.GetInsertInfo(uidoc);
            }
            catch
            {
                TaskDialog.Show("취소됨", "위치를 선택하지 않았습니다.");
                return Result.Cancelled;
            }

            using (Transaction tx = new Transaction(doc, $"Insert {selectedUnit} Unit"))
            {
                tx.Start();

                try
                {
                    //모듈 삽입
                    FamilyInstance instance = InstanceCreator.Create(doc, insertInfo.Point, familyPath, selectedUnit.Value, expConst, expDir);
                    doc.Regenerate();

                    //모듈 회전
                    Line axis = Line.CreateBound(insertInfo.Point, insertInfo.Point + new XYZ(0, 0, 1));
                    ElementTransformUtils.RotateElement(doc, instance.Id, axis, insertInfo.RotationAngle);


                    //벽체 생성
                    string summary = LineToWall.CreateWalls(doc, instance);
                    TaskDialog.Show("모듈 삽입", summary);
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    tx.RollBack();
                    return Result.Failed;
                }

                tx.Commit();
            }

            var metadata = new InsertedModules
            {
                UnitType = selectedUnit.ToString(),
                ModuleType = selectedModule,
                ExpansionConstant = expConst,
                ExpansionDirection = expDir,
                InsertPoint = new XYZWrapper(insertInfo.Point),
                RotationAngle = insertInfo.RotationAngle
            };

            string dateStamp = DateTime.Now.ToString("MMdd");
            ModuleLogger.Save(metadata, $@"C:\Users\hmryu\SVMC\SVMC\InsertedModules\insert_log_{dateStamp}.json");

            return Result.Succeeded;

        }
    }
}
