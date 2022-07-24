using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApp.Data.Entities;
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

    public AdminController(GroupService groupService, ImageService imageService, UserService userService,
        IWebHostEnvironment env)
    {
        _groupService = groupService;
        _imageService = imageService;
        _userService = userService;
        _appEnvironment = env;
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
            Groups = (List<Group>) await _groupService.GetAllAsync()
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

    /// <summary>
    /// Добавляет картинку в группу
    /// </summary>
    /// <param name="formFile">Файл группы</param>
    /// <param name="id">id группы</param>
    /// <returns></returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Manager(IFormFile formFile, ulong id)
    {
        var path = "/images/" + formFile.FileName;
        // сохраняем файл в папку Files в каталоге wwwroot
        await using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
        {
            await formFile.CopyToAsync(fileStream);
        }
        
        var group = await _groupService.GetById(id);
        var image = new Image(null, path, true, group);
        
        await _imageService.CreateAsync(image);
        return Content(formFile.FileName);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _userService.GetByName(viewModel.Name);

            if (user.Password == viewModel.Password)
            {
                await Authenticate(user);

                return RedirectToAction("Manager");
            }

            ModelState.AddModelError("", "Некорректные логин и(или) пароль");
        }

        return View(viewModel);
    }

    private async Task Authenticate(User user)
    {
        // создаем один claim
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Name),
            new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
        };

        // создаем объект ClaimsIdentity
        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        // установка аутентификационных куки
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}