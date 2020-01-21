using Ed_Fi.Credential.Domain.Enums;
using System.Configuration;

namespace Ed_Fi.Credential.Business
{
    public static class UrlBusiness
    {
        public static string EdFiWebApiUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationSettingKeys.EDFI_WEBAPI_URL]; }
        }

        public static string EdFiWebApiAuthUrl
        {
            get { return EdFiWebApiUrl + "/oauth/authorize"; }
        }

        public static string EdFiWebApiBaseUrl
        {
            get { return EdFiWebApiUrl + "/api/v2.0"; }
        }

        public static string EdFiSwaggerUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationSettingKeys.EDFI_SWAGGER_URL]; }
        }




        public static string EdFiV3WebApiUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationSettingKeys.EDFI_V3_WEBAPI_URL]; }
        }

        public static string EdFiV3WebApiAuthUrl
        {
            get { return EdFiV3WebApiUrl + "oauth/token"; }
        }

        public static string EdFiV3WebApiBaseUrl
        {
            get { return EdFiV3WebApiUrl + "data/v3"; }
        }

        public static string EdFiV3SwaggerUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationSettingKeys.EDFI_V3_SWAGGER_URL]; }
        }


        public static string SecureHomeUrl
        {
            get { return ConfigurationManager.AppSettings[ApplicationSettingKeys.SECURE_HOME_URL]; }
        }

    }
}
