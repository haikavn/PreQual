$(document).ready(function() {
	var urlCanada = 'https://formz.azurewebsites.net/form/form-canada.html?v13';
	$.get(urlCanada, function(data) {
		$('#FormGeneration').html(data);
	});
});
