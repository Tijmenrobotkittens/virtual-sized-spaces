using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateEvent : UnityEvent<States.State> { }


public class StateManager
{
    private States.State _currentState;
    private List<States.State> _history = new List<States.State>();
    public StateEvent stateChanged = new StateEvent();
    public StateEvent stateAgain = new StateEvent();
    public List<States.State> WaitingStates = new List<States.State>();
    public List<States.State> RequestedStates = new List<States.State>();
    private bool _ignorenext = false;
    public bool locked = false;
    public List<States.State> ignoreStates = new List<States.State>();
    //public Dictionary<string, string> _notLoggedInStates = new Dictionary<string, string>();

    public void Display(){
        Debug.LogError("-------------------states");
        foreach (States.State state in _history)
        {
            Debug.LogError(state);
        }
        Debug.LogError("-------------------/states");
    }

    public void Remove(States.State removeState) {
        List<States.State> _newhistory = new List<States.State>();
        foreach (States.State state in _history)
        {
            
            if (state != removeState) {
                _newhistory.Add(state);
            }

        }
        _history = _newhistory;
    }

    public void ClearHistory(){
        _history.Clear();
    }

    public void DeleteCurrentState(){        
        _currentState = _history[_history.Count-1];
        if (_history.Count > 1)
        {
            _history.RemoveAt(_history.Count - 1);
        }
    }

    public List<States.State> GetHistoryOfStates()
    {
        return _history;
    }

    //public void AddIgnore(string ignore) {
    //    ignoreStates.Add(ignore);
    //}

    //public void Back(){
    //    if (!ignoreStates.Contains(previous.state))
    //    {
    //        SetState(previous.state);
    //    }
    //    else {
    //        if (_history.Count > 1)
    //        {
    //            _history.RemoveAt(_history.Count - 1);
    //            Back();
    //        }
           
    //        return;
    //    }
    //    //Display();

    //    if (_history.Count > 1)
    //    {
    //        _history.RemoveAt(_history.Count - 1);
    //    }
    //    if (_history.Count > 1)
    //    {
    //        _history.RemoveAt(_history.Count - 1);
    //    }
    //    //Display();
    //}

    public States.State currentState
    {
        get {
            return _currentState;
        }
    }

    public void SetSilentState(States.State _state) {
        RKLog.Log("SetSilentState " + _currentState, "statemanager");
        _currentState = _state;
    }

    public void Lock() {
        RKLog.Log("lock "+ _currentState, "statemanager");
        locked = true;
    }

    public void Unlock() {
        RKLog.Log("unlock "+_currentState, "statemanager");
        locked = false;
        CheckStates();
    }

    public void CheckStates() {
        if (WaitingStates.Count > 0) {
            RKLog.Log("Unlocked, check states " + WaitingStates[0], "statemanager");
            SetState(WaitingStates[0]);
            WaitingStates.RemoveAt(0);
        }
    }

    public void SetState(States.State _state){

        RKLog.Log("request " + _state + " currentstate "+_currentState, "statemanager");
        if (locked) {
            RKLog.Log("setstate locked! " + _currentState, "statemanager");
            WaitingStates.Add(_state);
            return;
        }

        if (_state != _currentState) {
            
            if (_currentState != null)
            {
                if (_ignorenext == false)
                {
                      _history.Add(_currentState);
                }
                else {
                    _ignorenext = false;
                }

            }

           
            _currentState = _state;
            RKLog.Log("setstate " + _currentState, "statemanager");
            Lock();
            stateChanged.Invoke(_currentState);
            
            //if (ignore == true) {
            //    _ignorenext = true;
            //}
        }

        else if (_state == _currentState) {
            if (_currentState != null)
            {
                stateAgain.Invoke(_currentState);
            }
        }
    }

    public States.State previous {
        get
        {

            return _history[_history.Count - 1];
        }
    }
}