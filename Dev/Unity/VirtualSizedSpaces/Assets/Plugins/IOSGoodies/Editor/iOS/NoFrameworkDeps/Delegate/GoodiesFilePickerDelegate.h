#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface GoodiesFilePickerDelegate : NSObject <UIDocumentPickerDelegate>

@property(nonatomic, copy) void (^callbackPickedSuccessfully)(NSString *fileURLs);
@property(nonatomic, copy) void (^callbackCancelled)();

@end

NS_ASSUME_NONNULL_END
