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
    private bool _isSpeaching;
    public bool isSpeaching
    {
        get { return this._isSpeaching; }
    }

    bool isConnected
    {
        get { return this.tcpClient != null && tcpClient.Connected; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="hostIp">ホストIP</param>
    /// <param name="hostPort">ホストポート</param>
    public BoyomiClient(string hostIp, int hostPort)
    {
        this.hostIp = hostIp;
        this.hostPort = hostPort;
    }

    /// <summary>
    /// 棒読みちゃんに接続を試みる
    /// </summary>
    public bool Connect()
    {
        try
        {
            tcpClient = new TcpClient(hostIp, hostPort);
            return tcpClient.Connected;
        }
        catch
        {
            //nice catch
            return false;
        }
    }


    /// <summary>
    ///  棒読みちゃん発声状態を問い合わせる(同期処理）
    /// </summary>
    /// <returns>発声中かどうか</returns>
    public void checkSpeaching()
    {
        if (!isConnected) { Connect(); }
        try
        {
            NetworkStream ns = tcpClient.GetStream();
            BinaryWriter bw = new BinaryWriter(ns);

            //0x0120はGetNowPlaying（音声再生状態の取得）
            Int16 iCommand = 0x0120;
            bw.Write(iCommand);
            bw.Flush();
            BinaryReader br = new BinaryReader(ns);
            _isSpeaching = br.ReadByte() > 0;
        }
        catch
        {
            return;
        }
    }
}

