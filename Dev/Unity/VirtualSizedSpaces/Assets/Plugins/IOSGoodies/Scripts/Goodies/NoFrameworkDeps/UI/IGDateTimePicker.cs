using JetBrains.Annotations;

#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using AOT;
	using Internal;
	using UnityEngine;

	/// <summary>
	///     Class to present native iOS UI Date and Time Pickers.
	/// </summary>
	[PublicAPI]
	public static class IGDateTimePicker
	{
		/// <summary>
		///     Displays hour and minute (e.g. 1 | 53).
		///     All the input will be blocked until the date and time is picked.
		/// </summary>
		/// <param name="dateTimePickerCallback">Date time picker callback.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		/// <param name="minuteInterval"> A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60. </param>
		public static void ShowCountDownTimer(Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithInitialValue(
				0, 0, 0, 0, minuteInterval,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.CountDownTimer,
				minuteInterval);
		}

		[MonoPInvokeCallback(typeof(OnDateSelectedDelegate))]
		static void OnDateTimeSelectedCallback(IntPtr actionPtr, int year, int month, int day, int hour, int minute)
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log(string.Format("OnDateTimeSelectedCallback y:{0} m:{1} d:{2} h:{3} min:{4}", year, month, day,
					hour, minute));
			}

			if (actionPtr != IntPtr.Zero)
			{
				var action = actionPtr.Cast<Action<DateTime>>();
				action(new DateTime(year, month, day, hour, minute, 0));
			}
		}

		enum UIDatePickerMode
		{
			Time = 0,
			Date = 1,
			DateAndTime = 2,
			CountDownTimer = 3
		}

		/// <summary>
		/// A date picker style determines how your app displays the date picker value and its editor.
		/// </summary>
		[PublicAPI]
		public enum DatePickerStyle
		{
			/// <summary>
			/// A style indicating that the system picks the concrete style based on the current platform and date picker mode.
			/// </summary>
			Automatic = 0,

			/// <summary>
			/// A style indicating that the date picker displays as a label that when tapped displays a calendar-style editor.
			/// </summary>
			Compact = 1,

			/// <summary>
			/// A style indicating that the date pickers displays as an inline, editable field.
			/// </summary>
			Inline = 2,

			/// <summary>
			/// A style indicating that the date picker displays as a wheel picker.
			/// </summary>
			Wheels = 4
		}

		internal delegate void OnDateSelectedDelegate(IntPtr actionPtr, int year, int month, int day, int hour,
			int minute);

		#region date_picker

		/// <summary>
		///     Displays month, day, and year depending on the locale setting (e.g. November | 15 | 2007) provided with initial date.
		///     All the input will be blocked until the date is picked.
		/// </summary>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="day">Day.</param>
		/// <param name="dateTimePickerCallback">Callback that receives the date that user pickes.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		public static void ShowDatePicker(int year, int month, int day, Action<DateTime> dateTimePickerCallback,
			Action onCancel)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithInitialValue(year, month, day, 0, 0,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Date, 1);
		}

		/// <summary>
		///     Displays month, day, and year depending on the locale setting (e.g. November | 15 | 2007).
		///     The displayed date is set to current date
		///     All the input will be blocked until the date is picked.
		/// </summary>
		/// <param name="dateTimePickerCallback">Callback that receives the date that user pickes.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		public static void ShowDatePicker(Action<DateTime> dateTimePickerCallback, Action onCancel)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePicker(dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Date, 1);
		}

		/// <summary>
		/// Displays month, day, and year depending on the locale setting (e.g. November | 15 | 2007) provided with initial
		///     date. The calendar range is limited between minimum and maximum values.
		///     All the input will be blocked until the date is picked.
		/// </summary>
		public static void ShowDatePickerWithRestrains(int currentYear, int currentMonth, int currentDay,
			Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minYear, int minMonth, int minDay,
			int maxYear, int maxMonth, int maxDay)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithRestrains(currentYear, currentMonth, currentDay, 0, 0,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Date,
				minYear, minMonth, minDay, 0, 0, maxYear, maxMonth, maxDay, 0, 0, 1);
		}

		#endregion

		#region time_picker

		/// <summary>
		///     Displays hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. 6 | 53 | PM).
		///     All the input will be blocked until the time is picked.
		/// </summary>
		/// <param name="hourOfDay">Hour of day.</param>
		/// <param name="minute">Minute.</param>
		/// <param name="minuteInterval"> A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60. </param>
		/// <param name="dateTimePickerCallback">Date time picker callback.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		public static void ShowTimePicker(int hourOfDay, int minute,
			Action<DateTime> dateTimePickerCallback, Action onCancel, int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithInitialValue(0, 0, 0, hourOfDay, minute,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Time, minuteInterval);
		}

		/// <summary>
		/// Displays hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. 6 | 53 | PM).
		/// Picker bounds will be restrained by minimum and maximum values.
		/// All the input will be blocked until the time is picked.
		/// </summary>
		/// <param name="currentHour"> Hour of day. </param>
		/// <param name="currentMinute"> Minute.</param>
		/// <param name="dateTimePickerCallback"> Date time picker callback.</param>
		/// <param name="onCancel"> Callback when user cancelled picking.</param>
		/// <param name="minHour"> Minimum allowed hour.</param>
		/// <param name="minMinute">Minimum allowed minute. </param>
		/// <param name="maxHour"> Maximum allowed hour.</param>
		/// <param name="maxMinute">Maximum allowed minute. </param>
		/// <param name="minuteInterval"> A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60.</param>
		public static void ShowTimePickerWithRestrains(int currentHour, int currentMinute,
			Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minHour, int minMinute, int maxHour, int maxMinute, int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithRestrains(0, 0, 0, currentHour, currentMinute,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Time,
				0, 0, 0, minHour, minMinute, 0, 0, 0, maxHour, maxMinute, minuteInterval);
		}

		/// <summary>
		///     Displays hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. 6 | 53 | PM).
		///     All the input will be blocked until the time is picked.
		/// </summary>
		/// <param name="dateTimePickerCallback">Date time picker callback.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		/// <param name="minuteInterval">A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60.</param>
		public static void ShowTimePicker(Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePicker(
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.Time, minuteInterval);
		}

		#endregion

		#region date_time_picker

		/// <summary>
		///     Displays date, hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. Wed Nov 15 | 6
		///     | 53 | PM)
		///     All the input will be blocked until the date and time is picked.
		/// </summary>
		/// <param name="year">Year.</param>
		/// <param name="month">Month.</param>
		/// <param name="day">Day.</param>
		/// <param name="hourOfDay">Hour of day.</param>
		/// <param name="minute">Minute.</param>
		/// <param name="minuteInterval"> A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60.</param>
		/// <param name="dateTimePickerCallback">Date time picker callback.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		public static void ShowDateAndTimePicker(int year, int month, int day, int hourOfDay, int minute,
			Action<DateTime> dateTimePickerCallback, Action onCancel, int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithInitialValue(year, month, day, hourOfDay, minute, dateTimePickerCallback.GetPointer(),
				OnDateTimeSelectedCallback, onCancel.GetPointer(), IGUtils.ActionVoidCallback,
				(int) UIDatePickerMode.DateAndTime, minuteInterval);
		}

		/// <summary>
		///     Displays date, hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. Wed Nov 15 | 6
		///     | 53 | PM)
		///     All the input will be blocked until the date and time is picked.
		/// </summary>
		/// <param name="dateTimePickerCallback">Date time picker callback.</param>
		/// <param name="onCancel">Callback when user cancelled picking</param>
		/// <param name="minuteInterval"> A minimum amount of minutes, that can be changed during time pick.
		/// The interval value must be evenly divided into 60.</param>
		public static void ShowDateAndTimePicker(Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePicker(dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.DateAndTime, minuteInterval);
		}

		/// <summary>
		/// Displays date, hour, minute, and optionally AM/PM designation depending on the locale setting (e.g. Wed Nov 15 | 6
		///     | 53 | PM). The choice range is limited between minimum and maximum values.
		///     All the input will be blocked until the date and time is picked.
		/// Note: when both time and date is picked simultaneously, user won't be able to see the year.
		/// </summary>
		public static void ShowDateTimePickerWithRestrains(
			int currentYear, int currentMonth, int currentDay, int currentHour, int currentMinute,
			Action<DateTime> dateTimePickerCallback, Action onCancel,
			int minYear, int minMonth, int minDay, int minHour, int minMinute,
			int maxYear, int maxMonth, int maxDay, int maxHour, int maxMinute, int minuteInterval = 1)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(dateTimePickerCallback, "dateTimePickerCallback");

			_showDatePickerWithRestrains(currentYear, currentMonth, currentDay, currentHour, currentMinute,
				dateTimePickerCallback.GetPointer(), OnDateTimeSelectedCallback,
				onCancel.GetPointer(), IGUtils.ActionVoidCallback, (int) UIDatePickerMode.DateAndTime,
				minYear, minMonth, minDay, minHour, minMinute, maxYear, maxMonth, maxDay, maxHour, maxMinute,
				minuteInterval);
		}

		#endregion

		[DllImport("__Internal")]
		static extern void _showDatePickerWithInitialValue(int year, int month, int day, int hourOfDay, int minute,
			IntPtr successCallbackPtr, OnDateSelectedDelegate onDateSelectedCallback,
			IntPtr cancelCallbackPtr, IGUtils.ActionVoidCallbackDelegate onCancelCallback,
			int datePickerType, int minuteInterval);

		[DllImport("__Internal")]
		static extern void _showDatePicker(
			IntPtr successCallbackPtr, OnDateSelectedDelegate onDateSelectedCallback,
			IntPtr cancelCallbackPtr, IGUtils.ActionVoidCallbackDelegate onCancelCallback,
			int datePickerType, int minuteInterval);

		[DllImport("__Internal")]
		static extern void _showDatePickerWithRestrains(
			int currentYear, int currentMonth, int currentDay, int currentHourOfDay, int currentMinute,
			IntPtr successCallbackPtr, OnDateSelectedDelegate onDateSelectedCallback,
			IntPtr cancelCallbackPtr, IGUtils.ActionVoidCallbackDelegate onCancelCallback, int datePickerType,
			int minYear, int minMonth, int minDay, int minHourOfDay, int minMinute,
			int maxYear, int maxMonth, int maxDay, int maxHourOfDay, int maxMinute, int minuteInterval);
	}
}
#endif