#import "GoodiesVideoEditDelegate.h"

@implementation GoodiesVideoEditDelegate

- (void)videoEditorController:(UIVideoEditorController *)editor
     didSaveEditedVideoToPath:(NSString *)editedVideoPath {
	[editor dismissViewControllerAnimated:YES completion:nil];
	_callbackFinishedEditing(editedVideoPath);
}

- (void)videoEditorController:(UIVideoEditorController *)editor didFailWithError:(NSError *)error {
	[editor dismissViewControllerAnimated:YES completion:nil];
	NSLog(@"Editing failed");
}

- (void)videoEditorControllerDidCancel:(UIVideoEditorController *)editor {
	[editor dismissViewControllerAnimated:YES completion:nil];
	NSLog(@"Editing was cancelled");
}

@end
