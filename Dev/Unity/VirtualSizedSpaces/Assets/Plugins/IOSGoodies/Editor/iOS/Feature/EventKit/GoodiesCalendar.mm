#import <Foundation/Foundation.h>
#import <EventKit/EventKit.h>

#import "GoodiesUtils.h"
#import "BridgeGoodisFunctionDefs.h"

NSDateComponents *createDateComponentsFrom(int year, int month, int day, int hour, int minute);

NSDateComponents *createDateComponentsFrom(int year, int month, int day, int hour, int minute) {
	NSDateComponents *components = [[NSDateComponents alloc] init];
	[components setMinute:minute];
	[components setHour:hour];
	[components setDay:day];
	[components setMonth:month];
	[components setYear:year];
	return components;
}

NSDate *createDateFrom(int year, int month, int day, int hour, int minute);

NSDate *createDateFrom(int year, int month, int day, int hour, int minute) {
	NSDateComponents *components = createDateComponentsFrom(year, month, day, hour, minute);
	NSCalendar *calendar = [[NSCalendar alloc] initWithCalendarIdentifier:NSCalendarIdentifierGregorian];
	return [calendar dateFromComponents:components];
}

EKEvent *createBasicEvent(EKEventStore *store, NSString *title, NSString *notes, NSDate *startDate, NSDate *endDate);

EKEvent *createBasicEvent(EKEventStore *store, NSString *title, NSString *notes, NSDate *startDate, NSDate *endDate) {
	EKEvent *event = [EKEvent eventWithEventStore:store];
	event.title = title;
	event.notes = notes;
	event.startDate = startDate;
	event.endDate = endDate;
	event.calendar = [store defaultCalendarForNewEvents];
	return event;
}

EKReminder *createBasicReminder(EKEventStore *store, NSString *title, NSDateComponents *startDate, NSDateComponents *dueDate);

EKReminder *createBasicReminder(EKEventStore *store, NSString *title, NSDateComponents *startDate, NSDateComponents *dueDate) {
	EKReminder *reminder = [EKReminder reminderWithEventStore:store];
	reminder.title = title;
	reminder.startDateComponents = startDate;
	reminder.dueDateComponents = dueDate;
	reminder.calendar = [store defaultCalendarForNewReminders];
	reminder.completed = NO;
	return reminder;
}

EKRecurrenceRule *createRecurrencyRule(NSDate *repeatUntilDate, int ruleType, int interval);

EKRecurrenceRule *createRecurrencyRule(NSDate *repeatUntilDate, int ruleType, int interval) {
	EKRecurrenceFrequency frequency = EKRecurrenceFrequencyDaily;
	switch (ruleType) {
		case 0:
			frequency = EKRecurrenceFrequencyDaily;
			break;
		case 1:
			frequency = EKRecurrenceFrequencyWeekly;
			break;
		case 2:
			frequency = EKRecurrenceFrequencyMonthly;
			break;
		case 3:
			frequency = EKRecurrenceFrequencyYearly;
			break;
		default:
			break;
	}
	EKRecurrenceRule *rule = [[EKRecurrenceRule alloc]
			initRecurrenceWithFrequency:frequency
							   interval:interval
									end:[EKRecurrenceEnd recurrenceEndWithEndDate:repeatUntilDate]];
	return rule;
}

void saveEventToCalendar(EKEventStore *store, EKEvent *event, NSString *identifier,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr);

void saveEventToCalendar(EKEventStore *store, EKEvent *event, NSString *identifier,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr) {
	NSError *err = nil;
	[store saveEvent:event span:EKSpanThisEvent commit:YES error:&err];
	identifier = event.eventIdentifier;

	if (err != nil) {
		dispatch_async(dispatch_get_main_queue(), ^{
			callback(errorPtr, [GoodiesUtils createCStringFrom:err.localizedDescription]);
		});
	} else {
		dispatch_async(dispatch_get_main_queue(), ^{
			callback(successPtr, [GoodiesUtils createCStringFrom:identifier]);
		});
	}
}

void saveReminder(EKEventStore *store, EKReminder *reminder, NSString *identifier,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr);

void saveReminder(EKEventStore *store, EKReminder *reminder, NSString *identifier,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr) {
	NSError *err = nil;
	[store saveReminder:reminder commit:YES error:&err];
	identifier = reminder.calendarItemIdentifier;

	if (err != nil) {
		dispatch_async(dispatch_get_main_queue(), ^{
			callback(errorPtr, [GoodiesUtils createCStringFrom:err.localizedDescription]);
		});
	} else {
		dispatch_async(dispatch_get_main_queue(), ^{
			callback(successPtr, [GoodiesUtils createCStringFrom:identifier]);
		});
	}
}

EKCalendarItem *getCalendarItemWithId(NSString *itemId);

extern "C" {
NSString *identifier;

void _openCalendar() {
	NSURL *url = [NSURL URLWithString:@"calshow://"];
	[[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
}

void _openCalendarAt(int year, int month, int day) {
	NSDate *date = createDateFrom(year, month, day, 0, 0);
	NSInteger interval = [date timeIntervalSinceReferenceDate];
	NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"calshow:%ld", (long) interval]];
	[[UIApplication sharedApplication] openURL:url];
}

void _createCalendarEvent(const char *title, const char *notes, int startYear, int startMonth,
		int startDay, int startHour, int startMinute, int endYear,
		int endMonth, int endDay, int endHour, int endMinute,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr) {
	__block NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
	__block NSString *notesStr = [GoodiesUtils createNSStringFrom:notes];
	EKEventStore *store = [EKEventStore new];
	[store requestAccessToEntityType:EKEntityTypeEvent completion:^(BOOL granted, NSError *error) {
		if (!granted) {
			dispatch_async(dispatch_get_main_queue(), ^{
				callback(errorPtr, [GoodiesUtils createCStringFrom:error.localizedDescription]);
			});
			return;
		}
		EKEvent *event = createBasicEvent(store, titleStr, notesStr,
				createDateFrom(startYear, startMonth, startDay, startHour, startMinute),
				createDateFrom(endYear, endMonth, endDay, endHour, endMinute));

		saveEventToCalendar(store, event, identifier, callback, successPtr, errorPtr);
	}];
}

void _createRepeatingEvent(const char *title, const char *notes, int startYear,
		int startMonth, int startDay, int startHour, int startMinute,
		int endYear, int endMonth, int endDay, int endHour, int endMinute,
		int repeatUntilYear, int repeatUntilMonth, int repeatUntilDay,
		int repeatUntilHour, int repeatUntilMinute,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr,
		int ruleType, int interval) {
	__block NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
	__block NSString *notesStr = [GoodiesUtils createNSStringFrom:notes];
	EKEventStore *store = [EKEventStore new];
	[store requestAccessToEntityType:EKEntityTypeEvent completion:^(BOOL granted, NSError *error) {
		if (!granted) {
			dispatch_async(dispatch_get_main_queue(), ^{
				callback(errorPtr, [GoodiesUtils createCStringFrom:error.localizedDescription]);
			});
			return;
		}
		EKEvent *event = createBasicEvent(store, titleStr, notesStr,
				createDateFrom(startYear, startMonth, startDay, startHour, startMinute),
				createDateFrom(endYear, endMonth, endDay, endHour, endMinute));

		EKRecurrenceRule *rule = createRecurrencyRule(createDateFrom(repeatUntilYear, repeatUntilMonth, repeatUntilDay, repeatUntilHour, repeatUntilMinute), ruleType, interval);
		[event addRecurrenceRule:rule];

		saveEventToCalendar(store, event, identifier, callback, successPtr, errorPtr);
	}];
}

void _removeCalendarEvent(const char *eventId) {
	EKEventStore *store = [EKEventStore new];
	__block NSString *idStr = [GoodiesUtils createNSStringFrom:eventId];
	[store requestAccessToEntityType:EKEntityTypeEvent completion:^(BOOL granted, NSError *error) {
		if (!granted) {
			return;
		}
		EKEvent *eventToRemove = [store eventWithIdentifier:idStr];
		if (eventToRemove) {
			NSError *error = nil;
			[store removeEvent:eventToRemove span:EKSpanFutureEvents commit:YES error:&error];
		}
	}];
}

void _createReminder(const char *title,
		int startYear, int startMonth, int startDay, int startHour, int startMinute,
		int dueYear, int dueMonth, int dueDay, int dueHour, int dueMinute,
		ActionStringCallbackDelegate callback, void *successPtr, void *errorPtr) {
	__block NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
	EKEventStore *store = [EKEventStore new];
	[store requestAccessToEntityType:EKEntityTypeReminder completion:^(BOOL granted, NSError *error) {
		if (!granted) {
			dispatch_async(dispatch_get_main_queue(), ^{
				callback(errorPtr, [GoodiesUtils createCStringFrom:error.localizedDescription]);
			});
			return;
		}
		EKReminder *reminder = createBasicReminder(store, titleStr, createDateComponentsFrom(startYear, startMonth, startDay, startHour, startMinute), createDateComponentsFrom(dueYear, dueMonth, dueDay, dueHour, dueMinute));

		saveReminder(store, reminder, identifier, callback, successPtr, errorPtr);
	}];
}

void _completeReminder(const char *reminderId, bool complete) {
	EKEventStore *store = [EKEventStore new];

	EKReminder *reminder = (EKReminder *) [store calendarItemWithIdentifier:[GoodiesUtils createNSStringFrom:reminderId]];
	if (reminder) {
		reminder.completed = complete;

		NSError *err = nil;
		[store saveReminder:reminder commit:YES error:&err];
	}
}

void _deleteReminder(const char *reminderId) {
	EKEventStore *store = [EKEventStore new];
	EKReminder *reminder = (EKReminder *) [store calendarItemWithIdentifier:[GoodiesUtils createNSStringFrom:reminderId]];
	if (reminder) {
		NSError *err = nil;
		[store removeReminder:reminder commit:YES error:&err];
	}
}
}
