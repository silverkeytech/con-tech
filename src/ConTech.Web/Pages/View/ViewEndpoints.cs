using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    public static async Task<IResult> AddViewLevelAsync(HttpRequest request, IProjectViewRepository repo)
    {
        try
        {
            //var realId = Convert.ToInt32(id);
            //var result = await repo.GetProjectViewByIdAsync(realId);

            //if (result.IsNotFound)
            //    return Results.NotFound();

            //return Results.Json(result.Item);


            if (!request.HasFormContentType)
                return Results.BadRequest("Expected multipart form data");

            var form = await request.ReadFormAsync();
            var metadataJson = form["metadata"];

            if (string.IsNullOrEmpty(metadataJson))
                return Results.BadRequest("Metadata is required");

            var metadata = JsonSerializer.Deserialize<UploadMetadata>(metadataJson!);
            var files = form.Files;

            // Process files and metadata
            var results = new List<FileUploadResult>();
            foreach (var file in files)
            {
                // Save file or process as needed
                results.Add(new FileUploadResult(
                    file.FileName,
                    file.Length,
                    file.ContentType
                ));
            }

            return Results.Ok(new
            {
                Author = metadata.LevelName,
                Files = results,
                TotalSize = results.Sum(f => f.Size)
            });

        }
        catch (Exception ex)
        {
            return Results.Problem();
        }
    }

    public record UploadMetadata(
        [property: JsonPropertyName("levelId")] int LevelId,
        [property: JsonPropertyName("levelName")] string LevelName,
        [property: JsonPropertyName("levelScale")] int LevelScale,
        [property: JsonPropertyName("fileInfo")] List<FileInfo> FileInfo
    );

    public record FileInfo(
        [property: JsonPropertyName("originalName")] string OriginalName,
        [property: JsonPropertyName("size")] long Size,
        [property: JsonPropertyName("type")] string Type
    );
    record FileUploadResult(string FileName, long Size, string ContentType);
}