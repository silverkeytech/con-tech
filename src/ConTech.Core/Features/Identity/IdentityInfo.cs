using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace ConTech.Core.Features.Identity;

public class IdentityInfo(HttpContext accessor) : IByUser
{
    public string FullName
        => GetValue(ClaimTypes.Name);

    public int UserId
        => int.Parse("1"/*GetValue(ApplicationClaimTypes.UserId)*/);

    public int PersonId
        => int.Parse("1"/*GetValue(ApplicationClaimTypes.PersonId)*/);

    public string Email
        => GetValue(ClaimTypes.Email);

    //public int RoleId => int.Parse(GetValue(ApplicationClaimTypes.RoleId));

    //public UserTypes Role => UserTypesEx.FromInt(RoleId);

    //public List<int> RoleIds => GetListIntValue(ApplicationClaimTypes.RoleId);

    private string GetValue(string key)
    {
        if (accessor.User == null || key == null) return string.Empty;

        return accessor.User.FindFirstValue(key) ?? string.Empty;
    }

    private List<int> GetListIntValue(string key)
    {
        if (accessor.User == null) return [];

        var value = accessor.User.FindAll(key);

        if (value.Any() is false) return [];

        return value.Select(x => int.Parse(x.Value)).ToList();
    }
}