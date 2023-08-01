using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;

using Tada.Server.Shared;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class ProjectGroupDbContext : IDbContext<ProjectGroup>
    {
        private readonly string tableName = "ProjectGroup";

        public ProjectGroup ConvertValues(SqlDataReader reader)
        {
            var result = new ProjectGroup();

            result.Id = ConvertUtil.ToInt(reader["Id"]);
            result.Name = ConvertUtil.ToString(reader["Name"]);
            result.Description = ConvertUtil.ToString(reader["Description"]);
            result.CreateUserId = ConvertUtil.ToInt(reader["CreateUserId"]);
            result.CreateDate = ConvertUtil.ToDateTime(reader["CreateDate"]);
            result.UpdateUserId = ConvertUtil.ToInt(reader["UpdateUserId"]);
            result.UpdateDate = ConvertUtil.ToDateTime(reader["UpdateDate"]); 

            return result;
        }

        public List<ProjectGroup> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " ORDER BY Id";
            return db.Query(ConvertValues, sql);
        }

        public List<ProjectGroup> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE Id=@Id ";
            var param = new { Id = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<ProjectGroup> FindByIdAndSeq(int id, int seq)
        {
            // ProjectGroupはシーケンスを持たないので、このメソッドは実装しない
            throw new NotSupportedException();
        }

        public ProjectGroup FindByPk(ProjectGroup vo)
        {
            return GetViewListData(vo).First();
        }


        public List<ProjectGroup> GetViewListData(ProjectGroup vo)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE Id = @Id ";
            var param = new { Id = vo.Id };
            return db.Query(ConvertValues, sql, param);
        }

        public bool Insert(ProjectGroup table)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO " + tableName + " (Id, Name, Description, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (dbo.GetMaxProjectGroup(), @Name, @Description, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { Name = table.Name, Description = table.Description, CreateUserId = table.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);
        }

        public bool Update(ProjectGroup table)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET Name=@Name, Description=@Description, UpdateUserId=@UpdateUserId, UpdateDate=@UpdateDate WHERE Id=@Id";
            var param = new { Name = table.Name, Description = table.Description, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now, Id = table.Id };
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
            // ProjectGroupはシーケンスを持たないので、このメソッドは実装しない
            throw new NotSupportedException();
        }

        List<ProjectGroup> IDbContext<ProjectGroup>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
