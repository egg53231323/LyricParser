using UnityEngine;
using System.Collections;
using System;

public class LyricView : MonoBehaviour {

    protected Lyric mLyric = new Lyric();
    protected Int64 mStartTime = 0;

	// Use this for initialization
	void Start () {
        string dir = Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar;
        string musicPath = dir + "test.mp3";
        string lyricPath = dir + "test.lrc";

        mLyric.Load(lyricPath);
        // just for debug
        mLyric.PrintInfo();

        mStartTime = GetCurrentTime();
    }
	
	// Update is called once per frame
	void Update () {
        Int64 timestamp = GetCurrentTime() - mStartTime;
        LyricItem item = mLyric.SearchCurrentItem(timestamp);
        string text = "";
        if (null != item)
        {
            text = item.mText;
        }

        string uiTextLyricName = "Canvas/TextLyric";
        string uiTextTimeName = "Canvas/TextTime";
        UnityEngine.UI.Text uiTextLyric = GameObject.Find(uiTextLyricName).GetComponent<UnityEngine.UI.Text>();
        if (null != uiTextLyric)
        {
            uiTextLyric.text = text;
        }
        else
        {
            Debug.LogError("GetComponent " + uiTextLyricName + " failed");
        }
        UnityEngine.UI.Text uiTextTime = GameObject.Find(uiTextTimeName).GetComponent<UnityEngine.UI.Text>();
        if (null != uiTextTime)
        {
            uiTextTime.text = "time: " + Lyric.TimestampToString(timestamp);
        }
        else
        {
            Debug.LogError("GetComponent " + uiTextTimeName + " failed");
        }
    }

    Int64 GetCurrentTime()
    {
        return (Int64)(Time.time * 1000.0f);
    }
}
