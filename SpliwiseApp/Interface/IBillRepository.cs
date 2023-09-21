using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IBillRepository
    {
         Task<IActionResult> UploadFile(IFormFile file);
        Task<Bills> DownloadBill(int Id);
    }
}
