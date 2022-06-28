$(document).ready(function() {
	$('.sf-input input').on('keydown change keyup blur input', function() {
		if ($(this).data('valid') == 'name') {
			if (validateName($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('valid') == 'zip') {
			if (validateZIP($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('valid') == 'postcode') {
			$(this).val(
				$(this)
					.val()
					.toUpperCase()
			);

			if (validatePostcode($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
		if ($(this).data('valid') == 'email') {
			if (validateEmail($(this).val())) $(this).removeClass('error');
			else $(this).addClass('error');
		}
	});

	$('#SF-Shortform').on('submit', function(e) {
		if ($('.sf-shortform .error').length == 0) {
			var data = {
				amount: $('#SF-LoanAmount').val(),
				firstname: $('#SF-Firstname').val(),
				zip: $('#SF-ZIP').val() ? $('#SF-ZIP').val() : '',
				email: $('#SF-Email').val()
			};

			StoreShortFormData(data);
		}
		e.preventDefault();
	});
});

function validateEmail(value) {
	var re = /^[a-zA-Z0-9][a-zA-Z0-9-_\.]+@([a-zA-Z]|[a-zA-Z0-9]?[a-zA-Z0-9-]+[a-zA-Z0-9])\.[a-zA-Z0-9]{2,10}(?:\.[a-z]{2,10})?$/;
	return re.test(value);
}
function validateName(value) {
	return /^[a-zA-Z ]+$/.test(value);
}
function validateZIP(value) {
	return /^\d{5}(-\d{4})?$/.test(value);
}

function validatePostcode(value) {
	return /^(?!.*[DFIOQU])[A-VXY][0-9][A-Z]?[0-9][A-Z][0-9]$/.test(value);
}

function StoreShortFormData(data) {
	$.ajax({
		type: 'POST',
		url: 'https://proffiliant.adrack.com/home/AddShortFormData',
		async: true,
		data: data,
		success: function(response) {
			console.log(response);
			location.href =
				'/get-started/?amount_request=' +
				data.amount +
				'&fname=' +
				data.firstname +
				'&zip=' +
				data.zip +
				'&email=' +
				data.email +
				'&submit=Get Started';
		}
	});
}
