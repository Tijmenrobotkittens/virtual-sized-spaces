#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"
#import <Foundation/Foundation.h>
#import <LocalAuthentication/LocalAuthentication.h>

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"

extern "C" {
bool _isLocalAuthenticationAvailable() {
	LAContext *context = [[LAContext alloc] init];
	NSError *error = nil;

	return [context canEvaluatePolicy:LAPolicyDeviceOwnerAuthenticationWithBiometrics error:&error];
}

void _authenticateWithBiometrics(const char *reason, int policy,
		ActionVoidCallbackDelegate successCallBack,
		ActionIntCallbackDelegate failureCallBack,
		void *successPtr, void *failurePtr) {
	LAContext *context = [[LAContext alloc] init];
	LAPolicy authPolicy;

	if (policy == 0) {
		authPolicy = LAPolicyDeviceOwnerAuthenticationWithBiometrics;
	} else {
		authPolicy = LAPolicyDeviceOwnerAuthentication;
	}

	[context evaluatePolicy:authPolicy localizedReason:[GoodiesUtils createNSStringFrom:reason] reply:^(BOOL success, NSError *error) {
		dispatch_async(dispatch_get_main_queue(), ^{
			if (success) {
				successCallBack(successPtr);
			} else {
				failureCallBack(failurePtr, (int) error.code);
			}
		});
	}];
}
}

#pragma clang diagnostic pop
