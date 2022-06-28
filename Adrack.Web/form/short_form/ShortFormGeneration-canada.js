$(document).ready(function() {
	var urlCanada = 'https://formz.azurewebsites.net/form/short_form/shortform-canada.html?v20';
	$.get(urlCanada, function(data) {
		$('#ShortFormGeneration').html(data);
	});
});
