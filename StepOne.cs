using System.Threading;

namespace TaxBot
{
    public class StepOne
    {
        public Output Run(Step step)
        {
            Thread.Sleep(3000);
            Output output = new Output();
            output.Message = "Step one completed";
            return output;
        }
    }
}