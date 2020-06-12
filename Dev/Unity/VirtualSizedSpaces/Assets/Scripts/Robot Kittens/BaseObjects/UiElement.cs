using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobotKittens
{
    public class UiElement : MonoBehaviour
    {
        protected bool _innited = false;
        private static Dictionary<string, UiElement> cachedUiElements = new Dictionary<string, UiElement>();

        // Start is called before the first frame update

        public void BaseInit()
        {
            if (_innited == false)
            {
                Init();
                _innited = true;
            }
        }

        protected virtual void Init()
        {
        }

        void Awake()
        {
            BaseInit();
        }

    }
}
