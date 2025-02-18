using Microsoft.AspNetCore.Http;

namespace DonoControl.Services.Interfaces
{
    public interface IPresentationService
    {
        Task<IResult> GetSlidesAsync();
    }
}
