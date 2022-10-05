using ESAPIX.Constraints;
using ESAPIX.Extensions;
using System;
using VMS.TPS.Common.Model.API;

namespace ESAPX_StarterUI.ViewModels
{
    public class CTDateConstraint : IConstraint
    {
        public string Name => "CT Date Checker";
        public string FullName => "CT Date < 60 Days";

        public ConstraintResult CanConstrain(PlanningItem pi)
        {
            var pq = new PQAsserter(pi);
            return pq.HasImage()
                .CumulativeResult;
        }


        public ConstraintResult Constrain(PlanningItem pi)
        {
            var image = pi.GetImage();
            return ConstraintCTDate(image.CreationDateTime);
        }

        public ConstraintResult ConstraintCTDate(DateTime? creationDate)
        {
            var diffDays = (DateTime.Now - creationDate).Value.TotalDays;
            var msg = $"CT is {diffDays} days old";

            if (diffDays <= 60)
            {
                return new ConstraintResult(this, ResultType.PASSED, msg);
            }
            else
            {
                return new ConstraintResult(this, ResultType.ACTION_LEVEL_3, msg);
            }
        }
    }
}