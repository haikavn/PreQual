-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE GetAffiliateResponsesByLeadId
(
    -- Add the parameters for the stored procedure here
    @leadId bigint
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    SELECT * from AffiliateResponse where LeadId = @leadId order by Id;
END