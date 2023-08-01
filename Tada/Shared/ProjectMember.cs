namespace Tada.Shared
{
    public class ProjectMember
    {
        public int ProjectId { get; set; } = 0;
        public int Seq { get; set; } = 0;
        public int EmployeeNumber { get; set; } = 0;

        public string EmployeeNumberValue { get; set; } = "";

        public string EMail { get; set; } = "";
        public string Password { get; set; } = "";
        public string Name { get; set; } = "";
        public DateTime? Birthday { get; set; }
        public string Position { get; set; } = "";

        public DateTime? JoiningDate { get; set; } 
        
        public DateTime? ResignationDate { get; set; }

        public bool IsLock { get; set; } = false;

        public int CreateUserId { get; set; } = 0;

        public DateTime CreateDate { get; set; }

        public int UpdateUserId { get; set; } = 0;

        public DateTime UpdateDate { get; set; }
    }
}
