using AfterFrame.Application.Reviews.SaveTitleReview;
using AfterFrame.Application.Reviews.DeleteTitleReview;
using Microsoft.AspNetCore.Mvc;

namespace AfterFrame.Api.Controllers;

[ApiController]
[Route("api/library")]
public sealed class LibraryController(SaveTitleReviewService saveTitleReview, 
    DeleteTitleReviewService deleteTitleReview) : ControllerBase
{
    [HttpPut("{titleId:guid}/review")]
    public async Task<IActionResult> SaveReview(Guid titleId, [FromBody] SaveTitleReviewCommand command, CancellationToken ct)
    {
        await saveTitleReview.ExecuteAsync(titleId, command, ct);

        return NoContent();
    }
    
    [HttpDelete("{titleId:guid}/review")]
    public async Task<IActionResult> DeleteReview(Guid titleId, CancellationToken ct)
    {
        await deleteTitleReview.ExecuteAsync(titleId, ct);

        return NoContent();
    }
}