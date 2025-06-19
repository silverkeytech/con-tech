using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;
using OrchardCore.Localization.PortableObject;

namespace ConTech.Web;

/// <summary>
/// Creates a new instance of <see cref="ContentRootPoFileLocationProvider"/>.
/// </summary>
/// <param name="hostingEnvironment"><see cref="IHostEnvironment"/>.</param>
/// <param name="localizationOptions">The IOptions<LocalizationOptions>.</param>
public class MultiplePoFilesLocationProvider(IHostEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions) : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider = hostingEnvironment.ContentRootFileProvider;

    private readonly string _resourcesContainer = localizationOptions.Value.ResourcesPath;

    /// <inheritdocs />
    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        foreach (var file in Directory.EnumerateFiles(_resourcesContainer).Where(f => f.EndsWith(cultureName + ".po")))
        {
            yield return _fileProvider.GetFileInfo(file);
        }

        foreach (var file in Directory.EnumerateFiles(_resourcesContainer).Where(f => f.EndsWith("all.po")))
        {
            yield return _fileProvider.GetFileInfo(file);
        }
    }
}