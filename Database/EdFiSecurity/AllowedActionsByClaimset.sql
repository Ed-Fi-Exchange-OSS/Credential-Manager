
CREATE VIEW [dbo].[AllowedActionsByClaimset]
	AS 

	Select rc.ResourceName, a.ActionName, cs.ClaimSetName
	from dbo.ResourceClaims rc
	inner join dbo.ResourceClaimAuthorizationMetadatas rcam on rc.ResourceClaimId = rcam.ResourceClaim_ResourceClaimId
	inner join dbo.ClaimSetResourceClaims csrc on rc.ResourceClaimId = csrc.ResourceClaim_ResourceClaimId
	inner join dbo.Actions a on a.ActionId = csrc.Action_ActionId and a.ActionId = rcam.Action_ActionId
	inner join dbo.ClaimSets cs on csrc.ClaimSet_ClaimSetId = cs.ClaimSetId
GO


