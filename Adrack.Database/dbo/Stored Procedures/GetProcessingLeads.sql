-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[GetProcessingLeads] 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select count(Id) from LeadMain where cast(Created as date) = cast(getutcdate() as date) and Status = 4;

	--declare @value varchar(10) = '0';
	--declare @leads int = 0;

	--select @value = [Value] from Setting where [Key] = 'ProcessingLeads';

	--if @value is not null
	--begin
	--	set @leads = cast(@value as int);
	--end;

	--select @leads;
END