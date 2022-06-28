-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[CheckBlackListValue]
(
    -- Add the parameters for the stored procedure here
	@Name varchar(50),
	@Value varchar(150)
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

    -- Insert statements for procedure here
    
	declare @blackListTypeId bigint = 0;

	select @blackListTypeId = Id from BlackListType where LOWER(Name) = @Name;

	select top 1 * from BlackListValue where BlackListTypeId = @blackListTypeId and 
	(
		([Condition] = 1 and [Value] = @Value)
		or 
		([Condition] = 2 and @Value like [Value] + '%')
		or 
		([Condition] = 3 and @Value like '%' + [Value])
		or 
		([Condition] = 4 and @Value like '%' + [Value] + '%')
	)
END