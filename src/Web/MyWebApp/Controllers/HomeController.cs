using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Domain.Exceptions;
using MyWebApp.Domain.Services;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GroupService _groupService;

    public HomeController(ILogger<HomeController> logger, GroupService groupService)
    {
        _logger = logger;
        _groupService = groupService;
    }

    /// <summary>
    /// This action shows all groups in Database
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120), HttpGet]
    public async ValueTask<IActionResult> IndexAsync()
    {
        _logger.LogInformation($"[GET] Index. IP = {HttpContext.Connection.RemoteIpAddress}");

        var groups = await _groupService.GetAllAsync();

        var model = new IndexViewModel
        {
            Groups = groups
        };

        return View(model);
    }

    /// <summary>
    /// This action shows all images in Database
    /// </summary>
    /// <param name="id">group id</param>
    /// <returns></returns>
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120), HttpGet]
    public async ValueTask<IActionResult> GroupViewAsync(ulong id)
    {
        try
        {
            var group = await _groupService.GetById(id);

            var model = new GroupViewModel
            {
                Group = group
            };

            return View(model);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError($"{ex}");

            return NotFound();
        }
    }

    [ResponseCache(Duration = 300), HttpGet]
    public IActionResult Privacy() => View();


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
        {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});

    //[Authorize(Roles = "admin"), HttpGet]
    //public IActionResult Manager()
    //{
    //    return Content($"Role: {User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value}");
    //}
}