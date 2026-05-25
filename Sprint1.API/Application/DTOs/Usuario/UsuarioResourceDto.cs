namespace Sprint1.DTOs.Usuario;

public class UsuarioResourceDto : UsuarioResponseDto
{
    public Dictionary<string, Link> Links { get; set; } = new();

    public void AddLink(string rel, Link link)
    {
        Links[rel] = link;
    }
}

// Made with Bob
