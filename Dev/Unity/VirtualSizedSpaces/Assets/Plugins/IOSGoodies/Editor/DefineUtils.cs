using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;

namespace DeadMosquito.IosGoodies.Editor
{
	[PublicAPI]
	public class DefineUtils
	{
		public static void ToggleDefineIos(string define, bool enable)
		{
			ToggleDefine(define, enable, BuildTargetGroup.iOS);
		}
		
		public static void ToggleDefine(string define, bool enable, params BuildTargetGroup[] supportedPlatforms)
		{
			foreach (var targetPlatform in supportedPlatforms)
			{
				ToggleDefine(define, enable, targetPlatform);
			}
		}

		public static void ToggleDefine(string define, bool enable, BuildTargetGroup targetPlatform)
		{
			var scriptDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetPlatform);
			var flags = new List<string>(scriptDefines.Split(';'));

			if (flags.Contains(define))
			{
				if (!enable)
				{
					flags.Remove(define);
				}
			}
			else
			{
				if (enable)
				{
					flags.Add(define);
				}
			}

			var result = string.Join(";", flags.ToArray());

			if (scriptDefines != result)
			{
				PlayerSettings.SetScriptingDefineSymbolsForGroup(targetPlatform, result);
			}
		}

		public static void RemoveDefineFromAll(string define)
		{
			ToggleDefine(define, false, GetAllAvailablePlatforms());
		}

		public static void AddDefineToAll(string define)
		{
			ToggleDefine(define, true, GetAllAvailablePlatforms());
		}

		static BuildTargetGroup[] GetAllAvailablePlatforms()
		{
			var allPlatforms = Enum.GetValues(typeof (BuildTargetGroup)).Cast<BuildTargetGroup>().ToList();
			allPlatforms.Remove(BuildTargetGroup.Unknown);
			return allPlatforms.ToArray();
		}
	}
}