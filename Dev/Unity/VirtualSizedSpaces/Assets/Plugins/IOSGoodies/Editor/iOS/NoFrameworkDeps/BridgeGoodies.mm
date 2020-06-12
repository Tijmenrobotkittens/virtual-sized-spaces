#import "GoodiesAlertViewDelegate.h"
#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

extern "C" {
	
	GoodiesAlertViewDelegate *handler;
	UIActivityIndicatorView *loadingDialog;
	UIView *coverView;
	
	void _showConfirmationDialog(const char *title, const char *message,
								 const char *buttonTitle,
								 ActionVoidCallbackDelegate callback,
								 void *onSuccessActionPtr) {
		NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
		NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
		NSString *buttonTitleStr = [GoodiesUtils createNSStringFrom:buttonTitle];
		
		handler = [GoodiesAlertViewDelegate new];
		handler.callbackButtonClicked = ^(long index) {
			callback(onSuccessActionPtr);
		};
		
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:titleStr
														message:messageStr
													   delegate:handler
											  cancelButtonTitle:nil
											  otherButtonTitles:buttonTitleStr, nil];
		[alert show];
	}
	
	void _showQuestionDialog(const char *title, const char *message,
							 const char *buttonOkTitle,
							 const char *buttonCancelTitle,
							 ActionVoidCallbackDelegate callback,
							 void *onSuccessActionPtr, void *onCancelActionPtr) {
		
		NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
		NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
		NSString *buttonTitleStr = [GoodiesUtils createNSStringFrom:buttonOkTitle];
		NSString *buttonCancelStr =
		[GoodiesUtils createNSStringFrom:buttonCancelTitle];
		
		handler = [GoodiesAlertViewDelegate new];
		handler.callbackButtonClicked = ^(long index) {
			if (index == 0) {
				callback(onCancelActionPtr);
			} else {
				callback(onSuccessActionPtr);
			}
		};
		
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:titleStr
														message:messageStr
													   delegate:handler
											  cancelButtonTitle:buttonCancelStr
											  otherButtonTitles:buttonTitleStr, nil];
		[alert show];
	}
	
	void _showOptionalDialog(const char *title, const char *message,
							 const char *buttonFirst,
							 const char *buttonSecond,
							 const char *buttonCancel,
							 ActionVoidCallbackDelegate callback,
							 void *onFirstButtonActionPtr,
							 void *onSecondButtonActionPtr,
							 void *onCancelActionPtr) {
		
		NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
		NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
		
		NSString *buttonCancelStr = [GoodiesUtils createNSStringFrom:buttonCancel];
		NSString *buttonFirstStr = [GoodiesUtils createNSStringFrom:buttonFirst];
		NSString *buttonSecondStr = [GoodiesUtils createNSStringFrom:buttonSecond];
		
		handler = [GoodiesAlertViewDelegate new];
		handler.callbackButtonClicked = ^(long index) {
			switch (index) {
				case 0:
					callback(onCancelActionPtr);
					break;
					
				case 1:
					callback(onFirstButtonActionPtr);
					break;
					
				default:
					callback(onSecondButtonActionPtr);
					break;
			}
		};
		
		UIAlertView *alert =
		[[UIAlertView alloc] initWithTitle:titleStr
								   message:messageStr
								  delegate:handler
						 cancelButtonTitle:buttonCancelStr
						 otherButtonTitles:buttonFirstStr, buttonSecondStr, nil];
		[alert show];
	}
	
	void _showLoadingDialog() {
		loadingDialog = [[UIActivityIndicatorView alloc] initWithActivityIndicatorStyle: UIActivityIndicatorViewStyleWhiteLarge];
		loadingDialog.frame = CGRectMake(0, 0, 100, 100);
		UIView *view = UnityGetGLView();
		loadingDialog.center = view.center;
		[loadingDialog setHidden:NO];
		
		CGRect viewRect = [[UIScreen mainScreen] bounds];
		coverView = [[UIView alloc] initWithFrame:viewRect];
		coverView.backgroundColor = [[UIColor blackColor] colorWithAlphaComponent:0.8];
		[view addSubview:coverView];
		[view addSubview:loadingDialog];
		
		[loadingDialog bringSubviewToFront:view];
		[loadingDialog startAnimating];
		
		[[UIApplication sharedApplication] beginIgnoringInteractionEvents];
	}
	
	void _dismissLoadingDialog() {
		if(loadingDialog != nil)
		{
			[loadingDialog stopAnimating];
			[loadingDialog setHidden:YES];
			[loadingDialog removeFromSuperview];
			if(coverView != nil)
			{
				[coverView removeFromSuperview];
			}
		}
		
		[[UIApplication sharedApplication] endIgnoringInteractionEvents];
	}
	
	void _showInputFieldDialog(const char *title,
							   const char *text,
							   const char *inputPlaceHolder,
							   const char *buttonOkTitle,
							   const char *buttonCancelTitle,
							   ActionVoidCallbackDelegate cancelCallback,
							   ActionStringCallbackDelegate successCallback,
							   void *cancelPtr,
							   void *successPtr) {
		UIAlertController *dialog = [UIAlertController alertControllerWithTitle:[GoodiesUtils createNSStringFrom:title] message:[GoodiesUtils createNSStringFrom:text] preferredStyle:UIAlertControllerStyleAlert];
		
		[dialog addTextFieldWithConfigurationHandler:^(UITextField *textField) {
			textField.placeholder = [GoodiesUtils createNSStringFrom:inputPlaceHolder];
			textField.textColor = [UIColor blackColor];
			textField.clearButtonMode = UITextFieldViewModeWhileEditing;
			textField.borderStyle = UITextBorderStyleRoundedRect;
		}];
		
		UIAlertAction *okAction = [UIAlertAction actionWithTitle:[GoodiesUtils createNSStringFrom:buttonOkTitle] style:UIAlertActionStyleDefault handler: ^(UIAlertAction *action) {
			char* text = [GoodiesUtils createCStringFrom: dialog.textFields[0].text];
			successCallback(successPtr, text);
		}];
		
		UIAlertAction *cancelAction = [UIAlertAction actionWithTitle:[GoodiesUtils createNSStringFrom:buttonCancelTitle] style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
			cancelCallback(cancelPtr);
		}];
		
		[dialog addAction:okAction];
		[dialog addAction:cancelAction];
		
		UIViewController *viewController = UnityGetGLViewController();
		[viewController presentViewController:dialog animated:true completion:nil];
	}
	
	void _showShareMessageWithImage(const char *message, const void *data,
									const unsigned long data_length,
									int posX, int posY,
									ActionStringCallbackDelegate callback,
									void *successPtr, void *errPtr) {
		
		NSMutableArray *array = [NSMutableArray new];
		
		if (message != nil) {
			NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
			[array addObject:messageStr];
		}
		
		if (data_length > 0) {
			UIImage *image = [GoodiesUtils createImageFromByteArray:data dataLength:data_length];
			[array addObject:image];
		}
		
		UIActivityViewController *controller =
		[[UIActivityViewController alloc] initWithActivityItems:array
										  applicationActivities:nil];
		[GoodiesUtils configurePopoverWithPosition:posX posY:posY controller:controller];
		UIActivityViewController *weakController = controller;
		
		[UnityGetGLViewController() presentViewController:controller
												 animated:true
											   completion:nil];
		
		[controller setCompletionWithItemsHandler:^(UIActivityType activityType, BOOL completed, NSArray *returnedItems, NSError *activityError) {
			if (activityError != nil) {
				callback(errPtr, [GoodiesUtils createCStringFrom:activityError.localizedDescription]);
			} else {
				callback(successPtr, [GoodiesUtils createCStringFrom:activityType]);
			}
			weakController.completionWithItemsHandler = nil;
		}];
	}
	
	void _showShareMessageWithLink(const char *message, const char *link,
								   int posX, int posY,
								   ActionStringCallbackDelegate callback,
								   void *successPtr, void *errPtr) {
		NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
		NSString *linkStr = [GoodiesUtils createNSStringFrom:link];
		
		NSURL *url = [NSURL URLWithString:linkStr];
		NSArray *array = @[messageStr, url];
		
		UIActivityViewController *controller =
		[[UIActivityViewController alloc] initWithActivityItems:array
										  applicationActivities:nil];
		[GoodiesUtils configurePopoverWithPosition:posX posY:posY controller:controller];
		UIActivityViewController *weakController = controller;
		
		[UnityGetGLViewController() presentViewController:controller
												 animated:true
											   completion:nil];
		
		[controller setCompletionWithItemsHandler:^(UIActivityType activityType, BOOL completed, NSArray *returnedItems, NSError *activityError) {
			if (activityError != nil) {
				callback(errPtr, [GoodiesUtils createCStringFrom:activityError.localizedDescription]);
			} else {
				callback(successPtr, [GoodiesUtils createCStringFrom:activityType]);
			}
			weakController.completionWithItemsHandler = nil;
		}];
	}
	
	void _showActionSheet(const char *title, const char *message, const char *cancelButtonTitle,
						  const char *destructiveButtonTitle, const char *otherBtnTitles,
                          int posX, int posY,
						  ActionIntCallbackDelegate buttonCallback, void *buttonPtr,
						  ActionVoidCallbackDelegate callback, void *cancelPtr, void *destructivePtr) {
		NSString *titleStr = [GoodiesUtils createNSStringFrom:title];
		NSString *messageStr = [GoodiesUtils createNSStringFrom:message];
		NSString *cancelButtonTitleStr = [GoodiesUtils createNSStringFrom:cancelButtonTitle];
		NSString *destructiveButtonTitleStr = [GoodiesUtils createNSStringFrom:destructiveButtonTitle];
		NSMutableArray<NSString *> *buttonItems = [[NSMutableArray<NSString*> alloc] init];
		NSString *otherBtnTitlesStr = [GoodiesUtils createNSStringFrom:otherBtnTitles];
		
		UIAlertController *actionSheet = [UIAlertController alertControllerWithTitle:titleStr message:messageStr preferredStyle:UIAlertControllerStyleActionSheet];
        [GoodiesUtils configurePopoverWithPosition:posX posY:posY controller:actionSheet];
		
		if(destructiveButtonTitleStr.length > 0) {
			[actionSheet addAction: [UIAlertAction actionWithTitle:destructiveButtonTitleStr style:UIAlertActionStyleDestructive handler:^(UIAlertAction *action) {
				callback(destructivePtr);
			}]];
		}
		
		if ([otherBtnTitlesStr length] > 0) {
			[buttonItems addObjectsFromArray:[otherBtnTitlesStr componentsSeparatedByString:@"|"]];
			for (int i = 0; i < buttonItems.count; i++) {
				[actionSheet addAction:[UIAlertAction actionWithTitle:buttonItems[i] style:UIAlertActionStyleDefault handler:^(UIAlertAction *action) {
					buttonCallback(buttonPtr, i);
				}]];
			}
		}
		
		[actionSheet addAction:[UIAlertAction actionWithTitle:cancelButtonTitleStr style:UIAlertActionStyleCancel handler:^(UIAlertAction *action) {
			callback(cancelPtr);
		}]];
		
		[UnityGetGLViewController() presentViewController:actionSheet animated:YES completion:nil];
	}
	
	void _openUrl(const char *link) {
		NSString *linkStr = [GoodiesUtils createNSStringFrom:link];
		NSURL *url = [NSURL URLWithString:linkStr];
		
		UIApplication *application = [UIApplication sharedApplication];
		if ([application respondsToSelector:@selector(openURL:options:completionHandler:)]) {
			[application openURL:url
						 options:@{}
			   completionHandler:nil];
		} else {
			[application openURL:url];
		}
	}
	
	void _openYouTubeVideo(const char* id) {
		NSString *idStr = [GoodiesUtils createNSStringFrom:id];
		NSString *urlString = [NSString stringWithFormat:@"youtube://%@", idStr];
		NSURL *url = [NSURL URLWithString:urlString];
		if (![[UIApplication sharedApplication] canOpenURL:url]) {
			NSString *urlStringFallback = [NSString stringWithFormat:@"http://www.youtube.com/watch?v=%@", idStr];
			url = [NSURL URLWithString:urlStringFallback];
		}
		[[UIApplication sharedApplication] openURL:url options:@{} completionHandler:nil];
	}
	
	void _goodiesOpenAppSettings() {
		NSURL *_Nullable settingsUrl = [NSURL URLWithString:UIApplicationOpenSettingsURLString];
		[[UIApplication sharedApplication] openURL:settingsUrl];
	}
}

#pragma clang diagnostic pop
