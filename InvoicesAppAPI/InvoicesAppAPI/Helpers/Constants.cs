using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public static class Constants
    {
        #region " For Roles"
        public static readonly string isSuperAdmin = "superadmin"; 
        public static readonly string isAdmin = "admin";
        public static readonly string isSubAdmin = "subadmin";
        #endregion

        #region " Miscellaneous"
        public static readonly long baseCurrencyId = 1; 
        public static readonly string baseLanguage = "English";
        #endregion

        #region " Folder Path i.e Images Container Names"
        public static readonly string userImagesContainer = "UserImages";
        #endregion

        #region " For Filteration Static Fields"
        public static readonly int itemsPerPage = 10;
        #endregion

        #region " For Invoice Status"
        public static readonly string statusPaid = "PAID";
        public static readonly string statusUnpaid = "UNPAID";
        public static readonly string statusPending = "PENDING";
        public static readonly string statusRejected = "REJECTED";
        public static readonly string statusCancelled = "CANCELED";
        public static readonly string statusDrafted = "DRAFTED";
        public static readonly string statusSent = "SENT";
        #endregion

        #region " For Invoice Type Code"
        public static readonly string typeInvoice = "INV";
        public static readonly string typeQuotation = "QUO";
        #endregion

        #region " For Payment Modes"
        public static readonly string paymentModeBankTransfer = "BANK_TRANFER";
        public static readonly string paymentModeCheque = "CHEQUE";
        public static readonly string paymentModeCash = "CASH";
        public static readonly string paymentModeRTGS = "RTGS";
        public static readonly string paymentModePaypal = "PAYPAL";
        public static readonly string paymentModeWireTransfer = "WIRE_TRANFER";
        #endregion
    }
}
