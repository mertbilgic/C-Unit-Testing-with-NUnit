using System.Collections;
using NUnit.Framework;

namespace Loans.Test
{
    public class MonthlyRepaymentTestData
    {
        public static IEnumerable TestCase
        {
            get
            {
                yield return new TestCaseData(200_000m,6.5m,30,1264.14m);
                yield return new TestCaseData(500_000m,10m,30,4387.86m);

            }
        }
    }
}