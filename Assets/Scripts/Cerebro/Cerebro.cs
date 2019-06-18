using System;
using System.Collections.Generic;
using CerebroML.Genetics;

namespace CerebroML
{
    public class Cerebro
    {
        public Layer[] Layers
        {
            get; protected set;
        }

        public int GenomeSize
        {
            get {
                int sum = 0;
                foreach (var layer in this.Layers)
                {
                    sum += layer.GenomeSize;
                }
                return sum;
            }
        }

        /// =================================================
        /// <summary>
        /// Default Neural network constructor
        /// </summary>
        ///
        /// <param name="layers">The layers array</param>
        public Cerebro(Layer[] layers)
        {
            this.Layers = layers;
        }

        /// =================================================
        /// <summary>
        /// Calls Initialize() on the layers
        /// </summary>
        /// <param name="amplitude">The amplitude used to generate the weights and biases. Range: (-amplitude, amplitude)</param>
        public void Initialize(float amplitude)
        {
            foreach(var layer in this.Layers)
            {
                layer.Initialize(amplitude);
            }
        }

        /// =================================================
        /// <summary>
        /// Executes the feedfoward of the network
        /// </summary>
        ///
        /// <param name="input">The array with the input values</param>
        ///
        /// <returns>The result produced by the network</returns>
        public float[] Run(float[] input)
        {
            Matrix result = Matrix.From1DColum(input);

            foreach (var layer in this.Layers)
            {
                result = layer.Activate(result);
            }

            return result.Values;
        }

        /// =================================================
        /// <summary>
        /// Uses the given genome to modify the weigts and bias of the network
        /// </summary>
        ///
        /// <param name="genome">Genome class instance</param>
        public void SetGenome(Genome genome)
        {
            if (this.GenomeSize != genome.Size)
            {
                throw new InvalidOperationException(
                    $"The Network and the new genome are incompatible. Sizes {this.GenomeSize} != {genome.Size}"
                );
            }

            int index = 0;
            foreach (var layer in this.Layers)
            {
                float[] genomePart = new float[layer.GenomeSize];

                for (int i = 0; i < layer.GenomeSize; i++)
                {
                    genomePart[i] = genome.Genes[index + i];
                }

                layer.SetGenes(genomePart);

                index += layer.GenomeSize;
            }
        }

        /// =================================================
        /// <summary>
        /// Creates a genome object with the weights of the network
        /// </summary>
        ///
        /// <returns>The genome</returns>
        public Genome GetGenome()
        {
            List<float> genes = new List<float>();

            foreach (var layer in this.Layers)
            {
                genes.AddRange(layer.GetGenes());
            }

            return new Genome(genes.ToArray());
        }
    }
}
