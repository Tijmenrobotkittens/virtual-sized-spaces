#import <Foundation/Foundation.h>

@interface GoodiesVideoPickerDelegate : NSObject <UIImagePickerControllerDelegate, UINavigationControllerDelegate>

@property(nonatomic, copy) void (^videoPicked)(NSString *videoPath);

@property(nonatomic, copy) void (^videoPickCancelled)();

@end