using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Sprint1.Domain.Entities;
using Sprint1.Domain.Repositories;
using Sprint1.DTOs.Usuario;
using Sprint1.Infrastructure.Data.UseCase;
using Sprint1.Domain.Exceptions;
using Xunit;

namespace Sprint1.UnitTests.Application.UseCase;

[Trait("Category", "Unit")]
public class UsuarioUseCaseTests
{
    private readonly Mock<IUsuarioRepository> _repositoryMock;
    private readonly Mock<ILogger<UsuarioUseCase>> _loggerMock;
    private readonly UsuarioUseCase _useCase;

    public UsuarioUseCaseTests()
    {
        _repositoryMock = new Mock<IUsuarioRepository>();
        _loggerMock = new Mock<ILogger<UsuarioUseCase>>();
        _useCase = new UsuarioUseCase(_repositoryMock.Object, _loggerMock.Object);
    }

    #region CreateUsuarioAsync Tests

    [Fact]
    public async Task CreateUsuarioAsync_ValidData_ReturnsUsuarioResponseDto()
    {
        // Arrange
        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "12345678901"
        };

        _repositoryMock.Setup(r => r.EmailExistsAsync(dto.Email))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.CpfExistsAsync(dto.Cpf))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Usuario>()))
            .ReturnsAsync((Usuario u) => u);

        // Act
        var result = await _useCase.CreateUsuarioAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.NomeCompleto.Should().Be(dto.NomeCompleto);
        result.Email.Should().Be(dto.Email);
        result.Cpf.Should().Be(dto.Cpf);
        result.Ativo.Should().BeTrue();
        
        _repositoryMock.Verify(r => r.EmailExistsAsync(dto.Email), Times.Once);
        _repositoryMock.Verify(r => r.CpfExistsAsync(dto.Cpf), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task CreateUsuarioAsync_DuplicateEmail_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "12345678901"
        };

        _repositoryMock.Setup(r => r.EmailExistsAsync(dto.Email))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _useCase.CreateUsuarioAsync(dto);

        // Assert
        await act.Should().ThrowAsync<EmailDuplicadoException>()
            .WithMessage("*já está em uso*");
        
        _repositoryMock.Verify(r => r.EmailExistsAsync(dto.Email), Times.Once);
        _repositoryMock.Verify(r => r.CpfExistsAsync(It.IsAny<string>()), Times.Never);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task CreateUsuarioAsync_DuplicateCpf_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new CreateUsuarioDto
        {
            NomeCompleto = "João Silva",
            Email = "joao@email.com",
            Senha = "senha123",
            DataNascimento = new DateTime(1990, 1, 1),
            Cpf = "12345678901"
        };

        _repositoryMock.Setup(r => r.EmailExistsAsync(dto.Email))
            .ReturnsAsync(false);
        _repositoryMock.Setup(r => r.CpfExistsAsync(dto.Cpf))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () => await _useCase.CreateUsuarioAsync(dto);

        // Assert
        await act.Should().ThrowAsync<CpfDuplicadoException>()
            .WithMessage("*já está em uso*");
        
        _repositoryMock.Verify(r => r.EmailExistsAsync(dto.Email), Times.Once);
        _repositoryMock.Verify(r => r.CpfExistsAsync(dto.Cpf), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Usuario>()), Times.Never);
    }

    #endregion

    #region GetUsuarioByIdAsync Tests

    [Fact]
    public async Task GetUsuarioByIdAsync_ExistingId_ReturnsUsuario()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);

        // Act
        var result = await _useCase.GetUsuarioByIdAsync(usuarioId);

        // Assert
        result.Should().NotBeNull();
        result.NomeCompleto.Should().Be(usuario.NomeCompleto);
        result.Email.Should().Be(usuario.Email);
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
    }

    [Fact]
    public async Task GetUsuarioByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        var usuarioId = 999;

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        // Act
        Func<Task> act = async () => await _useCase.GetUsuarioByIdAsync(usuarioId);

        // Assert
        await act.Should().ThrowAsync<UsuarioNotFoundException>()
            .WithMessage("*não foi encontrado*");
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
    }

    #endregion

    #region GetAllUsuariosAsync Tests

    [Fact]
    public async Task GetAllUsuariosAsync_ReturnsListOfUsuarios()
    {
        // Arrange
        var usuarios = new List<Usuario>
        {
            new Usuario("João Silva", "joao@email.com", "senha123", new DateTime(1990, 1, 1), "12345678901"),
            new Usuario("Maria Santos", "maria@email.com", "senha456", new DateTime(1992, 5, 15), "98765432109")
        };

        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(usuarios);

        // Act
        var result = await _useCase.GetAllUsuariosAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].NomeCompleto.Should().Be("João Silva");
        result[1].NomeCompleto.Should().Be("Maria Santos");
        
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAllUsuariosAsync_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<Usuario>());

        // Act
        var result = await _useCase.GetAllUsuariosAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
        
        _repositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
    }

    #endregion

    #region AlterarEmailUsuarioAsync Tests

    [Fact]
    public async Task AlterarEmailUsuarioAsync_ValidEmail_UpdatesEmail()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );
        var dto = new AlterarEmailDto { Email = "novo@email.com" };

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync((Usuario)null);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Usuario>()))
            .ReturnsAsync((Usuario u) => u);

        // Act
        var result = await _useCase.AlterarEmailUsuarioAsync(usuarioId, dto);

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be(dto.Email);
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
        _repositoryMock.Verify(r => r.GetByEmailAsync(dto.Email), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task AlterarEmailUsuarioAsync_NonExistingUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var usuarioId = 999;
        var dto = new AlterarEmailDto { Email = "novo@email.com" };

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        // Act
        Func<Task> act = async () => await _useCase.AlterarEmailUsuarioAsync(usuarioId, dto);

        // Assert
        await act.Should().ThrowAsync<UsuarioNotFoundException>()
            .WithMessage("*não foi encontrado*");
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    [Fact]
    public async Task AlterarEmailUsuarioAsync_EmailInUseByAnotherUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );
        var outroUsuario = new Usuario(
            "Maria Santos",
            "maria@email.com",
            "senha456",
            new DateTime(1992, 5, 15),
            "98765432109"
        );
        var dto = new AlterarEmailDto { Email = "maria@email.com" };

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.GetByEmailAsync(dto.Email))
            .ReturnsAsync(outroUsuario);

        // Act
        Func<Task> act = async () => await _useCase.AlterarEmailUsuarioAsync(usuarioId, dto);

        // Assert
        await act.Should().ThrowAsync<EmailDuplicadoException>()
            .WithMessage("*já está em uso*");
        
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    #endregion

    #region AlterarSenhaUsuarioAsync Tests

    [Fact]
    public async Task AlterarSenhaUsuarioAsync_CorrectPassword_UpdatesPassword()
    {
        // Arrange
        var usuarioId = 1;
        var senhaAtual = "senha123";
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            senhaAtual,
            new DateTime(1990, 1, 1),
            "12345678901"
        );
        var dto = new AlterarSenhaDto 
        { 
            SenhaAtual = senhaAtual,
            NovaSenha = "novaSenha456"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Usuario>()))
            .ReturnsAsync((Usuario u) => u);

        // Act
        var result = await _useCase.AlterarSenhaUsuarioAsync(usuarioId, dto);

        // Assert
        result.Should().NotBeNull();
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task AlterarSenhaUsuarioAsync_IncorrectPassword_ThrowsInvalidOperationException()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );
        var dto = new AlterarSenhaDto 
        { 
            SenhaAtual = "senhaErrada",
            NovaSenha = "novaSenha456"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);

        // Act
        Func<Task> act = async () => await _useCase.AlterarSenhaUsuarioAsync(usuarioId, dto);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Senha atual incorreta");
        
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Never);
    }

    #endregion

    #region DeleteUsuarioAsync Tests

    [Fact]
    public async Task DeleteUsuarioAsync_ExistingUser_DeletesUser()
    {
        // Arrange
        var usuarioId = 1;
        var usuario = new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync(usuario);
        _repositoryMock.Setup(r => r.DeleteAsync(usuario))
            .Returns(Task.CompletedTask);

        // Act
        await _useCase.DeleteUsuarioAsync(usuarioId);

        // Assert
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(usuario), Times.Once);
    }

    [Fact]
    public async Task DeleteUsuarioAsync_NonExistingUser_ThrowsInvalidOperationException()
    {
        // Arrange
        var usuarioId = 999;

        _repositoryMock.Setup(r => r.GetByIdAsync(usuarioId))
            .ReturnsAsync((Usuario)null);

        // Act
        Func<Task> act = async () => await _useCase.DeleteUsuarioAsync(usuarioId);

        // Assert
        await act.Should().ThrowAsync<UsuarioNotFoundException>()
            .WithMessage("*não foi encontrado*");
        
        _repositoryMock.Verify(r => r.GetByIdAsync(usuarioId), Times.Once);
        _repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Usuario>()), Times.Never);
    }

    #endregion
}

