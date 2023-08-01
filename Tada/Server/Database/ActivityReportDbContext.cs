using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;

using Tada.Server.Shared;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class ActivityReportDbContext : IDbContext<ActivityReport>
    {
        private readonly string tableName = "ActivityReport";

        public ActivityReport ConvertValues(SqlDataReader reader)
        {
            var result = new ActivityReport
            {
                ProjectId = ConvertUtil.ToInt(reader["ProjectId"]),
                Seq = ConvertUtil.ToInt(reader["Seq"]),
                ReportDay = ConvertUtil.ToDateTime(reader["ReportDay"]),
                ReportName = ConvertUtil.ToString(reader["ReportName"]),
                FilePath = ConvertUtil.ToString(reader["FilePath"]),
                CreateUserId = ConvertUtil.ToInt(reader["CreateUserId"]),
                CreateDate = ConvertUtil.ToDateTime(reader["CreateDate"]),
                UpdateUserId = ConvertUtil.ToInt(reader["UpdateUserId"]),
                UpdateDate = ConvertUtil.ToDateTime(reader["UpdateDate"])
            };
            return result;
        }

        public List<ActivityReport> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + "  ORDER BY ProjectId, Seq";
            return db.Query(ConvertValues, sql);
        }
        public List<ActivityReport> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId ORDER BY Seq";
            var param = new { ProjectId = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<ActivityReport> FindByIdAndSeq(int id, int seq)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId AND Seq = @Seq ";
            var param = new { ProjectId = id, Seq = seq };
            return db.Query(ConvertValues, sql, param);
        }
        public ActivityReport FindByPk(ActivityReport vo)
        {
            return FindByIdAndSeq(vo.ProjectId, vo.Seq).First();
        }

        public List<ActivityReport> GetViewListData(ActivityReport vo)
        {
            return FindById(vo.ProjectId);
        }


        public bool Insert(ActivityReport table)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO ActivityReport (ProjectId, Seq, ReportDay, ReportName, FilePath, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (@ProjectId, dbo.GetMaxActivityReport(@ProjectId), @ReportDay, @ReportName, @FilePath, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { ProjectId = table.ProjectId, ReportDay = table.ReportDay, ReportName = table.ReportName, FilePath = table.FilePath, CreateUserId = table.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);

        }

        public bool Update(ActivityReport table)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET ReportDay=@ReportDay, ReportName=@ReportName, FilePath=@FilePath, UpdateUserId=@UpdateUserId, UpdateDate=@UpdateDate WHERE ProjectId=@ProjectId AND Seq = @Seq";
            var param = new { ReportDay = table.ReportDay, ReportName = table.ReportName, FilePath = table.FilePath, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now, ProjectId = table.ProjectId, Seq = table.Seq };
            return db.Execute(sql, param);
        }
        public bool DeleteById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = "DELETE FROM " + tableName + " WHERE ProjectId=@ProjectId";
            var param = new { ProjectId = id };
            return db.Execute(sql, param, false);   // 0件でもエラーにしない
        }

        public bool DeleteByIdAndSeq(int id, int seq)
        {
            var db = new SqlServerConnetion();
            var sql = "DELETE FROM " + tableName + " WHERE ProjectId=@ProjectId AND Seq = @Seq";
            var param = new { ProjectId = id, Seq = seq };
            return db.Execute(sql, param, false);   // 0件でもエラーにしない
        }

        List<ActivityReport> IDbContext<ActivityReport>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
