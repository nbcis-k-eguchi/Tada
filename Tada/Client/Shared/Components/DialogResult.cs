namespace Tada.Client.Shared.Components
{
    public class DialogResult
    {
        public bool IsOk { get; set; } = false;
        public string Message { get; set; } = "";

        public object? Data { get; set; }
    }
}
