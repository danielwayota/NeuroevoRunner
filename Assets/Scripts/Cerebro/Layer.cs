using System;
using CerebroML.Activation;
using CerebroML.Genetics;

namespace CerebroML
{
    public class Layer
    {
        public int GenomeSize
        {
            get {
                return this.weights.LinearSize + this.bias.LinearSize;
            }
        }

        private Matrix weights;
        private Matrix bias;

        private IActivator activator;

        /// =================================================
        /// <summary>
        /// Default layer constructor
        /// </summary>
        ///
        /// <param name="lastNeurons">The neuron count of the previous layer</param>
        /// <param name="neuronCount">Current layer neuron count</param>
        /// <param name="activator"></param>
        public Layer(int lastNeurons, int neuronCount, IActivator activator)
        {
            this.weights = new Matrix(neuronCount, lastNeurons);
            this.bias = new Matrix(neuronCount, 1);

            this.activator = activator;
        }

        /// =================================================
        /// <summary>
        /// Randomizes the weights and biases
        /// </summary>
        ///
        /// <param name="amplitude">The amplitude used to generate the weights and biases. Range: (-amplitude, amplitude)</param>
        public void Initialize(float amplitude)
        {
            this.weights.Randomize(amplitude);
            this.bias.Randomize(amplitude);
        }

        /// =================================================
        /// <summary>
        /// Performs the feedforward of this layer and returns the result
        /// </summary>
        ///
        /// <param name="input"></param>
        /// <returns></returns>
        public Matrix Activate(Matrix input)
        {
            Matrix result = Matrix.Product(weights, input);
            result.Add(bias);

            result.Map(this.activator.Activate);

            return result;
        }

        /// =================================================
        /// <summary>
        /// Uses some 1D float array to set the weight and bias values
        /// </summary>
        ///
        /// <param name="genes"></param>
        public void SetGenes(float[] genes)
        {
            if (this.GenomeSize != genes.Length)
            {
                throw new InvalidOperationException(
                    $"The Layer and the new genome are incompatible. Sizes {this.GenomeSize} != {genes.Length}"
                );
            }
            float[] w = Genome.Slice(genes, 0, this.weights.LinearSize);
            float[] b = Genome.Slice(genes, this.weights.LinearSize, this.weights.LinearSize + this.bias.LinearSize);

            this.weights.Set(w);
            this.bias.Set(b);
        }

        /// =================================================
        /// <summary>
        /// Returns some 1D float array with the weight and bias
        /// </summary>
        /// <returns></returns>
        public float[] GetGenes()
        {
            float[] fullValues = Genome.Concatenate(this.weights.Values, this.bias.Values);

            return fullValues;
        }
    }
}
