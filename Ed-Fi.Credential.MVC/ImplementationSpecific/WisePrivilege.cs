namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{
    public enum WisePrivilege
    {
        EditCredential,         //AllFunction - DPI
        EditAgreement,          //Edit role - district/school 
        EditVendorSubscription, //Edit role - district/school
        ViewCredentials,         //Vendor role
        ViewReport,              //Internal Staff
        ViewMonitoring          //VendorSupport Role
    }
}