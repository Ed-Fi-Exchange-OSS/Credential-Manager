using System.Collections;
using System.Collections.Generic;
using Ed_Fi.Credential.Business;
using Ed_Fi.Credential.Domain.Model;
using Ed_Fi.Credential.MVC.Areas.Vendor.Models;
using Ed_Fi.Credential.MVC.Models;

namespace Ed_Fi.Credential.MVC.Areas.Agency.Models
{
    public class VendorSubscriptionModel
    {
        public string AgreementMessage { get; set; }
        public string VendorSubscriptionMessage { get; set; }
        public bool HasAgreed { get; set; }

        public bool IsChoice { get; set; }
        public IEnumerable<ClaimsetDetail> ClaimsetDetails { get; set; }

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

        public IList<AgencyClientModel> AgencyApiClients { get; set; }
        public IList<AgencyClientModel> SchoolApiClients { get; set; }
    }
}