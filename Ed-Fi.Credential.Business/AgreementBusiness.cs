using System;
using System.Linq;
using Ed_Fi.Credential.Business.Common;
using Ed_Fi.Credential.Domain.Enums;
using Ed_Fi.Credential.Domain.Model;

namespace Ed_Fi.Credential.Business
{
    public interface IAgreementBusiness : IBaseBusiness<Agreement>
    {
        Agreement FindByAgencyNotVendorSub(int edOrgId, AgreementType agreementType);
        Agreement FindByWamsIdNotVendorSub(string wamsId, AgreementType agreementType);
        void UpsertNotVendorSub(int edOrgId, string wamsId, bool agree, AgreementType agreementType, string id);
        void UpsertVendorSub(int edOrgId, string wamsId, int vendorId);

    }

    public class AgreementBusiness : BaseEdFiAdminBusiness<Agreement>, IAgreementBusiness
    {
        public AgreementBusiness(IEdFiAdminDbContext context)
            : base(context)
        {
        }

        public Agreement FindByAgencyNotVendorSub(int edOrgId, AgreementType agreementType)
        {
            var agreement = Context.Query<Agreement>()
                .FirstOrDefault(a => a.EducationOrganizationId == edOrgId && a.AgreementTypeId == (int)agreementType);
            return agreement;
        }

        public Agreement FindByWamsIdNotVendorSub(string wamsId, AgreementType agreementType)
        {
            var agreement = Context.Query<Agreement>()
                .FirstOrDefault(a => a.WamsId == wamsId && a.AgreementTypeId == (int)agreementType);
            return agreement;
        }

        private Agreement FindVendorSubByAgency(int edOrgId, int vendorId)
        {
            var agreement = Context.Query<Agreement>()
                .FirstOrDefault(a => a.EducationOrganizationId == edOrgId && a.AgreementTypeId == (int)AgreementType.VendorSubscription && a.VendorId == vendorId);
            return agreement;
        }

        public void UpsertNotVendorSub(int edOrgId, string wamsId, bool agree, AgreementType agreementType, string id)
        {

            var agreement = agreementType == AgreementType.Agency ? FindByAgencyNotVendorSub(edOrgId, agreementType) : FindByWamsIdNotVendorSub(wamsId, agreementType);

            if (agreement != null)
            {
                agreement.Agree = agree;
                agreement.ModifiedDate = DateTime.Now;
                agreement.ModifierId = id;
            }
            else
            {
                agreement = new Agreement
                {
                    EducationOrganizationId = edOrgId,
                    WamsId = wamsId,
                    Agree = agree,
                    AgreementTypeId = (int)agreementType,
                    ModifiedDate = DateTime.Now,
                    ModifierId = id
                };

                Context.AddEntity(agreement);
            }

            Context.SaveChanges(wamsId);
        }

        public void UpsertVendorSub(int edOrgId, string wamsId, int vendorId)
        {
            var agreement = FindVendorSubByAgency(edOrgId, vendorId);

            if (agreement != null)
            {
                agreement.Agree = true;
                agreement.ModifiedDate = DateTime.Now;
                agreement.ModifierId = wamsId;
            }
            else
            {
                agreement = new Agreement
                {
                    EducationOrganizationId = edOrgId,
                    WamsId = wamsId,
                    Agree = true,
                    AgreementTypeId = (int)AgreementType.VendorSubscription,
                    ModifiedDate = DateTime.Now,
                    ModifierId = wamsId,
                    VendorId = vendorId
                };

                Context.AddEntity(agreement);
            }

            Context.SaveChanges(wamsId);
        }
    }
}
