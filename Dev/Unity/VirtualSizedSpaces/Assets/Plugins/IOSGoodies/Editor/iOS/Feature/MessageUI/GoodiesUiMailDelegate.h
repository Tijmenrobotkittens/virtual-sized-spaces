#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

@interface GoodiesUiMailDelegate : NSObject <MFMailComposeViewControllerDelegate>

@property(nonatomic, copy) void (^callbackSentSuccessfully)();

@property(nonatomic, copy) void (^callbackCancelled)();

@property(nonatomic, copy) void (^callbackFailed)();

@property(nonatomic, copy) void (^callbackSaved)();

@end
