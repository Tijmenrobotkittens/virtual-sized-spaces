#import "GoodiesFilePickerDelegate.h"

@implementation GoodiesFilePickerDelegate

- (void)documentPicker:(UIDocumentPickerViewController *)controller didPickDocumentsAtURLs:(NSArray<NSURL *> *)urls {
	if (urls.count == 0) {
		return;
	}

	NSMutableArray<NSString *> *paths = [[NSMutableArray<NSString *> alloc] init];
	switch (controller.documentPickerMode) {

		case UIDocumentPickerModeOpen: {
			for (int i = 0; i < urls.count; i++) {
				NSURL *url = urls[i];
				if ([url startAccessingSecurityScopedResource]) {
					[paths addObject:url.path];
				}
			}
			_callbackPickedSuccessfully([paths componentsJoinedByString:@","]);
			for (int i = 0; i < urls.count; i++) {
				[urls[i] stopAccessingSecurityScopedResource];
			}
			break;
		}

		default: {
			for (int i = 0; i < urls.count; i++) {
				[paths addObject:urls[i].path];
			}
			NSString *result = [paths componentsJoinedByString:@","];
			_callbackPickedSuccessfully(result);
			break;
		}
	}

}

- (void)documentPickerWasCancelled:(UIDocumentPickerViewController *)controller {
	_callbackCancelled();
}

@end
