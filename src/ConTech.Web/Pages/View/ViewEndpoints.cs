using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.IO.Compression;
using System.Text;

namespace ConTech.Web.Pages.View;

public class ViewEndpoints
{
    public static void Map(IEndpointRouteBuilder map)
    {
        var view = map.MapGroup("/admin/view");

        view.MapGet("/get-view-details-by-id/{id}", GetViewDetailsByIdAsync);
        view.MapPost("/add-view-level", AddViewLevelAsync);
    }

    public static async Task<IResult> GetViewDetailsByIdAsync(string id, IProjectViewRepository repo)
    {
        try
        {
            var realId = Convert.ToInt32(id);
            var result = await repo.GetProjectViewByIdAsync(realId);

            if (result.IsNotFound)
                return Results.NotFound();

            return Results.Json(result.Item);
        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

    public static async Task<IResult> AddViewLevelAsync(string id, IProjectViewRepository repo)
    {
        try
        {
            var realId = Convert.ToInt32(id);
            var result = await repo.GetProjectViewByIdAsync(realId);

            if (result.IsNotFound)
                return Results.NotFound();

            return Results.Json(result.Item);
        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

}