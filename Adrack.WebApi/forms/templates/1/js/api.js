function PostXml() {
	var AmountRequested = $('.step-1').data('value');
	var Homeowner = $('.step-4').data('value');
	var Carowner = $('.step-5').data('value');
	var ContactTime = $('.step-9').data('value');
	var ActiveDuty = $('.step-10').data('value');
	var IncomeSource = $('.step-12').data('value');
	var TimeEmployed = $('.step-13').data('value');
	var MonthlyNetIncome = $('.step-17').data('value');
	var IGetPaid = $('.step-18').data('value');
	var AcountType = $('.step-20').data('value');
	var DirectDeposit = $('.step-23').data('value');

	var dataSend = new Object();
	var secondDay = new Date($('#datepicker').val());
	secondDay.setDate(secondDay.getDate() + period);

	dataSend.REQUESTEDAMOUNTMIN = AmountRequested;
	dataSend.REQUESTEDAMOUNTMAX = AmountRequested;
	dataSend.SSN = $('#SSN').val();
	dataSend.DOB = $('#months').val() + '-' + $('#days').val() + '-' + $('#years').val();
	dataSend.FIRSTNAME = $('#FirstName').val();
	dataSend.LASTNAME = $('#LastName').val();
	dataSend.ADDRESS = $('#Address').val();
	dataSend.CITY = $('#City').val();
	dataSend.STATE = $('#State').val();
	dataSend.ZIP = $('#ZIP').val();
	dataSend.CELLPHONE = $('#phone').val();
	dataSend.MOBILEPHONE = $('#MobilePhone').val();
	dataSend.DLSTATE = $('#DriversLicenseState').val();
	dataSend.DLNUMBER = $('#DriversLicenseNumber').val();
	dataSend.ARMEDFORCES = ActiveDuty;
	dataSend.CONTACTTIME = ContactTime;
	dataSend.HOMEOWNER = Homeowner;
	dataSend.CAROWNER = Carowner;
	dataSend.EMAIL = $('#email').val();
	dataSend.INCOMETYPE = IncomeSource;
	dataSend.EMPTIME = skipped ? 12 : TimeEmployed;
	dataSend.EMPNAME = skipped ? IncomeSource : $('#EmployerName').val();
	dataSend.EMPPHONE = skipped ? $('#phone').val() : $('#WorkPhone').val();
	dataSend.JOBTITLE = skipped ? IncomeSource : $('#JobTitle').val();
	dataSend.PAYFREQUENCY = IGetPaid;
	dataSend.NEXTPAYDATE = $('#datepicker').val();
	dataSend.SECONDPAYDATE = moment(secondDay).format('MM/DD/YYYY');
	dataSend.BANKNAME = $('#BankName').val();
	dataSend.ACCOUNTTYPE = AcountType;
	dataSend.ROUTINGNUMBER = $('#RoutingNumber').val();
	dataSend.ACCOUNTNUMBER = $('#AccountNumber').val();
	dataSend.NETMONTHLYINCOME = MonthlyNetIncome;
	dataSend.DIRECTDEPOSIT = DirectDeposit;
	dataSend.REFERRINGURL = document.location.href;

	var REQUESTEDAMOUNTMIN = AmountRequested;
	var REQUESTEDAMOUNTMAX = AmountRequested;
	var SSN = $('#SSN').val();
	var DOB = $('#months').val() + '-' + $('#days').val() + '-' + $('#years').val();
	var FIRSTNAME = $('#FirstName').val();
	var LASTNAME = $('#LastName').val();
	var ADDRESS = $('#Address').val();
	var CITY = $('#City').val();
	var STATE = $('#State').val();
	var ZIP = $('#ZIP').val();
	var CELLPHONE = $('#phone').val();
	var MOBILEPHONE = $('#MobilePhone').val();
	var DLSTATE = $('#DriversLicenseState').val();
	var DLNUMBER = $('#DriversLicenseNumber').val();
	var ARMEDFORCES = ActiveDuty;
	var CONTACTTIME = ContactTime;
	var HOMEOWNER = Homeowner;
	var CAROWNER = Carowner;
	var EMAIL = $('#email').val();
	var INCOMETYPE = IncomeSource;
	var EMPTIME = skipped ? 12 : TimeEmployed;
	var EMPNAME = skipped ? IncomeSource : $('#EmployerName').val();
	var EMPPHONE = skipped ? $('#phone').val() : $('#WorkPhone').val();
	var JOBTITLE = skipped ? IncomeSource : $('#JobTitle').val();
	var PAYFREQUENCY = IGetPaid;
	var NEXTPAYDATE = $('#datepicker').val();
	var SECONDPAYDATE = moment(secondDay).format('MM/DD/YYYY');
	var BANKNAME = $('#BankName').val();
	var ACCOUNTTYPE = AcountType;
	var ROUTINGNUMBER = $('#RoutingNumber').val();
	var ACCOUNTNUMBER = $('#AccountNumber').val();
	var NETMONTHLYINCOME = MonthlyNetIncome;
	var DIRECTDEPOSIT = DirectDeposit;
	var REFERRINGURL = document.location.href;

	var postXml =
		'<REQUEST>\
			<REFERRAL>\
				<CHANNELID>31b6474</CHANNELID>\
				<PASSWORD>a0555f21</PASSWORD>\
				<AFFSUBID></AFFSUBID>\
				<AFFSUBID2></AFFSUBID2>\
				<AFFSUBID3></AFFSUBID3>\
				<AFFSUBID4></AFFSUBID4>\
				<AFFSUBID5></AFFSUBID5>\
				<REFERRINGURL>' + document.location.href + '</REFERRINGURL>\
				<MINPRICE>0</MINPRICE>\
			</REFERRAL>\
			<CUSTOMER>\
				<PERSONAL>\
					<IPADDRESS>' + 'REMOTE_HOST' + '</IPADDRESS>\
					<REQUESTEDAMOUNT>' + REQUESTEDAMOUNTMIN+ '</REQUESTEDAMOUNT>\
					<SSN>' + SSN+ '</SSN>\
					<DOB>' + DOB+ '</DOB>\
					<FIRSTNAME>' +FIRSTNAME+ '</FIRSTNAME>\
					<LASTNAME>' +LASTNAME+ '</LASTNAME>\
					<ADDRESS>' +ADDRESS+ '</ADDRESS>\
					<CITY>' +CITY+ '</CITY>\
					<STATE>' +STATE+ '</STATE>\
					<ZIP>' +ZIP+ '</ZIP>\
					<HOMEPHONE>' +CELLPHONE+ '</HOMEPHONE>\
					<CELLPHONE>' +MOBILEPHONE+ '</CELLPHONE>\
					<DLSTATE>' +DLSTATE+ '</DLSTATE>\
					<DLNUMBER>' +DLNUMBER+ '</DLNUMBER>\
					<ARMEDFORCES>' +ARMEDFORCES+ '</ARMEDFORCES>\
					<CONTACTTIME>' +CONTACTTIME+ '</CONTACTTIME>\
					<RENTOROWN>' +HOMEOWNER+ '</RENTOROWN>\
					<EMAIL>' +EMAIL+ '</EMAIL>\
					<ADDRESSMONTH>24</ADDRESSMONTH>\
					<CITIZENSHIP></CITIZENSHIP>\
					<OWNCAR>' +CAROWNER+ '</OWNCAR>\
					<TCPAOPTIN></TCPAOPTIN>\
					<USERAGENT>' + 'HTTP_USER_AGENT' + '</USERAGENT>\
				</PERSONAL>\
				<EMPLOYMENT>\
					<INCOMETYPE>' +INCOMETYPE+ '</INCOMETYPE>\
					<PAYTYPE></PAYTYPE>\
					<EMPTIME>' +EMPTIME+ '</EMPTIME>\
					<EMPNAME>' +EMPNAME+ '</EMPNAME>\
					<EMPPHONE>' +EMPPHONE+ '</EMPPHONE>\
					<JOBTITLE>' +JOBTITLE+ '</JOBTITLE>\
					<PAYFREQUENCY>' +PAYFREQUENCY+ '</PAYFREQUENCY>\
					<NEXTPAYDATE>' + NEXTPAYDATE + '</NEXTPAYDATE>\
					<SECONDPAYDATE>' + SECONDPAYDATE + '</SECONDPAYDATE>\
				</EMPLOYMENT>\
				<BANK>\
					<BANKNAME>' +BANKNAME+ '</BANKNAME>\
					<BANKPHONE></BANKPHONE>\
					<ACCOUNTTYPE>' +ACCOUNTTYPE+ '</ACCOUNTTYPE>\
					<ROUTINGNUMBER>' +ROUTINGNUMBER+ '</ROUTINGNUMBER>\
					<ACCOUNTNUMBER>' +ACCOUNTNUMBER+ '</ACCOUNTNUMBER>\
					<BANKMONTHS>12</BANKMONTHS>\
					<NETMONTHLYINCOME>' +NETMONTHLYINCOME+ '</NETMONTHLYINCOME>\
					<DIRECTDEPOSIT>' +DIRECTDEPOSIT+ '</DIRECTDEPOSIT>\
				</BANK>\
			</CUSTOMER>\
		</REQUEST>';

	console.log(postXml);
	$.ajax({
		type: 'POST',
		// url: 'https://formz.azurewebsites.net/form/post.php',
		url: 'https://qa-lead-distribution-api.adrack.com/Import/',
		// url: 'https://localhost:44331/Import/',
		
		// processData: false,
		contentType: 'text/plain',
		//contentType: 'application/json',
		// dataType: 'text',
		// data: dataSend,
		data: postXml,

		success: function(retData) {
			console.log(retData);
			var xml = $.parseXML(retData),
				$xml = $(xml),
				$ret = $xml.find('status');
			if ($ret.text() == 'sold') {
				$redirect = $xml.find('redirect');
				document.location.href = $redirect
					.text()
					.replace('![CDATA[', '')
					.replace(']]', '');
			} else {
				$('.secondsBlock').hide();
				$('.waitBlock .w-subtitle').text('Sorry but you were not matched with any lenders.');
				$('.waitBlock .w-title').text('Searching finished.');
			}
		},
		error: function(err) {
			console.error('Error: ' + err);
		}
	});
}
