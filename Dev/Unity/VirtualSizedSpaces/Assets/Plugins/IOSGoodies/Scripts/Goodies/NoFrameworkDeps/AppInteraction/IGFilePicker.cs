using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

#if UNITY_IOS
namespace DeadMosquito.IosGoodies
{
	using System;
	using System.Runtime.InteropServices;
	using Internal;
	using JetBrains.Annotations;

	/// <summary>
	///     Class for manipulating documents on the file system
	/// </summary>
	[PublicAPI]
	public class IGFilePicker
	{
		enum FilePickerMode
		{
			Import = 0,
			Open = 1,
			MoveToService = 2,
			ExportToService = 3
		}

		/// <summary>
		///     Import file(s) into the application sandbox directory.
		/// </summary>
		/// <param name="fileTypes">
		///     Uniform type identifiers for the file types to choose from.
		///     For example, use "public.image" for image picking, "public.text" for text files and "public.audiovisual-content" for audio and video files.
		///     More info about the uniform type identifiers can be found here:
		///     https://developer.apple.com/library/archive/documentation/FileManagement/Conceptual/understanding_utis/understand_utis_conc/understand_utis_conc.html#//apple_ref/doc/uid/TP40001319-CH202-BCGJGJGA
		/// </param>
		/// <param name="onSuccess">
		///     Action to perform with the string representation of the picked files URLs, separated by commas.
		///     These documents are temporary files. They remain available only until your application terminates.
		///     To keep a permanent copy, move these files to a permanent location inside your sandbox.
		/// </param>
		/// <param name="onCancel"> Action to perform when user cancels the picking.</param>
		/// <param name="allowMultiple"> Whether to allow multiple file selection. </param>
		/// <exception cref="ArgumentNullException"> Is thrown when <see cref="fileTypes" /> are null.</exception>
		/// <exception cref="ArgumentException"> Is thrown when <see cref="fileTypes" /> are not specified.</exception>
		public static void Import([NotNull] string[] fileTypes, Action<List<string>> onSuccess, Action onCancel, bool allowMultiple)
		{
			PickFiles(fileTypes, onSuccess, onCancel, allowMultiple, FilePickerMode.Import, string.Empty);
		}

		/// <summary>
		///     Select an external document. The document picker provides access to the document, and the user can edit the document in place.
		/// </summary>
		/// <param name="fileTypes">
		///     Uniform type identifiers for the file types to choose from.
		///     For example, use "public.image" for image picking, "public.text" for text files and "public.audiovisual-content" for audio and video files.
		///     More info about the uniform type identifiers can be found here:
		///     https://developer.apple.com/library/archive/documentation/FileManagement/Conceptual/understanding_utis/understand_utis_conc/understand_utis_conc.html#//apple_ref/doc/uid/TP40001319-CH202-BCGJGJGA
		/// </param>
		/// <param name="onSuccess">
		///     Action to perform with the string representation of the picked files URLs, separated by commas.
		///     The open operation provides security-scoped URLs for all external documents.
		/// </param>
		/// <param name="onCancel"> Action to perform when user cancels the picking.</param>
		/// <param name="allowMultiple"> Whether to allow multiple file selection. </param>
		/// <exception cref="ArgumentNullException"> Is thrown when <see cref="fileTypes" /> are null.</exception>
		/// <exception cref="ArgumentException"> Is thrown when <see cref="fileTypes" /> are not specified.</exception>
		public static void Open([NotNull] string[] fileTypes, Action<List<string>> onSuccess, Action onCancel, bool allowMultiple)
		{
			PickFiles(fileTypes, onSuccess, onCancel, allowMultiple, FilePickerMode.Open, string.Empty);
		}

		/// <summary>
		///     Move a file from the application sandbox location to selected service.
		/// </summary>
		/// <param name="onSuccess">
		///     Action to perform with the string representation of the picked files URLs, separated by commas.
		///     The URLs refer to the documents' new locations.
		/// </param>
		/// <param name="onCancel"> Action to perform when user cancels the picking.</param>
		/// <param name="paths"> List of the local files paths within the application's sandbox to be moved. </param>
		public static void MoveToService(Action<List<string>> onSuccess, Action onCancel, List<string> paths)
		{
			var result = string.Empty;
			foreach (var path in paths)
			{
				if (!File.Exists(path))
				{
					Debug.Log(string.Format("IGFIlePicker.MoveToService: file at {0} does not exist.", path));
				}
				else
				{
					result += result.Equals(string.Empty) ? path : "," + path;
				}
			}

			if (result.Equals(string.Empty))
			{
				Debug.Log("IGFIlePicker.MoveToService: no valid file paths found");
				return;
			}
			
			PickFiles(new[] {"public.data"}, onSuccess, onCancel, false, FilePickerMode.MoveToService, result);
		}

		/// <summary>
		///     Export a file (copy) from the application sandbox location to selected service.
		/// </summary>
		/// <param name="onSuccess">
		///     Action to perform with the string representation of the picked files URLs, separated by commas.
		///     The URLs refer to new copies of the exported documents at the selected destination.
		/// </param>
		/// <param name="onCancel"> Action to perform when user cancels the picking.</param>
		/// <param name="paths"> List of the local files paths within the application's sandbox to be exported. </param>
		public static void ExportToService(Action<List<string>> onSuccess, Action onCancel, List<string> paths)
		{
			var result = string.Empty;
			foreach (var path in paths)
			{
				if (!File.Exists(path))
				{
					Debug.Log(string.Format("IGFIlePicker.ExportToService: file at {0} does not exist.", path));
				}
				else
				{
					result += result.Equals(string.Empty) ? path : "," + path;
				}
			}
			
			if (result.Equals(string.Empty))
			{
				Debug.Log("IGFIlePicker.ExportToService: no valid file paths found");
				return;
			}
			
			PickFiles(new[] {"public.data"}, onSuccess, onCancel, false, FilePickerMode.ExportToService, result);
		}

		static void PickFiles([NotNull] string[] fileTypes, Action<List<string>> onSuccess, Action onCancel, bool allowMultiple, FilePickerMode mode, string path)
		{
			if (fileTypes == null)
			{
				throw new ArgumentNullException("fileTypes");
			}

			if (fileTypes.Length == 0)
			{
				throw new ArgumentException("At least one file type must be provided");
			}

			if (IGUtils.IsIosCheck())
			{
				return;
			}

			Action<string> onSuccessStr = s =>
			{
				var paths = s.SplitString();
				onSuccess(paths.ToList());
			};

			_iosGoodiesPickFiles(fileTypes.JoinByComma(), IGUtils.ActionStringCallback, IGUtils.ActionVoidCallback,
				onSuccessStr.GetPointer(), onCancel.GetPointer(), allowMultiple, (int) mode, path);
		}

		[DllImport("__Internal")]
		static extern void _iosGoodiesPickFiles(string fileTypes, IGUtils.ActionStringCallbackDelegate successCallback,
			IGUtils.ActionVoidCallbackDelegate cancelCallback, IntPtr successPtr, IntPtr cancelPtr, bool allowMultiple, int mode,
			string path);
	}
}
#endif