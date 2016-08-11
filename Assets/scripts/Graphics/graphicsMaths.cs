using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meg.Maths
{
    class graphicsMaths
    {
        public static float remapValue(float value, float inMin, float inMax, float outMin, float outMax)
        {
            //= (X-A)/(B-A) * (D-C) + C
            return ((value - inMin) / (inMax - inMin) * (outMax - outMin)) + outMin;
        }
    }
}
