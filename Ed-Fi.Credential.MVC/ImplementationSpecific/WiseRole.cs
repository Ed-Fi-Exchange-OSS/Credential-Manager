namespace Ed_Fi.Credential.MVC.ImplementationSpecific
{
    public enum WiseRole
    {
        AdminRead,      //DPI User                  (EDFICRED_DPI)
        AllFunctions,   //DPI User or district      (EDFICRED_DPI, EDFICRED)
        Vendor,         //SIS User                  (EDFICRED_DPI)
        Edit,            //District User             (EDFICRED)
        VendorSupport
    }
}