namespace TaxBot
{
    public class Step
    {
        public StepType StepType { get; set; }

        public Step(StepType stepType)
        {
            StepType = stepType;
        }
    }
}