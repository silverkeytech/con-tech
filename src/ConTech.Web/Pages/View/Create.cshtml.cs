using ConTech.Core;
using ConTech.Core.Features.Identity;
using ConTech.Core.Features.View;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Localization;
using System.Diagnostics;

namespace ConTech.Web.Pages.View;

public class CreateModel : PageModel
{
    private readonly IProjectViewRepository _repo;
    private readonly IStringLocalizer<Global> _local;

    public CreateModel(IProjectViewRepository repo, IStringLocalizer<Global> local)
    {
        _repo = repo;
        _local = local;
    }

    [BindProperty(SupportsGet = true)]
    public int ProjectId { get; set; }

    [BindProperty]
    public ProjectViewNewInput View { get; set; }

    public IActionResult OnGet(int projectId)
    {
        ProjectId = projectId;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        View.ProjectId = ProjectId;
        // Check what values made it through binding
        var keys = ModelState.Keys.Where(k => ModelState[k].AttemptedValue != "");
        foreach (var key in keys)
        {
            Debug.WriteLine($"{key}: {ModelState[key].AttemptedValue}");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Validate PDF file
        if (View.PdfFile == null || View.PdfFile.Length == 0)
        {
            ModelState.AddModelError("ProjectViewNewInput.PdfFile", "Please select a PDF file.");
            return Page();
        }

        // Check file extension
        var extension = Path.GetExtension(View.PdfFile.FileName).ToLowerInvariant();
        if (extension != ".pdf")
        {
            ModelState.AddModelError("ProjectViewNewInput.PdfFile", "Only PDF files are allowed.");
            return Page();
        }

        var currentUser = new IdentityInfo(HttpContext);

        //if (Project != null)
            var result = await _repo.CreateProjectViewAsync(View, currentUser);

        return RedirectToPage("./Index", new { projectId = View.ProjectId });
    }
}