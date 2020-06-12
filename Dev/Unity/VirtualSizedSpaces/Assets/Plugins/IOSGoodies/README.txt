Please contact me before writing a negative review on Asset Store.

Before updating - remove previous package from you project.

Full Documentation: https://docs.ninevastudios.com/#/unity-plugins/ios-goodies
Support: support@ninevastudios.com
Ask us anything on our Discord server: https://bit.ly/nineva_support_discord

### CHANGELOG

v2.5.0

+ FIXED Some obsolete classes warnings (`WWW`)
+ FIXED Date picker style issues on iOS 14

---

v2.4.0

+ FIXED Date picker not working properly with dark theme

---

v2.3.0

+ FIXED Some date picker minor bug fixes
+ FIXED Action sheets crashing on iPads
+ FIXED Video editing

---

v2.2.1

+ ADDED methods to pick/export/import files `IGFilePicker.cs`

---

v2.1

+ ADDED method to pick video files `IGImagePicker.PickVideoFromPhotoLibrary()`
+ ADDED `IGVideoEditor.cs` to open native video editor

---

v2.0

+ ADDED New editor window to disable unused features to avoid unneeded dependencies in the XCode project. Please go to `Window -> iOS Goodies -> Edit Settings`

+ ADDED `SendEmailViaController` method to `IGShare` class, that allows e-mail sharing with callbacks
+ REMOVED deprecated social sharing methods
+ ADDED possibility to create/delete calendar events and reminders using `IGCalendar` class
+ ADDED the possibility to provide minute interval for `DateTimePicker` methods

---

v1.9

+ ADDED `IGHapticFeedback` class with haptic feedback and vibration functionality
+ ADDED `IGLoadingScreen` class with native loader overlay functionality
+ ADDED `IGDialogs.ShowInputFieldDialog` method to show alert dialog with input field
+ ADDED `IGLocalAuthentication` class for biometric authentication (face id + fingerprint)

---

v1.8

+ ADDED Functionality to toggle the flashlight and its intensity
+ UPDATED `IGDateTimePicker.cs` with methods to pick date/time within limited values
+ FIXED Issue with cancelling contact picker window
+ ADDED `IGLoadingScreen` class for use of native loading dialog screen
+ ADDED `IGDialogs.ShowInputFieldDialog` method to show alert dialog with input field

---

v1.7

+ ADDED Contact picker
+ ADDED Method to open application settings
+ ADDED Method to open app iTunes AppStore page
+ IMPROVED You can now select popover screen position when picking image on iPad
+ FIXED Some orientation problems when picking images

---

v1.6

IMPORTANT!!! Since iOS 11 Apple disabled direct sharing to Facebook and Twitter and they will now behave as all other apps. So this plugin functionality will no longer work on iOS 11 and above.

- IMPROVED iOS 11 Fixes and adaptations
- IMPROVED When showing share dialog on iPad the position on screen where to show popover can be specified
+ ADDED Dialog to request app review on the App Store
+ FIXED Minor issue with time when setting it to 1 minute

---

v1.6

- ADDED Method to open application settings where user can enable/disable permissions

---


v1.5

- ADDED Method to show phone dialer `IGApps.OpenDialer()`
- ADDED Method to share Text+Link (Useful if you want to share in FB Messenger e.g.)
- ADDED Functionality to save image to Camera Roll (`IGImagePicker.SaveImageToGallery()`)
- ADDED Ability to specify initial date when showing Date/Time picker.
- IMPROVED Sharing Text+Image, now there is error callback and onSuccess callback returns activity type that was posted

---

v1.4

IMPORTANT! In this version all the files are now in a single folder 'IOSGoodies' under 'Assets'

- ADDED UIActionSheet functionality
- ADDED functionality to pick image from Gallery/Photos Library
- ADDED functionality to take photo from camera and receive it

---

v1.3

- ADDED Twitter and Facebook native sharing functionality
- FIXED Problem with native sharing on iPads

---

v1.2

- ADDED UiDevice functionality: getting device UUID, getting battery state and level, enabling proximity sensor, getting device name, os version etc.

---

v1.1

- ADDED Date/Time Picker Native Ui

---

v1.0

Initial release
