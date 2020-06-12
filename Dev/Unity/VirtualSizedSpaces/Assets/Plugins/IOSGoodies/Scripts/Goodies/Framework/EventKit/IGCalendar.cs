#if UNITY_IOS && !DISABLE_CALENDAR_AND_REMINDERS
namespace DeadMosquito.IosGoodies
{
	using System;
	using JetBrains.Annotations;
	using System.Runtime.InteropServices;
	using Internal;

	/// <summary>
	/// Class for calendar-related actions.
	/// </summary>
	[PublicAPI]
	public static class IGCalendar
	{
		/// <summary>
		/// Recurrence types for repeating events and reminders.
		/// </summary>
		[PublicAPI]
		public enum RecurrenceRuleType
		{
			Daily = 0,
			Weekly = 1,
			Monthly = 2,
			Yearly = 3
		}

		/// <summary>
		/// Opens the default calendar app at the specified date. if <see cref="date"/> is null, the current date is opened
		/// </summary>
		public static void OpenCalendar(DateTime? date = null)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			if (date == null)
			{
				_openCalendar();
			}
			else
			{
				_openCalendarAt(date.Value.Year, date.Value.Month, date.Value.Day);
			}
		}

		/// <summary>
		/// Creates a calendar event with specified start/end dates and title.
		/// </summary>
		/// <param name="title"> Event name.</param>
		/// <param name="notes"> Event notes.</param>
		/// <param name="startDate"> Start date of the event.</param>
		/// <param name="endDate"> End date of the event.</param>
		/// <param name="onSuccess"> Action to be performed with the unique identifier of the event.</param>
		/// <param name="onError"> Action to be performed with the error description. </param>
		public static void CreateCalendarEvent(Action<string> onSuccess, Action<string> onError,
			string title, DateTime startDate, DateTime endDate, string notes = null)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_createCalendarEvent(title, notes, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute,
				endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute,
				IGUtils.ActionStringCallback, onSuccess.GetPointer(), onError.GetPointer());
		}

		/// <summary>
		/// Creates a repeating calendar event.
		/// </summary>
		/// <param name="title"> Event name.</param>
		/// <param name="notes"> Event notes.</param>
		/// <param name="startDate"> Start date of the event.</param>
		/// <param name="endDate"> End date of the event.</param>
		/// <param name="repeatUntilDate"> Date the event should be repeated until.</param>
		/// <param name="ruleType"> The pattern for event repetition (one of <see cref="RecurrenceRuleType")/></param>
		/// <param name="interval"> The number of <see cref="RecurrenceRuleType"/> instances to be skipped until event repetition.
		/// For example, if you choose <see cref="RecurrenceRuleType.Daily"/> and pass 5 as interval,
		/// the event will be repeated every 5 days.</param>
		/// <param name="onSuccess"> Action to be performed with the unique identifier of the event.</param>
		/// <param name="onError"> Action to be performed with the error description. </param>
		public static void CreateRepeatingCalendarEvent(Action<string> onSuccess, Action<string> onError,
			string title, DateTime startDate, DateTime endDate, DateTime repeatUntilDate,
			RecurrenceRuleType ruleType = RecurrenceRuleType.Daily, int interval = 1, [CanBeNull] string notes = null)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_createRepeatingEvent(title, notes,
				startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute,
				endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute,
				repeatUntilDate.Year, repeatUntilDate.Month, repeatUntilDate.Day, repeatUntilDate.Hour, repeatUntilDate.Minute,
				IGUtils.ActionStringCallback, onSuccess.GetPointer(), onError.GetPointer(), (int) ruleType, interval);
		}

		/// <summary>
		/// Removes a calendar event with the specified identifier.
		/// </summary>
		/// <param name="id">Unique event identifier, returned as callback to the onSuccess action after
		/// <see cref="CreateCalendarEvent"/> and <see cref="CreateRepeatingCalendarEvent"/></param>
		public static void RemoveCalendarEvent([NotNull] string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(id, "id");

			_removeCalendarEvent(id);
		}

		/// <summary>
		/// Creates a reminder with given parameters.
		/// </summary>
		/// <param name="title"> Reminder title. </param>
		/// <param name="startDate"> Start date of the reminder. </param>
		/// <param name="dueDate"> Due date of the reminder. </param>
		/// <param name="onSuccess"> Action to be performed with the unique identifier of the reminder.</param>
		/// <param name="onError"> Action to be performed with the error description. </param>
		public static void CreateReminder(Action<string> onSuccess, Action<string> onError,
			string title, DateTime startDate, DateTime dueDate)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_createReminder(title, startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute,
				dueDate.Year, dueDate.Month, dueDate.Day, dueDate.Hour, dueDate.Minute,
				IGUtils.ActionStringCallback, onSuccess.GetPointer(), onError.GetPointer());
		}

		/// <summary>
		/// Marks reminder with specified identifier as completed/incomplete.
		/// </summary>
		/// <param name="id"> Unique reminder identifier. 
		/// Returned as callback from <see cref="CreateReminder"/></param>
		/// <param name="complete"> Whether to mark reminder as completed.</param>
		public static void CompleteReminder([NotNull] string id, bool complete)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(id, "id");

			_completeReminder(id, complete);
		}

		/// <summary>
		/// Deletes the reminder with specified identifier.
		/// </summary>
		/// <param name="id"> Unique reminder identifier. 
		/// Returned as callback from <see cref="CreateReminder"/></param>
		public static void RemoveReminder([NotNull] string id)
		{
			if (id == null)
			{
				throw new ArgumentNullException("id");
			}

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Check.Argument.IsNotNull(id, "id");

			_deleteReminder(id);
		}

		[DllImport("__Internal")]
		static extern void _openCalendar();

		[DllImport("__Internal")]
		static extern void _openCalendarAt(int year, int month, int day);

		[DllImport("__Internal")]
		static extern void _createCalendarEvent(string title, string notes, int startYear, int startMonth, int startDay,
			int startHour, int startMinute, int endYear, int endMonth, int endDay, int endHour, int endMinute,
			IGUtils.ActionStringCallbackDelegate callback, IntPtr successPtr, IntPtr errorPtr);

		[DllImport("__Internal")]
		static extern void _createRepeatingEvent(string title, string notes,
			int startYear, int startMonth, int startDay, int startHour, int startMinute,
			int endYear, int endMonth, int endDay, int endHour, int endMinute,
			int repeatUntilYear, int repeatUntilMonth, int repeatUntilDay, int repeatUntilHour, int repeatUntilMinute,
			IGUtils.ActionStringCallbackDelegate callback, IntPtr successPtr, IntPtr errorPtr, int ruleType, int interval);

		[DllImport("__Internal")]
		static extern void _removeCalendarEvent(string id);

		[DllImport("__Internal")]
		static extern void _createReminder(string title,
			int startYear, int startMonth, int startDay, int startHour, int startMinute,
			int dueYear, int dueMonth, int dueDay, int dueHour, int dueMinute,
			IGUtils.ActionStringCallbackDelegate callback, IntPtr successPtr, IntPtr errorPtr);

		[DllImport("__Internal")]
		static extern void _completeReminder(string reminderId, bool complete);

		[DllImport("__Internal")]
		static extern void _deleteReminder(string reminderId);
	}
}
#endif