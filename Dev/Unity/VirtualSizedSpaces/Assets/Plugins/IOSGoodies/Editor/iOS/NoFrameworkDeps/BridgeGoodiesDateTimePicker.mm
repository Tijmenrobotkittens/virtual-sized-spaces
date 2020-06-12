#import "GoodiesDateTimePicker.h"
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

GoodiesDateTimePicker *newPicker(void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
		void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType, int minuteInterval) {
	if (@available(iOS 13, *)) {
		UnityGetGLView().window.overrideUserInterfaceStyle = UIUserInterfaceStyleLight;
	}
	return [[GoodiesDateTimePicker alloc] initWithCallbackPtr:callbackPtr
	                                   onDateSelectedDelegate:onDateSelectedDelegate
	                                              onCancelPtr:cancelPtr
	                                         onCancelDelegate:onCancel
	                                           datePickerType:datePickerType
	                                           minuteInterval:minuteInterval];
}

GoodiesDateTimePicker *createDateTimePickerWithInitialValues(void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
		void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType,
		int year, int month, int day, int hourOfDay, int minute, int minuteInterval) {
	GoodiesDateTimePicker *pickerController = newPicker(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel,
			datePickerType, minuteInterval);
	[pickerController setInitialValuesWithYear:year
	                                     month:month day:day hour:hourOfDay minute:minute];
	return pickerController;
}

extern "C" {

GoodiesDateTimePicker *pickerController;

void _showDatePickerWithInitialValue(
		int year, int month, int day, int hourOfDay, int minute,
		void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
		void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType, int minuteInterval) {
	pickerController = createDateTimePickerWithInitialValues(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel,
			datePickerType, year, month, day, hourOfDay, minute, minuteInterval);
	[pickerController showPicker];
}

void _showDatePicker(
		void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
		void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType, int minuteInterval) {
	pickerController = newPicker(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel, datePickerType, minuteInterval);

	[pickerController setInitialValueToNow];
	[pickerController showPicker];
}

void _showDatePickerWithRestrains(
		int year, int month, int day, int hourOfDay, int minute,
		void *callbackPtr, OnDateSelectedDelegate *onDateSelectedDelegate,
		void *cancelPtr, ActionVoidCallbackDelegate onCancel, int datePickerType,
		int minYear, int minMonth, int minDay, int minHourOfDay, int minMinute,
		int maxYear, int maxMonth, int maxDay, int maxHourOfDay, int maxMinute, int minuteInterval) {
	pickerController = createDateTimePickerWithInitialValues(callbackPtr, onDateSelectedDelegate, cancelPtr, onCancel,
			datePickerType, year, month, day, hourOfDay, minute, minuteInterval);
	NSDate *minDate = [GoodiesUtils setDateFromYear:minYear month:minMonth day:minDay hour:minHourOfDay minute:minMinute];
	NSDate *maxDate = [GoodiesUtils setDateFromYear:maxYear month:maxMonth day:maxDay hour:maxHourOfDay minute:maxMinute];
	[pickerController showPickerWithMinDate:minDate MaxDate:maxDate];
}
}
#pragma clang diagnostic pop
