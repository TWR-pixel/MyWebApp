using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Data.Entities;
using MyWebApp.Domain.Exceptions;
using MyWebApp.Domain.Services;
using MyWebApp.Models;

namespace MyWebApp.Controllers;

/// <summary>
/// This controller needs for administrating content
/// </summary>
public class AdminController : Controller
{
    private readonly GroupService _groupService;
    private readonly ImageService _imageService;
    private readonly UserService _userService;
    private readonly IWebHostEnvironment _appEnvironment;
    private readonly ILogger<AdminController> _logger;

    public AdminController(GroupService groupService, ImageService imageService, UserService userService,
        IWebHostEnvironment env, ILogger<AdminController> logger)
    {
        _groupService = groupService;
        _imageService = imageService;
        _userService = userService;
        _appEnvironment = env;
        _logger = logger;
    }

    /// <summary>
    /// Панель админа, где можно все редактировать
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Manager()
    {
        var model = new ManagerViewModel
        {
            Groups = await _groupService.GetAllAsync() // !!!!!!!!
        };

        return View(model);
    }

    /// <summary>
    /// Добавляет группу
    /// </summary>
    /// <param name="model">view model группы</param>
    /// <returns></returns>
    //[Authorize]
    [HttpPost]
    public async Task<IActionResult> AddGroup(GroupAdminViewModel model)
    {
        var group = new Group
        {
            Name = model.Name
        };

        await _groupService.CreateAsync(group);

        return RedirectToAction("Manager", "Admin");
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> DeleteGroup(uint groupId)
    {
        try
        {
            var group = await _groupService.GetById(groupId);
            var images = group.Images;

            foreach (var image in images)
            {
                System.IO.File.Delete(_appEnvironment.WebRootPath + image.Path);
            }

            await _groupService.DeleteByIdAsync(groupId);

            return RedirectToAction("Manager", "Admin");
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogError(ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// Добавляет картинку в группу
    /// </summary>
    /// <param name="formFile">Файл группы</param>
    /// <param name="id">id группы</param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize, HttpPost]
    public async Task<IActionResult> Manager(ManagerFileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var path = "/images/" + model.FormFile?.FileName;
            // сохраняем файл в папку Files в каталоге wwwroot
            await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await model.FormFile?.CopyToAsync(fileStream)!;
            }

            var group = await _groupService.GetById(model.Id);
            var image = new Image(null, path, true, group.Id);

            await _imageService.CreateAsync(image);
            var g = await _groupService.GetAllAsync();
            return RedirectToAction("Manager", "Admin");
        }

        return Content("ERROR");
    }

    [HttpGet]
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = await _userService.GetByName(viewModel.Name);

                if (user.Password == viewModel.Password)
                {
                    await AuthenticateAsync(user);

                    return RedirectToAction("Manager");
                }

                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            catch (EntityNotFoundException ex)
            {
                _logger.LogError(ex.Message);
                return Content("Not right name");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        return View(viewModel);
    }


    private async Task AuthenticateAsync(User user)
    {
        // создаем один claim
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Name),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
        };

        // создаем объект ClaimsIdentity
        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}