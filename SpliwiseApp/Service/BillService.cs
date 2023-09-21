using Microsoft.AspNetCore.Mvc;
using SpliwiseApp.Data;
using SpliwiseApp.Interface;
using SpliwiseApp.Models;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace SpliwiseApp.Service
{
    public class BillService:IBillService
    {
        private readonly IBillRepository _billRepository;
        private readonly SplitContext _splitContext;
        public BillService(IBillRepository billRepository,SplitContext splitContext) 
        {
            _billRepository = billRepository;
            _splitContext = splitContext;
        
        }

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _billRepository.UploadFile(file);
            return result;
        }

        public async Task<FileResult> DownloadBill(int id)
        {
            var result =await _billRepository.DownloadBill(id);
            var fileContentResult = new FileContentResult(result.Content, "application/pdf")
            {
                FileDownloadName = result.Name
            };

            return fileContentResult;
        }


        public async Task<IEnumerable<Bills>> GetAllBillsAsync()
        {
            var bills = _splitContext.Bills.ToList();
            return bills;
        }

    }
}
