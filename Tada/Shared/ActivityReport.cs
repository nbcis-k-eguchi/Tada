using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tada.Shared
{
    public class ActivityReport
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public int ProjectId { get; set; }

        /// <summary>
        /// 連番
        /// </summary>
        public int Seq { get; set; } = 0;

        /// <summary>
        /// 報告日
        /// </summary>
        public DateTime ReportDay { get; set; } = DateTime.Now;

        /// <summary>
        /// 報告題名
        /// </summary>
        public string ReportName { get; set; } = "";

        /// <summary>
        /// 報告書格納先
        /// </summary>
        public string FilePath { get; set; } = "";

        public int CreateUserId { get; set; } = 0;

        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateDate { get; set; } = DateTime.Now;

        public int UpdateUserId { get; set; } = 0;

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateDate { get; set; } = DateTime.Now;


    }
}
