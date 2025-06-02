using System;
using System.Linq;
using System.Reflection.Emit;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using SVMC.PanelControl;
using SVMC.Settings;
using SVMC.Structure;
using static SVMC.Settings.Constants;

namespace SVMC.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class Panel_System : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            string dateStamp = DateTime.Now.ToString("MMdd");
            string path = $@"C:\Users\hmryu\SVMC\SVMC\InsertedPanels\panel_log_{dateStamp}.json";

            UnitType? selectedUnit = SelectedTypes.SelectedUnit;
            if (selectedUnit == null)
            {
                TaskDialog.Show("오류", "모듈이 먼저 삽입되지 않았거나, Unit 정보를 찾을 수 없습니다.");
                return Result.Failed;
            }
            (double UnitShort, double UnitLong) = UnitTypeParameters.GetParameterValues(selectedUnit.Value);

            try
            {
                // 1. 사용자에게 선 선택 요청
                IList<Reference> pickedLines = uidoc.Selection.PickObjects(ObjectType.Element, new LineSelectionFilter(), "벽 기준이 될 선을 모두 선택한 후, '완료'를 누르세요.");
                if (pickedLines == null || pickedLines.Count == 0)
                {
                    TaskDialog.Show("취소됨", "선택이 취소되었습니다.");
                    return Result.Cancelled;
                }

                foreach (Reference pickedLine in pickedLines)
                {
                    Element element = doc.GetElement(pickedLine);
                    CurveElement curveElem = element as CurveElement;
                    if (curveElem == null)
                    {
                        TaskDialog.Show("오류", "선택한 요소가 선(Curve)이 아닙니다.");
                        return Result.Failed;
                    }

                    Line line = curveElem.GeometryCurve as Line;
                    if (line == null)
                    {
                        TaskDialog.Show("오류", "선택한 곡선이 직선(Line)이 아닙니다.");
                        return Result.Failed;
                    }
                }

                PanelContext panel;
                try
                {
                    panel = LineToPanel.GetPanelContext(doc);
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("오류", $"패널 생성 중 오류 발생: {ex.Message}");
                    return Result.Failed;
                }

                //ISegmentOptimizer optimizer = new FixedLengthOptimizer(1800.0 / MF);
                ISegmentOptimizer optimizer = new SmartSegmentOptimizer(UnitShort / MF, UnitLong / MF);
                

                // 4. 트랜잭션 내에서 벽 생성
                using (Transaction tx = new Transaction(doc, "벽 생성 및 분할"))
                {
                    tx.Start();

                    foreach (Reference pickedLine in pickedLines)
                    {
                        Element element = doc.GetElement(pickedLine);
                        if (element is CurveElement curveElem && curveElem.GeometryCurve is Line line)
                        {
                            double totalLength = line.Length;
                            List<double> segLengths = optimizer.GetSegmentLengths(totalLength);
                            PanelCreator.CreateWalls(doc, line, panel.WallType, panel.Level, panel.Height, segLengths);

                            var panelLog = new InsertedPanels
                            {
                                WallTypeName = panel.WallType.Name,
                                LevelName = panel.Level.Name,
                                Height = panel.Height * MF,
                                StartPoint = new XYZWrapper(line.GetEndPoint(0)),
                                EndPoint = new XYZWrapper(line.GetEndPoint(1)),
                                PanelCount = segLengths.Count,
                                SegmentLengths = segLengths.Select(length => Math.Round(length * MF, 2)).ToList(),
                                TotalLength = Math.Round(totalLength * MF, 2)
                            };


                            PanelLogger.Save(panelLog, path);
                        }
                    }

                    tx.Commit();
                }

                TaskDialog.Show("성공", "벽이 생성되었습니다.");
                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                return Result.Cancelled;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("예외 발생", $"에러 메시지: {ex.Message}");
                return Result.Failed;
            }


        }
    }
}
