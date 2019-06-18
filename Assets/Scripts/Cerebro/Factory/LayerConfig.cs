namespace CerebroML.Factory
{
    public struct LayerConfig
    {
        public int neuronCount;
        public LayerType type;

        public LayerConfig(int nCount, LayerType layerType)
        {
            this.neuronCount = nCount;
            this.type = layerType;
        }
    }
}
