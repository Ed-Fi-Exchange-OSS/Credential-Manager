using System.Collections.Generic;

namespace Ed_Fi.Credential.MVC
{
    public static class Constants
    {
        public const int DPI = 48856;
        public const string CLAIMSET_MODELS = "CLAIMSET_MODELS";
        public const string PROFILE_MODELS = "PROFILE_MODELS";
        public const string YEAR_MODELS = "YEAR_MODELS";

        public const string CONTENT_TYPES = "CONTENT_TYPES";

        public const string CAN_USER_VIEW_REPORT = "CanUserViewReport";
        public const string CAN_USER_EDIT_CREDENTIAL = "CanUserEditCredential";
        public const string CAN_USER_EDIT_VENDOR_SUBSCRIPTION = "CanUserEditVendorSubscription";
        public const string CAN_USER_VIEW_CREDENTIALS = "CanUserViewCredentials";

        public const string HASHED_SECRET_MESSAGE = "If you have forgotten the secret it must be reset. To reset, click on Reset Secret.";
        public const string NOT_HASHED_SECRET_MESSAGE = "After the first API communication the secret will be hashed. Secret must be reset if it is lost or forgotten.";

    }
}