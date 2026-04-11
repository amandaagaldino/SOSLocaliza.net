using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Sprint1.Domain.Entities;
using Sprint1.DTOs.Usuario;
using Sprint1.IntegrationTests.Fixtures;
using Xunit;

namespace Sprint1.IntegrationTests.Controllers;

[Trait("Category", "Integration")]
public class UsuarioControllerTests
{

    #region POST /api/usuario Tests

    [Fact]
    public async Task POST_CreateUsuario_ReturnsCreated()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();
        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "12345678901"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/usuario", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();
        result.Should().NotBeNull();
        result!.NomeCompleto.Should().Be(dto.NomeCompleto);
        result.Email.Should().Be(dto.Email);
        result.Ativo.Should().BeTrue();
    }

    [Fact]
    public async Task POST_CreateUsuario_DuplicateEmail_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "Maria Santos",
                "maria@email.com",
                "senha123",
                new DateTime(1992, 5, 15),
                "98765432109"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "maria@email.com", // Email duplicado
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "12345678901"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/usuario", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task POST_CreateUsuario_DuplicateCpf_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "Maria Santos",
                "maria@email.com",
                "senha123",
                new DateTime(1992, 5, 15),
                "98765432109"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "98765432109" // CPF duplicado
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/usuario", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region GET /api/usuario/{id} Tests

    [Fact]
    public async Task GET_GetUsuarioById_ReturnsOk()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/usuario/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();
        result.Should().NotBeNull();
        result!.NomeCompleto.Should().Be("João Silva");
        result.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public async Task GET_GetUsuarioById_NonExisting_ReturnsNotFound()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/usuario/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region GET /api/usuario Tests

    [Fact]
    public async Task GET_GetAllUsuarios_ReturnsOkWithList()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario1 = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            var usuario2 = new Usuario(
                "Maria Santos",
                "maria@email.com",
                "senha456",
                new DateTime(1992, 5, 15),
                "98765432109"
            );
            db.Usuarios.AddRange(usuario1, usuario2);
        });
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/usuario");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<UsuarioResponseDto>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GET_GetAllUsuarios_EmptyDatabase_ReturnsOkWithEmptyList()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/usuario");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<List<UsuarioResponseDto>>();
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #region PATCH /api/usuario/{id}/email Tests

    [Fact]
    public async Task PATCH_AlterarEmail_ReturnsOk()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        var dto = new AlterarEmailDto { Email = "novo@email.com" };

        // Act
        var response = await client.PatchAsJsonAsync("/api/usuario/1/email", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>();
        result.Should().NotBeNull();
        result!.Email.Should().Be("novo@email.com");
    }

    [Fact]
    public async Task PATCH_AlterarEmail_NonExistingUser_ReturnsNotFound()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();
        var dto = new AlterarEmailDto { Email = "novo@email.com" };

        // Act
        var response = await client.PatchAsJsonAsync("/api/usuario/999/email", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task PATCH_AlterarEmail_EmailInUse_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario1 = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            var usuario2 = new Usuario(
                "Maria Santos",
                "maria@email.com",
                "senha456",
                new DateTime(1992, 5, 15),
                "98765432109"
            );
            db.Usuarios.AddRange(usuario1, usuario2);
        });
        var client = factory.CreateClient();

        var dto = new AlterarEmailDto { Email = "maria@email.com" };

        // Act
        var response = await client.PatchAsJsonAsync("/api/usuario/1/email", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region PATCH /api/usuario/{id}/senha Tests

    [Fact]
    public async Task PATCH_AlterarSenha_ReturnsOk()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        var dto = new AlterarSenhaDto 
        { 
            SenhaAtual = "senha123",
            NovaSenha = "novaSenha456"
        };

        // Act
        var response = await client.PatchAsJsonAsync("/api/usuario/1/senha", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PATCH_AlterarSenha_IncorrectPassword_ReturnsBadRequest()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        var dto = new AlterarSenhaDto 
        { 
            SenhaAtual = "senhaErrada",
            NovaSenha = "novaSenha456"
        };

        // Act
        var response = await client.PatchAsJsonAsync("/api/usuario/1/senha", dto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region DELETE /api/usuario/{id} Tests

    [Fact]
    public async Task DELETE_DeleteUsuario_ReturnsNoContent()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory(seedAction: db =>
        {
            var usuario = new Usuario(
                "João Silva",
                "joao@email.com",
                "senha123",
                new DateTime(1990, 1, 1),
                "12345678901"
            );
            db.Usuarios.Add(usuario);
        });
        var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/api/usuario/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DELETE_DeleteUsuario_NonExisting_ReturnsNotFound()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.DeleteAsync("/api/usuario/999");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Health Check Tests

    [Fact]
    public async Task GET_Health_ReturnsHealthy()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Healthy");
    }

    [Fact]
    public async Task GET_HealthReady_ReturnsHealthy()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/ready");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GET_HealthLive_ReturnsHealthy()
    {
        // Arrange
        using var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/health/live");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    #endregion
}

// Made with Bob
