using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using Tada.Server.Database;
using Tada.Shared;

namespace Tada.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectGroupController : ApiControllerBase<ProjectGroup, ProjectGroupController>
    {

        public ProjectGroupController(ILogger<ProjectGroupController> logger, IDbContext<ProjectGroup> dbContext) : base(logger, dbContext)
        {
        }

        /// <summary>
        /// Delete　主キーひとつの場合
        /// </summary>
        /// <param name="id">主キー</param>
        /// <returns>成否結果</returns>
        [HttpDelete("{id:int}")]
        public override ActionResult Delete(int id)
        {
            bool result = false;

            // ロギング
            _logger.LogInformation(TypeName + "Controller.Delete(" + id + ")");

            // ProjectGroupを削除する場合、関連するテーブルも削除する
            var dbContextProjectMember = new ProjectMemberDbContext();
            result = dbContextProjectMember.DeleteById(id);
            if (!result)
            {
                return BadRequest();
            }

            var dbContextProjectEvent = new ProjectEventDbContext();
            result = dbContextProjectEvent.DeleteById(id);
            if (!result)
            {
                return BadRequest();
            }

            var dbContextBalanceSheet = new BalanceSheetDbContext();
            result = dbContextBalanceSheet.DeleteById(id);
            if (!result)
            {
                return BadRequest();
            }

            var dbContextActivityReport = new ActivityReportDbContext();
            result = dbContextActivityReport.DeleteById(id);
            if (!result)
            {
                return BadRequest();
            }

            return _dbContext.DeleteById(id) ? Ok() : BadRequest();
        }
    }
}
