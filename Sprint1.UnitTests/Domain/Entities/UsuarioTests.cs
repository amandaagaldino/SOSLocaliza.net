using FluentAssertions;
using Sprint1.Domain.Entities;
using Xunit;

namespace Sprint1.UnitTests.Domain.Entities;

[Trait("Category", "Unit")]
public class UsuarioTests
{
    #region Constructor Tests

    [Fact]
    public void Constructor_ValidData_CreatesUsuario()
    {
        // Arrange
        var nomeCompleto = "João Silva";
        var email = "joao@email.com";
        var senha = "senha123";
        var dataNascimento = new DateTime(1990, 1, 1);
        var cpf = "12345678901";

        // Act
        var usuario = new Usuario(nomeCompleto, email, senha, dataNascimento, cpf);

        // Assert
        usuario.Should().NotBeNull();
        usuario.NomeCompleto.Should().Be(nomeCompleto);
        usuario.Email.Should().Be(email);
        usuario.Senha.Should().Be(senha);
        usuario.DataNascimento.Should().Be(dataNascimento);
        usuario.Cpf.Should().Be(cpf);
        usuario.Ativo.Should().BeTrue();
        usuario.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    #endregion

    #region AlterarEmail Tests

    [Fact]
    public void AlterarEmail_ValidEmail_UpdatesEmail()
    {
        // Arrange
        var usuario = CreateValidUsuario();
        var novoEmail = "novo@email.com";

        // Act
        usuario.AlterarEmail(novoEmail);

        // Assert
        usuario.Email.Should().Be(novoEmail);
        usuario.DataAtualizacao.Should().NotBeNull();
        usuario.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AlterarEmail_EmptyEmail_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarEmail("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email não pode ser vazio*");
    }

    [Fact]
    public void AlterarEmail_NullEmail_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarEmail(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email não pode ser vazio*");
    }

    [Fact]
    public void AlterarEmail_InvalidEmail_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarEmail("email-invalido");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Email inválido*");
    }

    #endregion

    #region AlterarSenha Tests

    [Fact]
    public void AlterarSenha_ValidPassword_UpdatesPassword()
    {
        // Arrange
        var usuario = CreateValidUsuario();
        var novaSenha = "novaSenha123";

        // Act
        usuario.AlterarSenha(novaSenha);

        // Assert
        usuario.Senha.Should().Be(novaSenha);
        usuario.DataAtualizacao.Should().NotBeNull();
        usuario.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AlterarSenha_EmptyPassword_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarSenha("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Senha não pode ser vazia*");
    }

    [Fact]
    public void AlterarSenha_ShortPassword_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarSenha("12345");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Senha deve ter pelo menos 6 caracteres*");
    }

    [Fact]
    public void AlterarSenha_NullPassword_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarSenha(null);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Senha não pode ser vazia*");
    }

    #endregion

    #region AlterarNome Tests

    [Fact]
    public void AlterarNome_ValidName_UpdatesName()
    {
        // Arrange
        var usuario = CreateValidUsuario();
        var novoNome = "Maria Silva";

        // Act
        usuario.AlterarNome(novoNome);

        // Assert
        usuario.NomeCompleto.Should().Be(novoNome);
        usuario.DataAtualizacao.Should().NotBeNull();
        usuario.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void AlterarNome_EmptyName_ThrowsArgumentException()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        Action act = () => usuario.AlterarNome("");

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Nome completo não pode ser vazio*");
    }

    #endregion

    #region Desativar/Ativar Tests

    [Fact]
    public void Desativar_ActiveUser_SetsAtivoToFalse()
    {
        // Arrange
        var usuario = CreateValidUsuario();

        // Act
        usuario.Desativar();

        // Assert
        usuario.Ativo.Should().BeFalse();
        usuario.DataAtualizacao.Should().NotBeNull();
        usuario.DataAtualizacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Ativar_InactiveUser_SetsAtivoToTrue()
    {
        // Arrange
        var usuario = CreateValidUsuario();
        usuario.Desativar();

        // Act
        usuario.Ativar();

        // Assert
        usuario.Ativo.Should().BeTrue();
        usuario.DataAtualizacao.Should().NotBeNull();
    }

    #endregion

    #region Helper Methods

    private Usuario CreateValidUsuario()
    {
        return new Usuario(
            "João Silva",
            "joao@email.com",
            "senha123",
            new DateTime(1990, 1, 1),
            "12345678901"
        );
    }

    #endregion
}

