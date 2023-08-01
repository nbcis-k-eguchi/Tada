using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class AccountUserDbContext : IDbContext<AccountUser>
    {
        private readonly string tableName = "AccountUser";

        public AccountUser ConvertValues(SqlDataReader reader)
        {
            var user = new AccountUser();
            user.Id = reader.GetInt32(0);
            user.Name = reader.GetString(1);
            user.Email = reader.GetString(2);
            user.Password = reader.GetString(3);
            user.Token = reader.GetString(4);
            user.Role = reader.GetString(5);
            user.CreateUserId = reader.GetInt32(6);
            user.CreateDate = reader.GetDateTime(7);
            user.UpdateUserId = reader.GetInt32(8);
            user.UpdateDate = reader.GetDateTime(9);
            return user;
        }

        public List<AccountUser> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + "  ORDER BY Id"; 
            return db.Query(ConvertValues, sql);
        }

        public List<AccountUser> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE Id = @Id ";
            var param = new { Id = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<AccountUser> FindByIdAndSeq(int id, int seq)
        {
            // AccountUserはシーケンスを持たないので、このメソッドは実装しない
            throw new NotSupportedException();
        }

        public AccountUser FindByPk(AccountUser vo)
        {
            return FindById(vo.Id).First();
        }

        public List<AccountUser> GetViewListData(AccountUser vo)
        {
            return FindAll();
        }


        public bool Insert(AccountUser user)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO " + tableName + " (Name, EMail, Password, Role, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (@Name, @EMail, @Password, @Role, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { Name = user.Name, EMail = user.Email, Password = user.Password, Role = user.Role, CreateUserId = user.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = user.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);
        }

        public bool Update(AccountUser user)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET Name=@Name, EMail=@EMail, Password=@Password, Role=@Role, UpdateUserId=@UpdateUserId, UpdateDate=@UpdateDate WHERE Id=@Id";
            var param = new { Name = user.Name, EMail = user.Email, Password = user.Password, Role = user.Role, UpdateUserId = user.UpdateUserId, UpdateDate = DateTime.Now, Id = user.Id };
            return db.Execute(sql, param);
        }

        public bool DeleteById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = "DELETE FROM " + tableName + " WHERE Id=@Id";
            var param = new { Id = id };
            return db.Execute(sql, param, false);   // 0件でもエラーにしない
        }

        public bool DeleteByIdAndSeq(int id, int seq)
        {
            // AccountUserはシーケンスを持たないので、このメソッドは実装しない
            throw new NotSupportedException();
        }

        List<AccountUser> IDbContext<AccountUser>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
