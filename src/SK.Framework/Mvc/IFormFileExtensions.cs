﻿using Microsoft.AspNetCore.Http;

namespace SK.Framework.MVC;

public static class IFormFileExtensions
{
    public static async Task<byte[]> GetBytes(this IFormFile formFile)
    {
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
