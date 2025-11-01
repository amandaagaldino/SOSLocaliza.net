using Microsoft.AspNetCore.Mvc;
using Sprint1.DTOs.Usuario;
using Sprint1.Infrastructure.Data.UseCase;

namespace Sprint1.Controllers;

public class UsuarioController : Controller
{
    private readonly IUsuarioUseCase _usuarioUseCase;
    private readonly TestConnectionUseCase _testConnectionUseCase;

    public UsuarioController(
        IUsuarioUseCase usuarioUseCase,
        TestConnectionUseCase testConnectionUseCase)
    {
        _usuarioUseCase = usuarioUseCase;
        _testConnectionUseCase = testConnectionUseCase;
    }

    public async Task<IActionResult> Index()
    {
        var usuarios = await _usuarioUseCase.GetAllUsuariosAsync();
        return View(usuarios);
    }

    public async Task<IActionResult> Details(int id)
    {
        var usuario = await _usuarioUseCase.GetUsuarioByIdAsync(id);
        
        if (usuario == null)
        {
            return NotFound();
        }

        return View(usuario);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUsuarioDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        try
        {
            var usuario = await _usuarioUseCase.CreateUsuarioAsync(dto);
            return RedirectToAction(nameof(Details), new { id = usuario.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(dto);
        }
    }

    [HttpGet]
    public IActionResult EditEmail(int id)
    {
        ViewBag.UsuarioId = id;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmail(int id, AlterarEmailDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UsuarioId = id;
            return View(dto);
        }

        try
        {
            await _usuarioUseCase.AlterarEmailUsuarioAsync(id, dto);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(dto);
        }
    }

    [HttpGet]
    public IActionResult EditSenha(int id)
    {
        ViewBag.UsuarioId = id;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSenha(int id, AlterarSenhaDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UsuarioId = id;
            return View(dto);
        }

        try
        {
            await _usuarioUseCase.AlterarSenhaUsuarioAsync(id, dto);
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(dto);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _usuarioUseCase.DeleteUsuarioAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction(nameof(Details), new { id });
        }
    }

    public async Task<IActionResult> TestConnection()
    {
        var result = await _testConnectionUseCase.ExecuteAsync();
        return View(result);
    }
}

