using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

/// <summary>
/// Unity非依存なSocket通信部分
/// </summary>
class BoyomiClient
{
    private TcpClient tcpClient;
    private String hostIp;
    private int hostPort;
    byte[] readbuf;
    bool _isSpeaching;

    /// <summary>
    /// 発声中であるか
    /// </summary>
    public bool isSpeaching
    {
        get
        {
            return this._isSpeaching;
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="hostIp">ホストIP</param>
    /// <param name="hostPort">ホストポート</param>
    public BoyomiClient(string hostIp,int hostPort)
    {
        this.hostIp = hostIp;
        this.hostPort = hostPort;
        readbuf = new byte[1024];
    }

    /// <summary>
    /// 棒読みちゃんに接続を試みる
    /// </summary>
    public void Connect()
    {
        try
        {
            tcpClient = new TcpClient(hostIp, hostPort);
            //コールバック指定
            tcpClient.GetStream().BeginRead(readbuf, 0, readbuf.Length, CallBackBeginReceive, null);
        }
        catch (Exception e)
        {
            //nice catch!
        }
    }

    /// <summary>
    /// 受信CallBack
    /// 発声中であるかどうかしか確認しない
    /// </summary>
    /// <param name="ar">IAsyncResult</param>
    private void CallBackBeginReceive(IAsyncResult ar)
    {
        var bytes = tcpClient.GetStream().EndRead(ar);
        //結果が0でないなら発声中
        _isSpeaching = readbuf[0] > 0;
    }

    /// <summary>
    /// 棒読みちゃん発声状態を問い合わせる
    /// 結果は最終的に_isSpeachingに入る
    /// </summary>
    public void CheckSpeaking()
    {
        //一回のRequestごとに接続が切れるので毎回つなぐ
        Connect();
        try
        {
            using (NetworkStream ns = tcpClient.GetStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ns))
                {
                    //0x0120はGetNowPlaying（音声再生状態の取得）
                    Int16 iCommand = 0x0120;
                    bw.Write(iCommand);
                }
            }
        }
        catch (Exception e)
        {
            //nice catch!
        }
    }
}

