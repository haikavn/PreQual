function PostXml() {
	var AmountRequested = $('.step-1').data('value');
	var Homeowner = $('.step-5').data('value');
	//var IncomeSource = $('.step-11').data('value');
	var TimeEmployed = $('.step-12').data('value') ? $('.step-12').data('value') : '';
	var MonthlyNetIncome = $('.step-17').data('value');
	var IGetPaid = $('.step-18').data('value');
	var AcountType = $('.step-20').data('value');
	var DirectDeposit = $('.step-25').data('value');

	/* Deleted Fields */
	//var Carowner = $('.step-5').data('value');
	//var ContactTime = $('.step-9').data('value');
	//var ActiveDuty = $('.step-10').data('value');
	/*-------*/

	var dataSend = new Object();
	var secondDay = new Date($('#datepicker').val());
	secondDay.setDate(secondDay.getDate() + period);

	dataSend.REQUESTEDAMOUNTMIN = AmountRequested;
	dataSend.REQUESTEDAMOUNTMAX = AmountRequested;
	dataSend.FIRSTNAME = $('#FirstName')
		.val()
		.trim();
	dataSend.LASTNAME = $('#LastName')
		.val()
		.trim();
	dataSend.ADDRESS = $('#Address').val();
	dataSend.CITY = $('#City').val();
	dataSend.PROVINCE = $('#State').val();
	dataSend.POSTCODE = $('#ZIP').val();
	dataSend.ADDRESSMONTH = $('#AddressMonth').val();
	dataSend.HOMEOWNER = Homeowner;
	dataSend.DOB = $('#months').val() + '-' + $('#days').val() + '-' + $('#years').val();
	dataSend.EMAIL = $('#email').val();
	dataSend.HOMEPHONE = $('#phone').val();
	dataSend.MOBILEPHONE = $('#MobilePhone').val();
	dataSend.SIN = $('#SIN').val();
	dataSend.INCOMETYPE = $('#IncomeType').val();
	dataSend.EMPTIME = skipped ? 12 : TimeEmployed;
	dataSend.JOBTITLE = skipped ? $('#IncomeType').val() : $('#JobTitle').val();
	dataSend.EMPNAME = skipped ? $('#IncomeType').val() : $('#EmployerName').val();

	dataSend.EMPADDRESS = skipped ? $('#Address').val() : $('#Address2').val();
	dataSend.EMPCITY = skipped ? $('#City').val() : $('#City2').val();
	dataSend.EMPPROVINCE = skipped ? $('#State').val() : $('#State2').val();
	dataSend.EMPPOSTCODE = skipped ? $('#ZIP').val() : $('#ZIP2').val();

	dataSend.WORKPHONE = skipped ? $('#phone').val() : $('#WorkPhone').val();
	dataSend.NETMONTHLYINCOME = MonthlyNetIncome;
	dataSend.PAYFREQUENCY = IGetPaid;
	dataSend.NEXTPAYDATE = $('#datepicker').val();
	dataSend.SECONDPAYDATE = moment(secondDay).format('MM/DD/YYYY');
	dataSend.ACCOUNTTYPE = AcountType;
	dataSend.BRACHNUMBER = $('#BranchNumber').val();
	dataSend.BANKNAME = $('#BankName').val();
	dataSend.ACCOUNTNUMBER = $('#AccountNumber').val();
	dataSend.BANKMONTHS = $('#BankMonth').val();
	dataSend.INSTITUTIONNUMBER = $('#InstitutionNumber').val();
	dataSend.DIRECTDEPOSIT = DirectDeposit;

	/* Deleted Fields */
	//dataSend.DLSTATE = $('#DriversLicenseState').val();
	//dataSend.DLNUMBER = $('#DriversLicenseNumber').val();
	//dataSend.ARMEDFORCES = ActiveDuty;
	//dataSend.CONTACTTIME = ContactTime;
	//dataSend.CAROWNER = Carowner;
	//dataSend.ROUTINGNUMBER = $('#RoutingNumber').val();
	/* ----- */

	$.ajax({
		type: 'POST',
		url: 'https://formz.azurewebsites.net/form/post-canada.php',
		data: dataSend,

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
