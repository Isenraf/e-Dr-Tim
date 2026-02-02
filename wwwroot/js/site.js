$(document).on('click', '#create', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=generique_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=generique_0';");
	
});

$(document).on('click', '#create_1', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=consultation_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=consultation_0';");
	
});
$(document).on('click', '#create_2', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=examens_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=examens_0';");
	
});
$(document).on('click', '#create_3', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=actes_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=actes_0';");
	
});
$(document).on('click', '#create_4', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=medicament_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=medicament_0';");
	
});

$(document).on('click', '#proforma', function(e) {
	$("#createfactassur").attr("onclick","location.href='/Facture/Create2?nd="+document.getElementById('numerodossier').value+"&target=proforma_assurance';");
	$("#createfact").attr("onclick","location.href='/Facture/Create?nd="+document.getElementById('numerodossier').value+"&target=proforma_0';");
	
});





$(document).on('click', '#fusion', function(e) {
	var trump1=document.getElementById("datedebut").value;
	var trump2=document.getElementById("datefin").value;
	var trump3=document.getElementById("typefact").value;
	if (trump1!=null && trump2!=null && trump3!=null) {
		var donnees = {	
			
			"debut": Nom,
			"fin": contenu,
			"type": categorie,
			};

				// $.ajax({
				// 	type: "POST",
				// 	contentType: "application/json; charset=utf-8",
				// 	url: "/Examen/Add",
				// 	dataType: "json",
				// 	data: JSON.stringify(donnees),
				// 	success: function (response) {
				// 			console.log(response.responseText)
				// 			location.href = "/Facture/DetailsFusion/";
				// 		}
				// 	,error: function (response) {
				// 			console.log("error")
				// 		}
				// });
				
	}
	
});




//fonction de recherche tableau
$("#search").on("keyup", function() {
    var value = $(this).val().toLowerCase();
    $("#testfiltre .montest").filter(function() {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});



//download Excel

var tableToExcel = (function() {
	var uri = 'data:application/vnd.ms-excel;base64,',
	  template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns=""><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
	  base64 = function(s) {
		return window.btoa(unescape(encodeURIComponent(s)))
	  },
	  format = function(s, c) {
		return s.replace(/{(\w+)}/g, function(m, p) {
		  return c[p];
		})
	  }
	return function(table, name) {
	  if (!table.nodeType) table = document.getElementById(table)
	  var ctx = {
		worksheet: name || 'Worksheet',
		table: table.innerHTML
	  }
	  window.location.href = uri + base64(format(template, ctx))
	}
  })()

  

$(document).on('click', '#savetemplate', function(e) {
    console.log(document.getElementsByClassName("note-editable").length)
   	var Id=document.getElementById("identifiant").value;
    var Nom="λλλλ";
	var montant=0;
	var categorie="λλλλ";
	var code="λλλλ";
	var cote="λλλλ";
	var ExamType="λλλλ";
	var Min=0;
	var Max=0;
	var contenu="λλλλ";
	if(document.getElementsByClassName("note-editable").length>0){
		contenu=document.getElementsByClassName("note-editable")[0].innerHTML;
	}else{
		 Nom=document.getElementById("nom_examen").value;
		 montant=document.getElementById("montant_examen").value;
		 categorie=document.getElementById("categorie_examen").value;
		 code=document.getElementById("code").value;
		 cote=document.getElementById("cote").value;
		 ExamType=document.getElementById("ExamType").value;
		 Min=document.getElementById("min").value;
		 Max=document.getElementById("max").value;

	}
	if (contenu!="λλλλ") {
		contenu=contenu+"<style>p,ul{margin: 0px;}</style>";
	}


	var donnees = {	
		"Id": parseInt(Id),
		"Nom": Nom,
		"Contenu": contenu,
		"Categorie": categorie,
		"Montant": parseInt(montant),
		"Cote": cote,
		"Code": code,
		"ExamType": ExamType,
		"Min": parseInt(Min),
		"Max": parseInt(Max)
		};
		console.log(donnees)
			if(Nom!="" && categorie!=""){
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Examen/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response.responseText)
						if (document.getElementById("categorie_examen").value=="Anapath") {
							location.href = "/Examen/Index2_1?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Ophtalmologie"){
							location.href = "/Examen/Index2_2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Analyse Médicale"){
							location.href = "/Examen/Index2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Imagerie Médicale"){
							location.href = "/Examen/Index0_2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Exploration Fonctionnelle"){
							location.href = "/Examen/Index2_3?tipe="+document.getElementById("categorie_examen").value;
						}

					}
				,error: function (response) {
						console.log("error")

					}
			});
			}

	});





	//save template 
	$(document).on('click', '#savetemplate2', function(e) {
    console.log(document.getElementsByClassName("note-editable").length)
   	var Id=document.getElementById("identifiant").value;
    var Nom="λλλλ";
	var montant=0;
	var categorie="λλλλ";
	var code="λλλλ";
	var cote="λλλλ";
	var ExamType="λλλλ";
	var Min=0;
	var Max=0;
	var contenu="λλλλ";
	
	
	const iframe = document.getElementsByTagName('iframe')[0];
	const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
  	//const element = iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText;
	var Id=iframeDoc.getElementById("identifiant").value;
	//var contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].outerHTML;

	console.log(contenu)
	var paragraphs = iframeDoc.getElementsByClassName("text-zinc-700")[0].getElementsByTagName("p");
	for(var i = 0; i < paragraphs.length; i++)
	{
		if (paragraphs[i].innerText.trim()=="" && document.getElementsByClassName("text-zinc-700").length==0) {
			console.log("ok");
			paragraphs[i].outerHTML="<br/>";
		}
	}

	if(iframeDoc.getElementsByClassName("text-zinc-700").length>0){
		contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText+"<style>p,ul{margin: 0px;}</style>";
	}

	if (contenu!="λλλλ") {
		contenu=contenu.replaceAll('<td colspan="1" rowspan="1">','<td colspan="1" rowspan="1" style="border-width: 1px;border-style: solid;">');
	}
	


	var donnees = {	
		"Id": parseInt(Id),
		"Nom": Nom,
		"Contenu": contenu,
		"Categorie": categorie,
		"Montant": parseInt(montant),
		"Cote": cote,
		"Code": code,
		"ExamType": ExamType,
		"Min": parseInt(Min),
		"Max": parseInt(Max)
		};
		console.log(donnees)
			if(Nom!="" && categorie!=""){
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Examen/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response.responseText)
						if (document.getElementById("categorie_examen").value=="Anapath") {
							location.href = "/Examen/Index2_1?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Ophtalmologie"){
							location.href = "/Examen/Index2_2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Analyse Médicale"){
							location.href = "/Examen/Index2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Imagerie Médicale"){
							location.href = "/Examen/Index0_2?tipe="+document.getElementById("categorie_examen").value;
						}else if(document.getElementById("categorie_examen").value=="Exploration Fonctionnelle"){
							location.href = "/Examen/Index2_3?tipe="+document.getElementById("categorie_examen").value;
						}
					}
				,error: function (response) {
						console.log("error")

					}
			});
			}

	});


	$(document).on('click', '#saveresultat', function(e) {
		//    console.log(document.getElementsByClassName("note-editable")[0].innerHTML)
			var Id=document.getElementById("identifiant").value;
			var contenu=document.getElementsByClassName("note-editable")[0].innerHTML+"<style>p,ul{margin: 0px;}</style>";
			var chaine1="λ"+document.getElementById("t1").value+"λ";
			var chaine2="λ"+document.getElementById("t2").value+"λ";
			for (var a = document.querySelectorAll('table.inventory1 tbody tr'), i = 0; a[i]; ++i) {
				moninput=a[i].querySelectorAll('input');
				monselect=a[i].querySelectorAll('select');
				if (moninput[0].value!="") {
					chaine1+=moninput[0].value+"λ"+monselect[0].value+"λ";
				}
				
			}
			for (var a = document.querySelectorAll('table.inventory2 tbody tr'), i = 0; a[i]; ++i) {
				moninput=a[i].querySelectorAll('input');
				monselect=a[i].querySelectorAll('select');
				if (moninput[0].value!="") {
					chaine2+=moninput[0].value+"λ"+monselect[0].value+"λ";
				}
			}
			console.log(chaine1)
			console.log(chaine2)

			var donnees = {	
				"Id": Id,
				"Contenu": contenu,
				"Cote":chaine1,
				"Code":chaine2
				};

					$.ajax({
						type: "POST",
						contentType: "application/json; charset=utf-8",
						url: "/Resultat/Add",
						dataType: "json",
						data: JSON.stringify(donnees),
						success: function (response) {
								console.log(response.responseText)
								location.href = "/Resultat/Details/"+Id;
							}
						,error: function (response) {
								console.log("error")
		
							}
					});

			});



$(document).on('click', '#saveresultat2', function(e) {
	const iframe = document.getElementsByTagName('iframe')[0];
	const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
  	//const element = iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText;
	var Id=iframeDoc.getElementById("identifiant").value;
	var contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].outerHTML;

	contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText+"<style>p,ul{margin: 0px;}</style>";

	//console.log(document.getElementsByTagName("iframe"))
	
	
	var donnees = {	
		"Id": Id,
		"Contenu": contenu.replaceAll('<td colspan="1" rowspan="1">','<td colspan="1" rowspan="1" style="border-width: 1px;border-style: solid;">'),
		};
		console.log(donnees.Contenu)

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Resultat/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						//console.log(response.responseText)
						location.href = "/Resultat/Details/"+Id;
					}
				,error: function (response) {
						console.log("error")

					}
			});

	});


	$(document).on('click', '#savefiche', function(e) {
	const iframe = document.getElementsByTagName('iframe')[0];
	const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;
	var Id=document.getElementById("identifiant").value;
	var nomfiche=document.getElementById("nomfiche").value;
	var categoriefiche=document.getElementById("cat").value;
	var contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText;

	contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText+"<style>p,ul{margin: 0px;}</style>";


	var donnees = {	
		"Id": parseInt(Id),
		"Nom": nomfiche,
		"Categorie": categoriefiche,
		"Contenu": contenu.replaceAll('<td colspan="1" rowspan="1">','<td colspan="1" rowspan="1" style="border-width: 1px;border-style: solid;">'),
		};
		console.log(donnees)

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Fiche/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						location.href = "/Fiche/Index/";
					}
				,error: function (response) {
						console.log("error")

					}
			});

	});



	$(document).on('click', '#actualiser_resultat', function(e) {
		//console.log(document.getElementsByClassName("editor-content")[0].innerHTML)
		var Id=document.getElementById("identifiant_result").value;
		//var contenu=document.getElementsByClassName("text-zinc-700")[0].innerText;

		
		
		
		var donnees = {	
			"Id": Id,
			};
			console.log(donnees.Contenu)

				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Resultat/Actualiser",
					dataType: "json",
					data: JSON.stringify(donnees),
					success: function (response) {
							//console.log(response.responseText)
							location.href = "/Resultat/Details/"+Id;
						}
					,error: function (response) {
							console.log("error")
	
						}
				});

		});


					





$(document).on('click', '.details_stock', function(e) {
   
    $("#tt").text($(this).find('.cl_00').text());
    $("#p1").text($(this).find('.cl_1').text());
    $("#p2").text($(this).find('.cl_2').text());
    $("#p3").text($(this).find('.cl_3').text());
	$("#p4").text($(this).find('.cl_4').text());
	$("#p5").text($(this).find('.cl_5').text());
	$("#p6").text($(this).find('.cl_6').text());
	$("#imag").attr("src", "/uploads/img/"+ $(this).find('.cl_000').text());
	
    $("#detastock").attr("onclick", "window.location='/Stock/Details/"+ $(this).find('.cl_0').text()+ "';");
});

$(document).on('click', '.modifierhono', function(e) {
//    console.log($(this).parent().parent().find('.medecin').text())
//    console.log($(this).parent().parent().find('.montant').text())
//    console.log($(this).parent().parent().find('.pourcentage').text())
//    console.log($(this).parent().parent().find('.montantmedecin').text())
//    console.log($(this).parent().parent().find('.montantpaye').text())
   

   $("#recept_medecin").val($(this).parent().parent().find('.medecin').text());
   $("#recept_montant").val(parseInt($(this).parent().parent().find('.montant').text().replace(/\s/g, '')));
   $("#recept_pourcentage").val(parseFloat($(this).parent().parent().find('.pourcentage').text().replace(/\s/g, '')));
   $("#recept_apayer").val(parseInt($(this).parent().parent().find('.montantmedecin').text().replace(/\s/g, '')));
   $("#recept_paye").val(parseInt($(this).parent().parent().find('.montantpaye').text().replace(/\s/g, '')));
   $("#defaultModalLabel").text($(this).parent().parent().find('.nomacte').text());
   $("#identifiant").val(parseInt($(this).parent().parent().find('.id').text()));
   $("#ident_doublons").hide();
   $("#ident_medecin").hide();

    $("#recept_apayer").val($("#recept_pourcentage").val()*$("#recept_montant").val()/100)
	$("#recept_paye").val($("#recept_pourcentage").val()*$("#recept_montant").val()/100)

   

});


$(document).on('click', '.ajouterhono', function(e) {
	//    console.log($(this).parent().parent().find('.medecin').text())
	//    console.log($(this).parent().parent().find('.montant').text())
	//    console.log($(this).parent().parent().find('.pourcentage').text())
	//    console.log($(this).parent().parent().find('.montantmedecin').text())
	//    console.log($(this).parent().parent().find('.montantpaye').text())
	   
	
	   //$("#recept_medecin2").val($(this).parent().parent().find('.medecin').text());
	   $("#recept_montant2").val(parseInt($(this).parent().parent().find('.montant').text().replace(/\s/g, '')));
	   //$("#recept_pourcentage2").val(parseFloat($(this).parent().parent().find('.pourcentage').text().replace(/\s/g, '')));
	   //$("#recept_apayer2").val(parseInt($(this).parent().parent().find('.montantmedecin').text().replace(/\s/g, '')));
	   //$("#recept_paye2").val(parseInt($(this).parent().parent().find('.montantpaye').text().replace(/\s/g, '')));
	   $("#defaultModalLabel2").text($(this).parent().parent().find('.nomacte').text());
	   $("#identifiant2").val(parseInt($(this).parent().parent().find('.id').text()));
	   $("#ident_doublons2").hide();
	   $("#ident_medecin2").hide();
	
		$("#recept_apayer").val($("#recept_pourcentage").val()*$("#recept_montant").val()/100)
		$("#recept_paye").val($("#recept_pourcentage").val()*$("#recept_montant").val()/100)
	
	   
	
});

$(document).on('click', '#savehono', function(e) {
 
	var donnees = {	
	 "Id": parseInt($("#identifiant").val()),
	 "Medecin": $("#recept_medecin").val(),
	 "Montant":parseInt($("#recept_montant").val()),
	 "Pourcentage":parseFloat($("#recept_pourcentage").val()),
	 "Montant_medecin":parseInt($("#recept_apayer").val()),
	 "Montant_paye":0

	 };
	
	 console.log(donnees)
	 if (donnees.Medecin!="" && donnees.Medecin!=null && donnees.Montant_medecin>0 && $("#brow")[0].outerHTML.includes(donnees.Medecin)) {
		$.ajax({
			type: "POST",
			contentType: "application/json; charset=utf-8",
			url: "/Honoraire/Add",
			dataType: "json",
			data: JSON.stringify(donnees),
			success: function (response) {
			   if (response=="doublons") {
				   $("#ident_doublons").show();
				   console.log(response.responseText)
			   }else if(response=="ok"){
				localStorage.setItem('scrollY', window.scrollY);
				   location.reload();
			   }else if(response=="medecin"){
				$("#ident_medecin").show();
			   }
					
				   
				}
			,error: function (response) {
					console.log("error")

				}
		});
	 }
		 
	
 
 });



 $(document).on('click', '#savehono2', function(e) {
 
	var donnees = {	
	 "Id": parseInt($("#identifiant2").val()),
	 "Medecin": $("#recept_medecin2").val(),
	 "Montant":parseInt($("#recept_montant2").val()),
	 "Pourcentage":parseFloat($("#recept_pourcentage2").val()),
	 "Montant_medecin":parseInt($("#recept_apayer2").val()),
	 "Montant_paye":0

	 };
	
	 console.log(donnees)
	 console.log()
	 if (donnees.Medecin!="" && donnees.Medecin!=null && donnees.Montant_medecin>0 && $("#brow")[0].outerHTML.includes(donnees.Medecin)) {
		 $.ajax({
			 type: "POST",
			 contentType: "application/json; charset=utf-8",
			 url: "/Honoraire/Clone",
			 dataType: "json",
			 data: JSON.stringify(donnees),
			 success: function (response) {
				console.log(response)
				if (response=="doublons") {
					$("#ident_doublons2").show();
					console.log(response.responseText)
				}else if(response=="ok"){
					localStorage.setItem('scrollY', window.scrollY);
					location.reload();
				}else if(response=="medecin"){
					$("#ident_medecin2").show();
				   }
					 
					
				 }
			 ,error: function (response) {
					 console.log("error")
 
				 }
		 });
	 }
 
 });


 $(document).on('click', '.paid', function(e) {
 
	 $("#ident").val(parseInt($(this).parent().parent().find('.id').text()))
 
 });


 $(document).on('click', '#savepaid', function(e) {
 
	var donnees = {	
	 "Id": parseInt($("#ident").val()),
	 "Paid_date":$("#date_paid").val()

	 };
	
	 console.log(donnees)
 
		 $.ajax({
			 type: "POST",
			 contentType: "application/json; charset=utf-8",
			 url: "/Honoraire/Paid",
			 dataType: "json",
			 data: JSON.stringify(donnees),
			 success: function (response) {
					 console.log(response.responseText)
					 localStorage.setItem('scrollY', window.scrollY);
					 location.reload();
				 }
			 ,error: function (response) {
					 console.log("error")
 
				 }
		 });
	
 
 });

 $(document).on('click', '.delete', function(e) {
 
	var donnees = {	
	 "Id": parseInt($(this).parent().parent().find('.id').text()),
	 };
	
	 location.href = "/Honoraire/delete/"+donnees.Id;
 
 });
 

 

 $(document).on('click', '#calculauto', function(e) {
 
	var donnees = {	
	 "Create_date":$("#dateconcernee").val(),
	 "Mode_paiement":$("#tippe").val()
	 };
	
	 console.log(donnees)
 
		 $.ajax({
			 type: "POST",
			 contentType: "application/json; charset=utf-8",
			 url: "/Honoraire/Auto",
			 dataType: "json",
			 data: JSON.stringify(donnees),
			 success: function (response) {
					 console.log(response.responseText)
					 location.reload();
				 }
			 ,error: function (response) {
					 console.log("error")
 
				 }
		 });
	
 
 });


 


 


 $(document).on('change keyup', '.edh', function(e) {
	
	$("#recept_apayer").val($("#recept_pourcentage").val()*$("#recept_montant").val()/100)
	$("#recept_paye").val($("#recept_apayer").val())

	$("#recept_apayer2").val($("#recept_pourcentage2").val()*$("#recept_montant2").val()/100)
	$("#recept_paye2").val($("#recept_apayer2").val())
});
$(document).on('change keyup', '.edh1', function(e) {
	
	$("#recept_pourcentage").val($("#recept_apayer").val()/$("#recept_montant").val()*100)
	$("#recept_pourcentage2").val($("#recept_apayer2").val()/$("#recept_montant2").val()*100)
	//$("#recept_paye").val($("#recept_apayer").val())
});



$(document).on('click', '.btn-warning', function(e) {
   
    $("#t1").text($(this).find('.cl_0').text());
    $("#t2").text($(this).find('.cl_1').text());
    $("#ident").val($(this).find('.cl_2').text());
    
    //$("#cloturetache").attr("onclick", "window.location='/Tache/Cloture/" + $(this).find('.cl_0').text() + "';");
    
});


$(document).on('click', '.openModalBtn', function(e) {
	$(this).find('input').val()
	console.log($(this).find('input').val())
	document.getElementById("target").setAttribute("src", "/Facture/Pdf/"+$(this).closest('td').find('input')[0].value+"?target="+$(this).find('input').val()+"#navpanes=0&view=FitH");
	modal.style.display = 'block';
    
});

$(document).on('click', '#paidButton', function(e) {
	console.log("ok")
	modal.style.display = 'block';
});

$(document).on('click', '#printResultat', function(e) {
	$(this).find('input').val()
	console.log($(this).find('input').val())
	var result="";

	if (document.getElementById("mlist")) {
		const container = document.getElementById("mlist");
		const checkboxes = container.querySelectorAll('input[type="checkbox"]:checked');
		
		var i=0;
		checkboxes.forEach(cb => {
			if(i==0){
				result+="target="+cb.value;
			}else{
				result+="&target="+cb.value;
			}
			i++;
		});
		if (checkboxes.length==0) {
			result="";
		}
	}

	
	
	document.getElementById("target").setAttribute("src", "/Resultat/Pdf/"+$("#identifiant").val()+"?"+result+"#navpanes=0");
	modal.style.display = 'block';
    
});


$(document).on('click', '#printResultat0', function(e) {
	$(this).find('input').val()

	document.getElementById("target").setAttribute("src", "/Examen/Pdf/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});

$(document).on('click', '#printFiche0', function(e) {
	//$(this).find('input').val()
	document.getElementById("target").setAttribute("src", "/Fiche/Pdf/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});

$(document).on('click', '#printFiche1', function(e) {
	//$(this).find('input').val()
	document.getElementById("target").setAttribute("src", "/FinalFiche/Pdf/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});

$(document).on('click', '.printFiche2', function(e) {
	//$(this).find('input').val()
	document.getElementById("target").setAttribute("src", "/FinalFiche/Pdf2/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});

$(document).on('click', '#voirrapport', function(e) {
	modal1.style.display = 'block';
});

$(document).on('click', '.ficheassurance', function(e) {
	console.log("yyyyyyyyyy")
	console.log($(this).find('input').val())
	document.getElementById("target").setAttribute("src", "/FicheAssurance/Pdf/"+$(this).find('input').val());
	modal.style.display = 'block';
    
});

$(document).on('click', '.fichearchive', function(e) {
	console.log($(this).find('input').val())
	console.log($(this).find('span').text())
	document.getElementById("target").setAttribute("src", "/archives/"+$(this).find('input').val()+"/"+$(this).find('span').text());
	modal.style.display = 'block';
    
});



document.querySelectorAll('.editFiche2').forEach(button => {
    button.addEventListener('click', function () {
        const span = this.querySelector('span');
        const id = span.textContent.trim();
        console.log("ID récupéré :", id);
		console.log(document.getElementById("nd").value);
		document.getElementById("target").setAttribute("src", "/FinalFiche/edit0/"+id+"?nd="+document.getElementById('nd').value);
		modal.style.display = 'block';
        // Tu peux maintenant utiliser `id` comme tu veux
    });
});





$(document).on('click', '#printcertificat0', function(e) {
	//$(this).find('input').val()
	document.getElementById("target").setAttribute("src", "/Documents/Pdf/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});






$(document).on('click', '#printResultat1', function(e) {
	$(this).find('input').val()
	console.log($(this).find('input').val())
	var result="target=A4";

	document.getElementById("target").setAttribute("src", "/Resultat/CouverturePdf/"+$("#identifiant").val()+"?"+result+"#navpanes=0");
	modal.style.display = 'block';
    
});

$(document).on('click', '#printResultat2', function(e) {
	$(this).find('input').val()
	console.log($(this).find('input').val())
	var result="target=A5";

	document.getElementById("target").setAttribute("src", "/Resultat/CouverturePdf/"+$("#identifiant").val()+"?"+result+"#navpanes=0");
	modal.style.display = 'block';
    
});


$(document).on('click', '#selectall', function(e) {
	const container = document.getElementById("mlist");
	const checkboxes = container.querySelectorAll('input[type="checkbox"]');
	checkboxes.forEach(cb => cb.checked = true);
});



$(document).on('click', '#printfact', function(e) {
	modal.style.display = 'block';
    
});


$(document).on('click', '#print_fusion', function(e) {
	//$(this).find('input').val()
	var result="λ";
	const checkboxes = document.querySelectorAll('#monTableau input[type="checkbox"]:checked');
	checkboxes.forEach(cb => {
	result+=cb.value+"λ";
	});
	//console.log($(this).find('input').val())
	
	if (result!="λ") {
		document.getElementById("target").setAttribute("src", "/Facture/Pdf_fusion?target="+result+"#navpanes=0");
		modal.style.display = 'block';
	}

});

$(document).on('change', '.mon-checkbox', function(e) {

	const checkboxes = document.querySelectorAll("input[type='checkbox']:checked");
	var resultat = "λ";

	checkboxes.forEach(cb => {
		resultat+=cb.value+"λ";
	});
	document.getElementById("accm").value = resultat;
	console.log(resultat)
	console.log(document.getElementById("accm").value)
	
});




document.addEventListener("DOMContentLoaded", function () {
  
	const select1 = document.getElementById("monSelect1");
    const liste1 = Array.from(select1.selectedOptions).map(opt => opt.value);

	const select2 = document.getElementById("choices");
    const liste2 = Array.from(select2.selectedOptions).map(opt => opt.value);
	
	
	
	const obj = {
		debut: document.getElementById("datedebut").value, // format ISO 8601
		fin: document.getElementById("datefin").value,
		docta: liste2,
		Secretaire: liste1
	};
	console.log(obj)

	fetch("/Honoraire/Pdf", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(obj)
	})
	.then(response => response.blob())
	.then(blob => {
		const url = URL.createObjectURL(blob);
		document.getElementById("pdfFrame").src = url;
	})
	.catch(error => console.error("Erreur PDF :", error));
});


$(document).on('click', '#valid_selection', function(e) {
	const select1 = document.getElementById("monSelect1");
    const liste1 = Array.from(select1.selectedOptions).map(opt => opt.value);

	const select2 = document.getElementById("choices");
    const liste2 = Array.from(select2.selectedOptions).map(opt => opt.value);
	
	
	
	const obj = {
		debut: document.getElementById("datedebut").value, // format ISO 8601
		fin: document.getElementById("datefin").value,
		docta: liste2,
		Secretaire: liste1
	};
	console.log(obj)

	fetch("/Honoraire/Pdf", {
		method: "POST",
		headers: { "Content-Type": "application/json" },
		body: JSON.stringify(obj)
	})
	.then(response => response.blob())
	.then(blob => {
		const url = URL.createObjectURL(blob);
		document.getElementById("pdfFrame").src = url;
		modal.style.display = 'block';
		loader.style.display = "none";
	})
	.catch(error => console.error("Erreur PDF :", error));
	

});


//save template 
	$(document).on('click', '#interactionmedoclabo', function(e) {
    const inputs = document.getElementById('target_exam').innerHTML;
    var prompt="faire une analyse de ce résultat d'examen:(en une phrase)"+ inputs;
	document.getElementById("reponseia").innerHTML = "Analyse IA en cours patientez...";
	$("#reponseia").show();
	var donnees = {	
			"Contenu": prompt,
		};
		console.log(donnees)
			if(donnees.Nom!="" && donnees.Categorie!=""){
				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Dmi/IAtest",
					dataType: "json",
					data: JSON.stringify(donnees),
					success: function (response) {
							console.log(response)
							document.getElementById("reponseia").innerHTML = response;
						}
					,error: function (response) {
							console.log("error")

						}
				});
			}

});







window.onresize = displayWindowSize;
window.onload = displayWindowSize;
function displayWindowSize() {
    document.getElementById('size').innerHTML = window.innerWidth + ' x ' + window.innerHeight;
    document.querySelector('.fontsize').innerHTML = 'Font size: ' + window.getComputedStyle(document.body).getPropertyValue('font-size');
};






function SeparateurDeMillier(a, b) {
    a = '' + a;
    b = b || ' ';
    var c = '',
        d = 0;
    while (a.match(/^0[0-9]/)) {
      a = a.substr(1);
    }
    for (var i = a.length-1; i >= 0; i--) {
      c = (d != 0 && d % 3 == 0) ? a[i] + b + c : a[i] + c;
      d++;
    }
    return c;
}


window.onload = function() {
    //console.log("separateur")
    var cells;
    //var i=0;
    cells = document.querySelectorAll('.separateur');
    //console.log(cells)
    for (let index = 0; index < cells.length; index++) {
      //console.log(cells[i].innerHTML)
        cells[index].innerHTML=SeparateurDeMillier(cells[index].innerHTML)

    }
	console.log(document.getElementsByClassName("text-zinc-700").length==0)
	if (document.getElementsByClassName("text-zinc-700").length!=0) {
		document.getElementsByClassName("text-zinc-700")[0].style.display='none';
		document.getElementsByClassName("text-foreground")[0].style.display='none';
	}

	var paragraphs = document.getElementsByTagName("p");
		for(var i = 0; i < paragraphs.length; i++)
		{
			if (paragraphs[i].innerText.trim()=="" && document.getElementsByClassName("text-zinc-700").length==0) {
				paragraphs[i].outerHTML="<br/>";
			}
		}
	
	

};


// mise à jour suivi courrier
$(document).on('click', '#enregistrer_ordre', function(e) {
	
	var ordre = document.querySelectorAll(".orderprint");
	var facture=document.getElementById("numfact").value;
	var chaine="λ";
		ordre.forEach(element => {
			if (element.value!="") {
				chaine+=element.value+"λ"
			}
			
		});
		location.href = "/Resultat/Print?fact="+facture+"&chaine="+chaine;
});



