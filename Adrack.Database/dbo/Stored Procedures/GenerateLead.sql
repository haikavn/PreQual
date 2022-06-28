-- =============================================
-- Author:      <Author, , Name>
-- Create Date: <Create Date, , >
-- Description: <Description, , >
-- =============================================
CREATE PROCEDURE [dbo].[GenerateLead]
(
    -- Add the parameters for the stored procedure here
	@created datetime
)
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON

	declare @email varchar(50);
	set @email = SUBSTRING(CONVERT(varchar(40), NEWID()),0,9) + '@adrack.com';

	declare @Upper int = 3;
	declare @Lower int = 1;

	DECLARE @status SMALLINT
	set @status = ROUND(((@Upper - @Lower) * RAND() + @Lower), 0);

	print @status;

	DECLARE @buyerchannelid bigint
	set @buyerchannelid = ROUND(((2 - 1 -1) * RAND() + 1), 0)

	declare @buyerprice decimal = 15;
	declare @affiliateprice decimal = 5;

	DECLARE @buyerchannelid2 bigint = @buyerchannelid;
	if @status <> 1 
	begin
		set @buyerchannelid2 = null;
		set @buyerprice = 0;
		set @affiliateprice = 0;
	end

	declare @response varchar(max);
	set @response = '<response>
					  <id>0001</id>
					  <status>sold</status>
					  <message></message>
					  <price>0.5</price>
					  <redirect><![CDATA[]]></redirect>
					</response>';

	if @status = 2
	begin
		set @response = '<response>
						  <id>0</id>
						  <status>error</status>
						  <message>error message here</message>
						  <price></price>
						  <redirect></redirect>
						</response>';
	end;

	if @status = 3
	begin
		set @response = '<response>
						  <id>0</id>
						  <status>reject</status>
						  <message>lead was not sold in marketplace</message>
						  <price></price>
						  <redirect></redirect>
						</response>';
	end;

	declare @leadid bigint = 0;

    -- Insert statements for procedure here
    insert into leadmain values(@created, 1, 1, @status, 1, 0, 0, 0, 0, 0, '<REQUEST>
	<REFERRAL>
		<CHANNELID>f56ec58</CHANNELID>
		<PASSWORD>ecd1ad14</PASSWORD>
		<AFFSUBID>7937</AFFSUBID>
		<AFFSUBID2>980890</AFFSUBID2>
		<REFERRINGURL>www.leadz.aa</REFERRINGURL>
		<MINPRICE>3</MINPRICE>
	</REFERRAL>
	<CUSTOMER>
		<PERSONAL>
			<IPADDRESS>71.453.454.123</IPADDRESS>
			<REQUESTEDAMOUNT>100</REQUESTEDAMOUNT>
			<SSN>123123123</SSN>
			<DOB>05-02-1988</DOB>
			<FIRSTNAME>Eliza</FIRSTNAME>
			<LASTNAME>Brown</LASTNAME>
			<ADDRESS>California, Glendale Woodroad 554 </ADDRESS>
			<CITY>Glendale</CITY>
			<STATE>WA</STATE>
			<ZIP>90005</ZIP>
			<HOMEPHONE>8184564561</HOMEPHONE>
			<CELLPHONE>8184564564</CELLPHONE>
			<DLSTATE>CA</DLSTATE>
			<DLNUMBER>35252</DLNUMBER>
			<ARMEDFORCES>no</ARMEDFORCES>
			<CONTACTTIME>anytime</CONTACTTIME>
			<RENTOROWN>rent</RENTOROWN>
			<EMAIL>elizabrown@gmail.com</EMAIL>
			<ADDRESSMONTH>24</ADDRESSMONTH>
			<CITIZENSHIP>yes</CITIZENSHIP>
		</PERSONAL>
		<EMPLOYMENT>
			<INCOMETYPE>job income</INCOMETYPE>
			<EMPTIME>45</EMPTIME>
			<EMPNAME>Google inc.</EMPNAME>
			<EMPPHONE>2484441111</EMPPHONE>
			<JOBTITLE>CEO</JOBTITLE>
			<PAYFREQUENCY>weekly</PAYFREQUENCY>
			<NEXTPAYDATE>11-11-2016</NEXTPAYDATE>
			<SECONDPAYDATE>10-12-2018</SECONDPAYDATE>
		</EMPLOYMENT>
		<BANK>
			<BANKNAME>Bank of America</BANKNAME>
			<BANKPHONE>2486796849</BANKPHONE>
			<ACCOUNTTYPE>Checking Account </ACCOUNTTYPE>
			<ROUTINGNUMBER>104000016</ROUTINGNUMBER>
			<ACCOUNTNUMBER>23423424234</ACCOUNTNUMBER>
			<BANKMONTHS>78</BANKMONTHS>
			<NETMONTHLYINCOME>10000</NETMONTHLYINCOME>
			<DIRECTDEPOSIT>yes</DIRECTDEPOSIT>
		</BANK>
	</CUSTOMER>
</REQUEST>', @buyerchannelid2, 0, GETUTCDATE(), GETUTCDATE(), GETUTCDATE(), GETUTCDATE(), '127.0.0.1', 0);

	set @leadid = scope_identity();

	insert into leadcontent values(@leadid, '127.0.0.1', 2, 'John', 'Smith', 'Address', 'Glendale', 'WA', '123456', '12345678', '12345678', @email, '', '', '', '', 0, 0, 0, GETUTCDATE(), 29, 0, '', '', '', '', '', '', @created, 1, 0, '', '', '', GETUTCDATE(), '');

	insert into leadmainresponse values(1, @buyerchannelid, @response, 0, @leadid, 1, 1, 1, '', @status, @affiliateprice, @buyerprice, 0, '', @created, 0, getutcdate(), 0, 0, '12345678');

	insert into posteddata values(@leadid, 1, '', @created, 0, @status);

	insert into affiliateresponse values(@leadid, 1, @created, '', 1, 0, getutcdate(), '', @status, 0, 0, '', 'WA');
END