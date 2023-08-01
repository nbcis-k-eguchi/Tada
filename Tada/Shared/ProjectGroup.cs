using System.ComponentModel.DataAnnotations;

namespace Tada.Shared
{
    /// <summary>
    /// プロジェクトグループ
    /// </summary>
    public class ProjectGroup
    {
        /// <summary>
        /// プロジェクトID
        /// </summary>
        public int Id { get; set; } = 0;

        /// <summary>
        /// プロジェクト名
        /// </summary>
        [Required(ErrorMessage = "プロジェクト名は必須項目です。")]
        [StringLength(20, ErrorMessage = "プロジェクト名は20文字以内にしてください。")]
        public string Name { get; set; } = "";

        /// <summary>
        /// プロジェクトの説明
        /// </summary>
        public string Description { get; set; } = "";

        public int CreateUserId { get; set; } = 0;

        /// <summary>
        /// 作成日
        /// </summary>
        public DateTime CreateDate { get; set; }

        public int UpdateUserId { get; set; } = 0;

        /// <summary>
        /// 更新日
        /// </summary>
        public DateTime UpdateDate { get; set; }

    }
}
