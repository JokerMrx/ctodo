using CTodo.Providers;
using Microsoft.AspNetCore.Mvc;

namespace Ctodo.Controllers;

public class ConfigController : Controller
{
    private readonly StorageTypeProvider _storageTypeProvider;

    public ConfigController(StorageTypeProvider storageTypeProvider)
    {
        _storageTypeProvider = storageTypeProvider;
    }

    [HttpPost]
    public IActionResult SetStorageType(string StorageType)
    {
        _storageTypeProvider.UpdateStorageType(StorageType.ToLower());

        return RedirectToActionPermanent("Index", "Todo");
    }
}