$(document).ready(function() {
	var urlUS = 'https://formz.azurewebsites.net/form/form.html?v13';
	var urlCanada = 'https://formz.azurewebsites.net/form/form-canada.html?v13';

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
