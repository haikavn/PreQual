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
	dataSend.EMPTIME = TimeEmployed;
	dataSend.EMPNAME = $('#EmployerName').val();
	dataSend.EMPPHONE = $('#WorkPhone').val();
	dataSend.JOBTITLE = $('#JobTitle').val();
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

	$.ajax({
		type: 'POST',
		url: 'https://formz.azurewebsites.net/form/post.php',
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
