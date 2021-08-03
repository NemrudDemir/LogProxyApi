using LogProxyApi.Abstractions;
using System;

namespace LogProxyApi.Implementations
{
    public class SampleIdGenerator : IIdGenerator
    {
        public string GetId() => DateTime.Now.Ticks.ToString();
    }
}
