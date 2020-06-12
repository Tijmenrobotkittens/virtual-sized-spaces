#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesImagePickerDelegate.h"
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"


extern "C" {
GoodiesImagePickerDelegate *imagePickerDelegate;

static void extracted(UIImagePickerController *pickerView) {
	if ([[UIDevice currentDevice] userInterfaceIdiom] == UIUserInterfaceIdiomPad) {
		pickerView.modalPresentationStyle = UIModalPresentationPopover;
		UIPopoverPresentationController *presentationController = [pickerView popoverPresentationController];
		presentationController.permittedArrowDirections = UIPopoverArrowDirectionAny;
		presentationController.sourceView = UnityGetGLView();
	}
}

void _pickImageFromGallery(
		ImageResultDelegate callback, void *callbackPtr,
		ActionVoidCallbackDelegate cancelCallback, void *cancelPtr,
		/*options*/
		int source,
		float compressionQuality,
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
		pickerView.mediaTypes = @[@"public.image"];

		[GoodiesUtils configureImagePickerForIpad:pickerView];

		// delegate
		imagePickerDelegate = [[GoodiesImagePickerDelegate alloc] initWithCompressionQuality:compressionQuality];
		imagePickerDelegate.imagePicked = ^(const void *arrayPtr, int length) {
			callback(callbackPtr, arrayPtr, length);
			[pickerView dismissViewControllerAnimated:NO completion:nil];
		};
		imagePickerDelegate.imagePickCancelled = ^() {
			[pickerView dismissViewControllerAnimated:YES completion:nil];
			if (cancelPtr) {
				cancelCallback(cancelPtr);
			}
		};
		pickerView.delegate = imagePickerDelegate;
		[GoodiesUtils configurePopoverWithPosition:posX posY:posY controller:pickerView];
		[UnityGetGLViewController() presentViewController:pickerView animated:YES completion:^{

		}];
	} else {
		NSLog(@"Picking image from gallery not available on current device");
	}
}
}


#pragma clang diagnostic pop
