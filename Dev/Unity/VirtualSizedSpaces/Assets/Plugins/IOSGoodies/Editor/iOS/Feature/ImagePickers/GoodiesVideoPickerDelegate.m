#import "GoodiesVideoPickerDelegate.h"

@implementation GoodiesVideoPickerDelegate {
}

- (void)imagePickerController:(UIImagePickerController *)picker didFinishPickingMediaWithInfo:(NSDictionary<UIImagePickerControllerInfoKey, id> *)info {
	NSURL *videoUrl = (NSURL *) info[UIImagePickerControllerMediaURL];
	NSString *moviePath = [videoUrl path];

	if (_videoPicked) {
		_videoPicked(moviePath);
	}
}

- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
	if (_videoPickCancelled) {
		_videoPickCancelled();
	}
}


@end