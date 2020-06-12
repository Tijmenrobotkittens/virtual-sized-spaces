using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class EmptyEvent : UnityEvent { }
public class StringEvent : UnityEvent<string> { }
public class StringStringEvent : UnityEvent<string,string> { }
public class StringStringIntEvent : UnityEvent<string, string,int> { }
public class IntEvent : UnityEvent<int> { }
public class IntListEvent : UnityEvent<List<int>> { }
public class IntIntEvent : UnityEvent<int,int> { }
public class BoolEvent : UnityEvent<bool> { }
public class DateTimeEvent : UnityEvent<DateTime> { }
public class DateDateTimeEvent : UnityEvent<DateTime, DateTime> { }