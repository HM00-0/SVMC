using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVMC.PanelControl
{
    public interface ISegmentOptimizer
    {
        List<double> GetSegmentLengths(double totalLength);
    }

    public class FixedLengthOptimizer : ISegmentOptimizer
    {
        private readonly double _unitLength;

        public FixedLengthOptimizer(double unitLength)
        {
            _unitLength = unitLength;
        }

        public List<double> GetSegmentLengths(double totalLength)
        {
            var segments = new List<double>();
            double offset = 0;

            while (offset < totalLength)
            {
                double segLength = Math.Min(_unitLength, totalLength - offset);
                segments.Add(segLength);
                offset += segLength;
            }

            return segments;
        }
    }

    public class SmartSegmentOptimizer : ISegmentOptimizer
    {
        private readonly double _segmentA;  // 예: 1800
        private readonly double _segmentB;  // 예: 1500

        public SmartSegmentOptimizer(double segmentA, double segmentB)
        {
            _segmentA = segmentA;
            _segmentB = segmentB;
        }

        public List<double> GetSegmentLengths(double totalLength)
        {
            int bestX = 0, bestY = 0;
            double bestRemainder = double.MaxValue;
            int bestCount = int.MaxValue;

            int maxX = (int)(totalLength / _segmentA);

            for (int x = 0; x <= maxX; x++)
            {
                double usedA = x * _segmentA;
                double remainingAfterA = totalLength - usedA;

                int y = (int)(remainingAfterA / _segmentB);
                for (; y >= 0; y--)
                {
                    double usedB = y * _segmentB;
                    double totalUsed = usedA + usedB;

                    if (totalUsed > totalLength) continue;

                    double remainder = totalLength - totalUsed;
                    int count = x + y;

                    if (remainder < bestRemainder ||
                       (Math.Abs(remainder - bestRemainder) < 1e-6 && count < bestCount))
                    {
                        bestX = x;
                        bestY = y;
                        bestRemainder = remainder;
                        bestCount = count;
                    }
                }
            }

            var segments = new List<double>();
            for (int i = 0; i < bestX; i++) segments.Add(_segmentA);
            for (int i = 0; i < bestY; i++) segments.Add(_segmentB);

            return segments;
        }
    }
}
