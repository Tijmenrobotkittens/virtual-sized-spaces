#import <AVFoundation/AVFoundation.h>
#import <AudioToolbox/AudioToolbox.h>

extern "C" {
void _goodiesEnableFlashlight(bool enable) {
	AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
	if ([device hasTorch]) {
		[device lockForConfiguration:nil];
		[device setTorchMode:enable ? AVCaptureTorchModeOn : AVCaptureTorchModeOff];
		[device unlockForConfiguration];
	}
}

bool _goodiesDeviceHasFlashlight() {
	return [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo].hasTorch;
}

void _goodiesSetFlashlightLevel(float level) {
	AVCaptureDevice *device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
	if ([device hasTorch]) {
		[device lockForConfiguration:nil];
		if (level <= 0.0) {
			[device setTorchMode:AVCaptureTorchModeOff];
		} else {
			if (level >= 1.0) {
				level = AVCaptureMaxAvailableTorchLevel;
			}
			BOOL success = [device setTorchModeOnWithLevel:level error:nil];
		}
		[device unlockForConfiguration];
	}
}

void _goodiesOnNotificationOccured(int type) {
	UINotificationFeedbackType notificationType;
	switch (type) {
		case 0:
			notificationType = UINotificationFeedbackTypeError;
			break;
		case 1:
			notificationType = UINotificationFeedbackTypeWarning;
			break;
		case 2:
			notificationType = UINotificationFeedbackTypeSuccess;
			break;
		default:
			notificationType = UINotificationFeedbackTypeError;
			break;
	}
	UINotificationFeedbackGenerator *notification = [[UINotificationFeedbackGenerator alloc] init];
	[notification prepare];
	[notification notificationOccurred:notificationType];
}

void _goodiesOnSelectionOccured() {
	UISelectionFeedbackGenerator *selector = [[UISelectionFeedbackGenerator alloc] init];
	[selector prepare];
	[selector selectionChanged];
}

void _goodiesOnImpactOccured(int type) {
	UIImpactFeedbackStyle impactType;
	switch (type) {
		case 0:
			impactType = UIImpactFeedbackStyleLight;
			break;
		case 1:
			impactType = UIImpactFeedbackStyleMedium;
			break;
		case 2:
			impactType = UIImpactFeedbackStyleHeavy;
			break;
		default:
			impactType = UIImpactFeedbackStyleLight;
			break;
	}
	UIImpactFeedbackGenerator *impact = [[UIImpactFeedbackGenerator alloc] initWithStyle:impactType];
	[impact prepare];
	[impact impactOccurred];
}

void _goodiesVibrate() {
	AudioServicesPlayAlertSound(SystemSoundID(kSystemSoundID_Vibrate));
}

}
