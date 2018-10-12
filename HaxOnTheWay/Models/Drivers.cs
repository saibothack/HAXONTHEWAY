using SQLite;

namespace HaxOnTheWay.Models
{
    public class Drivers
    {
        [PrimaryKey]
        public int iDriver { get; set; }
        public string sEmail { get; set; }
        public string sPhone { get; set; }
        public string sName { get; set; }
        public string sNumber { get; set; }
        //public string sToken { get; set; }
        public string sAddress { get; set; }

        public override string ToString()
        {
            return string.Format("[Drivers: iDriver={0}, sName={1}, sNumber={2}, sAddress={2}, sPhone={2}, sEmail={2}]", 
                                 iDriver, sName, sNumber, sAddress, sPhone, sEmail);
        }
    }
}
