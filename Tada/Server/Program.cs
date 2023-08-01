using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;

using Tada.Server.Authentication;
using Tada.Server.Database;
using System.Text;
using Tada.Shared;

namespace Tada
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            // appsettings.jsonの設定を取得
            var connectionString = builder.Configuration.GetConnectionString("SqlServer");
            if (connectionString == null)
            {
                // 接続文字列が取得できない場合はエラー
                throw new Exception("接続文字列が取得できませんでした。");
            }

            // SQLServer接続用のクラスをDIコンテナに登録
            builder.Services.AddSingleton(new SqlServerConnetion(connectionString));
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtAuthenticationManager.JWT_SECURITY_KEY)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            // UserAccountServiceを追加
            builder.Services.AddSingleton<UserAccountService>();

            // データベースコンテキストをDIコンテナに登録（認識させないと利用時にエラーが発生する）
            builder.Services.AddScoped<IDbContext<ProjectGroup>, ProjectGroupDbContext>();
            builder.Services.AddScoped<IDbContext<ProjectMember>, ProjectMemberDbContext>();
            builder.Services.AddScoped<IDbContext<ProjectEvent>, ProjectEventDbContext>();
            builder.Services.AddScoped<IDbContext<BalanceSheet>, BalanceSheetDbContext>();
            builder.Services.AddScoped<IDbContext<ActivityReport>, ActivityReportDbContext>();
            builder.Services.AddScoped<IDbContext<AccountUser>, AccountUserDbContext>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            //// 認証用のミドルウェア追加
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}