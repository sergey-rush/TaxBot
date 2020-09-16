using System.Threading;

namespace TaxBot
{
    public class StepTwo
    {
        public Output Run(Step step)
        {
            Thread.Sleep(3000);
            Output output = new Output();
            output.Message = "Step two completed";
            return output;
        }
    }
}