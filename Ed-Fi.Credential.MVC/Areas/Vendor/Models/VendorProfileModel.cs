using System.Collections.Generic;
using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.MVC.Models;

namespace Ed_Fi.Credential.MVC.Areas.Vendor.Models
{
    public class VendorProfileModel
    {
        public string VendorName { get; set; }
        public bool HasAgreed { get; set; }
        public bool HasNonTestApiClients { get; set; }

        public bool NeedsToSeeAgreement
        {
            get
            {
                return HasAgreed == false &&
                       HasNonTestApiClients;
            }
        }

        public List<UrlModel> Urls
        {
            get
            {

                return new List<UrlModel>
                {
                    new UrlModel{Name = "Authentication", Url = UrlBusiness.EdFiWebApiAuthUrl},
                    new UrlModel{Name = "Base", Url = UrlBusiness.EdFiWebApiBaseUrl},
                    new UrlModel{Name = "Swagger", Url = UrlBusiness.EdFiSwaggerUrl}
                    
                };
            }
        }

        public List<UrlModel> V3Urls
        {
            get
            {

                return new List<UrlModel>
                {
                    new UrlModel{Name = "Authentication", Url = UrlBusiness.EdFiV3WebApiAuthUrl},
                    new UrlModel{Name = "Base", Url = UrlBusiness.EdFiV3WebApiBaseUrl},
                    new UrlModel{Name = "Swagger", Url = UrlBusiness.EdFiV3SwaggerUrl}

                };
            }
        }
        public List<UserModel> Users {get;set; }
        public UserModel CurrentUser { get; set; }

        public string AgreementMessage { get; set; }
    }
}