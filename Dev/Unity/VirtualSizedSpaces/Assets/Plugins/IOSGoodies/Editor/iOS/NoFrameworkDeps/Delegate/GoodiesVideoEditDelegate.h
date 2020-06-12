//
//  GoodiesVideoEditDelegate.h
//  Unity-iPhone
//
//  Created by Taras Leskiv on 10/26/19.
//

#import <Foundation/Foundation.h>

NS_ASSUME_NONNULL_BEGIN

@interface GoodiesVideoEditDelegate : NSObject <UINavigationControllerDelegate, UIVideoEditorControllerDelegate>

@property(nonatomic, copy) void (^callbackFinishedEditing)(NSString *path);
@end

NS_ASSUME_NONNULL_END
