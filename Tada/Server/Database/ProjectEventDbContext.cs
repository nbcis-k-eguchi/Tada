using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;

using Tada.Server.Shared;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class ProjectEventDbContext : IDbContext<ProjectEvent>
    {
        private readonly string tableName = "ProjectEvent";

        public ProjectEvent ConvertValues(SqlDataReader reader)
        {
            return new ProjectEvent
            {
                ProjectId = ConvertUtil.ToInt(reader["ProjectId"]),
                Seq = ConvertUtil.ToInt(reader["Seq"]),
                EventDay = ConvertUtil.ToDateTime(reader["EventDay"]),
                EventAdapt = ConvertUtil.ToInt(reader["EventAdapt"]),
                StartTime = ConvertUtil.ToDateTime(reader["StartTime"]),
                EndTime = ConvertUtil.ToDateTime(reader["EndTime"]),
                Description = ConvertUtil.ToString(reader["Description"]),
                Location = ConvertUtil.ToString(reader["Location"]),
                MemberCount = ConvertUtil.ToInt(reader["MemberCount"]),
                CreateUserId = ConvertUtil.ToInt(reader["CreateUserId"]),
                CreateDate = ConvertUtil.ToDateTime(reader["CreateDate"]),
                UpdateUserId = ConvertUtil.ToInt(reader["UpdateUserId"]),
                UpdateDate = ConvertUtil.ToDateTime(reader["UpdateDate"])
            };

        }

        public List<ProjectEvent> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " ORDER BY ProjectId, Seq"; 
            return db.Query(ConvertValues, sql);
        }

        public List<ProjectEvent> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId ORDER BY Seq ";
            var param = new { ProjectId = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<ProjectEvent> FindByIdAndSeq(int id, int seq)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId AND Seq = @Seq ";
            var param = new { ProjectId = id, Seq = seq };
            return db.Query(ConvertValues, sql, param);
        }


        public ProjectEvent FindByPk(ProjectEvent vo)
        {
            return FindByIdAndSeq(vo.ProjectId, vo.Seq).First();
        }


        public List<ProjectEvent> GetViewListData(ProjectEvent vo)
        {
            return FindById(vo.ProjectId);
        }


        public bool Insert(ProjectEvent table)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO ProjectEvent (ProjectId, Seq, EventDay, EventAdapt, StartTime, EndTime, Description, Location, MemberCount, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (@ProjectId, dbo.GetMaxProjectEvent(@ProjectId), @EventDay, @EventAdapt, @StartTime, @EndTime, @Description, @Location, @MemberCount, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { ProjectId = table.ProjectId, EventDay = table.EventDay, EventAdapt = table.EventAdapt, StartTime = table.StartTime, EndTime = table.EndTime, Description = table.Description, Location = table.Location, MemberCount = table.MemberCount, CreateUserId = table.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);
        }

        public bool Update(ProjectEvent table)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET EventDay=@EventDay, EventAdapt=@EventAdapt, StartTime=@StartTime, EndTime=@EndTime, Description=@Description, Location=@Location, MemberCount=@MemberCount, UpdateUserId=@UpdateUserId, UpdateDate=@UpdateDate WHERE ProjectId=@ProjectId AND Seq = @Seq";
            var param = new { EventDay = table.EventDay, EventAdapt = table.EventAdapt, StartTime = table.StartTime, EndTime = table.EndTime, Description = table.Description, Location = table.Location, MemberCount = table.MemberCount, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now, ProjectId = table.ProjectId, Seq = table.Seq };
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

        List<ProjectEvent> IDbContext<ProjectEvent>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
