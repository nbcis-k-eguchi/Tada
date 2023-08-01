using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tada.Shared;

namespace Tada.Server.Authentication
{
    public class JwtAuthenticationManager
    {
        // JWTのセキュリティキー
        public const string JWT_SECURITY_KEY = "yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP";
        private const int JWT_EXPIRY_MINUTES = 30;

        private readonly UserAccountService _userAccountService;

        public JwtAuthenticationManager(UserAccountService userAccountService)
        {
            _userAccountService = userAccountService;
        }

        public UserSession? GenarateJwtToken(string userName, string password)
        {
            // 引数が空の場合はNull
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            // ユーザー情報を取得
            var user = _userAccountService.FindUser(userName, password);
            if (user == null)
            {
                return null;
            }

            return GenarateJwtToken(user);
        }

        public UserSession? GenarateJwtToken(AccountUser accountUser)
        {

            // JWTのTokenを生成
            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_EXPIRY_MINUTES);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var claimsIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(ClaimTypes.Sid, accountUser.Id.ToString()),
                new Claim(ClaimTypes.Email, accountUser.Email),
                new Claim(ClaimTypes.Role, accountUser.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwrSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwrSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwrSecurityTokenHandler.WriteToken(securityToken);

            var userSession = new UserSession
            {
                UserId = accountUser.Id,
                UserName = accountUser.Email,   // メールアドレスをユーザー名として使用
                Role = accountUser.Role,
                Token = token,
                ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
            };

            return userSession;


        }

    }
}

