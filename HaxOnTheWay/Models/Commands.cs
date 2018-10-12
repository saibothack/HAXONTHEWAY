using System;
using SQLite;
using Xamarin.Forms;

namespace HaxOnTheWay.Models
{
    public class Commands
    {
        [PrimaryKey]
        public int iCommand { get; set; }
        public string sTypeCommand { get; set; }
        public string sTypeCommandColor { get; set; }
        public string sSubservices { get; set; }
        public string sSubservicesColor { get; set; }
        public string sSchedule { get; set; }
        public string sTypeTruck { get; set; }
        public string sCustomer { get; set; }
        public string sStatus { get; set; }
        public int iEstatus { get; set; }
        public string sCompany { get; set; }
        public string sContact { get; set; }
        public string sAddress { get; set; }
        public string sSuite { get; set; }
        public string sCity { get; set; }
        public string sCP { get; set; }
        public string sPhone { get; set; }
        public string sCellPhone     { get; set; }
        public string sCompanyDelivery { get; set; }
        public string sContactDelivery { get; set; }
        public string sAddressDelivery { get; set; }
        public string sSuiteDelivery { get; set; }
        public string sCityDelivery { get; set; }
        public string sCPDelivery { get; set; }
        public string sPhoneDelivery { get; set; }
        public string sCellPhoneDelivery { get; set; }
        public string sQuanty { get; set; }
        public string sWeight { get; set; }
        public string sDescription { get; set; }    
        public string sReference { get; set; }
        public string sTransfer { get; set; }
        public string sInstruction { get; set; }
        public string sDate { get; set; }
        public string sColor { get; set; }
    }
}
