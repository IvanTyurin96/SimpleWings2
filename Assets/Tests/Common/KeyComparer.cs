using System;
using System.Collections.Generic;

namespace Assets.Tests.Common
{
    /// <summary>
    /// Comparer for keys of animation curve.
    /// </summary>
    public class KeyComparer : IEqualityComparer<float>
    {
        private decimal _delta { get; }

        public KeyComparer(decimal delta)
        {
            _delta = delta;
        }

        public bool Equals(float x, float y)
        {
            return MathF.Abs(x - y) <= (float)_delta;
        }

        public int GetHashCode(float obj)
        {
            return 0;
        }
    }
}
