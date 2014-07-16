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

    private bool _isSpeaching = false;

    //何秒おきに棒読みちゃん問い合わせるか
    public float checkSecond = 1.0f;

    /// <summary>
    /// 発声状態
    /// </summary>
    public bool isSpeaching
    {
        get
        {
            return this.boyomi.isSpeaching;
        }
    }

    // Use this for initialization
    void Start()
    {
        this.boyomi = new BoyomiClient(hostIp, hostPort);
        InvokeRepeating("checkSpeaching", checkSecond, checkSecond);
    }

    void checkSpeaching()
    {
        this.boyomi.checkSpeaching();
    }
}
