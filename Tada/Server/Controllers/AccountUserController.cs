using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

using Tada.Server.Database;
using Tada.Shared;
using Tada.Server.Authentication;

namespace Tada.Server.Controllers
{
    /// <summary>
    /// アカウントユーザー用APIコントロール
    /// </summary>
    /// <remarks>
    /// このコントロールはロールやルートの制御を行うため共通化しない
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountUserController : ControllerBase
    {
        private readonly ILogger<AccountUserController> _logger;
        private readonly UserAccountService _userAccountService = new UserAccountService();

        public AccountUserController(ILogger<AccountUserController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// サインイン
        /// </summary>
        /// <param name="loginResuest"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Route("signin")]
        [AllowAnonymous]
        public ActionResult<UserSession> Signin(string id)
        {
            // idをキーにAccessUserを検索
            var user = _userAccountService.FindUser(int.Parse(id));
            if (user is null)
            {
                return Unauthorized();
            }
            else
            {
                // ユーザーが存在する場合はトークンを再作成して返す
                var jwtAuthentionManager = new JwtAuthenticationManager(_userAccountService);
                var userSession = jwtAuthentionManager.GenarateJwtToken(user);

                if (userSession is null)
                {
                    return Unauthorized();
                }
                else
                {

                    // トークンを更新
                    user.Token = userSession.Token;
                    if (!_userAccountService.UpdateToken(user))
                    {
                        return Unauthorized();
                    }
                    
                    return userSession;
                }
            }

        }
        /// <summary>
        /// サインイン
        /// </summary>
        /// <param name="loginResuest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signin")]
        [AllowAnonymous]
        public ActionResult<UserSession> Signin([FromBody] LoginRequest loginResuest)
        {
            // ユーザー認証を行い、ユーザーセッションを取得する
            var jwtAuthentionManager = new JwtAuthenticationManager(_userAccountService);
            var userSession = jwtAuthentionManager.GenarateJwtToken(loginResuest.Email, loginResuest.Password);

            if (userSession is null)
            {
                return Unauthorized();
            }
            else
            {
                return userSession;
            }
        }

        /// <summary>
        /// サインイン
        /// </summary>
        /// <param name="loginResuest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
        public ActionResult<UserSession> Signup([FromBody] AccountUser loginResuest)
        {
            // ユーザー登録を行う
            var accountUser = new AccountUser { Name = loginResuest.Name, Password = loginResuest.Password, Email = loginResuest.Email, CreateDate = DateTime.Now, UpdateDate = DateTime.Now, Role = "User" };
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO AccessUser (Name, Password, Email, CreateDate, UpdateDate, Role) VALUES (@Name, @Password, @Email, @CreateDate, @UpdateDate, @Role)";
            var param = new { Name = accountUser.Name, Password = accountUser.Password, Email = accountUser.Email, CreateDate = accountUser.CreateDate, UpdateDate = accountUser.UpdateDate, Role = accountUser.Role };
            if (!db.Execute(sql, param))
            {
                return Unauthorized();
            }

            var jwtAuthentionManager = new JwtAuthenticationManager(_userAccountService);
            var userSession = jwtAuthentionManager.GenarateJwtToken(accountUser);

            if (userSession is null)
            {
                return Unauthorized();
            }
            else
            {
                // トークンを更新
                sql = "UPDATE AccessUser SET Token = @Token WHERE Id = @Id";



                return userSession;
            }
        }

    }
}
