#import "GoodiesUtils.h"
#import "GoodiesSelectorsContainer.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"


extern "C" {
GoodiesSelectorsContainer *saveImageCallbacks;

// TODO finish in anyone ever needs a callback
void _saveImageToGallery(const void *data,
		const unsigned long data_length) {
	if (data_length > 0) {
		UIImage *image = [GoodiesUtils createImageFromByteArray:data dataLength:data_length];
		saveImageCallbacks = [GoodiesSelectorsContainer new];
		saveImageCallbacks.onImageSaved = ^{
			NSLog(@"Image successfully saved");
		};
		saveImageCallbacks.onError = ^{
			NSLog(@"Failed to save image");
		};

		UIImageWriteToSavedPhotosAlbum(image, saveImageCallbacks, @selector(thisImage:hasBeenSavedInPhotoAlbumWithError:usingContextInfo:), nil);
	}
}
}

#pragma clang diagnostic pop
