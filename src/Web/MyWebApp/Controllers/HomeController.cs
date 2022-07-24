using Microsoft.AspNetCore.Mvc;
using MyWebApp.Domain.Services;
using MyWebApp.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyWebApp.Data.Entities;
using MyWebApp.Domain.Exceptions;
using Microsoft.Extensions.Caching.Memory;

namespace MyWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ImageService _imgService;
    private readonly GroupService _groupService;

    public HomeController(ILogger<HomeController> logger, ImageService imgService, GroupService groupService)
    {
        _imgService = imgService;
        _logger = logger;
        _groupService = groupService;
    }

    /// <summary>
    /// This view shows all groups in Database
    /// </summary>
    /// <returns></returns>
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120)]
    [HttpGet]
    public async ValueTask<IActionResult> IndexAsync()
    {
        _logger.LogInformation("[GET] Index");

        var groups = await _groupService.GetAllAsync();

        var model = new IndexViewModel
        {
            Groups = groups
        };

        return View(model);
    }

    /// <summary>
    /// This view shows all images in Database
    /// </summary>
    /// <param name="Id">group id</param>
    /// <returns></returns>
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 120)]
    [HttpGet]
    public async ValueTask<IActionResult> GroupViewAsync(ulong Id)
    {

        try
        {

            var group = await _groupService.GetById(Id);

            var model = new GroupViewModel
            {
                Group = group
            };

            return View(model);
        }
        catch (EntityNotFoundException ex)
        {
            return
                NotFound(new
                {
                    Message = "No such group"
                }); // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }
    }

    [ResponseCache(Duration = 300)]
    [HttpGet]
    public IActionResult Privacy() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel
    { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

    [Authorize(Roles = "admin")]
    [HttpGet]
    public IActionResult Manager()
    {
        return Content($"Role: {User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value}");
    }
}

