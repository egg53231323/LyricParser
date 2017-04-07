using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class LyricView : MonoBehaviour {

    protected Lyric mLyric = new Lyric();
    protected AudioSource mAudioSource;

	// Use this for initialization
	void Start () {
        string dir = Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar;
        string lyricPath = dir + "test.lrc";

        mAudioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        mLyric.Load(lyricPath);
        // just for debug
        mLyric.PrintInfo();

        mAudioSource.Play();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateLyric();
        UnityEngine.UI.Slider slider = GameObject.Find("Canvas/Slider").GetComponent<UnityEngine.UI.Slider>();
        slider.value = mAudioSource.time / mAudioSource.clip.length;
    }

    protected void UpdateLyric()
    {
        Int64 timestamp = GetCurrentTimestamp();
        LyricItem currentItem = mLyric.SearchCurrentItem(timestamp);
        string text = "";
        string uiTextLyricName = "Canvas/TextLyric";
        string uiTextTimeName = "Canvas/TextTime";
        UnityEngine.UI.Text uiTextLyric = GameObject.Find(uiTextLyricName).GetComponent<UnityEngine.UI.Text>();
        if (null != uiTextLyric)
        {
            List<LyricItem> items = mLyric.GetItems();
            int showLyricSize = 3;
            foreach (LyricItem item in items)
            {
                if (item == currentItem)
                {
                    text += Lyric.WrapStringWithColorTag(item.mText, 255, 0, 0) + System.Environment.NewLine;
                }
                else if ((null == currentItem && item.mIndex < showLyricSize) 
                    || (null != currentItem && item.mIndex >= currentItem.mIndex - showLyricSize 
                    && item.mIndex <= currentItem.mIndex + showLyricSize))
                {
                    text += item.mText + System.Environment.NewLine;
                }
            }
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

    protected Int64 GetCurrentTimestamp()
    {
        return (Int64)(mAudioSource.time * 1000.0f);
    }


    public void OnSliderChanged()
    {
        UnityEngine.UI.Slider slider = GameObject.Find("Canvas/Slider").GetComponent<UnityEngine.UI.Slider>();
        float value = Mathf.Clamp(slider.value, 0.0f, 1.0f);
        mAudioSource.time = value * mAudioSource.clip.length;
        UpdateLyric();
    }
}
