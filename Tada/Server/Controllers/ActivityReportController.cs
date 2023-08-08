using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Tada.Server.Database;
using Tada.Server.FileAccess;
using Tada.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Tada.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityReportController : ControllerBase
    //public class ActivityReportController : ApiControllerBase<ActivityReport, ActivityReportController>
    {
        public readonly ILogger<ActivityReportController> _logger;

        public readonly IDbContext<ActivityReport> _dbContext;

        public string TypeName
        {
            get { return typeof(ActivityReport).Name; }
        }

        public ActivityReportController(ILogger<ActivityReportController> logger, IDbContext<ActivityReport> dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}")]
        public ActivityReport[] Get(int id)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + id + ")");

            return _dbContext.FindById(id).ToArray();

        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <param name="seq">主キー2</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}/{seq:int}")]
        public ActivityReport[] Get(int id, int seq)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + id + ", " + seq + ")");

            return _dbContext.FindByIdAndSeq(id, seq).ToArray();

        }


        [HttpGet]
        public IActionResult GetFile(string fileName)
        {
            var filePath = Path.Combine(FileConnection.ActivityReportUploadPath, fileName);

            if (System.IO.File.Exists(filePath))
            {
                return File(System.IO.File.ReadAllBytes(filePath), contentType: "application/pdf", fileName);
            }
            return NotFound();
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="body">登録データ</param>
        /// <returns>成否結果</returns>
        [HttpPost]
        public ActionResult Post([FromBody] ActivityReport body)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Post()");

            // DB接続
            var db = new SqlServerConnetion();

            return _dbContext.Insert(body) ? Ok() : BadRequest();

        }

        /// <summary>
        /// Put　主キーひとつの場合
        /// </summary>
        /// <param name="id">主キー</param>
        /// <param name="body">更新データ</param>
        /// <returns>成否結果</returns>
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ActivityReport body)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Put(" + id + ")");

            return _dbContext.Update(body) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Put　主キーふたつの場合
        /// </summary>
        /// <param name="id">主キー１</param>
        /// <param name="seq">主キー２</param>
        /// <param name="body">更新データ</param>
        /// <returns>成否結果</returns>
        [HttpPut("{id:int}/{seq:int}")]
        public ActionResult Put(int id, int seq, [FromBody] ActivityReport body)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Put(" + id + ", " + seq + ")");

            return _dbContext.Update(body) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Delete　主キーひとつの場合
        /// </summary>
        /// <param name="id">主キー</param>
        /// <returns>成否結果</returns>
        [HttpDelete("{id:int}")]
        public virtual ActionResult Delete(int id)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Delete(" + id + ")");

            return _dbContext.DeleteById(id) ? Ok() : BadRequest();
        }

        /// <summary>
        /// Delete　主キーひとつの場合
        /// </summary>
        /// <param name="id">主キー</param>
        /// <returns>成否結果</returns>
        [HttpDelete("{id:int}/{seq:int}")]
        public ActionResult Delete(int id, int seq)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Delete(" + id + ", " + seq + ")");

            // ファイルがあれば削除する
            var vo = _dbContext.FindByIdAndSeq(id, seq).First();
            if (vo != null)
            {
                var filePath = Path.Combine(FileConnection.ActivityReportUploadPath, vo.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            return _dbContext.DeleteByIdAndSeq(id, seq) ? Ok() : BadRequest();
        }

    }

}
