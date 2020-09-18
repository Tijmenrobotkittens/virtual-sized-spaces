using System.Linq;
using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {

        public bool IsInitialised { get; private set; } = false;

        // Check to see if we're about to be destroyed.
        private static bool sShuttingDown { get; set; } = false;
        private static object sLock = new object();
        private static T sInstance;
        private static bool _innited = false;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (sShuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                        "' already destroyed. Returning null.");
                    return null;
                }

                lock (sLock)
                {
                    if (sInstance == null)
                    {
                        // Search for existing instance.
                        sInstance = (T)FindObjectOfType(typeof(T));

                        // Create new instance if one doesn't already exist.
                        if (sInstance == null)
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            var singletonObject = new GameObject();
                            sInstance = singletonObject.AddComponent<T>();
                            sInstance.name = "[" + typeof(T).ToString().Split('.').Last() + "]";
                            _innited = true;
                            if (Application.isPlaying)
                                DontDestroyOnLoad(sInstance); // doesn't work in editor

                        }

#if UNITY_EDITOR
                        // Check for multiple instances
                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("[Singleton] there should never be more than 1 singleton of type " + typeof(T) + "'.", sInstance);
                            return null;
                        }
#endif
                       
                        // Initialize 
                        sInstance.Init();

                    }
                   

                    return sInstance;
                }
            }
        }
        //protected virtual void Awake()
        //{

        //    if (!IsInitialised)
        //        Init();

        //}

        /// <summary>
        /// Called when the instance is used for the first time.
        /// Put specific initialization code in here.
        /// </summary>
        ///

        public static bool Exists() {
            if (_innited)
            {
                return true;
            }
            else {
                return false;
            }
        }

        protected virtual void Init()
        {

            if (IsInitialised)
            {
                Debug.LogWarning("This singleton is already initialised?");
                return;
            }

            if(sInstance == null)
                sInstance = (T)this;

            Debug.Log(sInstance.name + "initialised");

            IsInitialised = true;

        }

        private void OnApplicationQuit()
        {
            sShuttingDown = true;
        }

        private void OnDestroy()
        {
            sShuttingDown = true;
            sInstance = null;
        }

    }