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
        public static readonly string mainTemplatesContainer = "Templates";
        public static readonly string emailTemplatesContainer = "EmailTemplates";
        public static readonly string invoicesTemplatesContainer = "InvoiceTemplates";
        #endregion

        #region " For Filteration Static Fields"
        public static readonly int itemsPerPage = 10;
        #endregion

        #region " For Invoice Status"
        public static readonly string statusPaid = "PAID";
        public static readonly string statusUnpaidOrPending = "PENDING"; 
        public static readonly string statusRejected = "REJECTED"; 
        public static readonly string statusDrafted = "DRAFTED";
        public static readonly string statusSent = "SENT";
        public static readonly string statusOverdue = "OVERDUE";
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

        #region " Html template names for emails"
        public static readonly string email_template_Confirm_Account_Registration = "Confirm_Account_Registration.html";
        public static readonly string email_template_Confirm_Account_Registration_Success = "Confirm_Account_Registration_Success.html";
        public static readonly string email_template_Welcome_Email_Template = "Welcome_Email_Template.html"; 
        #endregion

        #region " Html template names for invoices"
        public static readonly string invoice_template_Sample_Invoice_Template = "Sample_Invoice_Template.html";
        #endregion

        #region " Mail Subject for emails"
        public static readonly string subject_Confirm_Account_Registration_Success = "Account Registration Successfully Confirmed";
        public static readonly string subject_SendInvoice_to_customer = "You Recieve An Invoice";
        public static readonly string subject_SendQuotation_to_customer = "You Recieve An Quotation";
        #endregion
    }
}
