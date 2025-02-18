using Microsoft.AspNetCore.Http;

namespace DonoControl.Services.Interfaces
{
    public interface IImageServise
    {
        Task<IResult> GetImageAsync(string category);
        Task<IResult> GetImageFileAsync(string category);
    }
}
