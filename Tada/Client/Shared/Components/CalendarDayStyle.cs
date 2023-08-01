namespace Tada.Client.Shared.Components
{
	public class CalendarDayStyle
	{
		public DateTime Date { get; set; } = DateTime.Today;

		public int Day => Date.Day;
		public string DayValue => Day.ToString();

		public bool IsToday { get; set; } = false;

		public string? DayClass { get; set; } = null;

		// Callback
		public Action<CalendarDayStyle>? OnClick { get; set; } = null;

	}
}
