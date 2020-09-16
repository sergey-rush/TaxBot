using System.Threading;

namespace TaxBot
{
    public class StepThree
    {
        public Output Run(Step step)
        {
            Thread.Sleep(3000);
            Output output = new Output();
            output.Message = "Step three completed";
            return output;
        }
    }
}