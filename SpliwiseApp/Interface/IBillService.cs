using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Models;

namespace SpliwiseApp.Interface
{
    public interface IBillService
    {
         Task<IActionResult> UploadFile(IFormFile file);
        Task<FileResult> DownloadBill(int id);
        Task<IEnumerable<Bills>> GetAllBillsAsync();
    }
}
