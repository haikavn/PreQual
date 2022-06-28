$(document).ready(function() {
	var url = 'https://formz.azurewebsites.net/form/short_form/shortforminner.html?v20';
	$.get(url, function(data) {
		$('#ShortFormGeneration').html(data);
	});
});
