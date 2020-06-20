using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PLATFORM_ANDROID
using UnityEngine.Android;
using DeadMosquito.AndroidGoodies;
#endif

using DeadMosquito.IosGoodies;

#if PLATFORM_IOS
using UnityEngine.iOS;
#endif

public class PermissionController : MonoBehaviour
{
    public enum PermissionType { Camera, Microphone, Location, CalendarRead, CalendarWrite }

    private static PermissionController _instance;
    public static PermissionController Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("PermissionController").AddComponent<PermissionController>();
            }
            return _instance;
        }
    }

    public void StartHandlePermissionEvents(List<PermissionType> permissionTypes, Action<bool> callback)
    {
        StartCoroutine(HandlePermissionEvents(permissionTypes, callback));
    }

    public IEnumerator HandlePermissionEvents(List<PermissionType> permissionTypes, Action<bool> callback)
    {
        // Check whether both permissions are already granted, so it can return.
        if (GetPermission(PermissionType.Camera) && GetPermission(PermissionType.Microphone) && GetPermission(PermissionType.Location))
        {
            callback.Invoke(true);
            yield break;
        }

        for (int i = 0; i < permissionTypes.Count; i++)
        {
            //RKLog.Log("Index: " + i);

            // <Sync> Request Permission.
            RequestPermission(permissionTypes[i]);

            // Wait for a bit, so the next lines of code do not get checked immediately.
            // Application will then get in focus.

#if PLATFORM_IOS
            if (permissionTypes[i] == PermissionType.Location)
                yield return new WaitForSeconds(1.0f);
            else
                yield return new WaitForSeconds(0.5f); 
#endif

#if PLATFORM_ANDROID
            yield return new WaitForSeconds(1f);
#endif

            // Permission granted.
            if (GetPermission(permissionTypes[i]))
            {
                //RKLog.Log("Got Permission");
                continue;
            }
            // Permission declined.
            else
            {
                //RKLog.Log("Doesn't have permission.");
                callback.Invoke(false);
                yield break;
            }
        }
        callback.Invoke(true);
    }

    public bool GetPermission(PermissionType type)
    {

#if PLATFORM_ANDROID
        switch (type)
        {
            case PermissionType.Camera:
                return Permission.HasUserAuthorizedPermission(Permission.Camera);
            case PermissionType.Microphone:
                return Permission.HasUserAuthorizedPermission(Permission.Microphone);
            case PermissionType.Location:
                return Permission.HasUserAuthorizedPermission(Permission.CoarseLocation);
            case PermissionType.CalendarRead:
                return AGPermissions.IsPermissionGranted(AGPermissions.READ_CALENDAR);
            case PermissionType.CalendarWrite:
                return AGPermissions.IsPermissionGranted(AGPermissions.WRITE_CALENDAR);
        }
#endif

#if PLATFORM_IOS
        switch (type)
        {
            case PermissionType.Camera:
                return Application.HasUserAuthorization(UserAuthorization.WebCam);
            case PermissionType.Microphone:
                return Application.HasUserAuthorization(UserAuthorization.Microphone);
            case PermissionType.Location:
                return Input.location.isEnabledByUser;
        }
#endif

        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) return true;

        return false;
    }

    private void RequestPermission(PermissionType type)
    {
#if PLATFORM_ANDROID
        switch (type)
        {
            case PermissionType.Camera:
                //RKLog.Log("Request Camera");
                Permission.RequestUserPermission(Permission.Camera);
                break;
            case PermissionType.Microphone:
                //RKLog.Log("Request Microphone");
                Permission.RequestUserPermission(Permission.Microphone);
                break;
            case PermissionType.Location:
                //RKLog.Log("Request Location");
                Permission.RequestUserPermission(Permission.CoarseLocation);
                break;
            case PermissionType.CalendarRead:
                //RKLog.Log("Request Calendar Read");
                AGPermissions.RequestPermissions(new string[] { AGPermissions.READ_CALENDAR }, results => { });
                break;
            case PermissionType.CalendarWrite:
                //RKLog.Log("Request Calendar Write");
                AGPermissions.RequestPermissions(new string[] { AGPermissions.WRITE_CALENDAR }, results => { });
                break;
        }
#endif

#if PLATFORM_IOS
        switch (type)
        {
            case PermissionType.Camera:
                Application.RequestUserAuthorization(UserAuthorization.WebCam);
                break;
            case PermissionType.Microphone:
                Application.RequestUserAuthorization(UserAuthorization.Microphone);
                break;
            case PermissionType.Location:
                //LocationController.Instance.StartTracking(() => Debug.LogError("tracking succesfull"), () => Debug.LogError("tracking failed"));
                break;
        }
#endif
    }
}

