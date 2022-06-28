var step = 1;
var currentScrollPosition = 0;
var customStep = 0;
var period = 0;
var skipped = false;

$(document).ready(function() {
	var urlItems = location.search.split('&');

	urlItems.forEach(item => {
		var splited = item.split('=');
		if (splited[0] == 'fname') $('#FirstName').val(item.split('=')[1].replace('+', ' '));
		else if (splited[0] == 'zip') {
			$('#ZIP').val(item.split('=')[1]);
			$('#ZIP2').val(item.split('=')[1]);

			if ($('#ZIP').val().length == 6 && validateZIP($('#ZIP').val())) GetGeoDataByZip($('#ZIP').val(), '');
			else $('#ZIP').addClass('error');
			if ($('#ZIP2').val().length == 6 && validateZIP($('#ZIP2').val())) GetGeoDataByZip($('#ZIP2').val(), 2);
			else $('#ZIP2').addClass('error');
		} else if (splited[0] == 'email') $('#email').val(decodeURIComponent(item.split('=')[1]));
	});

	CountClick();
	currentScrollPosition = $(this).scrollTop();

	for (i = new Date().getFullYear() - 18; i > 1900; i--) {
		$('#years').append(
			$('<option />')
				.val(i)
				.html(i)
		);
	}
	for (i = 0; i < 12; i++) {
		$('#months').append(
			$('<option />')
				.val(i)
				.html(
					moment()
						.month(i)
						.format('MMMM')
				)
		);
	}
	updateNumberOfDays();

	$('#years, #months').change(function() {
		updateNumberOfDays();
	});

	$(window).scroll(function() {
		currentScrollPosition = $(this).scrollTop();
	});

	$('.inline').on('click', function() {
		val = $('span', this).data('val');
		if (val === '' || val === undefined) {
			val = $('span', this).text();
		}

		$('.step-' + step)
			.addClass('validate')
			.data('value', val);
		//$('.step-' + step + ' .inline').removeClass('active');
		//$(this).addClass('active');
		$('#ContinueBtn').trigger('click');
	});

	$('.step input').focus(function() {
		$(document).scrollTop(currentScrollPosition);
	});

	$('#IncomeType').on('change', function() {
		var type = $(this).val();

		if (type == 'benefits' || type == 'pension' || type == 'disability' || type == 'unemployed') {
			skipped = true;
			customStep = 17;
		} else {
			skipped = false;
			customStep = 0;
		}
	});

	$('.step input').on('keydown change keyup blur input', function() {
		if ($(this).data('type') == 'name') {
			if (validateName($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'text') {
			if (validateText($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'company') {
			if (validateCompany($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'address') {
			if (validateAddress($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'city') {
			if (validateCity($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'zip') {
			$(this).val(
				$(this)
					.val()
					.toUpperCase()
			);
			if (validateZIP($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'email') {
			if (validateEmail($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'number') {
			if (validateNumber($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'ssn') {
			if (validateSSN($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'branchNumber') {
			if (validateBranchNumber($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'accountNumber') {
			if (validateAccountNumber($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'institutionNumber') {
			if (validateInstitutionNumber($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'phone') {
			if (validateUSPhone($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'mobilephone') {
			if (validateUSPhone($(this).val()) || $(this).val() == '') $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'empty') {
			if ($(this).val().length > 0) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'job') {
			if (validateJob($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('type') == 'aba') {
			if (validateNumber($(this).val()) && $(this).val().length >= 8) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		/*if ($(this).data('type') == 'date') {
			if (validateDate($(this).val()) && Date.parse($(this).val()) - Date.parse(new Date()) >= 0)
				$(this).removeClass('error');
			else $(this).addClass('error');
		}*/

		checkStepValidation();
	});
	$('.step select').on('change', function() {
		if ($(this).val() == '') $(this).addClass('error');
		else $(this).removeClass('error');
		checkStepValidation();
	});

	$('#ContinueBtn').on('click', function() {
		if ($('.step-' + step).hasClass('validate')) {
			$(this).addClass('disabled');
			step++;

			if (customStep != 0 && step == 12) step = customStep;
			if (step == 9) step = 10;

			buttonToggle();

			if (step == 19) {
				if ($('#datepicker').length > 0) {
					var day = new Date();

					day.setDate(day.getDate() + 2);

					if (day.getDay() == 0) {
						day.setDate(day.getDate() + 1);
					} else if (day.getDay() == 6) {
						day.setDate(day.getDate() + 2);
					}

					var temp = day;
					var days = [];

					if ($('.step-18').data('value') == 'weekly') period = 7;
					else if ($('.step-18').data('value') == 'biweekly') period = 14;
					else if ($('.step-18').data('value') == 'twicemonthly') period = 14;
					else if ($('.step-18').data('value') == 'monthly') period = 31;

					for (var i = 0; i < period; i++) {
						temp = new Date(day.getTime());
						temp.setDate(temp.getDate() + i);
						if (temp.getDay() > 0 && temp.getDay() < 6) {
							days.push(moment(temp).format('YYYY-MM-DD'));
						}
					}

					$('#datepicker').datepicker('destroy');

					$('#datepicker').datepicker({
						defaultDate: day,
						numberOfMonths: 2,

						beforeShowDay: function(date) {
							var compareDate = moment(date).format('YYYY-MM-DD');
							return [days.indexOf(compareDate) > -1];
						}
					});
					$('#datepicker').val(getFormattedDate(day));
				}
				$(this).removeClass('disabled');
			}

			if (step == 9) $(this).removeClass('disabled');

			$('.step').hide();
			$('.step-' + step).show();
			if (step == 19) $('#datepicker').datepicker('show');

			$([document.documentElement, document.body]).animate(
				{
					scrollTop: $('#FormGeneration .stepsBlock').offset().top - 50
				},
				100
			);

			if (step == 7 && $('#email').val().length > 0) {
				if (validateEmail($('#email').val())) $('#email').removeClass('error');
				else $('#email').addClass('error');

				if ($('.step-' + step + ' .error').length == 0) {
					$('#ContinueBtn').removeClass('disabled');
					$('.step-' + step).addClass('validate');
				}
			}
		}
	});

	$('#GoBtn').on('click', function() {
		if ($('.step-' + step).hasClass('validate')) {
			$('.stepsBlock .contactForm').hide();
			$('.stepsBlock .waitBlock').show();
			$('#username').text($('#FirstName').val());
			StartCountdown();
			PostXml();

			$([document.documentElement, document.body]).animate(
				{
					scrollTop: $('#FormGeneration .stepsBlock').offset().top - 50
				},
				100
			);
		}
	});

	$('#ZIP').on('focusout', function() {
		if ($(this).val().length == 6) GetGeoDataByZip($(this).val(), '');
	});
	$('#ZIP2').on('focusout', function() {
		if ($(this).val().length == 6) GetGeoDataByZip($(this).val(), 2);
	});
	/*$('#RoutingNumber').on('focusout', function() {
		var num = $(this).val();
		if (num.length == 8) num = '0' + num;

		var c =
			3 * (parseInt(num[0]) + parseInt(num[3]) + parseInt(num[6])) +
			7 * (parseInt(num[1]) + parseInt(num[4]) + parseInt(num[7])) +
			(parseInt(num[2]) + parseInt(num[5]) + parseInt(num[8]));
		if (c % 10 == 0) {
			$(this).removeClass('error');
			GetBankByAbaNumber($(this).val());
		} else {
			$(this).addClass('error');
			$('.step-21 .note').css('display', 'inline-block');
		}
	});*/

	$('#Agree').on('change', function() {
		if ($(this).prop('checked')) {
			$('.step-' + step).addClass('validate');
			$('#GoBtn').removeClass('disabled');
		} else {
			$('.step-' + step).removeClass('validate');
			$('#GoBtn').addClass('disabled');
		}
	});

	$('#BackBtn').on('click', function() {
		step--;
		if (step + 1 == customStep && customStep != 0) {
			step = 11;
		}

		if (step == 0) step = 1;

		if (step == 9) step = 8;

		buttonToggle();

		$('.step').hide();
		$('.step-' + step).show();
		$('#ContinueBtn').removeClass('disabled');
		$([document.documentElement, document.body]).animate(
			{
				scrollTop: $('#FormGeneration .stepsBlock').offset().top - 50
			},
			100
		);
	});
});
function buttonToggle() {
	$('#GoBtn').hide();

	if (step == 1) $('#BackBtn').hide();
	else $('#BackBtn').css({ display: 'inline-block' });

	if ($('.step-' + step + ' .inline').length > 0) $('#ContinueBtn').hide();
	else $('#ContinueBtn').css({ display: 'inline-block' });

	if (step == 26) {
		$('#ContinueBtn').hide();
		$('#GoBtn').show();
	}
}
function validateEmail(email) {
	var re = /^[a-zA-Z0-9][a-zA-Z0-9-_\.]+@([a-zA-Z]|[a-zA-Z0-9]?[a-zA-Z0-9-]+[a-zA-Z0-9])\.[a-zA-Z0-9]{2,10}(?:\.[a-z]{2,10})?$/;
	return re.test(email);
}
function validateText(text) {
	return /^[a-zA-Z '.]+$/.test(text);
}
function validateName(text) {
	return /^[a-zA-Z\s\.\-`']+$/.test(text);
}
function validateCompany(text) {
	return /^[a-zA-Z0-9 .,']+$/.test(text);
}
function validateJob(text) {
	return /^[a-zA-Z0-9]+$/.test(text);
}
function validateZIP(sZip) {
	return /^(?!.*[DFIOQU])[A-VXY][0-9][A-Z]?[0-9][A-Z][0-9]$/.test(sZip);
}
function validateAddress(text) {
	return /^[A-Za-z0-9'\.\-\s\,]+$/.test(text);
}
function validateCity(text) {
	return /^[a-zA-z] ?([a-zA-z]|[a-zA-z] )*[a-zA-z]$/.test(text);
}
function validateUSPhone(text) {
	return /^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/.test(text);
}
function validateNumber(text) {
	return /^[0-9]+$/.test(text);
}
function validateDate(text) {
	return /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.test(text);
}

function validateSSN(text) {
	//return /^[0-9]{3}[\ ][0-9]{2}[\ ][0-9]{3}$/.test(text);
	return /^[0-9]{9}$/.test(text);
}

function validateBranchNumber(text) {
	//return /^[0-9]{3}[\ ][0-9]{2}[\ ][0-9]{3}$/.test(text);
	return /^[0-9]{5}$/.test(text);
}

function validateAccountNumber(text) {
	//return /^[0-9]{3}[\ ][0-9]{2}[\ ][0-9]{3}$/.test(text);
	return /^[0-9]{11}$/.test(text);
}

function validateInstitutionNumber(text) {
	//return /^[0-9]{3}[\ ][0-9]{2}[\ ][0-9]{3}$/.test(text);
	return /^[0-9]{3}$/.test(text);
}

function checkStepValidation() {
	var temp = true;
	$('.note').hide();

	$('.step-' + step + ' select').each(function() {
		if ($(this).val() == '') temp = false;
	});

	if (step == 3 && $('#ZIP').hasClass('error')) $('.step-3 .note').css('display', 'inline-block');
	if (step == 15 && $('#ZIP2').hasClass('error')) $('.step-15 .note').css('display', 'inline-block');
	//if (step == 21 && $('#RoutingNumber').hasClass('error')) $('.step-21 .note').css('display', 'inline-block');

	if (step == 6) {
		temp = checkAge($('#days').val(), $('#months').val(), $('#years').val());

		if (!temp) $('.step-6 .note').css('display', 'inline-block');
	}

	if (temp && $('.step-' + step + ' .error').length == 0 && $('.step-' + step + ' input:invalid').length == 0) {
		$('#ContinueBtn').removeClass('disabled');
		$('.step-' + step).addClass('validate');
	} else {
		$('#ContinueBtn').addClass('disabled');
		$('.step-' + step).removeClass('validate');
	}
}

function checkAge(day, month, year) {
	var age = 18;
	var setDate = new Date(parseInt(year) + age, parseInt(month) - 1, parseInt(day));
	var currdate = new Date();

	if (currdate >= setDate) return true;

	return false;
}

function getFormattedDate(date) {
	var year = date.getFullYear();
	var month = (1 + date.getMonth()).toString();
	month = month.length > 1 ? month : '0' + month;
	var day = date.getDate().toString();
	day = day.length > 1 ? day : '0' + day;
	return month + '/' + day + '/' + year;
}

function StartCountdown() {
	$('#circle_timer').TimeCircles({
		start: true, // determines whether or not TimeCircles should start immediately.
		animation: 'smooth', // smooth or ticks. The way the circles animate can be either a constant gradual rotating, slowly moving from one second to the other.
		count_past_zero: false, // This option is only really useful for when counting down. What it does is either give you the option to stop the timer, or start counting up after you've hit the predefined date (or your stopwatch hits zero).
		circle_bg_color: '#f4f4f4', // determines the color of the background circle.
		use_background: true, // sets whether any background circle should be drawn at all.
		fg_width: 0.1, //  sets the width of the foreground circle.
		bg_width: 1, // sets the width of the backgroundground circle.
		text_size: 0.05, // This option sets the font size of the text in the circles.
		total_duration: 'Auto', // This option can be set to change how much time will fill the largest visible circle.
		direction: 'Clockwise', // "Clockwise", "Counter-clockwise" or "Both".
		use_top_frame: false,
		start_angle: 0, // This option can be set to change the starting point from which the circles will fill up.
		time: {
			//  a group of options that allows you to control the options of each time unit independently.
			Days: {
				show: false
			},
			Hours: {
				show: false
			},
			Minutes: {
				show: false
			},
			Seconds: {
				show: true,
				text: 'sec',
				color: '#f4905e'
			}
		}
	});
	var counter = 1;

	var countdown = setInterval(function() {
		$('#DataList li:nth-child(' + counter + ')').addClass('active');
		counter++;
		if (counter === 4) {
			clearInterval(countdown);
		}
	}, 5000);
}

function updateNumberOfDays() {
	month = $('#months').val();
	year = $('#years').val();
	days = daysInMonth(month, year);
	selectedDay = $('#days').val();

	$('#days').html('');
	$('#days').append(
		$('<option selected="selected" />')
			.val('')
			.html('Day')
	);

	for (i = 1; i < days + 1; i++) {
		$('#days').append(
			$('<option />')
				.val(i)
				.html(i)
		);
	}
	if (selectedDay <= days) $('#days').val(selectedDay);
}

//helper function
function daysInMonth(month, year) {
	return new Date(year, month, 0).getDate();
}

function GetGeoDataByZip(zip, type) {
	$.ajax({
		type: 'GET',
		url: 'https://geocoder.ca/' + zip + '?json=1',
		async: true,
		success: function(response) {
			console.log(response);
			if (response.error) {
				$('#ZIP' + type).addClass('error');
			} else {
				$('#City' + type).val(response.standard.city);
				$('#State' + type).val(response.standard.prov);
			}
			checkStepValidation();
		}
	});
}
function GetBankByAbaNumber(aba) {
	$.ajax({
		type: 'GET',
		url: 'https://formz.azurewebsites.net/Common/GetBankByAbaNumber?abanumber=' + aba,
		async: true,
		success: function(response) {
			$('#BankName').val(response.bankname);
			checkStepValidation();
		}
	});
}

function CountClick() {
	$.ajax({
		type: 'POST',
		url: 'https://proffiliant.adrack.com/home/click?key=d14df555-f605-4338-9c2b-44dfd702b11c&type=1',
		async: true,
		success: function(response) {
			console.log(response);
		}
	});
}
