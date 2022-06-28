$(document).ready(function() {
	var urlUS = 'https://formz.azurewebsites.net/form/short_form/shortform-us.html?v20';
	var urlCanada = 'https://formz.azurewebsites.net/form/short_form/shortform-canada.html?v20';

	$.ajax({
		type: 'POST',
		url: 'https://formz.azurewebsites.net/form/post.php?get_country=1',
		success: function(retData) {
			if (retData == 'CA') {
				$.get(urlCanada, function(data) {
					$('#ShortFormGeneration').html(data);
				});
			} else {
				$.get(urlUS, function(data) {
					$('#ShortFormGeneration').html(data);
				});
			}
		}
	});
});
