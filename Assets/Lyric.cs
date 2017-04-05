using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LyricItem : IComparable<LyricItem>
{
    public string mText = "";
    public Int64 mTimeStamp = 0;
    public int CompareTo(LyricItem other)
    {
        if (mTimeStamp == other.mTimeStamp)
            return 0;

        if (mTimeStamp < other.mTimeStamp)
            return -1;

        return 1;
    }
}

public class Lyric  {
    // [ti : value]
    // title of music
    public string mTitle = "";

    // [ar : value]
    // name of artist
    public string mArtist = "";

    // [al : value]
    // name of album
    public string mAlbum = "";

    // [by : value]
    // name of lyric file creator
    public string mBy = "";

    // [offset : value]
    // integer in millisecond
    public Int64 mOffset = 0;

    // lyric data
    List<LyricItem> mItems = new List<LyricItem>();

    protected static string STRING_ID_TAG_TITLE = "ti";
    protected static string STRING_ID_TAG_ARTIST = "ar";
    protected static string STRING_ID_TAG_ALBUM = "al";
    protected static string STRING_ID_TAG_BY = "by";
    protected static string STRING_ID_TAG_OFFSET = "offset";

    public bool Load(string path)
    {
        if (null == path)
        {
            LyricLogError("file path is null");
            return false;
        }
        if (path.Length <= 0 || !System.IO.File.Exists(path))
        {
            LyricLogError(path + " not exist");
            return false;
        }
        LyricLog("Load lyric begin : " + path);

        System.IO.StreamReader streamReader = new System.IO.StreamReader(path);
        string line = null;
        while (true)
        {
            line = streamReader.ReadLine();
            if (null == line)
                break;

            ParseLine(line);
        }

        LyricLog("Load lyric end");
        return true;
    }

    protected bool ParseLine(string line)
    {
        if (null == line)
            return false;

        LyricLogDebug("current line: " + line);

        const int notFoundIndex = -1;
        const char openBracket = '[', closeBracket = ']';

        int openBracketIndex = 0, closedBracketIndex = 0;
        int startSearchIndex = 0;
        while (true)
        {
            if (startSearchIndex >= line.Length)
                break;

            openBracketIndex = line.IndexOf(openBracket, startSearchIndex);
            if (notFoundIndex == openBracketIndex)
                break;

            closedBracketIndex = line.IndexOf(closeBracket, openBracketIndex);
            if (notFoundIndex == closedBracketIndex || closedBracketIndex - openBracketIndex < 2)
                break;

            string tagString = line.Substring(openBracketIndex + 1, closedBracketIndex - openBracketIndex - 1);
            LyricLogDebug("tagString: " + tagString);

            string[] tagSplitArray = tagString.Split(':');
            foreach (string str in tagSplitArray)
            {
                LyricLogDebug("tagSplitArray: " + str);
            }

            if (!TryParseIDTag(tagString, tagSplitArray))
            {
                TryParseTimeTag(tagString, tagSplitArray);
            }

            startSearchIndex = closedBracketIndex + 1;
        }

        return true;
    }

    protected bool TryParseIDTag(string tagString, string[] tagSplitArray)
    {
        if (null == tagSplitArray || tagSplitArray.Length < 2)
            return false;

        if (tagSplitArray[0].Equals(STRING_ID_TAG_TITLE, StringComparison.CurrentCultureIgnoreCase))
        {
            mTitle = tagSplitArray[1];
        }
        else if (tagSplitArray[0].Equals(STRING_ID_TAG_ARTIST, StringComparison.CurrentCultureIgnoreCase))
        {
            mArtist = tagSplitArray[1];
        }
        else if (tagSplitArray[0].Equals(STRING_ID_TAG_ALBUM, StringComparison.CurrentCultureIgnoreCase))
        {
            mAlbum = tagSplitArray[1];
        }
        else if (tagSplitArray[0].Equals(STRING_ID_TAG_BY, StringComparison.CurrentCultureIgnoreCase))
        {
            mBy = tagSplitArray[1];
        }
        else if (tagSplitArray[0].Equals(STRING_ID_TAG_OFFSET, StringComparison.CurrentCultureIgnoreCase))
        {
            Int64 offset = 0;
            if (!Int64.TryParse(tagSplitArray[1], out offset))
                return false;

            mOffset = offset;
        }
        else
        {
            // not valid now
            return false;
        }

        return true;
    }

    protected bool TryParseTimeTag(string tagString, string[] tagSplitArray)
    {
        if (null == tagSplitArray || tagSplitArray.Length < 2)
            return false;

        if (null == tagString)
            return false;

        char[] separator = { ':', '.' };
        string[] timeSplitArray = tagString.Split(separator);
        if (null == timeSplitArray || timeSplitArray.Length < 2)
            return false;

        if (timeSplitArray.Length == 2)
        {
            // [minute : second]
        }
        else if (timeSplitArray.Length == 3)
        {
            // [minute : second : millisecond] or [minute : second . millisecond]
        }
        else
        {
            // not valid now
            return false;
        }

        return true;
    }

    protected void LyricLog(string msg)
    {
        Debug.Log("[Lyric] " + msg);
    }

    protected void LyricLogDebug(string msg)
    {
        Debug.Log("[Lyric] " + msg);
    }

    protected void LyricLogError(string msg)
    {
        Debug.LogError("[Lyric] " + msg);
    }
}
