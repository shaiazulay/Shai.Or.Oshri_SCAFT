using System.Net;


namespace SCAFTI
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
            return "Name: "+sUserName+" IP: " + oIP.ToString();
        } 

        public bool Equals(User oUser)
        {
            if (this.sUserName == oUser.sUserName && this.oIP == this.oIP)
                return true;
            else
                return false;
        }
    }
}