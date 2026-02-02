$( document ).ready(function() {
	makeCode();

	// //console.log("separateur")
    // var cells;
    // //var i=0;
    // cells = document.querySelectorAll('.separateur');
    // //console.log(cells)
    // for (let index = 0; index < cells.length; index++) {
    //   //console.log(cells[i].innerHTML)
    //     cells[index].innerHTML=SeparateurDeMillier(cells[index].innerHTML)

    // }

    if ($("#target").val().includes("assurance")) {
		$( ".colonneassurance" ).show();
		if ($("#target").val()=="consultation_assurance") {
			$(".table2").hide();
			$(".table3").hide();
			$(".table4").hide();
		}else if($("#target").val()=="examens_assurance"){
			$(".table1").hide();
			$(".table3").hide();
			$(".table4").hide();
		}else if($("#target").val()=="actes_assurance"){
			$(".table1").hide();
			$(".table2").hide();
			$(".table4").hide();
		}else if($("#target").val()=="medicament_assurance"){
			$(".table1").hide();
			$(".table2").hide();
			$(".table3").hide();
		}
		if ($("#lf1").val()=="λ") {
			$(".table1").hide();
		}
		if($("#lf2").val()=="λ"){
			$(".table2").hide();
		}
		if($("#lf3").val()=="λ"){
			$(".table3").hide();
		}
		if($("#lf4").val()=="λ"){
			$(".table4").hide();
		}


	}else{
		$(".colonneassurance").hide();
		if ($("#target").val()=="consultation_0") {
			$(".table2").hide();
			$(".table3").hide();
			$(".table4").hide();
		}else if($("#target").val()=="examens_0"){
			$(".table1").hide();
			$(".table3").hide();
			$(".table4").hide();
		}else if($("#target").val()=="actes_0"){
			$(".table1").hide();
			$(".table2").hide();
			$(".table4").hide();
		}else if($("#target").val()=="medicament_0"){
			$(".table1").hide();
			$(".table2").hide();
			$(".table3").hide();
		}

		if ($("#lf1").val()=="λ") {
			$(".table1").hide();
		}
		if($("#lf2").val()=="λ"){
			$(".table2").hide();
		}
		if($("#lf3").val()=="λ"){
			$(".table3").hide();
		}
		if($("#lf4").val()=="λ"){
			$(".table4").hide();
		}


	}
	$('#montant_lettre_fusion2').text(NumberToLetter(parseInt($('#netapa').val()))+" FCFA");
	$('#montant_lettre_fusion1').text(NumberToLetter(parseInt($('#netapa1').val()))+" FCFA");


});



var qrcode = new QRCode("qrcode");

function makeCode () {		
	var elText = document.getElementById("text");
	
	if (!elText.value) {
		alert("Input a text");
		elText.focus();
		return;
	}
	
	qrcode.makeCode(elText.value);
}



	$("#text").on("blur", function () {
		makeCode();
	}).
	on("keydown", function (e) {
		if (e.keyCode == 13) {
			makeCode();
		}
	});



	$(document).on('click', '#enentet', function(e) {
		document.getElementById("img1").style.zIndex = "-1";
		document.getElementById("tdt").style.color = "white";
		document.getElementById("tdt1").style.backgroundImage = 'url("")';
		

	});
	


function printContent(el){
	var restorepage = $('body').html();
	document.getElementById("pied").style.display="inline-block";
	var elements =document.getElementsByTagName("p");
	if(document.getElementById("terg").value!="NFS"){
		for (let index = 0; index < elements.length; index++) {
			elements[index].style.margin="0px";
		}
	}else{
		for (let index = 0; index < elements.length; index++) {
			elements[index].style.margin="1px";
		}
	}
	
	
	var printcontent = $('#' + el).clone();
	document.getElementById("pied").style.width="0px";
	$('body').empty().html(printcontent);

	window.print();
	$('body').html(restorepage);
	location.reload();
}







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



	

function Unite( nombre ){
	var unite;
	switch( nombre ){
		case 0: unite = "zéro";		break;
		case 1: unite = "un";		break;
		case 2: unite = "deux";		break;
		case 3: unite = "trois"; 	break;
		case 4: unite = "quatre"; 	break;
		case 5: unite = "cinq"; 	break;
		case 6: unite = "six"; 		break;
		case 7: unite = "sept"; 	break;
		case 8: unite = "huit"; 	break;
		case 9: unite = "neuf"; 	break;
	}//fin switch
	return unite;
}//-----------------------------------------------------------------------

function Dizaine( nombre ){
	switch( nombre ){
		case 10: dizaine = "dix"; break;
		case 11: dizaine = "onze"; break;
		case 12: dizaine = "douze"; break;
		case 13: dizaine = "treize"; break;
		case 14: dizaine = "quatorze"; break;
		case 15: dizaine = "quinze"; break;
		case 16: dizaine = "seize"; break;
		case 17: dizaine = "dix-sept"; break;
		case 18: dizaine = "dix-huit"; break;
		case 19: dizaine = "dix-neuf"; break;
		case 20: dizaine = "vingt"; break;
		case 30: dizaine = "trente"; break;
		case 40: dizaine = "quarante"; break;
		case 50: dizaine = "cinquante"; break;
		case 60: dizaine = "soixante"; break;
		case 70: dizaine = "soixante-dix"; break;
		case 80: dizaine = "quatre-vingt"; break;
		case 90: dizaine = "quatre-vingt-dix"; break;
	}//fin switch
	return dizaine;
}//-----------------------------------------------------------------------

function NumberToLetter( nombre ){
	var i, j, n, quotient, reste, nb ;
	var ch
	var numberToLetter='';
	//__________________________________
	
	if(  nombre.toString().replace( / /gi, "" ).length > 15  )	return "dépassement de capacité";
	if(  isNaN(nombre.toString().replace( / /gi, "" ))  )		return "Nombre non valide";

	nb = parseFloat(nombre.toString().replace( / /gi, "" ));
	if(  Math.ceil(nb) != nb  )	return  "Nombre avec virgule non géré.";
	
	n = nb.toString().length;
	switch( n ){
		 case 1: numberToLetter = Unite(nb); break;
		 case 2: if(  nb > 19  ){
					   quotient = Math.floor(nb / 10);
					   reste = nb % 10;
					   if(  nb < 71 || (nb > 79 && nb < 91)  ){
							 if(  reste == 0  ) numberToLetter = Dizaine(quotient * 10);
							 if(  reste == 1  ) numberToLetter = Dizaine(quotient * 10) + "-et-" + Unite(reste);
							 if(  reste > 1   ) numberToLetter = Dizaine(quotient * 10) + "-" + Unite(reste);
					   }else numberToLetter = Dizaine((quotient - 1) * 10) + "-" + Dizaine(10 + reste);
				 }else numberToLetter = Dizaine(nb);
				 break;
		 case 3: quotient = Math.floor(nb / 100);
				 reste = nb % 100;
				 if(  quotient == 1 && reste == 0   ) numberToLetter = "cent";
				 if(  quotient == 1 && reste != 0   ) numberToLetter = "cent" + " " + NumberToLetter(reste);
				 if(  quotient > 1 && reste == 0    ) numberToLetter = Unite(quotient) + " cents";
				 if(  quotient > 1 && reste != 0    ) numberToLetter = Unite(quotient) + " cent " + NumberToLetter(reste);
				 break;
		 case 4 :  quotient = Math.floor(nb / 1000);
					  reste = nb - quotient * 1000;
					  if(  quotient == 1 && reste == 0   ) numberToLetter = "mille";
					  if(  quotient == 1 && reste != 0   ) numberToLetter = "mille" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0    ) numberToLetter = NumberToLetter(quotient) + " mille";
					  if(  quotient > 1 && reste != 0    ) numberToLetter = NumberToLetter(quotient) + " mille " + NumberToLetter(reste);
					  break;
		 case 5 :  quotient = Math.floor(nb / 1000);
					  reste = nb - quotient * 1000;
					  if(  quotient == 1 && reste == 0   ) numberToLetter = "mille";
					  if(  quotient == 1 && reste != 0   ) numberToLetter = "mille" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0    ) numberToLetter = NumberToLetter(quotient) + " mille";
					  if(  quotient > 1 && reste != 0    ) numberToLetter = NumberToLetter(quotient) + " mille " + NumberToLetter(reste);
					  break;
		 case 6 :  quotient = Math.floor(nb / 1000);
					  reste = nb - quotient * 1000;
					  if(  quotient == 1 && reste == 0   ) numberToLetter = "mille";
					  if(  quotient == 1 && reste != 0   ) numberToLetter = "mille" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0    ) numberToLetter = NumberToLetter(quotient) + " mille";
					  if(  quotient > 1 && reste != 0    ) numberToLetter = NumberToLetter(quotient) + " mille " + NumberToLetter(reste);
					  break;
		 case 7: quotient = Math.floor(nb / 1000000);
					  reste = nb % 1000000;
					  if(  quotient == 1 && reste == 0  ) numberToLetter = "un million";
					  if(  quotient == 1 && reste != 0  ) numberToLetter = "un million" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " millions";
					  if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " millions " + NumberToLetter(reste);
					  break;  
		 case 8: quotient = Math.floor(nb / 1000000);
					  reste = nb % 1000000;
					  if(  quotient == 1 && reste == 0  ) numberToLetter = "un million";
					  if(  quotient == 1 && reste != 0  ) numberToLetter = "un million" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " millions";
					  if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " millions " + NumberToLetter(reste);
					  break;  
		 case 9: quotient = Math.floor(nb / 1000000);
					  reste = nb % 1000000;
					  if(  quotient == 1 && reste == 0  ) numberToLetter = "un million";
					  if(  quotient == 1 && reste != 0  ) numberToLetter = "un million" + " " + NumberToLetter(reste);
					  if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " millions";
					  if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " millions " + NumberToLetter(reste);
					  break;  
		 case 10: quotient = Math.floor(nb / 1000000000);
						reste = nb - quotient * 1000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un milliard";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un milliard" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " milliards";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " milliards " + NumberToLetter(reste);
					    break;	
		 case 11: quotient = Math.floor(nb / 1000000000);
						reste = nb - quotient * 1000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un milliard";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un milliard" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " milliards";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " milliards " + NumberToLetter(reste);
					    break;	
		 case 12: quotient = Math.floor(nb / 1000000000);
						reste = nb - quotient * 1000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un milliard";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un milliard" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " milliards";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " milliards " + NumberToLetter(reste);
					    break;	
		 case 13: quotient = Math.floor(nb / 1000000000000);
						reste = nb - quotient * 1000000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un billion";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un billion" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " billions";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " billions " + NumberToLetter(reste);
					    break; 	
		 case 14: quotient = Math.floor(nb / 1000000000000);
						reste = nb - quotient * 1000000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un billion";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un billion" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " billions";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " billions " + NumberToLetter(reste);
					    break; 	
		 case 15: quotient = Math.floor(nb / 1000000000000);
						reste = nb - quotient * 1000000000000;
						if(  quotient == 1 && reste == 0  ) numberToLetter = "un billion";
						if(  quotient == 1 && reste != 0  ) numberToLetter = "un billion" + " " + NumberToLetter(reste);
						if(  quotient > 1 && reste == 0   ) numberToLetter = NumberToLetter(quotient) + " billions";
						if(  quotient > 1 && reste != 0   ) numberToLetter = NumberToLetter(quotient) + " billions " + NumberToLetter(reste);
					    break; 	
	 }//fin switch
	 /*respect de l'accord de quatre-vingt*/
	 if(  numberToLetter.substr(numberToLetter.length-"quatre-vingt".length,"quatre-vingt".length) == "quatre-vingt"  ) numberToLetter = numberToLetter + "s";
	 
	 return numberToLetter;
}//-----------------------------------------------------------------------



	

