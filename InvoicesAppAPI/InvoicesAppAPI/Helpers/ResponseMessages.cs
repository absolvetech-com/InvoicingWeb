using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoicesAppAPI.Helpers
{
    public static class ResponseMessages
    {
        public static readonly string msgUserRegisterSuccess = "user registered succesfully";
        public static readonly string msgUserRoleNotAuthorized = "requested user type is not authorized";
        public static readonly string msgSomethingWentWrong = "something went wrong. ";
        public static readonly string msgParametersNotCorrect = "parameters are not correct";
        public static readonly string msgCouldNotFoundAssociatedUser = "could not found any user associated with this email";
        public static readonly string msgUserBlockedOrDeleted = "user blocked or deleted. please contact to administrator";
        public static readonly string msgEmailNotConfirmed = "email not confirmed";
        public static readonly string msgUserLoginSuccess = "user login successfully"; 
        public static readonly string msgInvalidCredentials = "username or password is incorrect";
        public static readonly string msgEmailConfirmationSuccess = "email confirmed successfully";
        public static readonly string msgConfirmationCodeSentSuccess = "confirmation code sent on your email";
        public static readonly string msgInvalidOTP = "otp is invalid";
        public static readonly string msgOTPSentSuccess = "otp sent on your email";
        public static readonly string msgPasswordResetSuccess = "password reset successfully";
        public static readonly string msgPasswordChangeSuccess = "password changed successfully";
        public static readonly string msgEmailAlreadyConfirmed = "email already confirmed";
        public static readonly string msgResetEmailOtpSendSuccess = "reset email OTP sent on your both emails";
        public static readonly string msgNewOldOtpInvalid = "either new or old email otp is invalid";
        public static readonly string msgEmailAlreadyUsed = "the email provided already in used. please try with other email";
        public static readonly string msgEmailResetSuccess = "email reset successfully. please login with new email";
        public static readonly string msgLogoutSuccess = "user logout successfully";
        public static readonly string msgDbConnectionError = "database connection error";
        public static readonly string msgInvoiceStatusInvalid = "requested status is not correct";
        public static readonly string msgInvoiceTypeInvalid = "requested type is not correct";
        public static readonly string msgBlockOrInactiveUserNotPermitted = "block or inactive user can't update detials";
        public static readonly string msgSessionExpired = "your current session has expired. please login again to keep all your service working";

        public static readonly string msgCreationSuccess = " created successfully";
        public static readonly string msgUpdationSuccess = " updated successfully";
        public static readonly string msgFoundSuccess = " found successfully";
        public static readonly string msgShownSuccess = " shown successfully";
        public static readonly string msgNotFound = "could not found any ";
        public static readonly string msgListFoundSuccess = " list shown successfully";
        public static readonly string msgDeletionSuccess = " deleted successfully";
        public static readonly string msgAlreadyExists = " already exists";
    } 
}
