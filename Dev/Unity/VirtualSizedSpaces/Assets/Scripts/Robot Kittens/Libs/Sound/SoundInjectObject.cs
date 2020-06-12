using System;

public class SoundInjectObject
{


    public float time = 0;
    public string sample;
    public bool played = false;

    public SoundInjectObject(float _time, string _sample) {
        time = _time;
        sample = _sample;
    }
}

