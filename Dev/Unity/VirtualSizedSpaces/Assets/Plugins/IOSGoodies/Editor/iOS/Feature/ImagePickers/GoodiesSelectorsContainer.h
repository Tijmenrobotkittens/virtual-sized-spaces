#import <Foundation/Foundation.h>

@interface GoodiesSelectorsContainer : NSObject

@property(nonatomic, copy) void (^onImageSaved)();

@property(nonatomic, copy) void (^onError)();

- (void)thisImage:(UIImage *)image hasBeenSavedInPhotoAlbumWithError:(NSError *)error usingContextInfo:(void *)ctxInfo;

@end

