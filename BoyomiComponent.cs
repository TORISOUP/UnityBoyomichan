using UnityEngine;
using System.Collections;

/// <summary>
/// Unityで使う方
/// </summary>
public class BoyomiComponent : MonoBehaviour
{
    BoyomiClient boyomi;
    public string hostIp = "127.0.0.1";
    public int hostPort = 50001;

    //何秒おきに棒読みちゃん問い合わせるか
    public float checkSecond = 0.2f;

    /// <summary>
    /// 発声状態
    /// </summary>
    public bool isSpeaching
    {
        get
        {
            if (boyomi == null) { return false; }
            return boyomi.isSpeaching;
        }
    }

	// Use this for initialization
	void Start () {
        this.boyomi = new BoyomiClient(hostIp, hostPort);
        InvokeRepeating("checkSpeacking", checkSecond, checkSecond);
	}

    void checkSpeacking()
    {
        this.boyomi.CheckSpeaking();
    }
}
