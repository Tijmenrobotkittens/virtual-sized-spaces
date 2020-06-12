#import "GoodiesAlertViewDelegate.h"
#import "BridgeGoodisFunctionDefs.h"
#import "Util/GoodiesUtils.h"
#import "Delegate/GoodiesFilePickerDelegate.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

extern "C" {

GoodiesFilePickerDelegate *pickerDelegate;

void _iosGoodiesPickFiles(const char *fileTypes,
		ActionStringCallbackDelegate successCallback,
		ActionVoidCallbackDelegate cancelCallback,
		void *onSuccessActionPtr,
		void *onCancelActionPtr,
		bool allowMultiple,
		int type,
		const char *paths) {
	NSString *typesStr = [GoodiesUtils createNSStringFrom:fileTypes];
	NSArray<NSString *> *types = [typesStr componentsSeparatedByString:@","];
	UIDocumentPickerMode mode;
	switch (type) {
		case 0:
			mode = UIDocumentPickerModeImport;
			break;
		case 1:
			mode = UIDocumentPickerModeOpen;
			break;
		case 2:
			mode = UIDocumentPickerModeMoveToService;
			break;
		case 3:
			mode = UIDocumentPickerModeExportToService;
			break;
		default:
			mode = UIDocumentPickerModeImport;
			break;
	}
	UIDocumentPickerViewController *documentPicker = [UIDocumentPickerViewController alloc];
	if (mode == UIDocumentPickerModeImport || mode == UIDocumentPickerModeOpen) {
		documentPicker = [documentPicker initWithDocumentTypes:types inMode:mode];
	} else {
		NSString *pathsStr = [GoodiesUtils createNSStringFrom:paths];
		NSArray<NSString *> *pathsStrArray = [pathsStr componentsSeparatedByString:@","];
		NSMutableArray<NSURL *> *urls = [[NSMutableArray<NSURL *> alloc] init];
		for (int i = 0; i < pathsStrArray.count; i++) {
			[urls addObject:[NSURL fileURLWithPath:pathsStrArray[i]]];
		}

		if (@available(iOS 11.0, *)) {
			documentPicker = [documentPicker initWithURLs:urls inMode:mode];
		} else {
			documentPicker = [documentPicker initWithURL:urls[0] inMode:mode];
		}
	}

	if (@available(iOS 11.0, *)) {
		documentPicker.allowsMultipleSelection = allowMultiple;
	}
	pickerDelegate = [GoodiesFilePickerDelegate new];
	pickerDelegate.callbackPickedSuccessfully = ^(NSString *fileURLs) {
		successCallback(onSuccessActionPtr, fileURLs.UTF8String);
	};
	pickerDelegate.callbackCancelled = ^{
		cancelCallback(onCancelActionPtr);
	};
	documentPicker.delegate = pickerDelegate;

	[UnityGetGLViewController() presentViewController:documentPicker animated:YES completion:NULL];
}
}

#pragma clang diagnostic pop
