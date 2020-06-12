#import "GoodiesSelectorsContainer.h"

@implementation GoodiesSelectorsContainer

- (void)thisImage:(UIImage *)image hasBeenSavedInPhotoAlbumWithError:(NSError *)error usingContextInfo:(void *)ctxInfo {
	if (error) {
		// TODO finish in anyone ever needs a callback
		_onError();
	} else {
		// TODO finish in anyone ever needs a callback
		_onImageSaved();
	}
}

@end
