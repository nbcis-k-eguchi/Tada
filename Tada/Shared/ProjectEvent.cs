namespace Tada.Shared
{
    public class ProjectEvent
    {

      public int ProjectId { get; set; }
        public int Seq { get; set; }
        public DateTime EventDay { get; set; }

        public int EventAdapt { get; set; } = 1;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; } = "";
        public string Location { get; set; } = "";
        public int MemberCount { get; set; }
        public int CreateUserId { get; set; } = 0;

        public DateTime CreateDate { get; set; }

        public int UpdateUserId { get; set; } = 0;
        public DateTime UpdateDate { get; set; }

    }
}
