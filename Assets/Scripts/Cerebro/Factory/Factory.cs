using CerebroML.Activation;
using CerebroML.Genetics;
using System.Collections.Generic;

namespace CerebroML.Factory
{
    public class Factory
    {
        private int inputNeurons;

        private List<LayerConfig> layerConfigs;

        private Genome genome;

        private float weightsBiasAmplitude;

        private Factory()
        {
            this.inputNeurons = 0;
            this.layerConfigs = new List<LayerConfig>();
            this.weightsBiasAmplitude = 1f;
        }

        /// =================================================
        /// <summary>
        /// Sets the input neuron count
        /// </summary>
        ///
        /// <param name="inputNeurons"></param>
        /// <returns></returns>
        public Factory WithInput(int inputNeurons)
        {
            this.inputNeurons = inputNeurons;
            return this;
        }

        /// =================================================
        /// <summary>
        /// Adds new layer configuration
        /// </summary>
        ///
        /// <param name="neuronCount"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public Factory WithLayer(int neuronCount, LayerType type)
        {
            this.layerConfigs.Add(new LayerConfig(neuronCount, type));
            return this;
        }

        /// =================================================
        /// <summary>
        /// Sets the result network genome
        /// </summary>
        ///
        /// <param name="genome"></param>
        /// <returns></returns>
        public Factory WithGenome(Genome genome)
        {
            this.genome = genome;
            return this;
        }

        public Factory WithWeightBiasAmplitude(float amplitude)
        {
            this.weightsBiasAmplitude = amplitude;
            return this;
        }

        /// =================================================
        /// <summary>
        /// Creates the neural network with the given configuration
        /// </summary>
        ///
        /// <returns></returns>
        public Cerebro Build()
        {
            if (this.inputNeurons <= 0)
            {
                throw new System.InvalidOperationException(
                    $"The input neuron count is invalid: {this.inputNeurons}"
                );
            }

            if (this.layerConfigs.Count == 0)
            {
                throw new System.InvalidOperationException("The layer configuration is empty");
            }

            Layer[] layers = new Layer[this.layerConfigs.Count];
            int lastNeuronCount = this.inputNeurons;

            for (int i = 0; i < this.layerConfigs.Count; i++)
            {
                LayerConfig config = this.layerConfigs[i];

                IActivator activator;
                switch (config.type)
                {
                    case LayerType.Sine:
                        activator = new Sine();
                        break;
                    case LayerType.Tanh:
                        activator = new Tanh();
                        break;
                    case LayerType.Sigmoid:
                    default:
                        activator = new Sigmoid();
                        break;
                }

                layers[i] = new Layer(lastNeuronCount, config.neuronCount, activator);
                lastNeuronCount = config.neuronCount;
            }

            Cerebro net = new Cerebro(layers);

            if (this.genome != null)
            {
                net.SetGenome(this.genome);
            }
            else
            {
                net.Initialize(this.weightsBiasAmplitude);
            }

            return net;
        }

        // Statics

        public static Factory Create()
        {
            return new Factory();
        }
    }
}
