using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace AccountAPI.Tests.T1.Framework.Builders;

public class MemoryCacheMockBuilder
{
    private IMemoryCache _memoryCache;

    public MemoryCacheMockBuilder()
    {
        _memoryCache = Substitute.For<IMemoryCache>()!;
    }

    public MemoryCacheMockBuilder WithTryGetValue(string key, object value, bool returns = true)
    {
        _memoryCache.TryGetValue(key, out Arg.Any<object?>()!).Returns(call =>
{
    call[1] = value;
    return returns;
});
        return this;
    }

    public IMemoryCache Build()
    {
        return _memoryCache;
    }
}
