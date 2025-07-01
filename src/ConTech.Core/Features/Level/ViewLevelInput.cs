using FluentValidation;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static System.Collections.Specialized.BitVector32;

namespace ConTech.Core.Features.Level;

public class ViewLevelNewInput
{
    public string? Description { get; set; }

    [JsonPropertyName("viewId")]
    public int ViewId { get; set; }
    //public string? LevelId { get; set; }
    [JsonPropertyName("levelName")]
    public string? LevelName { get; set; }

    [JsonPropertyName("levelScale")]
    public string? LevelScale { get; set; }

    [JsonPropertyName("fileInfo")]
    public List<FileInfo> FileInfo { get; set; } = new();

    public IFormFile? DxfFile { get; set; }
    public byte[]? DxfContent { get; set; }

    public IFormFile? ExcelFile { get; set; }
    public byte[]? ExcelContent { get; set; }

    //public string FileName { get; set; }
    //public string ContentType { get; set; }
    //public DateTime UploadDate { get; set; }
    //public long FileSize { get; set; }
    public ViewLevelEntity ToEntity()
    {
        var e = new ViewLevelEntity
        {
            Id = Guid.NewGuid(),
            Name = LevelName,
            Description = Description,
            ViewId = ViewId,
            Scale = Convert.ToSingle(LevelScale),
            ObjectStatus = ObjectStatus.Active,
            TransitionX = Convert.ToSingle(0),
            TransitionY = Convert.ToSingle(0),
            //CreatedByUserId = by.UserId,
            DateCreatedUtc = DateTime.UtcNow,
            IsNew = true
        };

        return e;
    }


    public class Validator : AbstractValidator<ViewLevelNewInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.LevelName).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}

public class ViewLevelUpdateInput
{

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("levelName")]
    public string? Name { get; set; }

    [JsonPropertyName("levelScale")]
    public string? LevelScale { get; set; }

    [JsonPropertyName("transitionX")]
    public string? TransitionX { get; set; }

    [JsonPropertyName("transitionY")]
    public string? TransitionY { get; set; }

    [JsonPropertyName("fileInfo")]
    public List<FileInfo> FileInfo { get; set; } = new();

    public IFormFile? DxfFile { get; set; }
    public byte[]? DxfContent { get; set; }

    public IFormFile? ExcelFile { get; set; }
    public byte[]? ExcelContent { get; set; }

    public ViewLevelUpdateInput()
    {

    }

    public ViewLevelUpdateInput(ViewLevelView v)
    {
        Name = v.Name;
        Description = v.Description;
        Id = v.Id.ToString();
    }

    public ViewLevelEntity ToEntity()
    {
        var e = new ViewLevelEntity
        {
            Id = new Guid(Id),
            Name = Name,
            Description = Description,
            Scale = Convert.ToSingle(LevelScale),
            TransitionX = Convert.ToSingle(TransitionX),
            TransitionY = Convert.ToSingle(TransitionY),
            //LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

    public class Validator : AbstractValidator<ViewLevelUpdateInput>
    {
        public Validator(IStringLocalizer<Global> local)
        {
            RuleFor(x => x.Name).NotNull().WithMessage(local["validate-project-name-required"]);
        }
    }
}


public class ViewLevelFilter : IFilter<ViewLevelEntity>
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = Constants.CommonPageSize;

    public IQueryable<ViewLevelEntity> Filter(IQueryable<ViewLevelEntity> query, LinqMetaData? meta = null)
    {


        return query;
    }
}

public class FileInfo
{


    [JsonPropertyName("originalName")]
    public string? OriginalName { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}



public class ViewLevelUpdateTransitionInput
{

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("transitionX")]
    public string? TransitionX { get; set; }

    [JsonPropertyName("transitionY")]
    public string? TransitionY { get; set; }


    public ViewLevelEntity ToEntity()
    {
        var e = new ViewLevelEntity
        {
            Id = new Guid(Id),
            TransitionX = Convert.ToSingle(TransitionX),
            TransitionY = Convert.ToSingle(TransitionY),
            //LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

}

public class LevelChildNewInput
{

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("levelId")]
    public string? LevelId { get; set; }

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("entityList")]
    public string? EntityList { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    public LevelChildEntity ToEntity()
    {
        var e = new LevelChildEntity
        {
            Id = Guid.NewGuid(),
            LevelId = new Guid(LevelId),
            ParentId = new Guid(ParentId),
            Name = Name,
            EntityList = EntityList,
            //CreatedByUserId = by.UserId,
            DateCreatedUtc = DateTime.UtcNow,
            IsNew = true
        };

        return e;
    }

}

public class LevelChildUpdateInput
{

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("levelId")]
    public string? LevelId { get; set; }

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("entityList")]
    public string? EntityList { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }
    public LevelChildEntity ToEntity()
    {
        var e = new LevelChildEntity
        {
            Id = new Guid(Id),
            LevelId = new Guid(LevelId),
            ParentId = new Guid(ParentId),
            Name = Name,
            EntityList = EntityList,
            //LastModifiedByUserId = by.UserId,
            LastModifiedUtc = Stamp.DateTimeUtc(),
            IsNew = false
        };

        return e;
    }

}
