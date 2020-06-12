#import <StoreKit/StoreKit.h>

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {

void _goodiesRequestReview() {
	[SKStoreReviewController requestReview];
}

}
#pragma clang diagnostic pop