using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    /// <summary>
    /// Class which contains extension methods for <see cref="System.Random"/>.
    /// </summary>
    public static class Random
    {
        /// <summary>
        /// Extension method which adds possibility to return float range between given numbers.
        /// </summary>
        /// <param name="random"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <returns>float numbert from between given range.</returns>
        public static float NextFloatRange(this System.Random random, float minimum, float maximum)
        {
            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
