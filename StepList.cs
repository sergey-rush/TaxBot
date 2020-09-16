using System.Collections.Generic;

namespace TaxBot
{
    public class StepList:List<Step>
    {
        public StepList()
        {
            Add(new Step(StepType.One));
            Add(new Step(StepType.Two));
            Add(new Step(StepType.Three));
        }
    }
}