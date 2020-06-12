#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using Internal;
	using JetBrains.Annotations;
	using UnityEngine.iOS;
	
	/// <summary>
	/// Class for haptic feedbacks and vibrations.
	/// </summary>
	[PublicAPI]
	public class IGHapticFeedback
	{
		/// <summary>
		/// Feedback types for <see cref="NotificationOccurred"/>
		/// </summary>
		[PublicAPI]
		public enum NotificationType
		{
			Error = 0,
			Warning = 1,
			Success = 2
		}

		/// <summary>
		/// Impact types for <see cref="ImpactOccurred"/>
		/// </summary>
		[PublicAPI]
		public enum ImpactType
		{
			Light = 0,
			Medium = 1,
			Heavy = 2
		}
	
		/// <summary>
		/// Check if device supports haptic feedbacks.
		/// </summary>
		[PublicAPI]
		public static bool IsHapticFeedbackSupported
		{
			get
			{
				if (IGUtils.IsIosCheck())
				{
					return false;
				}
				
				int deviceVersionCode;
				
				if (IGDevice.Model.StartsWith("iPhone"))
				{
					deviceVersionCode = (int) Device.generation;
				}
				else
				{
					deviceVersionCode = 0;
				}
				
				return deviceVersionCode > 30;
			}
		}

		/// <summary>
		/// Send the impact haptic feedback. Vibrates if the device does not support haptic feedbacks.
		/// </summary>
		/// <param name="type">Strength of impact. Can be one of <see cref="ImpactType"/></param>
		[PublicAPI]
		public static void ImpactOccurred(ImpactType type)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			if (IsHapticFeedbackSupported)
			{
				_goodiesOnImpactOccured((int) type);
			}
			else
			{
				_goodiesVibrate();
			}
		}

		/// <summary>
		/// Send the notification haptic feedback. Vibrates if the device does not support haptic feedbacks.
		/// </summary>
		/// <param name="type">Type of notification. Can be one of <see cref="NotificationType"/>.</param>
		[PublicAPI]
		public static void NotificationOccurred(NotificationType type)
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			if (IsHapticFeedbackSupported)
			{
				_goodiesOnNotificationOccured((int) type);
			}
			else
			{
				_goodiesVibrate();
			}
		}

		/// <summary>
		/// Send the selection haptic feedback. Vibrates if the device does not support haptic feedbacks.
		/// </summary>
		[PublicAPI]
		public static void SelectionChanged()
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}
			
			if (IsHapticFeedbackSupported)
			{
				_goodiesOnSelectionOccured();
			}
			else
			{
				_goodiesVibrate();
			}
		}

		/// <summary>
		/// Vibrate with standard vibration pattern
		/// </summary>
		[PublicAPI]
		public static void Vibrate()
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_goodiesVibrate();
		}
		
		[DllImport("__Internal")]
		static extern void _goodiesOnNotificationOccured(int type);

		[DllImport("__Internal")]
		static extern void _goodiesOnSelectionOccured();

		[DllImport("__Internal")]
		static extern void _goodiesOnImpactOccured(int type);

		[DllImport("__Internal")]
		static extern void _goodiesVibrate();
	}
}
#endif