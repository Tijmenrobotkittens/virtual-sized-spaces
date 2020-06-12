using System;

public class InfiniteScrollerCurrentStateData
{
    public int maxkey = 0;
    public int minkey = 0;
    public float currentmax = 0;
    public float currentmin = 0;
    public bool minset = false;
    public bool maxset = false;

    public string debug(){
        string s =  "maxkey = " + maxkey + "\n";
        s = s + "minkey = " + minkey + "\n";
        s = s + "currentmax = " + currentmax + "\n";
        s = s + "currentmin = " + currentmin + "\n";
        s = s + "minset = " + minset + "\n";
        s = s + "maxset = " + maxset + "\n";
        return s;
    }
}