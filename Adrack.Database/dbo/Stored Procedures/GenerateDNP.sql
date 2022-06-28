-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GenerateDNP]
(
    -- Add the parameters for the stored procedure here
	@count int
)
AS
BEGIN
	declare @index int = 1;
	declare @email varchar(50);

	while(@index <= @count)
	begin
		set @email = SUBSTRING(CONVERT(varchar(40), NEWID()),0,9) + '@adrack.com';

		insert into DoNotPresent values(@email, SUBSTRING(CONVERT(varchar(40), NEWID()),0,9), '', null, 1);

		set @index = @index + 1;
	end;

END