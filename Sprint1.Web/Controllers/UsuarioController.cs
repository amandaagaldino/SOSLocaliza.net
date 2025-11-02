using Microsoft.AspNetCore.Mvc;
using Sprint1.DTOs.Usuario;
using Sprint1.Infrastructure.Data.UseCase;
using Sprint1.Models.ViewModels;

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
        var viewModel = usuarios.Select(u => new UsuarioListViewModel
        {
            Id = u.Id,
            NomeCompleto = u.NomeCompleto,
            Email = u.Email,
            Cpf = u.Cpf,
            DataNascimento = u.DataNascimento,
            Ativo = u.Ativo,
            DataCriacao = u.DataCriacao
        }).ToList();
        
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var usuario = await _usuarioUseCase.GetUsuarioByIdAsync(id);
        
        if (usuario == null)
        {
            return NotFound();
        }

        var viewModel = new UsuarioDetailsViewModel
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            Cpf = usuario.Cpf,
            DataNascimento = usuario.DataNascimento,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Ativo = usuario.Ativo
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new UsuarioViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        try
        {
            var dto = new CreateUsuarioDto
            {
                NomeCompleto = viewModel.NomeCompleto,
                Email = viewModel.Email,
                Senha = viewModel.Senha,
                DataNascimento = viewModel.DataNascimento,
                Cpf = viewModel.Cpf
            };

            var usuario = await _usuarioUseCase.CreateUsuarioAsync(dto);
            TempData["Success"] = "Usuário criado com sucesso!";
            return RedirectToAction(nameof(Details), new { id = usuario.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(viewModel);
        }
    }

    [HttpGet]
    public IActionResult EditEmail(int id)
    {
        ViewBag.UsuarioId = id;
        return View(new AlterarEmailViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEmail(int id, AlterarEmailViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }

        try
        {
            var dto = new AlterarEmailDto
            {
                Email = viewModel.Email
            };

            await _usuarioUseCase.AlterarEmailUsuarioAsync(id, dto);
            TempData["Success"] = "Email alterado com sucesso!";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }
    }

    [HttpGet]
    public IActionResult EditSenha(int id)
    {
        ViewBag.UsuarioId = id;
        return View(new AlterarSenhaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSenha(int id, AlterarSenhaViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }

        try
        {
            var dto = new AlterarSenhaDto
            {
                SenhaAtual = viewModel.SenhaAtual,
                NovaSenha = viewModel.NovaSenha
            };

            await _usuarioUseCase.AlterarSenhaUsuarioAsync(id, dto);
            TempData["Success"] = "Senha alterada com sucesso!";
            return RedirectToAction(nameof(Details), new { id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewBag.UsuarioId = id;
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _usuarioUseCase.DeleteUsuarioAsync(id);
            TempData["Success"] = "Usuário removido com sucesso!";
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
        var viewModel = new TestConnectionViewModel
        {
            Success = result.Success,
            Message = result.Message,
            Database = result.Database,
            TotalUsuarios = result.TotalUsuarios,
            Servidor = result.Servidor,
            Timestamp = result.Timestamp,
            Error = result.Error,
            InnerError = result.InnerError,
            StackTrace = result.StackTrace,
            ConnectionState = result.ConnectionState
        };
        return View(viewModel);
    }
}

