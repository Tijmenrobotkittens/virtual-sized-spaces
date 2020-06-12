using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class SoundPlaylist : MonoBehaviour
{
    private Dictionary<string, SoundSample> soundSamples = new Dictionary<string, SoundSample>();
    private string[] _samples;
    private SoundPlayer _soundplayer;
    private Action<string, string> _callback;
    private Action<string, string> _internalcallback;
    private string _name;
    private int key = 0;
    private bool _stopped = false;
    public bool playing = false;
    private float _delay = 0;
    private float _duration = 0;


    public SoundPlaylist(string[] samples, SoundPlayer sp, string name, Action<string, string> internalcallback, Action<string, string> callback, float delay = 0)
    {
        _delay = delay;
        _name = name;
        _callback = callback;
        _internalcallback = internalcallback;
        _samples = samples;
        _soundplayer = sp;
        ParseDuration();
        play();
    }

    private void ParseDuration()
    {
        float d = 0;
        foreach (string sample in _samples)
        {
            d = d + _soundplayer.getDuration(sample);
        }
        _duration = d;
    }

    public float getDuration()
    {
        return _duration;
    }

    public float getPosition() {
        int k = 0;
        float p = 0;
        while (k < key) {
            p = p + _soundplayer.getDuration(_samples[k]);
            k++;
        }
        p = p + _soundplayer.getPosition(_samples[k]);
        return p;
    }

    //public float getPosition(){
        
    //}


    public void stop(){
        _stopped = true;
        playing = false;
    }

    private void play(){
        if (_stopped) {
            playing = false;
            return;
        }

        if (_samples.Length > key)
        {
            playing = true;
            float d = 0;
            if (key == 0) {
                d = _delay;
            }


            _soundplayer.play(_samples[key], false, 0, completed,null,d);
        }
        else {
            playing = false;
            _internalcallback(_name,"complete");
        }
    }

    public Action<string,string> callback {
        get {
            return _callback;
        }
    }

    public string samplename
    {
        get
        {
            return _name;
        }
    }

    public void dispose(){
        _soundplayer = null;
        soundSamples = null;
        _samples = null;
        _callback = null;
        _internalcallback = null;
        _name = null;
    } 

    private void completed(string sample, string type){
        key++;
        play();
    }
}
