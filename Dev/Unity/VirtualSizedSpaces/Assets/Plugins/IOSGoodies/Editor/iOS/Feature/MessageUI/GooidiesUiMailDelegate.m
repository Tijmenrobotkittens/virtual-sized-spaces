#include "GoodiesUiMailDelegate.h"

@implementation GoodiesUiMailDelegate
- (void)mailComposeController:(MFMailComposeViewController *)controller didFinishWithResult:(MFMailComposeResult)result error:(NSError *)error {
	switch (result) {
		case MFMailComposeResultSent:
			_callbackSentSuccessfully();
			break;
		case MFMailComposeResultSaved:
			_callbackSaved();
			break;
		case MFMailComposeResultCancelled:
			_callbackCancelled();
			break;
		case MFMailComposeResultFailed:
			_callbackFailed();
			break;
		default:
			_callbackFailed();
			break;
	}

	[controller dismissViewControllerAnimated:YES completion:NULL];
}
@end
