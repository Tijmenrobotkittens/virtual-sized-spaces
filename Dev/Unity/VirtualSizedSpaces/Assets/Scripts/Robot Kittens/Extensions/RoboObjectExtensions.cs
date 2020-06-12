
using UnityEngine;
using System;
using System.Linq;

namespace RobotKittens
{
	public static class RoboObjectExtensions
	{
        public static GameObject absolutePosition(this GameObject obj, Vector2 position)
		{
			obj.GetComponent<RectTransform>().anchoredPosition = position;
			return obj;
		}

		public static float x(this GameObject obj, float? x = null)
		{
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (x != null)
			{
				rectTransform.anchoredPosition = new Vector2((float)x, rectTransform.anchoredPosition.y);
			}
			//		Debug.LogError(rectTransform.anchoredPosition);
			return rectTransform.anchoredPosition.x;
		}

		public static float y(this GameObject obj, float? y = null)
		{            
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (y != null)
			{
				rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, (float)y);
			}
			return rectTransform.anchoredPosition.y;
		}

		public static void rectPos(this GameObject obj, float? left = null, float? right = null, float? top = null, float? height = null)
		{
			RectTransform rectTransform = obj.GetComponent<RectTransform>();

			float _left = rectTransform.offsetMin[0];
			if (left != null)
			{
				_left = (float)left;
			}

			float _bottom = rectTransform.offsetMin[1];
			if (height != null)
			{
				_bottom = 0 - (float)height;
			}

			float _right = rectTransform.offsetMax[0];
			if (right != null)
			{
				_right = (float)right;
			}

			float _top = rectTransform.offsetMax[1];
			if (top != null)
			{
				_top = (float)top;
			}

			//        Debug.LogError("set offset left " + _left + " right " + _right + " top " + _top + " bottom " + _bottom);

			rectTransform.offsetMin = new Vector2(_left, _bottom);
			rectTransform.offsetMax = new Vector2(_right, _top);

		}

		public static void center(this GameObject obj)
		{

			obj.x((obj.transform.parent.gameObject.GetComponent<RectTransform>().rect.width - obj.width()) / 2);
			//        Debug.LogError("parent width "+ obj.transform.parent.gameObject.GetComponent<RectTransform>().rect.width + " object width "+obj.width() + " set x "+obj.x());
		}


		public static float bottom(this GameObject obj)
		{
			return (0 - obj.y()) + obj.height();
		}

		public static float right(this GameObject obj)
		{
			return (obj.x()) + obj.width();
		}

		public static float height(this GameObject obj, float? height = null)
		{
			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (height != null)
			{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta[0], (float)height);
			}
			return rectTransform.sizeDelta[1];
		}

		public static float scaledWidth(this GameObject obj)
		{
			return obj.width() * obj.xScale();
		}

		public static float scaledHeight(this GameObject obj)
		{
			return obj.height() * obj.yScale();
		}

		public static float xScale(this GameObject obj)
		{
			return obj.transform.localScale[0];
		}

		public static float yScale(this GameObject obj)
		{
			return obj.transform.localScale[1];
		}

		public static float width(this GameObject obj, float? width = null)
		{

			RectTransform rectTransform = obj.GetComponent<RectTransform>();
			if (width != null)
			{
				rectTransform.sizeDelta = new Vector2((float)width, rectTransform.sizeDelta[1]);
			}
			return rectTransform.sizeDelta[0];
		}

		public static GameObject Clear(this GameObject go)
		{
			Transform transform = go.transform;
			foreach (Transform child in transform)
			{
				GameObject.Destroy(child.gameObject);
			}
			return go;
		}


		/// <summary>
		/// Returns all monobehaviours (casted to T)
		/// </summary>
		/// <typeparam name="T">interface type</typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T[] GetInterfaces<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			var mObjs = gObj.GetComponents<MonoBehaviour>();

			return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
		}

		/// <summary>
		/// Returns the first monobehaviour that is of the interface type (casted to T)
		/// </summary>
		/// <typeparam name="T">Interface type</typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T GetInterface<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			return gObj.GetInterfaces<T>().FirstOrDefault();
		}

		/// <summary>
		/// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T GetInterfaceInChildren<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
			return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
		}

		/// <summary>
		/// Gets all monobehaviours in children that implement the interface of type T (casted to T)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="gObj"></param>
		/// <returns></returns>
		public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
		{
			if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");

			var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();

			return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
		}
	}
}