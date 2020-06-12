using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using RobotKittens;
using UnityEngine;
using UnityEngine.Events;

public class SoundPlayerEvent : UnityEvent<string> { }

public class SoundPlayer : UiElement
{
    private Dictionary<string, SoundSample> soundSamples = new Dictionary<string, SoundSample>();
    public SoundPlayerEvent finished = new SoundPlayerEvent();
    public SoundPlayerEvent playing = new SoundPlayerEvent();
    private Dictionary<string, SoundPlaylist> playlists = new Dictionary<string, SoundPlaylist>();
    public bool muted = false;
    private float updateTime = 0.033f;
    private Dictionary<string, string> _callbackNameToSample = new Dictionary<string, string>();
    public static SoundPlayer Global;
    public bool IsGlobal = false;
       

    protected override void Init()
    {
        if (IsGlobal)
        {
            SoundPlayer.Global = this;
            Debug.LogError("SOUNDPLAYER IS GLOBAL!");
        }
        var foundListeners = FindObjectsOfType<AudioListener>();
        if (foundListeners.Length == 0)
        {
            gameObject.AddComponent<AudioListener>();
        }
        base.Init();
    }

  
    public void SetGlobal() {
        SoundPlayer.Global = this;
        IsGlobal = true;
    }


    


    public void mute(){
        muted = true;
    }

    public void unmute()
    {
        muted = false;
    }

    public void inject(string sample, float time, string forcedsample)
    {
        SoundInjectObject soundInject = new SoundInjectObject(time, forcedsample);
        soundSamples[sample].inject(soundInject);
    }

    public void load(string name)
    {
        if (exists(name))
        {
            soundSamples[name].load();
        }
    }

    public void UnloadAll(){
        playlists.Clear();
        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            sample.Value.Unload();
        }
    }

    public void PauseAll(){
        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            if (sample.Value.playing) {
                Debug.LogError("sample "+sample.Value.name + " is playing! pause!");

                sample.Value.Pause();
            }
        }
    }

    public void ResumeAll()
    {
        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            if (sample.Value.Paused)
            {
                sample.Value.Resume();
            }
        }
    }

    public bool isPlaying(string name) {
        bool ret = false;
        if (soundSamples.ContainsKey(name))
        {
            if (soundSamples[name].playing) {
                ret = true;
            }
        }
        else if (playlists.ContainsKey(name))
        {
            if (playlists[name].playing)
            {
                ret = true;
            }
        }
        return ret;
    }

    public void addSample(string name, string sample, float volume = 1, bool loop = false)
    {
        SoundSample snd = new SoundSample(this,name, sample,volume,loop);
        soundSamples[name] = snd;

    }

    private bool exists(string name) {
        if (soundSamples.ContainsKey(name)) {
            return true;
        }
        else if (playlists.ContainsKey(name))
        {
            return true;
        }
        else{
            Debug.LogError(name + " does not exist");
            return false;
        }
    }

    public void resetInjects(string name){
        soundSamples[name].resetInjects();
    }

    public void clearInjects(string name){
        if (exists(name))
        {
            soundSamples[name].clearInjects();
        }
    }

    public void playPlaylist(string[] samples, string name, Action<string,string> callback, float delay = 0) {
        playlists.Add(name,new SoundPlaylist(samples,this, name,completedPlaylist,callback,delay));
    }

    public List<string> getPlayingSamples(){
        List<string> samples = new List<string>();
        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            SoundSample snd = sample.Value;
            if (snd.playing)
            {
                samples.Add(snd.name);
            }
        }
        return samples;
    }

    public float getDuration(string name) {
        float dur = 0f;
        if (exists(name))
        {
            if (soundSamples.ContainsKey(name))
            {
                dur = soundSamples[name].getDuration();
            }
            else if (playlists.ContainsKey(name)) {
                dur = playlists[name].getDuration();
            }
        }
        else if (_callbackNameToSample.ContainsKey(name)) {
            dur = soundSamples[_callbackNameToSample[name]].getDuration();
        }
        return dur;
    }

    public void seek(string name, float time)
    {
        
        if (exists(name))
        {
            soundSamples[name].seek(time);
        }
    
    }

    private void completedPlaylist(string playlist,string type) {
        string key = "";
        foreach (KeyValuePair<string, SoundPlaylist> pl in playlists)
        {
            if (pl.Value.samplename == playlist) {
                key = pl.Key;
            }
        }

        if (key != "")
        {
            SoundPlaylist cp = playlists[playlist];
            cp.callback(playlist,"complete");
            playlists.Remove(key);
            cp.dispose();
        }
    }

    public void playRandom(string[] samples, bool donotplayifplaying = false, float volume = -1) {
        bool playRandomSample = true;

        if (donotplayifplaying)
        {
            foreach (string s in samples)
            {
                if (isPlaying(s)) {
                    playRandomSample = false;
                }
            }
        }

        if (playRandomSample == true)
        {
            int randomkey = UnityEngine.Random.Range(0, samples.Length);
            play(samples[randomkey]);
            if (volume != -1) {
                setVolume(samples[randomkey], volume);
            }
        }
    }

    public void setVolume(string name, float volume) {
        if (exists(name))
        {
            soundSamples[name].volume(volume);
        }
    }

    public void playOnce(string file) {
        SoundSample snd = new SoundSample(this, file, file, 1, false);
        snd.playonce = true;
        soundSamples[file] = snd;
        play(file);
    }

    public void Fade(string name, float end,float time){
        if (exists(name))
        {
            soundSamples[name].Fade(end,time);
        }
    }

    public void PlayAndStopOthers(string name, float fadein = 0, float fadeout = 0, string mustcontain = "") {
        stopAll(fadeout, mustcontain, name);
        play(name, true, fadein);
    }


    public void stopAll(float fadeout = 0, string mustcontain = "",string ignorefile = ""){
        foreach (KeyValuePair<string, SoundPlaylist> pl in playlists)
        {
            pl.Value.stop();

        }
        playlists.Clear();

        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            bool stop = true;

            if (mustcontain != "")
            {
                if (!sample.Key.Contains(mustcontain) || sample.Key == ignorefile)
                {
                    stop = false;
                }
            }
            if (stop == true) {
                sample.Value.stop(fadeout);
            }

            
        }
    }

    public void pitch(string name, float pitch) {
        if (exists(name))
        {
            soundSamples[name].pitch(pitch);
        }
    }

    public float getPosition(string name)
    {
        float r = 0;
               
        if (exists(name))
        {
            if (soundSamples.ContainsKey(name))
            {
                r = soundSamples[name].getPosition();
            }
            else if (playlists.ContainsKey(name))
            {
                r = playlists[name].getPosition();
            }
        }
      
        else if (_callbackNameToSample.ContainsKey(name))
        {
            r = soundSamples[_callbackNameToSample[name]].getPosition();
        }
        return r;
    }



    public void play(string name,bool ingoreifplaying = false, float fadein = 0,Action<string,string> method = null, string callbackName = null, float delay = 0, float volume = -1) {
        //Debug.Log("play "+name);
        if (exists(name)) {
            if (callbackName == null) {
                soundSamples[name].callbackName = null;
            }
            else {
                soundSamples[name].callbackName = callbackName;
            }

            if (callbackName != null)
            {
                _callbackNameToSample[callbackName] = name;
            }
            soundSamples[name].play(ingoreifplaying,fadein,method,delay);
            if (volume != -1) {
                setVolume(name, volume);
            }
            playing.Invoke(name);
        }
    }

    public void stop(string name, float fadeout = 0){
        //Debug.Log("stop "+name);
        if (exists(name))
        {
            soundSamples[name].stop(fadeout);
        }
        else {
            Debug.LogError(name + " does not exist");
        }
    }
	
	// Update is called once per frame
	void Update () {
        List<string> removes = new List<string>();
        foreach (KeyValuePair<string, SoundSample> sample in soundSamples)
        {
            SoundSample snd = sample.Value;
            if (snd.playing) {
                snd.tick();
                snd.checkInjects();
                if (snd.checkFinished()) {
                    finished.Invoke(snd.name);
                    //Debug.Log("yes, in update finish "+snd.name);
                    if (snd.callback != null) {
                        Debug.Log("execute callback !" +snd.callback);
                        if (snd.callbackName != null) {
                            snd.callback(snd.callbackName,"completed");
                        }
                        else {
                            snd.callback(snd.name, "completed");    
                        }

                        //snd.callback = null;
                    }

                    if (snd.playonce) {
                        removes.Add(sample.Key);
                    }
                }
            }
        }

        foreach (string remove in removes) {
            soundSamples.Remove(remove);
        }
	}
}
