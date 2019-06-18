using System;
using CerebroML.Util;

namespace CerebroML.Genetics
{
    public class Genome
    {
        public float[] Genes
        {
            get; protected set;
        }

        public int Size
        {
            get {
                return this.Genes.Length;
            }
        }

        /// =================================================
        /// <summary>
        /// Default constructor
        /// </summary>
        ///
        /// <param name="genes"></param>
        public Genome(float[] genes)
        {
            this.Genes = genes;
        }

        /// =================================================
        /// <summary>
        /// Randomly changes some gene
        /// </summary>
        ///
        /// <param name="chance"></param>
        /// <param name="amplitude">The amplitude used to generate the mutation. Range: (-amplitude, amplitude)</param>
        public void Mutate(float chance, float amplitude = 1f)
        {
            for (int i = 0; i < this.Genes.Length; i++)
            {
                float dice = StaticRandom.Next();

                if (dice < chance)
                {
                    this.Genes[i] = StaticRandom.NextBilinear(amplitude);
                }
            }
        }

        /// =================================================
        /// <summary>
        /// Performs the crossover and produces an offspring
        /// </summary>
        ///
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Genome Crossover(Genome a, Genome b)
        {
            float[] offspringGenes = new float[a.Genes.Length];

            for (int i = 0; i < a.Genes.Length; i++)
            {
                offspringGenes[i] = i % 2 == 0 ? a.Genes[i] : b.Genes[i];
            }

            return new Genome(offspringGenes);
        }

        /// =================================================
        /// <summary>
        /// Extracts some chunk from the full genome
        /// </summary>
        ///
        /// <param name="genes"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static float[] Slice(float[] genes, int start, int end)
        {
            int count = end - start;
            float[] slice = new float[count];

            for (int i = 0; i < count; i++)
            {
                slice[i] = genes[start + i];
            }

            return slice;
        }

        /// =================================================
        /// <summary>
        /// Concatenates two 1D float arrays
        /// </summary>
        ///
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float[] Concatenate(float[] a, float[] b)
        {
            float[] c = new float[a.Length + b.Length];

            Array.Copy(a, c, a.Length);
            Array.Copy(b, 0, c, a.Length, b.Length);

            return c;
        }
    }
}