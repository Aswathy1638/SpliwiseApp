using Azure;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Net.Http.Headers;

namespace SpliwiseApp.Repositories
{
    public class BillRepository:IBillRepository

    {
        public readonly SplitContext _splitContext;
        public BillRepository( SplitContext splitContext)
        {
            _splitContext = splitContext; 
        }
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            using (var mem  = new MemoryStream())
            {
                await file.CopyToAsync(mem);

                var doc = new Bills
                {
                    Name = file.Name,
                    Content = mem.ToArray()
                };
                _splitContext.Bills.Add(doc);
                await _splitContext.SaveChangesAsync();

                return new OkResult();

            }
        }

        public async Task<Bills> DownloadBill(int Id)
        {
            var bill = await _splitContext.Bills.FindAsync(Id);

            var read = new Bills
            {
               Name = bill.Name,
               Content = bill.Content

            };
            return read;
        }
    }
}
