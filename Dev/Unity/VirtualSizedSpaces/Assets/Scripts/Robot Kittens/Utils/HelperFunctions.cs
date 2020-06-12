using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using I2.Loc;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;
using Random = System.Random;
using System.IO;

public class HelperFunctions : MonoBehaviour
{
    private static HelperFunctions _instance;
    public enum DeviceTypes {MOBILE,TABLET,DESKTOP,DETECT};

    public static HelperFunctions Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = new GameObject("HelperFunctions").AddComponent<HelperFunctions>();
            }
            return _instance;
        }
    }

    public static List<string> GetFilesInDirectory(string path)
    {
        List<string> files = new List<string>();
        DirectoryInfo dir = new DirectoryInfo(path);
        FileInfo[] info = dir.GetFiles("*.*");
        foreach (FileInfo f in info)
        {
            files.Add(f.Name);
        }
        return files;
    }


    //calculate physical inches with pythagoras theorem
    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        return diagonalInches;
    }

    public static bool IsPhone() {
        if (HelperFunctions.GetDeviceType() == HelperFunctions.DeviceTypes.MOBILE) {
            return true;
        }
        else {
            return false;
        }
    }

    public static DeviceTypes GetDeviceType(){
       
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            {
                if (DeviceDiagonalSizeInInches() >= 7)
                {
                    return DeviceTypes.TABLET;
                }
                else
                {
                    return DeviceTypes.MOBILE;
                }
            }
            else {
                if (Screen.width > Screen.height)
                {
                    return DeviceTypes.TABLET;
                }
                else
                {
                return DeviceTypes.MOBILE;
                }
            }
        
    }
    public static float GetCanvasSize( float width = 0, float size = 0 )
    {
        float wi = width;
        if (wi == 0)
        {
            wi = Screen.width;
        }
        float factor = (float) width / (float) Screen.width;
        return size * factor;
    }

    public static bool OnDesktop()
    {
        return Application.platform == RuntimePlatform.WindowsEditor
               || Application.platform == RuntimePlatform.WindowsPlayer
               || Application.platform == RuntimePlatform.OSXEditor
               || Application.platform == RuntimePlatform.OSXPlayer
               || Application.platform == RuntimePlatform.LinuxEditor
               || Application.platform == RuntimePlatform.LinuxPlayer;
    }
    
    public static string GetNiceDate(DateTime date) {
        DateTime now = DateTime.Now;
        var daysbetween = (now - date).TotalDays;
        string time = date.ToString("d ") + HelperFunctions.DynamicText("variablen/monthshort_" + (date.Month - 1)) + date.ToString(" yyyy");

        daysbetween = Math.Round(daysbetween);
        if (daysbetween < 8)
        {
            if (Math.Round(daysbetween) == 0)
            {
                time = HelperFunctions.DynamicText("vandaag");
            }
            else if (daysbetween == 1)
            {
                time = daysbetween + " " + HelperFunctions.DynamicText("daggeleden");
            }
            else
            {
                time = daysbetween + " " + HelperFunctions.DynamicText("dagengeleden");
            }
        }
        return time;
    }

    public static Texture2D GetImageFromPath(string path)
    {
        Texture2D t = Resources.Load<Texture2D>(path);
        return t;
    }
    public static Sprite GetIconFromAtlas(string name)
    {
        System.Object[] data = Resources.LoadAll("TextureSheets/interfaceImages");
        foreach (System.Object o in data)
        {
            System.Type type = o.GetType();
            if (type == typeof(UnityEngine.Sprite))
            {
                Sprite t = o as Sprite;
                if (t.name == name)
                {
                    return t;
                }
            }
        }
        return null;
    }

    public static int Age(DateTime birthDate, DateTime now)
    {
       
        int age = now.Year - birthDate.Year;

        if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day))
            age--;

        return age;
    }

    public static string DynamicText(string key)
    {
        string text = "Cannot find text";
        if (LocalizationManager.GetTranslation(key) != null) {
            text = LocalizationManager.GetTranslation(key);
        }

        return text.Replace("\r", "\r\n");
    }

    public static float CalculateAspectRatio(Sprite sprite)
    {
        return sprite.rect.width / sprite.rect.height;
    }

    public static Color hexToColor(string hex)
	{
		hex = hex.Replace("0x", "");//in case the string is formatted 0xFFFFFF
		hex = hex.Replace("#", "");//in case the string is formatted #FFFFFF
		byte a = 255;//assume fully visible unless specified in hex
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		//Only use alpha if the string has enough characters
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r, g, b, a);
	}

    //public static float GetScrollHeight(float objectheight){
    //    float y = objectheight - HelperFunctions.GetHeight(640);
    //    if (y < 0)
    //    {
    //        y = 0;
    //    }
    //    float plus = 0;
    //    if (this._overlay)
    //    {
    //        plus = this._overlay.GetMenuHeight();
    //    }
    //    return y + plus;
    //}

    public static string NameList(string[] names) {
        string ret = "";
        int counter = 0;
        foreach (string name in names) {
            if (counter == names.Length - 2)
            {
                ret = ret + name + " & ";
            }
            else if (counter < names.Length-1) {
                ret = ret + name + ", ";
            }

            else {
                ret = ret + name;
            }

            counter++;
        }
        return ret;
    }

    public static int GetTimeLeft(string time)
    {
        DateTime t = Convert.ToDateTime(time);
        int totalMinutesToday = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
        int totalMinutes = t.Hour * 60 + t.Minute;
        int diff = totalMinutes - totalMinutesToday;
        return diff;
    }
    public string getText(string name) {
        TextAsset txt = (TextAsset)Resources.Load("text/"+name, typeof(TextAsset));
        string content = txt.text;
        return content;
    }

    public static void ClearOutRenderTexture(RenderTexture renderTexture)
    {
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = renderTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = rt;
    }

    public void Clear(GameObject go)
    {
        Transform transform = go.transform;
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag != "donotclear") {
                Destroy(child.gameObject);
            }

        }
    }

    public int Timestamp(){
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        return cur_time;
    }

    public List<int> getRandomNumbers(int min, int max, int number) {
        List<int> ret = new List<int>();
        bool finished = false;
        int range = max - min;


            while (finished == false) {
                int random = UnityEngine.Random.Range(min, max + 1);

                if (!ret.Contains(random)) {
                    ret.Add(random);
                }

                if (ret.Count == number || ret.Count == range+1){

                    finished = true;
                }
            }


        return ret;
    }

    public static float TopSpacing()
    {
        //return 200;
        return HelperFunctions.GetCanvasSize(Screen.safeArea.y);
    }

    public static float RangeToPercent(float from, float to, float value)
    {
        float range = to - from;
        float percent = (value - from) / range;
        return percent*100;
    }

    public static float PercentToRange(float from, float to, float percent)
    {
        float range = to - from;
        float value = from + ((range / 100) * percent);
        return value;
    }

    public static float BottomSpacing()
    {
        float bpos = HelperFunctions.GetHeight(Config.width) - ((HelperFunctions.GetCanvasSize(Screen.safeArea.height) + HelperFunctions.GetCanvasSize(Screen.safeArea.y)));
        return bpos;
    }

    /*public static T[] GetRandomArray<T>(T[] array, int size)
    {
        List<T> list = new List<T>();
        T element;
        int tries = 0;
        int maxTries = array.Length;

        while (tries < maxTries && list.Count < size)
        {
            element = array[UnityEngine.Random.Range(0, array.Length)];

            if (!list.Contains(element))
            {
                list.Add(element);
            }
            else
            {
                tries++;
            }
        }

        if (list.Count > 0)
        {
            return list.ToArray();
        }
        else
        {
            return null;
        }
    }*/

    public int Clamp(int value, int min, int max)
    {
        return (value < min) ? min : (value > max) ? max : value;
    }

    public static void ActivateOnCondition(bool condition, GameObject target) {
        if(condition && !target.activeSelf) {
            target.SetActive(true);
        } else if(!condition && target.activeSelf) {
            target.SetActive(false);
        }
    }

    public static void reloadLocalize(Transform targetTransform) {
        TextMeshProUGUI targetText = targetTransform.GetComponent<TextMeshProUGUI>();
        Localize targetLocalize = targetTransform.GetComponent<Localize>();

        if(targetText == null || targetLocalize == null) {
            Debug.LogError("Missing localize or textmesh to reload.");
            return;
        }

        targetText.text = LocalizationManager.GetTranslation(targetLocalize.Term);
    }

    public static GameObject GetPrefab2d(string name, GameObject parent = null, float x = 0, float y = 0, bool position = true)
    {
        GameObject obj = GetPrefab(name, parent);  
        if (position) obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
        obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1,1);
        return obj;
    }

    public static GameObject GetPrefab(string name, GameObject parent = null)
    {
        Transform t = null;
        if (parent != null) {
            t = parent.transform;
        }
        GameObject obj = Instantiate(Resources.Load(name, typeof(GameObject)), t) as GameObject;
        if (obj == null)
        {
            Debug.LogError("GetPrefab " + name + " not found");
        }

        return obj;
    }

    public Vector2 getCenter(float width,float height,float containerwidth,float containerheight) {
        float x = (containerwidth - width) / 2;
        float y = (containerheight - height) / 2;

        return new Vector2(x,0-y);
    }



    public static float GetHeight(float width = 0)
    {
        float wi = width;
        if (wi == 0)
        {
            wi = Screen.width;
        }

        float factor = (float)width / (float)Screen.width;
        return Screen.height * factor;
    }

    public static float GetWidth(float height = 0)
    {


        float hi = height;
        if (hi == 0)
        {
            hi = Screen.height;
        }

        float factor = (float)height / (float)Screen.height;


        return Screen.width * factor;
    }

	public static float clampOptional(float val, float? min, float? max) {
		if (min != null) {
			if (val < min) {
				val = (float)min;
			}
		}
		if (max != null) {
			if (val > max) {
				val = (float)max;
			}
		}
		return val;
	}
    
    public static float GetBottomSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;
        float restSize = 1 - anchorMin.y;
        return GetCanvasSize(Screen.height) - GetCanvasSize(Screen.height) * restSize;
    }

    public static float GetTopSafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        anchorMin.x /= screenWidth;
        anchorMin.y /= screenHeight;
        anchorMax.x /= screenWidth;
        anchorMax.y /= screenHeight;
        float restSize = anchorMax.y;
        return HelperFunctions.GetCanvasSize(Screen.height) - HelperFunctions.GetCanvasSize(Screen.height) * restSize;
    }

    public static float GetCanvasSize(float size = 0)
    {
        float wi = Config.width;
        if (wi == 0)
        {
            wi = Screen.width;
        }

        float factor = (float)wi / (float)Screen.width;

        

        return size * factor;
    }

    public static int switchInt(int i)
    {
        if (i == 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public static IEnumerator waitThen(float seconds, Action method) {
        yield return new WaitForSeconds(seconds);
        method();
    }

    public static int GetClosest(float pos, Dictionary<int, float> positions, int currentid = -1) {
        int counter = 0;
        double closest = 0;
        int closestKey = 0;
        bool closestSet = false;
        int finalid = 0;
        foreach (KeyValuePair<int, float> keyval in positions)
        {
            var dis = Math.Abs(keyval.Value-pos);
            //if (currentid != -1) {
            if (keyval.Key == currentid) {
                dis = dis * 5;
            }
            //}

            if (dis < closest || closestSet == false)
            {
                closestKey = counter;
                closest = dis;
                closestSet = true;
                finalid = keyval.Key;
            }
            counter++;
        }
        return finalid;
    }

    public static int GetClosest(float pos, Dictionary<float, int> positions)
    {
        int counter = 0;
        double closest = 0;
        int closestKey = 0;
        bool closestSet = false;
        int finalid = 0;
        foreach (KeyValuePair<float, int> keyval in positions)
        {
            var dis = Math.Abs(keyval.Key - pos);
            if (dis < closest || closestSet == false)
            {
                closestKey = counter;
                closest = dis;
                closestSet = true;
                finalid = keyval.Value;
            }
            counter++;
        }
        return finalid;
    }

    public static int GetRandomValueFromArrayExcluding(int[] values, int[] exclude)
    {
        List<int> tempValues = new List<int>();
        for (int i = 0; i < values.Length; i++)
        {
            bool contains = false;
            for (int j = 0; j < exclude.Length; j++)
            {
                if (values[i] == exclude[j])
                    contains = true;
            }
            if (contains) continue;
            tempValues.Add(values[i]);
        }
        int randValue = tempValues.Count == 0 ? -1 : tempValues[UnityEngine.Random.Range(0, tempValues.Count)];
        return randValue;
    }

    public static int GetRandomValueFromList(List<int> list)
    {
        int randValue = list.Count == 0 ? -1 : list[UnityEngine.Random.Range(0, list.Count)];
        return randValue;
    }

    public static string ParseDate(DateTime date)
    {
        return string.Format("{0:00}", date.Year) + "-" + string.Format("{0:00}", date.Month) +
               "-" + string.Format("{0:00}", date.Day) + " " + date.Hour.ToString("00") + ":" + date.Minute.ToString("00") +
               ":" + date.Second.ToString("00");
    }
    public static DateTime ParseDate(string date)
    {
        int day = int.Parse(date.Substring(8, 2));
        int month = int.Parse(date.Substring(5, 2));
        int year = int.Parse(date.Substring(0, 4));

        int hour = int.Parse(date.Substring(11, 2));
        int minute = int.Parse(date.Substring(14, 2));

        return new DateTime(year, month, day, hour, minute, 0);
    }
    
    public static string GetFormattedGender(string gender, bool plural = false)
    {
        string text = "";
        
        string man = plural
            ? DynamicText("ProfileCreation/men")
            : DynamicText("ProfileCreation/man");
        string woman = plural
            ? DynamicText("ProfileCreation/women")
            : DynamicText("ProfileCreation/woman");

        if (gender == "m")
            text = man;
        if (gender == "f")
            text = woman;
        if (gender == "nb")
            text = DynamicText("ProfileCreation/more");
        if (gender == "a")
            text = DynamicText("ProfileCreation/all");

        if (gender == man)
            text = "m";
        if (gender == woman)
            text = "f";
        if (gender == DynamicText("ProfileCreation/more"))
            text = "nb";
        if (gender == DynamicText("ProfileCreation/all"))
            text = "a";
        
        // lelijke code :)
        
        return text;
    }

    public static void GetSmartClosest(float pos, Dictionary<int, float> positions, out int finalid, out double rawDist)
    {
        int counter = 0;
        double closest = 0;
        rawDist = 0;
        int closestKey = 0;
        bool closestSet = false;
        finalid = 0;
        foreach (KeyValuePair<int, float> keyval in positions)
        {
            var dis = Math.Abs(keyval.Value - pos);
            if (dis < closest || closestSet == false)
            {
                closestKey = counter;
                closest = dis;
                closestSet = true;
                finalid = keyval.Key;
                rawDist = keyval.Value - pos;
            }
            counter++;
        }
    }

    private static double GetDistance(double x1, double y1, double x2, double y2)
    {
        return Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
    }

    public static bool ValidateEmail(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return false;
        }

        return Regex.IsMatch(value, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
    }

    //public static string DateTimeToNiceDate(DateTime dateTime)
    //{
    //    int parsedDay = (int)(Days)Enum.Parse(typeof(Days), dateTime.DayOfWeek.ToString());

    //    string dayOfWeek = DynamicText("day_" + parsedDay + "_short");
    //    string dayOfMonth = dateTime.Day.ToString();
    //    string month = DynamicText("month_" + dateTime.Month);

    //    string dateNotation = DynamicText("datenotation");

    //    dateNotation = dateNotation.Replace("{dayofweek}", dayOfWeek);
    //    dateNotation = dateNotation.Replace("{dayofmonth}", dayOfMonth);
    //    dateNotation = dateNotation.Replace("{month}", month);
    //    dateNotation = dateNotation.Replace("-", " ");

    //    return dateNotation + " " + dateTime.Year;
    //}
}
