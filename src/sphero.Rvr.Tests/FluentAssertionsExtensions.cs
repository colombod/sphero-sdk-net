using System.Collections.Generic;
using FluentAssertions;
using FluentAssertions.Collections;

namespace sphero.Rvr.Tests;

public static class FluentAssertionsExtensions
{
    public static AndConstraint<GenericCollectionAssertions<byte[]>> BeEquivalentMessageSetTo(this GenericCollectionAssertions<byte[]> assertion, byte[][] expectation)
    {
        return assertion.BeEquivalentTo(expectation, config: options => options.ComparingByValue<byte[]>().Using(new RawMessageComparer()));
    }
    private class RawMessageComparer : IEqualityComparer<byte[]>
    {
        public bool Equals(byte[]? x, byte[]? y)
        {
            var equal = x.Length == y.Length;
            var shouldMatchChecksum = true;
            var checksumIndices = new HashSet<int>
            {
                x.Length - 2,
                x.Length - 3
            };
            if (equal)
            {
                for (var i = 0; i < x.Length; i++)
                {
                    if (i != 6 && !checksumIndices.Contains(i))
                    {
                        equal = x[i] == y[i];
                    }
                    else if (i == 6)
                    {
                        shouldMatchChecksum = x[i] == y[i];
                    }
                    else if (checksumIndices.Contains(i) && shouldMatchChecksum)
                    {
                        equal = x[i] == y[i];
                    }

                    if (!equal)
                    {
                        break;
                    }
                }
            }

            return equal;
        }

        public int GetHashCode(byte[] obj)
        {
            return obj.GetHashCode();
        }
    }
}