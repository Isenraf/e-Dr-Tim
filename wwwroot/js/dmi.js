$(document).on('click', '#saveconsultation', function(e) {
	var Id=document.getElementById("identifiant").value;
    var Numero_dossier=document.getElementById("Numero_dossier").value;
    var Numero_de_facture=document.getElementById("Numero_de_facture").value;
    var Medecin=document.getElementById("Medecin").value;
    


    var trumpsList = [];
    trumpsList.push(document.getElementById("motifcon").innerHTML);
    trumpsList.push(document.getElementById("histoire").innerHTML);
    trumpsList.push(document.getElementById("antecedent").innerHTML);
    trumpsList.push(document.getElementById("rapportEnqueteSysteme").innerHTML);
    trumpsList.push(document.getElementById("rapportExamphy").innerHTML);
    trumpsList.push(document.getElementById("diagnos1").innerHTML);
    trumpsList.push(document.getElementById("compli").innerHTML);
    trumpsList.push(document.getElementById("examcom").innerHTML);
    trumpsList.push(document.getElementById("diagnos2").innerHTML);
    trumpsList.push(document.getElementById("rapptraitement").innerHTML);
    trumpsList.push(document.getElementById("suivi").innerHTML);
    trumpsList.push(document.getElementById("rappconclu").innerHTML);
    trumpsList.push("xtargettrumpx");
    trumpsList.push(document.getElementById("trump1").innerHTML);
    trumpsList.push(document.getElementById("trump2").innerHTML);
    trumpsList.push(document.getElementById("trump3").innerHTML);
    trumpsList.push(document.getElementById("trump4").innerHTML);
    trumpsList.push(document.getElementById("trump5").innerHTML);
    trumpsList.push(document.getElementById("trump6").innerHTML);
    trumpsList.push(document.getElementById("trump7").innerHTML);
    trumpsList.push(document.getElementById("trump8").innerHTML);
    trumpsList.push(document.getElementById("trump9").innerHTML);
    trumpsList.push(document.getElementById("trump10").innerHTML);
    trumpsList.push(document.getElementById("trump11").innerHTML);

    console.log(trumpsList)

	
	var donnees = {	
		"Id": parseInt(Id),
        "NumeroDossier": Numero_dossier,
        "NumeroDeFacture": Numero_de_facture,
        "Trumps": trumpsList,
        "Medecin":Medecin
		};

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Dmi/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response)
						window.parent.location.href = "/Dmi/Details/"+response;
					}
				,error: function (response) {
						console.log("error")

					}
			});

	});


  $(document).on('click', '#saveconsultation2', function(e) {
	var Id=document.getElementById("identifiant").value;
    var Numero_dossier=document.getElementById("Numero_dossier").value;
    var Numero_de_facture=document.getElementById("Numero_de_facture").value;
    var Medecin=document.getElementById("Medecin").value;
    


    var trumpsList = [];
    trumpsList.push(document.getElementById("motifcon").innerHTML);
    trumpsList.push(document.getElementById("histoire").innerHTML);
    trumpsList.push(document.getElementById("antecedent").innerHTML);
    trumpsList.push(document.getElementById("enquete").innerHTML);
    trumpsList.push(document.getElementById("examphy").innerHTML);
    trumpsList.push(document.getElementById("diagnos1").innerHTML);
    trumpsList.push(document.getElementById("compli").innerHTML);
    trumpsList.push(document.getElementById("examcom").innerHTML);
    trumpsList.push(document.getElementById("diagnos2").innerHTML);
    trumpsList.push(document.getElementById("rapptraitement").innerHTML);
    trumpsList.push(document.getElementById("suivi").innerHTML);
    trumpsList.push("rappconclu");
    trumpsList.push("xtargettrumpx");
    trumpsList.push(document.getElementById("trump1").innerHTML);
    trumpsList.push(document.getElementById("trump2").innerHTML);
    trumpsList.push(document.getElementById("trump3").innerHTML);
    trumpsList.push(document.getElementById("trump4").innerHTML);
    trumpsList.push(document.getElementById("trump5").innerHTML);
    trumpsList.push(document.getElementById("trump6").innerHTML);
    trumpsList.push(document.getElementById("trump7").innerHTML);
    trumpsList.push(document.getElementById("trump8").innerHTML);
    trumpsList.push(document.getElementById("trump9").innerHTML);
    trumpsList.push(document.getElementById("trump10").innerHTML);
    trumpsList.push(document.getElementById("trump11").innerHTML);

    console.log(trumpsList)

	
	var donnees = {	
		"Id": parseInt(Id),
        "NumeroDossier": Numero_dossier,
        "NumeroDeFacture": Numero_de_facture,
        "Trumps": trumpsList,
        "Medecin":Medecin,
        "Pc":"lite"
		};

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Dmi/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response)
						window.parent.location.href = "/Dmi/Details/"+response;
					}
				,error: function (response) {
						console.log("error")

					}
			});

	});


  

  $(document).on('click', '#savefiche3', function(e) {
	// var Id=document.getElementById("identifiant").value;
    var Numero_dossier=document.getElementById("NumeroDossier").value;
    var infirmere=document.getElementById("infirmere").value;
    var CreateDate=document.getElementById("CreateDate").value;
    


    var trumpsList = [];
    trumpsList.push(document.getElementById("a1").value);
    trumpsList.push(document.getElementById("a2").value);
    trumpsList.push(document.getElementById("a3").value);
    trumpsList.push(document.getElementById("a4").value);
    trumpsList.push(document.getElementById("a5").value);

    console.log(trumpsList)

	
	var donnees = {	
		    "Id": 0,
        "CreateDate": new Date(CreateDate),
        "NumeroDossier": Numero_dossier,
        "infirmere": infirmere,
        "Trumps":trumpsList
		};

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Fiche3/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response)
						location.reload();
					}
				,error: function (response) {
						console.log("error")

					}
			});

	});


  


  $(".transfert_go").on("click", function() {
    // on trouve l'input associé dans le même menu
    var valeur = $(this).closest("ul").find(".med").val();
    var facture = $(this).closest("ul").find(".facture").val();
    var donnees = {	
        "Numero_de_facture": facture,
        "Medecin": valeur,
		};

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Facture/Trans",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						console.log(response)
						location.reload();
					}
				,error: function (response) {
						console.log("error")

					}
			});
    
});


  $("#saveronde").on("click", function() {
    // on trouve l'input associé dans le même menu
    var plainte = document.getElementById("plainte").value;
    var parametres = document.getElementById("parametres").innerHTML;
    var examenphysique = document.getElementById("examenphysique").value;
    var conclusion = document.getElementById("conclusion").value;
    var nd = document.getElementById("nd").value;
    console.log("jjjj")

    


    var trumpsList1 = [];
      document.querySelectorAll('.a2').forEach(function(el) {
          trumpsList1.push(el.value);
      });

    var trumpsList2 = [];
      document.querySelectorAll('.a1').forEach(function(el) {
          trumpsList2.push(el.value);
      });

    var trumpsList3 = [];
      document.querySelectorAll('.a3').forEach(function(el) {
          trumpsList3.push(el.value);
      });

    var trumpsList4 = [];
      document.querySelectorAll('.a4').forEach(function(el) {
          trumpsList4.push(el.value);
      });


    var donnees = {	
        "Plainte": plainte,
        "Parametres": parametres,
        "Epj": examenphysique,
        "Trumps": trumpsList1,
        "Trumps2": trumpsList2,
        "Trumps3": trumpsList3,
        "Trumps4": trumpsList4,
        "Pc":conclusion,
        "NumeroDossier":nd
		};
    console.log(donnees)

			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Ronde/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
						location.href = "/Ronde/Details/"+response;
					}
				,error: function (response) {
						console.log("error")

					}
			});
    
});


//save template 
	$(document).on('click', '#signature', function(e) {
	var contenu="";
	
	const iframe = document.getElementsByTagName('iframe')[0];
	const iframeDoc = iframe.contentDocument || iframe.contentWindow.document;

	
  var nom=iframeDoc.getElementById("nom").value;
  var categorie=iframeDoc.getElementById("categorie").value;
  
  var dossier=document.getElementById("dossier").value;
  var genre=document.getElementById("genre").value;
  var date_naissance=document.getElementById("date_naissance").value;


	contenu=iframeDoc.getElementsByClassName("text-zinc-700")[0].innerText+"<style>p,ul{margin: 0px;}</style>";

		contenu=contenu.replaceAll('<td colspan="1" rowspan="1">','<td colspan="1" rowspan="1" style="border-width: 1px;border-style: solid;">');

	var donnees = {	
		"Id": 0,
		"Nom": nom,
		"Categorie": categorie,
		"Contenu": contenu,
    "Numero_dossier": dossier,
    "DatedeNaissance": new Date(date_naissance),
    "Genre": genre
		};
		console.log(donnees)
			if(donnees.Nom!="" && donnees.Categorie!=""){
			$.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/FinalFiche/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
          const modal = document.getElementById('pdfModal');
          modal.style.display = 'none';

						console.log(response)
						
					}
				,error: function (response) {
						console.log("error")

					}
			});
			}

});


//save template 
	$(document).on('click', '#interactionmedoc', function(e) {
    e.preventDefault();    // empêche le comportement par défaut
    e.stopPropagation();
    const inputs = document.querySelectorAll('.intmd');
    // Extraire leurs valeurs et les joindre par des virgules
    const valeurs = Array.from(inputs)
      .map(input => input.value.trim())
      .filter(val => val !== "") // ignore les vides
      .join(',');
    var prompt="faire une interaction médicamenteuse de ces médicaments:(en une phrase)"+ valeurs;
    document.getElementById("reponseia").innerHTML = "Recherche interaction médicamenteuse en cours patientez...";
    $("#reponseia").show();


    const textarea = document.querySelector('.inputObs');
    const colResultat = document.getElementById('colResultat');
    const result = document.getElementById('resultatIA');
    const btnValider = document.getElementById('interactionmedoc');

  

          var donnees = {	
        "Contenu":  prompt,
        };
        console.log(donnees)
          
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

  
   
	

	
			

});




$(document).on('click', '.btn-ia', function () {
    // On remonte jusqu'à la ligne parente
    let ligne = $(this).closest('.ligne-traitement2');
    // On récupère la valeur du champ intmd dans cette ligne
    let valeur = ligne.find('.intmd').val();
    console.log(valeur)
    var prompt="posologie (en une phrase)"+ valeur+" " + document.getElementById('param').value ;
    document.getElementById("reponseia").innerHTML = "Recherche en cours patientez...";
    $("#reponseia").show();

    var donnees = {	
        "Contenu":  prompt,
        };
        console.log(donnees)
          
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
    // Affiche ou utilise la valeur comme tu veux
    console.log("Valeur Prescription :", valeur);

    // Exemple : afficher la valeur dans le menu dropdown
    ligne.find('.resultat-ia').text("Résultat IA pour : " + valeur);
  });









$(document).on('click', '#printFiche2', function(e) {
	console.log("kkkkkkkkk")
	document.getElementById("target").setAttribute("src", "/FinalFiche/Pdf2/"+$("#identifiant").val());
	modal.style.display = 'block';
    
});






  




    

function getInitialDropdown() {
  return `
    <div class="btn-group">
      <button type="button" class="btn btn-outline-primary dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
        Valider
      </button>
      <ul class="dropdown-menu p-3" style="min-width: 300px;">
        <li>
          <div class="d-flex flex-column gap-2">
            <input type="date" class="form-control inputDate" />
            <input type="text" class="form-control inputLot" placeholder="# Numéro de lot" />
            <textarea rows="3" class="form-control inputObs" placeholder="Observation"></textarea>
            <button type="button" class="btn btn-outline-info btnSave">Enregistrer</button>
          </div>
        </li>
      </ul>
    </div>
  `;
}

document.addEventListener('click', function (e) {
  const saveBtn = e.target.closest('.btnSave');

  
  if (saveBtn) {
    const container = saveBtn.closest('.action-container');
    const row = saveBtn.closest('tr');

    const date = container.querySelector('.inputDate')?.value || '';
    const lot  = container.querySelector('.inputLot')?.value  || '';
    const obs  = container.querySelector('.inputObs')?.value  || '';

    if (row) row.classList.add('table-success');

    const tooltipContent = `Date : ${date}\nLot : ${lot}\nObs : ${obs}`;

    container.innerHTML = `
    <div class="d-flex align-items-center gap-2">
        <button type="button" class="btn btn-outline-primary"
                data-bs-toggle="tooltip"
                data-bs-placement="left"
                data-bs-custom-class="custom-tooltip-danger"
                data-bs-title="${tooltipContent.replace(/"/g, '&quot;')}">
        <i class="ri-arrow-left-s-fill"></i>
        </button>
        <button type="button" class="btn btn-outline-danger btnReset">
        Réinitialiser
        </button>
    </div>
    `;

    const newBtn = container.querySelector('[data-bs-toggle="tooltip"]');
    if (newBtn) new bootstrap.Tooltip(newBtn);

        
        var identifiant=document.getElementById("identifiant").value;
        var dossier=document.getElementById("dossier").value;
        var contenu=document.getElementById("calendarvac").innerHTML;
        var donnees = {	
		"Id": parseInt(identifiant),
        "Numero_Dossier": dossier,
        "contenu": contenu,

		};
        console.log(contenu)

        $.ajax({
				type: "POST",
				contentType: "application/json; charset=utf-8",
				url: "/Vaccination/Add",
				dataType: "json",
				data: JSON.stringify(donnees),
				success: function (response) {
                        

					}
				,error: function (response) {
						console.log("error")

					}
			});

    



  }

//   const resetBtn = e.target.closest('.btnReset');
//   if (resetBtn) {
//     const container = resetBtn.closest('.action-container');
//     const row = resetBtn.closest('tr');

//     // Restaurer le premier code
//     container.innerHTML = getInitialDropdown();

//     // Enlever le style vert de la ligne
//     if (row) row.classList.remove('table-success');
//   }
});






