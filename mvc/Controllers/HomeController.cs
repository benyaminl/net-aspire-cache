using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using mvc.Models;

namespace mvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDistributedCache _cache;

    public HomeController(ILogger<HomeController> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<IActionResult> Index()
    {
        // Check if there is cached content
        var cachedInfo = await _cache.GetAsync("myinfo");

        if (cachedInfo is null)
        {
            // There's no content in the cache so formulate it here
            // For example, query databases.
        
            // Store the content in the cache
            await _cache.SetAsync("myinfo", 
            System.Text.Encoding.UTF8.GetBytes("Random Text")            , new()
            { AbsoluteExpiration = DateTime.Now.AddSeconds(60) }
            );
        }
        _logger.LogInformation("Hello from {controller}!", cachedInfo);
        return View(model: System.Text.Encoding.UTF8.GetString(cachedInfo));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
