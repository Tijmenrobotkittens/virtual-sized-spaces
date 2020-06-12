#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System.Runtime.InteropServices;
	using JetBrains.Annotations;
	using Internal;
	
	/// <summary>
	/// Class for native loading dialog screen.
	/// </summary>
	[PublicAPI]
	public class IGLoadingScreen
	{
		/// <summary>
		/// Shows native loading dialog. Call <see cref="Dismiss"/> to dismiss.
		/// </summary>
		public static void Show()
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_showLoadingDialog();
		}

		/// <summary>
		/// Dismisses native loading dialog, created using <see cref="Show"/>
		/// </summary>
		public static void Dismiss()
		{
			if (IGUtils.IsIosCheck())
			{
				return;
			}

			_dismissLoadingDialog();
		}

		[DllImport("__Internal")]
		static extern void _showLoadingDialog();
		
		[DllImport("__Internal")]
		static extern void _dismissLoadingDialog();
	}
}
#endif