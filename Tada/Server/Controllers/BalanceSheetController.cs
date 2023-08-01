using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using Tada.Server.Database;
using Tada.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tada.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceSheetController : ApiControllerBase<BalanceSheet, BalanceSheetController>
    {

        public BalanceSheetController(ILogger<BalanceSheetController> logger, IDbContext<BalanceSheet> dbContext) : base(logger, dbContext)
        {
        }

    }
}
