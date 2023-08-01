namespace Tada.Client.Shared.Components
{
	/// <summary>
	/// カレンダーモデル
	/// </summary>
	public class CalendarModel
	{
		// 日にちをクリックしたときに呼び出だされるイベント
		public event Action<CalendarDayStyle>? OnClickDay;

		private DateTime _currentDay = DateTime.Today;
		public DateTime CurrentDay
		{
			get
			{
				return _currentDay;
			}
			set
			{
				_currentDay = value;
				Year = _currentDay.Year;
				Month = _currentDay.Month;
				Day = _currentDay.Day;
			}
		}

		private int _year = DateTime.Today.Year;
		public int Year {
			get
			{
				return _year;
			}
			set
			{
				_year = value;
				//// 存在しない日付であれば末日をdayに設定
				//if (DateTime.DaysInMonth(_year, _month) > _day)
				//{ 
				//	_day = DateTime.DaysInMonth(_year, _month);
				//}
				SetWeeks();
			}
		} 

		private int _month = DateTime.Today.Month;
		public int Month { 
			get
			{
				return _month;
			}
			set
			{
				// 変更前の月の最終日を取得
				var lastDay = DateTime.DaysInMonth(_year, _month);

				_month = value;
				//// 存在しない日付であれば末日をdayに設定
				//if (lastDay == _day && DateTime.DaysInMonth(_year, _month) > _day)
				//{
				//	_day = DateTime.DaysInMonth(_year, _month);
				//}
				_currentDay = new DateTime(_year, _month, _day);
				SetWeeks();
			}
		}
		private int _day = DateTime.Today.Day;
		public int Day
		{
			get
			{
				return _day;
			}
			set
			{
				// 存在しない日にちであればexception
				var validateDay  = new DateTime(Year, Month, value);
				
				_day = validateDay.Day;
			}
		}

		public List<CalendarDayStyle[]> Weeks { get; set; } = new List<CalendarDayStyle[]>();

		// 月の最初の日
		public DateTime FirstDayOfMonth
		{
			get
			{
				return new DateTime(Year, Month, 1);
			}
		}

		// 月の最後の日
		public DateTime LastDayOfMonth
		{
			get
			{
				return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
			}
		}

        public CalendarModel()
        {

			SetWeeks();

		}

		public void PrevMonth()
		{
			if (Month == 1)
			{
				Year--;
				Month = 12;
			}
			else
			{
				Month--;
			}
		}

		public void NextMonth()
		{
			if (Month == 12)
			{
				Year++;
				Month = 1;
			}
			else
			{
				Month++;
			}
		}

		/// <summary>
		/// 1カ月のすべての週を格納する
		/// </summary>
		/// <remarks>
		/// CurrentDay/Year/Month/Dayの変更は行わないこと。参照はOK
		/// </remarks>
		private void SetWeeks()
		{

			Weeks.Clear();

			// 前月の末日
			var prevMonth = FirstDayOfMonth.AddDays(-1);

			// 前月の最後の日が土曜でなければ、日曜まで日付けを戻す
			if (prevMonth.DayOfWeek != DayOfWeek.Sunday)
			{
				while (true)
				{
					prevMonth = prevMonth.AddDays(-1);
					if (prevMonth.DayOfWeek == DayOfWeek.Sunday) break;
				}
			}

			// 当月末まで1週間分を格納する
			var oneWeeks = new List<CalendarDayStyle>();
			while (true)
			{
				var oneDay = new CalendarDayStyle();
				oneDay.Date = new DateTime(prevMonth.Year, prevMonth.Month, prevMonth.Day);
				oneDay.IsToday = (prevMonth.Year == CurrentDay.Year && prevMonth.Month == CurrentDay.Month && prevMonth.Day == CurrentDay.Day);
				oneDay.OnClick += (day) =>
				{
					OnClickDay?.Invoke(day);
				};

				// 前月であればinactive
				if (prevMonth.Month != Month) oneDay.DayClass = "inactive";

				// 当日であればactive
				if (prevMonth.Year == DateTime.Now.Year && prevMonth.Month == DateTime.Now.Month && prevMonth.Day == DateTime.Now.Day) oneDay.DayClass = "active";
				oneWeeks.Add(oneDay);
				// 翌月
				var nextMonth = CurrentDay.AddMonths(1);

				prevMonth = prevMonth.AddDays(1);
				if (prevMonth.DayOfWeek == DayOfWeek.Sunday)
				{
					Weeks.Add(oneWeeks.ToArray());
					oneWeeks.Clear();
				}

				// 翌月になれば抜ける
				if (prevMonth.Month == nextMonth.Month) break;

			}

			// 当月末が土曜でなければ、翌月の日曜まで日付けを進める
			if (prevMonth.DayOfWeek != DayOfWeek.Sunday)
			{
				while (true)
				{
					var oneDay = new CalendarDayStyle();
					oneDay.Date = new DateTime(prevMonth.Year, prevMonth.Month, prevMonth.Day);
					oneDay.IsToday = false;
					oneDay.DayClass = "inactive";
					oneDay.OnClick += (day) =>
					{
						OnClickDay?.Invoke(day);
					};

					oneWeeks.Add(oneDay);
					prevMonth = prevMonth.AddDays(1);
					if (prevMonth.DayOfWeek == DayOfWeek.Sunday)
					{
						Weeks.Add(oneWeeks.ToArray());
						oneWeeks.Clear();
						break;
					}
				}
			}
		}

    }
}
