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
            // appsettings.json�̐ݒ���擾
            var connectionString = builder.Configuration.GetConnectionString("SqlServer");
            if (connectionString == null)
            {
                // �ڑ������񂪎擾�ł��Ȃ��ꍇ�̓G���[
                throw new Exception("�ڑ������񂪎擾�ł��܂���ł����B");
            }

            // SQLServer�ڑ��p�̃N���X��DI�R���e�i�ɓo�^
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
            // UserAccountService��ǉ�
            builder.Services.AddSingleton<UserAccountService>();

            // �f�[�^�x�[�X�R���e�L�X�g��DI�R���e�i�ɓo�^�i�F�������Ȃ��Ɨ��p���ɃG���[����������j
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
            //// �F�ؗp�̃~�h���E�F�A�ǉ�
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapRazorPages();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}