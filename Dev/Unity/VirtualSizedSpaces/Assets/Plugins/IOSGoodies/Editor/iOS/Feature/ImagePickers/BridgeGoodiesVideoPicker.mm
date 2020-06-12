#import <MobileCoreServices/MobileCoreServices.h>
#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"
#import "GoodiesVideoPickerDelegate.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

extern "C" {
GoodiesVideoPickerDelegate *videoPickerDelegate;

void _pickVideoFromGallery(
		ActionStringCallbackDelegate callback, void *callbackPtr,
		ActionVoidCallbackDelegate cancelCallback, void *cancelPtr,
		/*options*/
		int source,
		bool allowsEditing,
		int posX, int posY) {
	const int sourceTypePhotoLibrary = 0;

	UIImagePickerControllerSourceType sourceType;
	if (source == sourceTypePhotoLibrary) {
		sourceType = UIImagePickerControllerSourceTypePhotoLibrary;
	} else {
		sourceType = UIImagePickerControllerSourceTypeSavedPhotosAlbum;
	}

	if ([UIImagePickerController isSourceTypeAvailable:sourceType]) {
		UIImagePickerController *pickerView = [[UIImagePickerController alloc] init];
		pickerView.sourceType = sourceType;
		pickerView.allowsEditing = allowsEditing;
		pickerView.mediaTypes = @[(NSString *) kUTTypeMovie];

		[GoodiesUtils configureImagePickerForIpad:pickerView];

		// delegate
		videoPickerDelegate = [[GoodiesVideoPickerDelegate alloc] init];
		videoPickerDelegate.videoPicked = ^(NSString *path) {
			[pickerView dismissViewControllerAnimated:NO completion:nil];
			callback(callbackPtr, path.UTF8String);
		};
		videoPickerDelegate.videoPickCancelled = ^() {
			[pickerView dismissViewControllerAnimated:YES completion:nil];
			if (cancelPtr) {
				cancelCallback(cancelPtr);
			}
		};
		pickerView.delegate = videoPickerDelegate;
		[GoodiesUtils configurePopoverWithPosition:posX posY:posY controller:pickerView];
		[UnityGetGLViewController() presentViewController:pickerView animated:YES completion:^{

		}];
	} else {
		NSLog(@"Picking video from gallery not available on current device");
	}
}
}


#pragma clang diagnostic pop
