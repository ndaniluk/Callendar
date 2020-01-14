using System;
using System.Threading.Tasks;
using Callendar.Export.Excel;
using Callendar.Helpers.Employee;
using Microsoft.AspNetCore.Mvc;

namespace Callendar.Controllers
{
    [Route("accountant")]
    [ApiController]
    public class ExcelExportController : ControllerBase
    {
        private readonly CallendarDbContext _context;

        public ExcelExportController(CallendarDbContext context)
        {
            _context = context;
        }

        // GET: accountant/{userId}/excelExport
        [HttpGet("{userId}/excelExport")]
        public async Task<ActionResult<ExcelDocument>> GetExcelExport(Guid userId)
        {
            var userHelper = new UsersHelper(_context);
            if (!await userHelper.IsGuidCorrect(userId) || !await userHelper.IsAccountant(userId))
                return new OkObjectResult("You don't have needed permissions to export the data");

            ExcelDocumentCreator documentCreator = new ExcelDocumentCreator(_context);
            ExcelDocument document = await documentCreator.GetExcelDocument();

            return new OkObjectResult(document);
        }
    }
}