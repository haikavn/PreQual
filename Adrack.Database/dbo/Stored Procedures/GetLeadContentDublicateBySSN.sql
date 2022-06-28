-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadContentDublicateBySSN]
(
    -- Add the parameters for the stored procedure here
    @ssn varchar(100)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here

	select top 200 LeadContentDublicate.*, Affiliate.[Name] AS AffiliateName
	from LeadContentDublicate
	inner join Affiliate ON Affiliate.Id = LeadContentDublicate.AffiliateId
	where ssn = @ssn
	order by id desc

END