$(document).ready(function() {
	var urlUS = 'https://formz.azurewebsites.net/form/form.html?v13';

	$.get(urlUS, function(data) {
		$('#FormGeneration').html(data);
	});
});
