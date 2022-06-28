$(document).ready(function() {
	// var urlUS = '/forms/templates/1/form.html?v1';
	var urlUS = 'https://formz.azurewebsites.net/forms/templates/1/form.html?v3';
	
	var urlCanada = '/forms/templates/1/form-canada.html?v1';

	$.ajax({
		type: 'POST',
		url: 'https://formz.azurewebsites.net/form/post.php?get_country=1',
		success: function(retData) {
			if (retData == 'CA') {
				$.get(urlCanada, function(data) {
					$('#FormGeneration').html(data);
				});
			} else {
				$.get(urlUS, function(data) {
					$('#FormGeneration').html(data);
				});
			}
		}
	});
});
