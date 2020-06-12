#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"
#import "GoodiesUiMessageDelegate.h"
#import "GoodiesUiMailDelegate.h"

extern "C" {

GoodiesUiMessageDelegate *msgDelegate;
GoodiesUiMailDelegate *mailDelegate;

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
void _sendSms(const char *phoneNumber, const char *msgText,
		ActionVoidCallbackDelegate callback,
		void *onSuccessActionPtr,
		void *onCancelActionPtr,
		void *onFailureActionPtr) {
	NSString *phoneNumberStr = [GoodiesUtils createNSStringFrom:phoneNumber];
	NSString *msgTextStr = [GoodiesUtils createNSStringFrom:msgText];

	if (![MFMessageComposeViewController canSendText]) {
		NSLog(@"The device does not support sending SMS!");
		return;
	}

	msgDelegate = [GoodiesUiMessageDelegate new];
	msgDelegate.callbackSentSuccessfully = ^{
		callback(onSuccessActionPtr);
	};
	msgDelegate.callbackCancelled = ^{
		callback(onCancelActionPtr);
	};
	msgDelegate.callbackFailed = ^{
		callback(onFailureActionPtr);
	};
	MFMessageComposeViewController *messageController = [[MFMessageComposeViewController alloc] init];
	messageController.messageComposeDelegate = msgDelegate;

	NSArray *recipients = @[phoneNumberStr];
	[messageController setRecipients:recipients];
	[messageController setBody:msgTextStr];

	// Present message view controller on screen
	[UnityGetGLViewController() presentViewController:messageController animated:YES completion:nil];
}

void _sendEmail(const char *subject, const char *body,
		const void *data, const unsigned long data_length,
		const char *recipients, const char *cc, const char *bcc,
		ActionVoidCallbackDelegate callback, void *onSuccessActionPtr,
		void *onCancelActionPtr, void *onFailureActionPtr, void *onSavedActionPtr) {
	if ([MFMailComposeViewController canSendMail]) {
		MFMailComposeViewController *mailVC = [[MFMailComposeViewController alloc] init];

		mailDelegate = [GoodiesUiMailDelegate new];
		mailDelegate.callbackSentSuccessfully = ^{
			callback(onSuccessActionPtr);
		};
		mailDelegate.callbackSaved = ^{
			callback(onSavedActionPtr);
		};
		mailDelegate.callbackFailed = ^{
			callback(onFailureActionPtr);
		};
		mailDelegate.callbackCancelled = ^{
			callback(onCancelActionPtr);
		};
		mailVC.mailComposeDelegate = mailDelegate;

		[mailVC setSubject:[GoodiesUtils createNSStringFrom:subject]];
		[mailVC setMessageBody:[GoodiesUtils createNSStringFrom:body] isHTML:NO];
		if (data_length > 0) {
			UIImage *image = [GoodiesUtils createImageFromByteArray:data dataLength:data_length];
			NSData *dataForImage = UIImagePNGRepresentation(image);
			[mailVC addAttachmentData:dataForImage mimeType:@"image/png" fileName:@"Image"];
		}

		NSArray *recipientsArray = [[GoodiesUtils createNSStringFrom:recipients] componentsSeparatedByString:@","];
		NSArray *ccArray = [[GoodiesUtils createNSStringFrom:cc] componentsSeparatedByString:@","];
		NSArray *bccArray = [[GoodiesUtils createNSStringFrom:bcc] componentsSeparatedByString:@","];

		[mailVC setToRecipients:recipientsArray];
		if (ccArray.count > 0) {
			[mailVC setCcRecipients:ccArray];
		}
		if (bccArray.count > 0) {
			[mailVC setBccRecipients:bccArray];
		}
		dispatch_async(dispatch_get_main_queue(), ^{
			[UnityGetGLViewController() presentViewController:mailVC animated:YES completion:NULL];
		});
	} else {
		callback(onFailureActionPtr);
	}
}
#pragma clang diagnostic pop
}
