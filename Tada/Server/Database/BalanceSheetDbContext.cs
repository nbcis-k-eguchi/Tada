using DocumentFormat.OpenXml.Office2010.Excel;

using Microsoft.Data.SqlClient;

using Tada.Server.Shared;
using Tada.Shared;

namespace Tada.Server.Database
{
    public class BalanceSheetDbContext : IDbContext<BalanceSheet>
    {
        private readonly string tableName = "BalanceSheet";

        public BalanceSheet ConvertValues(SqlDataReader reader)
        {
            var result = new BalanceSheet();
            result.ProjectId = ConvertUtil.ToInt(reader["ProjectId"]);
            result.Seq = ConvertUtil.ToInt(reader["Seq"]);
            result.BalanceDate = ConvertUtil.ToDateTime(reader["BalanceDate"]);
            result.BalanceType = ConvertUtil.ToInt(reader["BalanceType"]);
            result.SubjectName = ConvertUtil.ToString(reader["SubjectName"]);
            result.Amount = ConvertUtil.ToInt(reader["Amount"]);
            result.IsExpense = ConvertUtil.ToBool(reader["IsExpense"]);
            result.Note = ConvertUtil.ToString(reader["Note"]);
            result.CreateUserId = ConvertUtil.ToInt(reader["CreateUserId"]);
            result.CreateDate = ConvertUtil.ToDateTime(reader["CreateDate"]);
            result.UpdateUserId = ConvertUtil.ToInt(reader["UpdateUserId"]);
            result.UpdateDate = ConvertUtil.ToDateTime(reader["UpdateDate"]);
            return result;
        }

        public List<BalanceSheet> FindAll()
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " ORDER BY ProjectId, Seq"; 
            return db.Query(ConvertValues, sql);
        }

        public List<BalanceSheet> FindById(int id)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId ORDER BY Seq";
            var param = new { ProjectId = id };
            return db.Query(ConvertValues, sql, param);
        }

        public List<BalanceSheet> FindByIdAndSeq(int id, int seq)
        {
            var db = new SqlServerConnetion();
            var sql = $"SELECT * FROM " + tableName + " WHERE ProjectId = @ProjectId AND Seq = @Seq ";
            var param = new { ProjectId = id, Seq = seq };
            return db.Query(ConvertValues, sql, param);
        }

        public BalanceSheet FindByPk(BalanceSheet vo)
        {
            return FindByIdAndSeq(vo.ProjectId, vo.Seq).First();
        }

        public List<BalanceSheet> GetViewListData(BalanceSheet vo)
        {
            return FindById(vo.ProjectId);
        }



        public bool Insert(BalanceSheet table)
        {
            var db = new SqlServerConnetion();
            var sql = "INSERT INTO BalanceSheet (ProjectId, Seq, BalanceDate, BalanceType, SubjectName, Amount, IsExpense, Note, CreateUserId, CreateDate, UpdateUserId, UpdateDate) VALUES (@ProjectId, dbo.GetMaxBalanceSheet(@ProjectId), @BalanceDate, @BalanceType, @SubjectName, @Amount, @IsExpense, @Note, @CreateUserId, @CreateDate, @UpdateUserId, @UpdateDate)";
            var param = new { ProjectId = table.ProjectId, BalanceDate = table.BalanceDate, BalanceType = table.BalanceType, SubjectName = table.SubjectName, Amount = table.Amount, IsExpense = table.IsExpense, Note = table.Note, CreateUserId = table.CreateUserId, CreateDate = DateTime.Now, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now };
            return db.Execute(sql, param);

        }

        public bool Update(BalanceSheet table)
        {
            var db = new SqlServerConnetion();
            var sql = "UPDATE " + tableName + " SET BalanceDate=@BalanceDate, BalanceType=@BalanceType, SubjectName=@SubjectName, Amount=@Amount, IsExpense=@IsExpense, Note=@Note, UpdateUserId = @UpdateUserId, UpdateDate=@UpdateDate WHERE ProjectId=@ProjectId AND Seq = @Seq";
            var param = new { BalanceDate = table.BalanceDate, BalanceType = table.BalanceType, SubjectName = table.SubjectName, Amount = table.Amount, IsExpense = table.IsExpense, Note = table.Note, UpdateUserId = table.UpdateUserId, UpdateDate = DateTime.Now, ProjectId = table.ProjectId, Seq = table.Seq };
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

        List<BalanceSheet> IDbContext<BalanceSheet>.FindQuery(string query)
        {
            var db = new SqlServerConnetion();
            return db.Query(ConvertValues, query);
        }
    }
}
