-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetDublicateLead] 
	-- Add the parameters for the stored procedure here
	@ssn varchar(150),
	@created DateTime,
	@affiliateid bigint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT top 1 * from dbo.LeadContent where Ssn = @ssn and Created >= @created and AffiliateId = @affiliateid order by Id desc
END