$(document).ready(function() {
	var urlUS = 'https://formz.azurewebsites.net/form/short_form/shortform-us.html?v20';
	$.get(urlUS, function(data) {
		$('#ShortFormGeneration').html(data);
	});
});
