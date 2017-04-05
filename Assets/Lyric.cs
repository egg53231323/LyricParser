using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LyricItem
{
    public string mText = "";
    public Int64 mTimeStamp = 0;
}

public class Lyric  {
    public string mTitle = "";
    public string mArtist = "";
    public string mAlbum = "";
    public string mBy = "";
    public Int64 mOffset = 0;
    List<LyricItem> mItems = new List<LyricItem>();

    public bool Load(string path)
    {
        if (null == path)
        {
            LrcLogError("file path is null");
            return false;
        }
        if (!System.IO.File.Exists(path))
        {
            LrcLogError(path + " not exist");
            return false;
        }

        return true;
    }

    protected void LrcLog(string msg)
    {
        Debug.Log("[Lyric] " + msg);
    }
    protected void LrcLogError(string msg)
    {
        Debug.LogError("[Lyric] " + msg);
    }
}
