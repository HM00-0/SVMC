namespace SVMC.Settings
{
    public enum UnitType
    {
        UnitA,
        UnitB,
        UnitC
    }


    public static class UnitTypeParameters
    {
        public static (double UnitShort, double UnitLong) GetParameterValues(UnitType unit)
        {
            return unit switch
            {
                UnitType.UnitA => (1200.0, 1500.0),
                UnitType.UnitB => (1500.0, 2100.0),
                UnitType.UnitC => (1200.0, 1800.0),
                _ => throw new ArgumentException("정의되지 않은 유닛 타입입니다.")
            };
        }
    }

    public static class SelectedUnitType
    {
        public static UnitType? CurrentUnit { get; set; } = null;
    }
}