using FluentAssertions;
using FluentAssertions.Collections;

namespace sphero.Rvr.Tests
{
    public static class FluentAssertionsExtensions
    {
        public static AndConstraint<GenericCollectionAssertions<byte[]>> BeEquivalentMessageSetTo(this GenericCollectionAssertions<byte[]> assertion, byte[][] expectation)
        {
            return assertion.BeEquivalentTo(expectation, config: options => options.ComparingByValue<byte[]>());
        }
    }

}
