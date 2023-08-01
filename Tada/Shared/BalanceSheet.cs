namespace Tada.Shared
{
    public class BalanceSheet
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Seq { get; set; }

        public DateTime BalanceDate { get; set; } = DateTime.Now;

        /// <summary>
        /// 収支型（0：収入、1：支出）
        /// </summary>
        public int BalanceType { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; } = "";

        /// <summary>
        /// 金額
        /// </summary>
        public int Amount { get; set; } = 0;

        /// <summary>
        /// 清算状況（false：未清算、true：清算済み）
        /// </summary>
        public bool IsExpense { get; set; } = false;

        /// <summary>
        /// 備考
        /// </summary>
        public string Note { get; set; } = "";


        public int CreateUserId { get; set; } = 0;

        public DateTime CreateDate { get; set; }

        public int UpdateUserId { get; set; } = 0;

        public DateTime UpdateDate { get; set; }
    }
}
