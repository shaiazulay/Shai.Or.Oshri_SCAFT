using System.Net;


namespace SCAFT
{
    public class User
    {
        public IPAddress oIP { get; set; }

        public string sUserName { get; set; }

        public User(IPAddress ip, string Username)
        {
            oIP = ip;
            sUserName = Username;
        }

        public override string ToString()
        {
            return sUserName;
        }

        public string sIApprovedThisFileNameToSendMe { get; set; }

        public string sIWantToSendThisFileNameToThisUser { get; set; }
    }
}