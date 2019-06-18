namespace CerebroML.Util
{
    public class StaticRandom
    {
        private static System.Random sysrnd;

        static StaticRandom() {
            sysrnd = new System.Random();
        }

        /// =================================================
        /// <summary>
        /// Creates some random number in the range: [0, amplitude)
        /// </summary>
        ///
        /// <param name="amplitude"></param>
        /// <returns></returns>
        public static float Next(float amplitude = 1f)
        {
            return (float)(sysrnd.NextDouble() * amplitude);
        }

        /// <summary>
        /// Creates some random number in the range: (-amplitude, amplitude)
        /// </summary>
        /// <param name="amplitude"></param>
        /// <returns></returns>
        public static float NextBilinear(float amplitude = 1f)
        {
            return (float) ((sysrnd.NextDouble() * 2) - 1) * amplitude;
        }
    }
}