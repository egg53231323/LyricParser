using UnityEngine;
using System.Collections;

public class LyricView : MonoBehaviour {

    protected Lyric mLyric = new Lyric();

	// Use this for initialization
	void Start () {
        string dir = Application.streamingAssetsPath + System.IO.Path.DirectorySeparatorChar;
        string musicPath = dir + "test.mp3";
        string lyricPath = dir + "test.lrc";

        mLyric.Load(lyricPath);
        // just for debug
        mLyric.PrintInfo();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
