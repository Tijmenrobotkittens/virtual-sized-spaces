using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SoundSample : MonoBehaviour  {
    private SoundPlayer _soundPlayer;
    private string _name;
    private string _sample;
    public AudioSource _audiosource;
    public AudioClip _audioclip;
    private float _volume;
    private Tween tween;
    private bool _playing;
    private bool _loop = false;
    private bool _inplaying = false;
    public bool playonce = false;
    private bool _paused = false;
    public Action<string,string> callback;
    private List<SoundInjectObject> _soundInjects = new List<SoundInjectObject>();
    private List<int> ingores = new List<int>();
    private bool wasplaying = false;
    public string callbackName = null;
    private bool _loaded = false;
    private Coroutine _playCoroutine;
    private int _wait = 0;
    private int _waitmax = 0;
    private bool _ignoreifplaying = false;
    private float _fadein = 0;

    public SoundSample(SoundPlayer soundPlayer, string name, string sample, float volume = 1, bool loop = false) {
        _soundPlayer = soundPlayer;
        _name = name;
        _sample = sample;
        _volume = volume;
        _loop = loop;

        if (_soundPlayer.muted) {
            _volume = 0;
        }

    }

    public void Unload(){
        if (_loaded)
        {
            stop();


            _audioclip.UnloadAudioData();
            Destroy(_audiosource);
            _loaded = false;
            _playing = false;
            _paused = false;
        }

    }

    public void inject(SoundInjectObject soundInjectObject) {
        _soundInjects.Add(soundInjectObject);
    }

    public string name {
        get {
            return _name;
        }
    }

    public float getPosition(){
        checkLoaded();
        return _audiosource.time;
    }


    private void checkLoaded(){
        if (_loaded == false) {
            _audiosource = _soundPlayer.gameObject.AddComponent<AudioSource>();
            if (!_audiosource)
            {
                Debug.LogError("NO AUDIO SOURCE " + _name);
            }

            _audioclip = Resources.Load(_sample) as AudioClip;
            if (!_audioclip)
            {
                Debug.LogError("NO AUDIO CLIP " + _name);
            }
            _audiosource.clip = _audioclip;
            _audiosource.loop = _loop;
            _audiosource.volume = _volume;
            _audiosource.playOnAwake = false;
            _loaded = true;
        }
    }

    public void load(){
        checkLoaded();
    }

    private void finishInject(string name,string type) {
        if (_audiosource)
        {
            _paused = false;
            _audiosource.Play();
        }
        else {
            _paused = false;
            _playing = false;
        }
    }

    public void checkInjects(){
        if (_soundInjects.Count > 0) {
            //Debug.Log(_audiosource.time);
            foreach (SoundInjectObject si in _soundInjects)
            {
                if (si.played == false && si.time <= _audiosource.time) {
                    si.played = true;
                    _audiosource.Pause();
                    _paused = true;
                    _soundPlayer.play(si.sample, false, 0, finishInject);
                }
            }
        }
    }

    private void stopTween(){
        if (tween != null) {
            if (tween.active) {
                tween.Kill();
            }
        }
    }

    public bool playing {
        get {
            return _playing;
        }
    }

    public void pitch(float pitch) {
        _audiosource.pitch = pitch;
    }

    public void volume(float volume)
    {
        checkLoaded();
        if (_soundPlayer.muted)
        {
            volume = 0;
        }

        _audiosource.volume = volume;
    }

    public float getDuration(){
        checkLoaded();
        return _audioclip.length;
    }

    public bool checkFinished() {
        if (!_loaded){
            return false;
        }
        bool ret = false;
        //Debug.Log(name);
        //Debug.Log(name + " " +_audiosource.time + " - "+ _audioclip.length);


        if (!_audiosource.isPlaying) {
            if (wasplaying == true && _paused == false) {
                Debug.Log("Sample "+ name + " was playing, but not anymore");
            }
        }

        if (_playing && _audiosource.time >= (_audioclip.length-0.1) && _paused == false && _audiosource.loop == false || _audiosource.time == 0 && _inplaying == true && _paused == false && _audiosource.loop == false) {
            _inplaying = false;
//            Debug.LogError("finished yes "+name);
            _playing = false;
            ret = true;
        }
        else if (_audiosource.time > 0 && _inplaying == false) {
            _inplaying = true;
        }
        else if (_loop == true && _paused == false && _audiosource.isPlaying == false){
            _inplaying = false;
            _playing = false;
            ret = true;
           // Debug.Log("finished no " + name + " time: "+ _audiosource.time + " totel: "+_audioclip.length);
        }

        wasplaying = _audiosource.isPlaying;

        return ret;
    }

    public void resetInjects(){
        foreach (SoundInjectObject si in _soundInjects)
        {
            si.played = false;
        }
    }

    public void clearInjects()
    {
        _soundInjects.Clear();
    }

    public bool Paused {
        get {
            return _paused;
        }
    }

    public void seek(float time) {
        checkLoaded();
        _audiosource.time = time;
    }

    public void Pause()
    {
        
        _paused = true;
        if (_audiosource)
        {
            _audiosource.Pause();
        }
        else
        {
            _playing = false;
        }
    }

    public void Resume(){
        _paused = false;
        if (_audiosource)
        {
            if (_waitmax > 0 && _wait < _waitmax)
            {
                
            }
            else {
                _audiosource.Play();
            }
        }
        else {
            _playing = false;
        }
    }

    public void play(bool ignoreifplaying, float fadein = 0, Action<string,string> method = null, float delay = 0){
        //        Debug.LogError("play sample "+name);
        if (_playing && _ignoreifplaying)
        {
           
            return;
        }

        checkLoaded();
        _paused = false;
        _inplaying = false;
        seek(0);
        stopTween();
        callback = method;
        _ignoreifplaying = ignoreifplaying;
        _fadein = fadein;
       

        if (delay > 0) {
            _playing = true;
            _waitmax = (int)(delay * (float)Application.targetFrameRate);
            return;
        }
        else {
            _waitmax = 0;
            _wait = 0;
        }


        if (ignoreifplaying == true && _audiosource.isPlaying)
        {

        }
        else if (fadein == 0)
        {
            killVolumeTween();
            if (_soundPlayer.muted)
            {
                _volume = 0;
            }

            _audiosource.volume = _volume;
            _audiosource.Play();
        }
        else {
            _audiosource.volume = 0;
            _audiosource.Play();
            killVolumeTween();
            tween = DOTween.To(() => _audiosource.volume, x => _audiosource.volume = x, _volume, fadein);
        }
        _playing = true;
    }
       

    public void Fade(float value, float time) {
        killVolumeTween();
        tween = DOTween.To(() => _audiosource.volume, x => _audiosource.volume = x, value, time);
    }

    public void stop(float fadeout = 0){
        //Debug.LogError("stop "+ fadeout);
        //Debug.LogError(_loaded);
        if (!_loaded) {
            return;
        }
        if (fadeout == 0)
        {
            if (_audiosource)
            {
                _audiosource.Stop();

            }
        }
        else {
            
            if (_soundPlayer.muted)
            {
                _volume = 0;
            }
            _audiosource.volume = _volume;
            killVolumeTween();
            tween = DOTween.To(() => _audiosource.volume, x => _audiosource.volume = x, 0, fadeout).OnComplete(stop2);
        }
    }

    private void killVolumeTween() {
        if (tween != null)
        {
            tween.Kill();
            tween = null;
        }
    }

    private void stop2(){
        //Debug.LogError("stop2");
        stop();
    }

	// Use this for initialization
	void Start () {
		
	}

    public void dispose(){
       
    }

   
	
	// Update is called once per frame
	public void tick () {
        if (_paused) {
            return;
        }

        if (_waitmax > 0 && _wait < _waitmax && _playing == true && _paused == false) {
           
            _wait++;
//            Debug.LogError("WAITING wait: " + _wait + " waitmax: "+_waitmax);
            if (_wait == _waitmax) {
                play(_ignoreifplaying, _fadein, callback,0);
            }
        }


        //Application.targetFrameRate
	}
}
