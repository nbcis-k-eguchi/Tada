using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;

using Tada.Server.Shared;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class ProjectMemberDbContext : IDbContext<ProjectMember>
    {
        private readonly string tableName = "ProjectMember";

        public ProjectMember ConvertValues(SqlDataReader reader)
        {
            return new ProjectMember()
            {
                ProjectId = ConvertUtil.ToInt(reader["ProjectId"]),
                Seq = ConvertUtil.ToInt(reader["Seq"]),
                EmployeeNumber = ConvertUtil.ToInt(reader["EmployeeNumber"]),
                EMail = ConvertUtil.ToString(reader["EMail"]),
                Password = ConvertUtil.ToString(reader["Password"]),
                Name = ConvertUtil.ToString(reader["Name"]),
                Birthday = ConvertUtil.ToDateTimeNullable(reader["Birthday"]),
                Position = ConvertUtil.ToString(reader["Position"]),
                JoiningDate = ConvertUtil.ToDateTimeNullable(reader["JoiningDate"]),
                ResignationDate = ConvertUtil.ToDateTimeNullable(reader["ResignationDate"]),
                IsLock = ConvertUtil.ToBool(reader["IsLock"]),
                CreateUserId = ConvertUtil.ToInt(reader["CreateUserId"]),
                CreateDate = ConvertUtil.ToDateTime(reader["CreateDate"]),
                UpdateUserId = ConvertUtil.ToInt(reader["UpdateUserId"]),
                UpdateDate = ConvertUtil.ToDateTime(reader["UpdateDate"])
            };
        }

        public List<ProjectMember> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " ORDER BY ProjectId, Seq"; 
            return db.Query(ConvertValues, sql);
        }

        public List<ProjectMember> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId ORDER BY Seq";
            var param = new { ProjectId = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<ProjectMember> FindByIdAndSeq(int id, int seq)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId AND Seq = @Seq ";
            var param = new { ProjectId = id, Seq = seq };
            return db.Query(ConvertValues, sql, param);
        }

        public ProjectMember FindByPk(ProjectMember vo)
        {
            return FindByIdAndSeq(vo.ProjectId, vo.Seq).First();
        }

        public List<ProjectMember> GetViewListData(ProjectMember vo)
        {
            return FindById(vo.ProjectId);
        }

        public bool Insert(ProjectMember table)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO ProjectMember (ProjectId, Seq, EmployeeNumber, EMail, Password, Name, Birthday, Position, JoiningDate, ResignationDate, IsLock, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (@ProjectId, dbo.GetMaxProjectMember(@ProjectId), @EmployeeNumber, @EMail, @Password, @Name, @Birthday, @Position, @JoiningDate, @ResignationDate, @IsLock, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { ProjectId = table.ProjectId, EmployeeNumber = table.EmployeeNumber, EMail = table.EMail, Password = table.Password, Name = table.Name, Birthday = table.Birthday, Position = table.Position, JoiningDate = table.JoiningDate, ResignationDate = table.ResignationDate, IsLock = table.IsLock, CreateUserId = table.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);
        }

        public bool Update(ProjectMember table)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET EmployeeNumber=@EmployeeNumber, EMail=@EMail, Password=@Password, Name=@Name, Birthday=@Birthday, Position=@Position, JoiningDate=@JoiningDate, ResignationDate=@ResignationDate, IsLock=@IsLock, UpdateUserId=@UpdateUserId, UpdateDate=@UpdateDate WHERE ProjectId=@ProjectId AND Seq = @Seq";
            var param = new { EmployeeNumber = table.EmployeeNumber, EMail = table.EMail, Password = table.Password, Name = table.Name, Birthday = table.Birthday, Position = table.Position, JoiningDate = table.JoiningDate, ResignationDate = table.ResignationDate, IsLock = table.IsLock, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now, ProjectId = table.ProjectId, Seq = table.Seq };
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

        List<ProjectMember> IDbContext<ProjectMember>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
