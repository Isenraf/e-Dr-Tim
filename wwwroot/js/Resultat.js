
$(document).on('click', '#valider_result', function(e) {
	//collecte des données
	var trump1=document.getElementById("identifiant_result").value;
	var trump2=document.getElementById("result").value;
	
	var resultat = {
		
		"Id": parseInt(trump1),
		"resultat_final": trump2,
	};
	//console.log(resultat)

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Resultat/Validation",
		dataType: "json",
		data: JSON.stringify(resultat),
		success: function (result) {
			if (result=="ok") {
				location.href = "/Resultat/Details/"+trump1;
			}else{
				location.href = "/stock/Page405";
			}
			
			
		},
		error: function (response) { 
				
		}
	});

});






$(document).on('change', '#result', function(e) {
	console.log($(this).val())
	if ($(this).val()!="") {
		$("#valider_result").show();
	}else{
		$("#valider_result").hide();
	}
	
});
