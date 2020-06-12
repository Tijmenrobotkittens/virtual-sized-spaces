#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesImagePickerDelegate.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

extern "C" {

GoodiesImagePickerDelegate *photoPickerDelegate;

void _pickImageFromCamera(
		ImageResultDelegate callback, void *callbackPtr,
		ActionVoidCallbackDelegate cancelCallback, void *cancelPtr,
		// options
		float compressionQuality,
		bool allowsEditing,
		bool rearCamera /*rear, front*/,
		int flashMode) {

	if ([UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera]) {
		UIImagePickerController *pickerView = [[UIImagePickerController alloc] init];

		pickerView.sourceType = UIImagePickerControllerSourceTypeCamera;
		pickerView.cameraDevice = rearCamera ? UIImagePickerControllerCameraDeviceRear : UIImagePickerControllerCameraDeviceFront;
		pickerView.cameraFlashMode = (UIImagePickerControllerCameraFlashMode) flashMode;
		pickerView.allowsEditing = allowsEditing;

		// delegate
		photoPickerDelegate = [[GoodiesImagePickerDelegate alloc] initWithCompressionQuality:compressionQuality];
		photoPickerDelegate.imagePicked = ^(const void *arrayPtr, int length) {
			callback(callbackPtr, arrayPtr, length);
			[pickerView dismissViewControllerAnimated:NO completion:nil];
		};
		photoPickerDelegate.imagePickCancelled = ^() {
			[pickerView dismissViewControllerAnimated:YES completion:nil];
			if (cancelPtr) {
				cancelCallback(cancelPtr);
			}
		};
		pickerView.delegate = photoPickerDelegate;
		[UnityGetGLViewController() presentViewController:pickerView animated:YES completion:^{
		}];
	} else {
		NSLog(@"Picking image from camera not available on current device");
	}
}
}

#pragma clang diagnostic pop
