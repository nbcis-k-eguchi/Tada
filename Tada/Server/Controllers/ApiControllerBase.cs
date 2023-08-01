using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using Tada.Server.Database;
using Tada.Shared;

namespace Tada.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiControllerBase<T1, T2> : ControllerBase
    {
        public readonly ILogger<T2> _logger;

        public readonly IDbContext<T1> _dbContext;

        public string TypeName
        {
            get { return typeof(T1).Name; }
        }

        public ApiControllerBase(ILogger<T2> logger, IDbContext<T1> dbContext) : base()
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <returns>データリスト</returns>
        [HttpGet]
        public T1[] Get()
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get()");

            return _dbContext.FindAll().ToArray();

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}")]
        public T1[] Get(int id)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + id + ")");

            return _dbContext.FindById(id).ToArray();

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="no">主キー1</param>
        /// <returns>データリスト</returns>
        [HttpGet("{no}")]
        public T1[] Get(int no, [FromQuery] string query)
        {
            // ロギング
            _logger.LogInformation(TypeName + "Controller.Get(" + no + ")");

            return _dbContext.FindById(no).ToArray();

        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">主キー1</param>
        /// <param name="seq">主キー2</param>
        /// <returns>データリスト</returns>
        [HttpGet("{id:int}/{seq:int}")]
        public T1[] Get(int id, int seq)
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
        public ActionResult Post([FromBody] T1 body)
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
        public ActionResult Put(int id, [FromBody] T1 body)
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
        public ActionResult Put(int id, int seq, [FromBody] T1 body)
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
