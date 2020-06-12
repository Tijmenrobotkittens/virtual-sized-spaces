@interface GoodiesAlertViewDelegate : NSObject <UIAlertViewDelegate>

@property(nonatomic, copy) void (^callbackButtonClicked)(long index);

@end
