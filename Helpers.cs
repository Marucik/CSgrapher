using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public static class Helpers
    {
        public static float NextFloatRange(this Random random, float minimum, float maximum)
        {
            return (float)random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
