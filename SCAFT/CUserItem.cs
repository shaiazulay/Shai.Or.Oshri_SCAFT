using System.Net;

public class CUserItem
{
    public string sUserName { get; set; }
    public IPAddress oUserIP { get; set; }

    public override string ToString()
    {
        return sUserName;
    }

    public CUserItem(string _sUserName)
    {
        sUserName = _sUserName;
    }

    public CUserItem(string _sUserName, IPAddress _oUserIP)
    {
        sUserName = _sUserName;
        oUserIP = _oUserIP;
    }
}