#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"
#import "GoodiesVideoEditDelegate.h"

extern "C" {
GoodiesVideoEditDelegate *delegate;

void _editVideoAtPath(const char *path, ActionStringCallbackDelegate successCallback,
		void *onFinishEditPtr, int maxDuration, int quality) {
	UIVideoEditorController *videoEditor = [[UIVideoEditorController alloc] init];

	delegate = [GoodiesVideoEditDelegate new];
	delegate.callbackFinishedEditing = ^(NSString *_Nonnull path) {
		successCallback(onFinishEditPtr, [GoodiesUtils createCStringFrom:path]);
	};

	NSString *videoPath = [GoodiesUtils createNSStringFrom:path];

	if ([UIVideoEditorController canEditVideoAtPath:videoPath]) {
		UIImagePickerControllerQualityType type = (UIImagePickerControllerQualityType) quality;

		videoEditor.videoPath = videoPath;
		videoEditor.videoMaximumDuration = maxDuration;
		videoEditor.videoQuality = type;
		videoEditor.delegate = delegate;

		[GoodiesUtils configureImagePickerForIpad:videoEditor];

		[UnityGetGLViewController() presentViewController:videoEditor animated:true completion:nil];
	} else {
		NSLog(@"can't edit video at %@", videoPath);
	}
}

bool _canEditVideoAtPath(const char *path) {
	NSString *videoPath = [GoodiesUtils createNSStringFrom:path];
	return [UIVideoEditorController canEditVideoAtPath:videoPath];
}
}

#pragma clang diagnostic pop
