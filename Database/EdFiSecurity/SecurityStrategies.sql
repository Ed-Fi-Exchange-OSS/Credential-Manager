CREATE VIEW [dbo].[SecurityStrategies]
	AS 

with claimStrategies as(
Select rc.ResourceName, a.ActionName, auths.AuthorizationStrategyName, cs.ClaimSetName, autho.AuthorizationStrategyName ClaimAuthorizationStrategy
,
a.ActionId, rc.ResourceClaimId, cs.ClaimSetId
from dbo.ResourceClaims rc
inner join dbo.ResourceClaimAuthorizationMetadatas rcam on rc.ResourceClaimId = rcam.ResourceClaim_ResourceClaimId
inner join dbo.AuthorizationStrategies auths on rcam.AuthorizationStrategy_AuthorizationStrategyId = auths.AuthorizationStrategyId
inner join dbo.ClaimSetResourceClaims csrc on rc.ResourceClaimId = csrc.ResourceClaim_ResourceClaimId
inner join dbo.Actions a on a.ActionId = csrc.Action_ActionId and a.ActionId = rcam.Action_ActionId
inner join dbo.ClaimSets cs on csrc.ClaimSet_ClaimSetId = cs.ClaimSetId
left outer join dbo.AuthorizationStrategies autho on csrc.AuthorizationStrategyOverride_AuthorizationStrategyId = autho.AuthorizationStrategyId
)



select csr.ResourceClaimId, csr.ClaimsetId, csr.ResourceName, csr.ClaimSetName, 
Coalesce(csr.ClaimAuthorizationStrategy, csr.AuthorizationStrategyName, 'Denied') ReadStrategy,
Coalesce(csc.ClaimAuthorizationStrategy, csc.AuthorizationStrategyName, 'Denied') CreateStrategy, 
Coalesce(csu.ClaimAuthorizationStrategy, csu.AuthorizationStrategyName, 'Denied') UpdateStrategy,
Coalesce(csd.ClaimAuthorizationStrategy, csd.AuthorizationStrategyName, 'Denied') DeleteStrategy
from claimStrategies csr
left outer join  claimStrategies csc on csr.ClaimSetId = csc.ClaimSetId and csr.ResourceClaimId = csc.ResourceClaimId 
	and csc.ActionId = 1
left outer join  claimStrategies csu on csr.ClaimSetId = csu.ClaimSetId and csr.ResourceClaimId = csu.ResourceClaimId 
	and csu.ActionId = 3
left outer join  claimStrategies csd on csr.ClaimSetId = csd.ClaimSetId and csr.ResourceClaimId = csd.ResourceClaimId 
	and csd.ActionId = 4
where csr.ActionId = 2 
