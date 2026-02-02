
$(document).on('click', '#livrer', function(e) {
	$(this).prop("disabled",true);
	var trump8=document.getElementById("identifiant_fact").value;
	
	var facture = {"Id": parseInt(trump8),
				   "fact_type":$("#facture_tip").val()
				};
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/livrer",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
			if (facture.fact_type=="medicament_0" || facture.fact_type=="medicament_assurance") {
				location.href = "/Facture/Details3/"+trump8;
			}else{
				location.href = "/Facture/details/"+trump8;
			}
			console.log(facture.fact_type)
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#retourner', function(e) {
	$(this).prop("disabled",true);
	var trump8=document.getElementById("identifiant_fact").value;
	
	var facture = {"Id": parseInt(trump8),
				   "fact_type":$("#facture_tip").val()
				};
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/retourner",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
			if (facture.fact_type=="medicament_0" || facture.fact_type=="medicament_assurance") {
				location.href = "/Facture/Details3/"+trump8;
			}else{
				location.href = "/Facture/details/"+trump8;
			}
			
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#genererexamens', function(e) {
	$(this).prop("disabled",true);
	var trump8=document.getElementById("identifiant_fact").value;
	
	var facture = {"Id": parseInt(trump8),
				   "fact_type":$("#facture_tip").val()
				};
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/GenererResultat",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
				location.href = "/Facture/Matricielle/"+trump8;
		},
		error: function (response) { 
				
		}
	});

});







// save paiement
$(document).on('click', '#valider_paiement', function(e) {
			//collecte des données
			$(this).prop("disabled",true);

			var trump1=document.getElementById("numerodossier").value;
			var trump2=document.getElementById("nom").value;
			var trump3=document.getElementById("prenom").value;
			var trump4=document.getElementById("numerofacture").value;
			var trump5=document.getElementById("obser_p").value;
			var trump66=document.getElementById("frais_retrait").value;
			var trump6=document.getElementById("Mop").value;
			var trump7=0;
			var trump8=document.getElementById("identifiant_fact").value;
			var trump9=document.getElementById("caution").value;
			var trump10=document.getElementById("cai").value;
			
			
		


			if (-(parseInt($("#napp").val())-parseInt($("#mrp").val())-parseInt($("#net").val()))>=0) {
				trump7=$("#napp").val()-$("#mrp").val();
			}else{
				trump7=$("#net").val();
			}

			if (isNaN(parseInt(trump66))) {
				trump66=0;
			}
			if (!(trump6=="orange money" || trump6=="momo")) {
				trump66=0;
			}
			

			
			var facture = {
				
				"Id": 0,
				"Numero_dossier": trump1,
				"Patient": trump2+" "+trump3,
				"Numero_facture": trump4,
				"Moyen_paiement": trump6,
				"Montant": parseInt(trump7),
				"Frais_retrait":parseInt(trump66),
				"Numerocaution":trump9,
				"Caissier":"",
				"caisse":trump10,
				"Create_date":new Date(Date.now()),
				"Observation": $("#caution").val()+" "+trump5,
				"fact_type":$("#facture_tip").val()
			};
			if (facture.Moyen_paiement!=null && facture.Moyen_paiement!="" ) {
				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Paid/SavePayment",
					dataType: "json",
					data: JSON.stringify(facture),
					success: function (result) {
						
						if (facture.fact_type=="medicament_0" || facture.fact_type=="medicament_assurance") {
							location.href = "/Facture/Details3/"+trump8;
						}else{
							location.href = "/Facture/Matricielle/"+trump8;
						}
						
					},
					error: function (response) { 
							
					}
				});
			}
			

			

});

$(document).on('click', '#manualgenerate', function(e) {
	$(this).prop("disabled",true);
	var trump8=document.getElementById("identifiant_fact").value;
	
	var facture = {"Id": parseInt(trump8),
				   "fact_type":$("#facture_tip").val()
				};
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/GenererResultat2",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
				location.href = "/Facture/Matricielle/"+trump8;
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#manualgenerate2', function(e) {
	$(this).prop("disabled",true);
	var trump8=document.getElementById("identifiant_fact").value;
	
	var facture = {"Id": parseInt(trump8),
				   "fact_type":$("#facture_tip").val(),
				   "Create_date":$("#datecreat").val()
				};
	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/GenererHonoraire2",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
				location.href = "/Facture/Matricielle/"+trump8;
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#whatsp', function(e) {
	location.href=window.location.href.replace("port2","port22");
	//console.log();
});

$(document).on('click', '#closewhats', function(e) {
	location.href = "/Honoraire/rapport2";
});






$(document).on('click', '#Cloturebt', function(e) {
	//collecte des données
	$(this).prop("disabled",true);

	var trump1=document.getElementById("numerodossier").value;
	var trump2=document.getElementById("nom").value;
	var trump3=document.getElementById("prenom").value;
	var trump4=document.getElementById("numerofacture").value;
	var trump5=document.getElementById("obser_p").value;
	var trump66=document.getElementById("frais_retrait").value;
	var trump6=document.getElementById("Mop").value;
	var trump7=0;
	var trump8=document.getElementById("identifiant_fact").value;
	

	if (-(parseInt($("#napp").val())-parseInt($("#mrp").val())-parseInt($("#net").val()))>=0) {
		trump7=$("#napp").val()-$("#mrp").val();
	}else{
		trump7=$("#net").val();
	}

	console.log(trump7)

	if (isNaN(parseInt(trump66))) {
		trump66=0;
	}
	if (!(trump6=="orange money" || trump6=="momo")) {
		trump66=0;
	}
	
	

	var facture = {
		
		"Id": 0,
		"Numero_dossier": trump1,
		"Patient": trump2+" "+trump3,
		"Numero_facture": trump4,
		"Moyen_paiement":"",
		"Montant": 0,
		"Frais_retrait":0,
		"Caissier":"",
		"caisse":"Caisse Poitiers",
		"Create_date":new Date(Date.now()),
		"Observation": "Assurance 100%",
		"fact_type":$("#facture_tip").val()
	};
	console.log(facture)

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/SaveCloture",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
			
			if (facture.fact_type=="medicament_0" || facture.fact_type=="medicament_assurance") {
				location.href = "/Facture/Details3/"+trump8;
			}else{
				location.href = "/Facture/Matricielle/"+trump8;
			}
			
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#decloturer', function(e) {
	//collecte des données
	$(this).prop("disabled",true);

	var trump1=document.getElementById("numerodossier").value;
	var trump2=document.getElementById("nom").value;
	var trump3=document.getElementById("prenom").value;
	var trump4=document.getElementById("numerofacture").value;
	//var trump5=document.getElementById("obser_p").value;
	//var trump66=document.getElementById("frais_retrait").value;
	//var trump6=document.getElementById("Mop").value;
	//var trump7=0;
	var trump8=document.getElementById("identifiant_fact").value;
	

	if (-(parseInt($("#napp").val())-parseInt($("#mrp").val())-parseInt($("#net").val()))>=0) {
		trump7=$("#napp").val()-$("#mrp").val();
	}else{
		trump7=$("#net").val();
	}

	console.log(trump7)

	
	
	

	var facture = {
		
		"Id": 0,
		"Numero_dossier": trump1,
		"Patient": trump2+" "+trump3,
		"Numero_facture": trump4,
		"Moyen_paiement":"",
		"Montant": 0,
		"Frais_retrait":0,
		"Caissier":"",
		"caisse":"Caisse Poitiers",
		"Create_date":new Date(Date.now()),
		"Observation": "Assurance 100%",
		"fact_type":$("#facture_tip").val()
	};
	console.log(facture)

	$.ajax({
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Paid/Decloturer",
		dataType: "json",
		data: JSON.stringify(facture),
		success: function (result) {
			location.href = "/Facture/Details4/"+trump8;
		},
		error: function (response) { 
				
		}
	});

});

$(document).on('click', '#printpaie', function(e) {
	console.log("llll")
	modal2.style.display = 'block';
    
});







$(document).on('change', '#Mop', function(e) {
	if ($(this).val()=="orange money" || $(this).val()=="momo") {
		$("#caution").val("");
		$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
		$("#fdr").show();
		$("#caut").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}else if($(this).val()=="caution"){
		$("#frais_retrait").val(0);
		$("#caut").show();
		$("#fdr").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#caution").val()!=""&& $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}
	else{
		$("#caut").hide();
		$("#caution").val("");
		$("#frais_retrait").val(0);
		$("#fdr").hide();
		console.log($("#frais_retrait").val())
		if (!isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
		
	}
	
	
});

$(document).on('keyup', '#net', function(e) {
	$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
	if ($("#Mop").val()=="orange money" || $(this).val()=="momo") {
			$("#caution").val("");
			$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
			$("#fdr").show();
			$("#caut").hide();
			if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
		}else if($("#Mop").val()=="caution"){
			$("#frais_retrait").val(0);
			$("#caut").show();
			$("#fdr").hide();
			if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#caution").val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
		}
		else{
			$("#caut").hide();
			$("#caution").val("");
			$("#frais_retrait").val(0);
			$("#fdr").hide();
			console.log($("#frais_retrait").val())
			if (!isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
			
		}

	$("#rembourssement").text(-(parseInt($("#napp").val())-parseInt($("#mrp").val())-parseInt($("#net").val())));
	if (-(parseInt($("#napp").val())-parseInt($("#mrp").val())-parseInt($("#net").val()))>=0) {
		$("#ldrr").css('color','#82b540');
	}else{
		$("#ldrr").css('color','red');
	}
});

$(document).on('keyup', '#frais_retrait', function(e) {
	if ($("#Mop").val()=="orange money" || $(this).val()=="momo") {
			$("#caution").val("");
			$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
			$("#fdr").show();
			$("#caut").hide();
			if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
		}else if($("#Mop").val()=="caution"){
			$("#frais_retrait").val(0);
			$("#caut").show();
			$("#fdr").hide();
			if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#caution").val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
		}
		else{
			$("#caut").hide();
			$("#caution").val("");
			$("#frais_retrait").val(0);
			$("#fdr").hide();
			console.log($("#frais_retrait").val())
			if (!isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
				$("#valider_paiement").show();
			}else{
				$("#valider_paiement").hide();
			}
			
		}
});

$(document).on('change', '#caution', function(e) {
	if ($("#Mop").val()=="orange money" || $(this).val()=="momo") {
		$("#caution").val("");
		$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
		$("#fdr").show();
		$("#caut").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}else if($("#Mop").val()=="caution"){
		$("#frais_retrait").val(0);
		$("#caut").show();
		$("#fdr").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#caution").val()!="" && $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}
	else{
		$("#caut").hide();
		$("#caution").val("");
		$("#frais_retrait").val(0);
		$("#fdr").hide();
		console.log($("#frais_retrait").val())
		if (!isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#cai").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
		
	}
	
});

$(document).on('change', '#cai', function(e) {
	if ($("#Mop").val()=="orange money" || $(this).val()=="momo") {
		$("#caution").val("");
		$("#frais_retrait").val((parseInt($("#net").val()))*0.01);
		$("#fdr").show();
		$("#caut").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#Mop").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}else if($("#Mop").val()=="caution"){
		$("#frais_retrait").val(0);
		$("#caut").show();
		$("#fdr").hide();
		if ( !isNaN(parseInt($("#frais_retrait").val())) && !isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#caution").val()!="" && $("#Mop").val()!="") {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
	}
	else{
		$("#caut").hide();
		$("#caution").val("");
		$("#frais_retrait").val(0);
		$("#fdr").hide();
		console.log($("#frais_retrait").val())
		if (!isNaN(parseInt($("#net").val())) && $(this).val()!="" && $("#Mop").val()!="" ) {
			$("#valider_paiement").show();
		}else{
			$("#valider_paiement").hide();
		}
		
	}
	
});


$(document).on('click', '#masquerdetails', function(e) {
	var divsToHide = document.getElementsByClassName("table1"); //divsToHide is an array
    for(var i = 0; i < divsToHide.length; i++){
        divsToHide[i].style.visibility = "hidden"; // or
        divsToHide[i].style.display = "none"; // depending on what you're doing
    }
	
});



