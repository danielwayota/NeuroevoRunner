using System;

namespace CerebroML.Activation
{
    public class Tanh : IActivator
    {
        public float Activate(float input)
        {
            return (float)Math.Tanh(input);
        }
    }
}
