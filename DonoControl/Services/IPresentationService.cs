using Microsoft.AspNetCore.Http;

namespace DonoControl.Services
{
    public interface IPresentationService
    {
        Task<IResult> GetSlidesAsync();
    }
}
