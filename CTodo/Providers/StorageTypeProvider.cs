using CTodo.Options;
using Microsoft.Extensions.Options;

namespace CTodo.Providers;

public class StorageTypeProvider
{
    private readonly IOptionsMonitor<StorageOptions> _optionsMonitor;
    private readonly IOptionsMonitorCache<StorageOptions> _optionsCache;

    public StorageTypeProvider(IOptionsMonitor<StorageOptions> optionsMonitor,
        IOptionsMonitorCache<StorageOptions> optionsCache)
    {
        _optionsMonitor = optionsMonitor;
        _optionsCache = optionsCache;
    }

    public void UpdateStorageType(string storageType)
    {
        var options = _optionsMonitor.CurrentValue;
        options.StorageType = storageType;
        _optionsCache.TryRemove("Microsoft.Extensions.Options.IOptions`1[StorageOptions]");
        _optionsCache.TryAdd("Microsoft.Extensions.Options.IOptions`1[StorageOptions]", options);
    }
}