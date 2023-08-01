using Microsoft.Data.SqlClient;

using Tada.Shared;

namespace Tada.Server.Database
{
    /// <summary>
    /// DbContext制約
    /// </summary>
    /// <typeparam name="T">テーブルのクラス：Tada.Sharedに用意する</typeparam>
    public interface IDbContext<T>
    {
        /// <summary>
        /// 全件取得
        /// </summary>
        /// <returns></returns>
        List<T> FindAll();


        /// <summary>
        /// キー検索（ID）
        /// </summary>
        /// <param name="id">テーブルのid</param>
        /// <returns></returns>
        List<T> FindById(int id);

        /// <summary>
        /// キー検索（ID, SEQ）
        /// </summary>
        /// <param name="id">テーブルのid</param>
        /// <param name="seq">テーブルのseq</param>
        /// <returns></returns>
        List<T> FindByIdAndSeq(int id, int seq);

        /// <summary>
        /// 主キー検索
        /// </summary>
        /// <param name="vo">テーブルのクラス</param>
        /// <returns></returns>
        T FindByPk(T vo);

        /// <summary>
        /// クエリー実行
        /// </summary>
        /// <param name="query">SQLのWhere構文</param>
        /// <returns></returns>
        List<T> FindQuery(string query);

        /// <summary>
        /// 一覧表示のデータリスト取得
        /// </summary>
        /// <param name="vo">抽出条件</param>
        /// <param name="orderby">ソート順</param>
        /// <returns></returns>
        List<T> GetViewListData(T vo);

        bool Insert(T table);
        bool Update(T table);
        bool DeleteById(int id);
        bool DeleteByIdAndSeq(int id, int seq);

        T ConvertValues(SqlDataReader reader);

    }
}
