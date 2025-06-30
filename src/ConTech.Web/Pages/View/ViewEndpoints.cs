using ConTech.Core.Features.Identity;
using ConTech.Core.Features.Level;
using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SD.LLBLGen.Pro.ORMSupportClasses;

//using Newtonsoft.Json;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ConTech.Web.Pages.View;

public class ViewEndpoints
{
    public static void Map(IEndpointRouteBuilder map)
    {
        var view = map.MapGroup("/admin/view");

        view.MapGet("/get-view-details-by-id/{id}", GetViewDetailsByIdAsync);
        view.MapPost("/add-view-level", AddViewLevelAsync);
        view.MapPost("/disable-view-level/{id}", DisableViewLevelAsync);
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

    public static async Task<IResult> AddViewLevelAsync(HttpRequest request, IViewLevelRepository repo)
    {
        try
        {

            if (!request.HasFormContentType)
                return Results.BadRequest("Expected multipart form data");

            var form = await request.ReadFormAsync();
            var metadataJson = form["metadata"];

            if (string.IsNullOrEmpty(metadataJson))
                return Results.BadRequest("Metadata is required");

            var metadata = JsonSerializer.Deserialize<ViewLevelNewInput>(metadataJson!);


            var dxfFile = form.Files.GetFiles("dxfFile");
            var excelFile = form.Files.GetFiles("excelFile");

            metadata.DxfFile = dxfFile[0];
            metadata.ExcelFile = excelFile[0];

            var result = await repo.CreateViewLevelAsync(metadata);

            return Results.Ok(new
            {
                LevelName = metadata.LevelName,
                //Files = results,
                //TotalSize = results.Sum(f => f.Size)
            });

        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }


    public static async Task<IResult> DisableViewLevelAsync(string id, IViewLevelRepository repo)
    {
        try
        {
            var realId = new Guid(id);
            var result = await repo.DisableViewLevelByIdAsync(realId);

            if (result.IsFalse)
                return Results.Problem();

            return Results.Ok(result.Value);
        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

}