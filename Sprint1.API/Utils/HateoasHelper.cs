using Sprint1.DTOs;
using Sprint1.DTOs.Usuario;
using Microsoft.AspNetCore.Http;

namespace Sprint1.Utils;

public static class HateoasHelper
{
    public static UsuarioResourceDto AddLinks(this UsuarioResponseDto usuario, HttpContext httpContext)
    {
        var resource = new UsuarioResourceDto
        {
            Id = usuario.Id,
            NomeCompleto = usuario.NomeCompleto,
            Email = usuario.Email,
            DataNascimento = usuario.DataNascimento,
            Cpf = usuario.Cpf,
            DataCriacao = usuario.DataCriacao,
            DataAtualizacao = usuario.DataAtualizacao,
            Ativo = usuario.Ativo
        };

        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";

        // Self link
        resource.AddLink("self", new Link(
            $"{baseUrl}/api/usuario/{usuario.Id}",
            "self",
            "GET"
        ));

        // Update email link
        resource.AddLink("update-email", new Link(
            $"{baseUrl}/api/usuario/{usuario.Id}/email",
            "update-email",
            "PATCH"
        ));

        // Update password link
        resource.AddLink("update-password", new Link(
            $"{baseUrl}/api/usuario/{usuario.Id}/senha",
            "update-password",
            "PATCH"
        ));

        // Delete link
        resource.AddLink("delete", new Link(
            $"{baseUrl}/api/usuario/{usuario.Id}",
            "delete",
            "DELETE"
        ));

        // All users link
        resource.AddLink("all-users", new Link(
            $"{baseUrl}/api/usuario",
            "all-users",
            "GET"
        ));

        return resource;
    }

    public static PagedResult<UsuarioResourceDto> AddLinksToPagedResult(
        this PagedResult<UsuarioResponseDto> pagedResult, 
        HttpContext httpContext,
        UsuarioQueryParameters parameters)
    {
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}";
        
        var resourceItems = pagedResult.Items
            .Select(u => u.AddLinks(httpContext))
            .ToList();

        var result = new PagedResult<UsuarioResourceDto>(
            resourceItems,
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.PageSize
        );

        return result;
    }

    public static Dictionary<string, Link> GeneratePaginationLinks(
        HttpContext httpContext,
        UsuarioQueryParameters parameters,
        int totalPages)
    {
        var links = new Dictionary<string, Link>();
        var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/usuario";

        // Self link
        links["self"] = new Link(
            BuildQueryString(baseUrl, parameters),
            "self",
            "GET"
        );

        // First page
        var firstPageParams = CloneParameters(parameters);
        firstPageParams.Page = 1;
        links["first"] = new Link(
            BuildQueryString(baseUrl, firstPageParams),
            "first",
            "GET"
        );

        // Last page
        var lastPageParams = CloneParameters(parameters);
        lastPageParams.Page = totalPages;
        links["last"] = new Link(
            BuildQueryString(baseUrl, lastPageParams),
            "last",
            "GET"
        );

        // Previous page
        if (parameters.Page > 1)
        {
            var prevPageParams = CloneParameters(parameters);
            prevPageParams.Page = parameters.Page - 1;
            links["prev"] = new Link(
                BuildQueryString(baseUrl, prevPageParams),
                "prev",
                "GET"
            );
        }

        // Next page
        if (parameters.Page < totalPages)
        {
            var nextPageParams = CloneParameters(parameters);
            nextPageParams.Page = parameters.Page + 1;
            links["next"] = new Link(
                BuildQueryString(baseUrl, nextPageParams),
                "next",
                "GET"
            );
        }

        return links;
    }

    private static UsuarioQueryParameters CloneParameters(UsuarioQueryParameters parameters)
    {
        return new UsuarioQueryParameters
        {
            Page = parameters.Page,
            PageSize = parameters.PageSize,
            SortBy = parameters.SortBy,
            SortOrder = parameters.SortOrder,
            Nome = parameters.Nome,
            Email = parameters.Email,
            Cpf = parameters.Cpf,
            Ativo = parameters.Ativo
        };
    }

    private static string BuildQueryString(string baseUrl, UsuarioQueryParameters parameters)
    {
        var queryParams = new List<string>
        {
            $"page={parameters.Page}",
            $"pageSize={parameters.PageSize}"
        };

        if (!string.IsNullOrWhiteSpace(parameters.SortBy))
            queryParams.Add($"sortBy={parameters.SortBy}");

        if (!string.IsNullOrWhiteSpace(parameters.SortOrder))
            queryParams.Add($"sortOrder={parameters.SortOrder}");

        if (!string.IsNullOrWhiteSpace(parameters.Nome))
            queryParams.Add($"nome={Uri.EscapeDataString(parameters.Nome)}");

        if (!string.IsNullOrWhiteSpace(parameters.Email))
            queryParams.Add($"email={Uri.EscapeDataString(parameters.Email)}");

        if (!string.IsNullOrWhiteSpace(parameters.Cpf))
            queryParams.Add($"cpf={parameters.Cpf}");

        if (parameters.Ativo.HasValue)
            queryParams.Add($"ativo={parameters.Ativo.Value}");

        return $"{baseUrl}?{string.Join("&", queryParams)}";
    }
}

// Made with Bob
