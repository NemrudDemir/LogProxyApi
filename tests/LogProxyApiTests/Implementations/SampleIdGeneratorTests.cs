using FluentAssertions;
using LogProxyApi.Implementations;
using NUnit.Framework;

namespace LogProxyApiTests.Implementations
{
    public class SampleIdGeneratorTests
    {
        [Test]
        public void GetIdShouldNotBeNullOrEmpty()
        {
            var classUnderTest = new SampleIdGenerator();
            classUnderTest.GetId().Should().NotBeNullOrEmpty();
        }
    }
}
