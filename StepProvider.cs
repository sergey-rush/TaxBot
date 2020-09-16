using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TaxBot
{
    public class StepProvider
    {
        private static StepProvider current;
        public static StepProvider Current
        {
            get
            {
                if (current == null)
                {
                    current = new StepProvider();
                }
                return current;
            }
        }

        public Dictionary<StepType, Func<Step, Output>> steps = new Dictionary<StepType, Func<Step, Output>>();

        public StepProvider()
        {
            if (steps.Count == 0)
            {
                steps.Add(StepType.One, DoStepOne);
                steps.Add(StepType.Two, DoStepTwo);
                steps.Add(StepType.Three, DoStepOne);
            }
        }

        public Output StepInvoke(Step step)
        {
            return steps[step.StepType].Invoke(step);
        }

        private Output DoStepOne(Step step)
        {
            StepOne stepOne = new StepOne();
            return stepOne.Run(step);
        }

        private Output DoStepTwo(Step step)
        {
            StepTwo stepTwo = new StepTwo();
            return stepTwo.Run(step);
        }

        private Output DoStepThree(Step step)
        {
            StepThree stepThree = new StepThree();
            return stepThree.Run(step);
        }
    }
}