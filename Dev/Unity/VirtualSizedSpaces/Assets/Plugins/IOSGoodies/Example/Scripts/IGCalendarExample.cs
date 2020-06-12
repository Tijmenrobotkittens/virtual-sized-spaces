namespace DeadMosquito.IosGoodies.Example
{
	using System;
	using JetBrains.Annotations;
	using UnityEngine;

	public class IGCalendarExample : MonoBehaviour
	{
#if UNITY_IOS && !DISABLE_CALENDAR_AND_REMINDERS
		readonly DateTime startDate = new DateTime(2018, 12, 8, 10, 0, 0);
		readonly DateTime endDate = new DateTime(2018, 12, 8, 15, 35, 0);
		readonly DateTime repeatUntilDate = new DateTime(2019, 10, 15, 18, 12, 0);
		readonly DateTime reminderDate = new DateTime(2018, 11, 23, 12, 32, 0);

		const string title = "GoodiesEvent";
		const string body = "Event Notes";

		string calendarItemId;

		[UsedImplicitly]
		public void OpenCalendar()
		{
			IGCalendar.OpenCalendar();
		}

		[UsedImplicitly]
		public void OpenCalendarAtDate()
		{
			IGCalendar.OpenCalendar(startDate);
		}

		public void CreateCalendarEvent()
		{
			IGCalendar.CreateCalendarEvent(id =>
			{
				calendarItemId = id;
				Debug.Log(string.Format("Calendar event was created with identifier: {0}", calendarItemId));
			}, error => { Debug.Log("An error occured: " + error); }, title, startDate, endDate, body);
		}

		public void CreateRepeatingCalendarEvent()
		{
			IGCalendar.CreateRepeatingCalendarEvent(id =>
				{
					calendarItemId = id;
					Debug.Log(string.Format("Calendar event was created with identifier: {0}", calendarItemId));
				}, error => { Debug.Log("An error occured: " + error); },
				title, startDate, endDate, repeatUntilDate, IGCalendar.RecurrenceRuleType.Weekly, 2, body);
		}

		public void CancelCalendarEvent()
		{
			if (calendarItemId != null)
			{
				Debug.Log(string.Format("Deleting calendar event with identifier: {0}", calendarItemId));
				IGCalendar.RemoveCalendarEvent(calendarItemId);
			}
			else
			{
				Debug.Log("String is empty!");
			}
		}

		public void CreateReminder()
		{
			IGCalendar.CreateReminder(id =>
			{
				calendarItemId = id;
				Debug.Log(string.Format("Calendar event was created with identifier: {0}", calendarItemId));
			}, error => { Debug.Log("An error occured: " + error); }, title, reminderDate, reminderDate);
		}

		public void CompleteReminder()
		{
			IGCalendar.CompleteReminder(calendarItemId, true);
		}

		public void DeleteReminder()
		{
			IGCalendar.RemoveReminder(calendarItemId);
		}
#endif
	}
}