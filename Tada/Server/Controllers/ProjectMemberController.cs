using Microsoft.AspNetCore.Mvc;

using Tada.Server.Database;
using Tada.Shared;

namespace Tada.Server.Controllers
{
    /// <summary>
    /// ProjectMemberのAPIコントローラー
    /// </summary>
    /// <remarks>
    /// 退職者を含む、含まないの抽出があるため、APIControllerBaseから外す
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectMemberController : ControllerBase
    {
        public readonly ILogger<ProjectMemberController> _logger;

        public readonly IDbContext<ProjectMember> _dbContext;

        public string TypeName
        {
            get { return typeof(ProjectMember).Name; }
        }

        public ProjectMemberController(ILogger<ProjectMemberController> logger, IDbContext<ProjectMember> dbContext) 
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns>データリスト</returns>
        [HttpGet]
        public ProjectMember[] Get()
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get()");

            return _dbContext.FindAll().ToArray();

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <param name="query">０：脱退者を含まない、１：脱退者を含む</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}")]
        public ProjectMember[] Get(int id, [FromQuery] string query)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + id + ")+query(" + query +")");


            string sql = "SELECT * FROM ProjectMember WHERE ProjectId = " + id;

            if (query == "0")
            {
                sql += " AND ResignationDate is null";
            }

            sql += " ORDER BY JoiningDate, Position Desc, EmployeeNumber";

            return _dbContext.FindQuery(sql).ToArray();

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <param name="seq">主キー2</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}/{seq:int}")]
        public ProjectMember[] Get(int id, int seq)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + id + ", " + seq + ")");

            return _dbContext.FindByIdAndSeq(id, seq).ToArray();

        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="body">登録データ</param>
        /// <returns>成否結果</returns>
        [HttpPost]
        public ActionResult Post([FromBody] ProjectMember body)
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
        public ActionResult Put(int id, [FromBody] ProjectMember body)
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
        public ActionResult Put(int id, int seq, [FromBody] ProjectMember body)
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

            return _dbContext.DeleteByIdAndSeq(id, seq) ? Ok() : BadRequest();
        }

    }
}
