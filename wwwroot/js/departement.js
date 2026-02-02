//Activation et désactivation bouton
let button = document.querySelector("#enregistrer_departement");
button.disabled = true; //setting button state to disabled

$("#nom_departement").on("input", function() {
	stateHandle()
});
$("#description_departement").on("input", function() {
	stateHandle()
});

function stateHandle() {
    if (document.querySelector("#nom_departement").value != "" && document.querySelector("#description_departement").value != "") {
        button.disabled = false; //button remains disabled
    } else {
        button.disabled = true; //button is enabled
    }
}



// mise à jour suivi courrier
$(document).on('click', '#enregistrer_departement', function(e) {
	
	var file = $('input[type="file"]').val().trim();
	var nom=document.getElementById("nom_departement").value;
	var description=document.getElementById("description_departement").value;
	var actif=document.getElementById("actif").value;
		if(actif=="on"){actif=true}else{actif=false}
		var Mydata = {
			"Nom":nom,
			"Description": description,
			"Actif": actif
			};

			console.log(Mydata)
			 $.ajax({
				 type: "POST",
				 contentType: "application/json; charset=utf-8",
				 url: "/Departement/CreateDep1",
				 dataType: "json",
				 data: JSON.stringify(Mydata),
				 success: function (result) { 
					if(file){
							var fileUpload = $("#FileUpload1").get(0);  
							var files = fileUpload.files;  
							
							// Create FormData object  
							var fileData = new FormData();  

							// Looping over all files and add it to FormData object  
							for (var i = 0; i < files.length; i++) {  
								fileData.append(files[i].name, files[i]);  
							}  
							
							$.ajax({  
								url: "/Departement/UploadFiles",  
								type: "POST",  
								contentType: false, // Not to set any content header  
								processData: false, // Not to process data  
								data: fileData,  
								success: function (result) {  
									//alert(result); 
									location.href = "/Departement/Index/"; 
								},  
								error: function (err) {  

									if(err.statusText="Request Entity Too Large"){
										$("#error_message").html("Taille du fichier volumineux, la taille du fichier doit être inférieure à 1 Mo");

									}else{
										$("#error_message").html(err.statusText);
									}
									
								}  
							});
							
						}else{
							location.href = "/Departement/Index/"; 
						} 

				   },  
				   error: function (err) {  
	   
					if(err.statusText=="Request Entity Too Large"){
						$("#error_message").html("Taille du fichier volumineux, la taille du fichier doit être inférieure à 1 Mo");
	
					}else{
						$("#error_message").html(err.statusText);
					}
					   
				   }  
				 
			 });


	
});


