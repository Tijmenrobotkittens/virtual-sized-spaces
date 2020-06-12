#import <Foundation/Foundation.h>

@interface GoodiesImagePickerDelegate : NSObject <UIImagePickerControllerDelegate, UINavigationControllerDelegate> {
	float _compressionQuality;
}

@property(nonatomic, copy) void (^imagePicked)(const void *arrayPtr, int length);

@property(nonatomic, copy) void (^imagePickCancelled)();

- (instancetype)initWithCompressionQuality:(float)compressionQuality;

@end