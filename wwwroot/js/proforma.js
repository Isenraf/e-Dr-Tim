//fixed div
var fixmeTop = $('.fixme').offset().top;       // get initial position of the element

$(window).scroll(function() {                  // assign scroll event listener

	var currentScroll = $(window).scrollTop(); // get current position

	if (currentScroll >= fixmeTop) {           // apply position: fixed if you
		$('.fixme').css({                      // scroll to that element or below it
			display:'inline-block',
			bottom: '0px',
			top: ''
			

		});
	} else {                                   // apply position: static
		$('.fixme').css({                      // if you scroll above it
			display:'none',
			top: '72px',
			bottom: ''
		});
	}

});


//ajout et suppression des lignes dans le tableau
$(document).on('click', '.add1', function(e) {
	document.querySelector('table.inventory1 tbody').appendChild(generateTableRow());
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.add2', function(e) {
	document.querySelector('table.inventory2 tbody').appendChild(generateTableRow2());
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.add3', function(e) {
	document.querySelector('table.inventory3 tbody').appendChild(generateTableRow3());
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.add4', function(e) {
	document.querySelector('table.inventory4 tbody').appendChild(generateTableRow4());
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.cut', function(e) {
	$(this.ancestorQuerySelector('tr')).remove();
	updateInvoice();
});
$(document).on('click', '.tr_clone_add1', function(e) {
	var $tr    = $(this).closest('.tr_clone1');
    var $clone = $tr.clone();
    $clone.find(':text').val('');
    $tr.after($clone);
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.tr_clone_add2', function(e) {
	var $tr    = $(this).closest('.tr_clone2');
    var $clone = $tr.clone();
    $clone.find(':text').val('');
    $tr.after($clone);
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.tr_clone_add3', function(e) {
	var $tr    = $(this).closest('.tr_clone3');
    var $clone = $tr.clone();
    $clone.find(':text').val('');
    $tr.after($clone);
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});
$(document).on('click', '.tr_clone_add4', function(e) {
	var $tr    = $(this).closest('.tr_clone4');
    var $clone = $tr.clone();
    $clone.find(':text').val('');
    $tr.after($clone);
	if ($("#target").val().includes("assurance")) {
		$(".colonneassurance").show();
	}else{
		$(".colonneassurance").hide();
	}
	updateInvoice();
});

function generateTableRow() {
	var emptyColumn = document.createElement('tr');
	emptyColumn.classList.add('tr_clone1');

	emptyColumn.innerHTML = '<td style=""><a style="color: #FFF;" class="cut btn theme-bg gradient">-</a><a style="color: #FFF;" class="tr_clone_add1 btn theme-bg gradient">+</a><input style="border-color: #69b440;" contenteditable class="form-control service1" list="service1" autocomplete="off"></td>'+
							'<td style="text-align: center;"><span></span></td>'+
							'<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: center;" class="form-control pourcent" type="number"></td>'+
                            '<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: right;" class="form-control plafond" type="number"></td>'+
							'<td style="text-align: right"><span>0</span><span data-prefix> FCFA</span></td>'+
							'<td style="" ><input style="border-color: #69b440;text-align: center" contenteditable class="form-control qte1" type="number" value="1"></td>'+
							'<td style="text-align: right;"><span>0</span><span data-prefix> FCFA</span></td>';

	return emptyColumn;
 }

 function generateTableRow2() {
	var emptyColumn = document.createElement('tr');
	emptyColumn.classList.add('tr_clone2');

	emptyColumn.innerHTML = '<td style=""><a style="color: #FFF;" class="cut btn theme-bg gradient">-</a><a style="color: #FFF;" class="tr_clone_add2 btn theme-bg gradient">+</a><input style="border-color: #69b440;" contenteditable class="form-control service2" list="service2" autocomplete="off"></td>'+
							'<td style="text-align: center;"><span></span></td>'+
							'<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: center;" class="form-control pourcent" type="number"></td>'+
                            '<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: right;" class="form-control plafond" type="number"></td>'+
							'<td style="text-align: right"><span>0</span><span data-prefix> FCFA</span></td>'+
							'<td style="" ><input style="border-color: #69b440;text-align: center" contenteditable class="form-control qte2" type="number" value="1"></td>'+
							'<td style="text-align: right;"><span>0</span><span data-prefix> FCFA</span></td>';
	return emptyColumn;
 }

 function generateTableRow3() {
	var emptyColumn = document.createElement('tr');
	emptyColumn.classList.add('tr_clone3');

	emptyColumn.innerHTML = '<td style=""><a style="color: #FFF;" class="cut btn theme-bg gradient">-</a><a style="color: #FFF;" class="tr_clone_add3 btn theme-bg gradient">+</a><input style="border-color: #69b440;" contenteditable class="form-control service3" list="service3" autocomplete="off"></td>'+
							'<td style="text-align: center;"><span></span></td>'+
							'<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: center;" class="form-control pourcent" type="number"></td>'+
                            '<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: right;" class="form-control plafond" type="number"></td>'+
							'<td style="text-align: right"><span>0</span><span data-prefix> FCFA</span></td>'+
							'<td style="" ><input style="border-color: #69b440;text-align: center" contenteditable class="form-control qte3" type="number" value="1"></td>'+
							'<td style="text-align: right;">0<span></span><span data-prefix> FCFA</span></td>';

	return emptyColumn;
 }

 function generateTableRow4() {
	var emptyColumn = document.createElement('tr');
	emptyColumn.classList.add('tr_clone4');

	emptyColumn.innerHTML = '<td style=""><a style="color: #FFF;" class="cut btn theme-bg gradient">-</a><a style="color: #FFF;" class="tr_clone_add4 btn theme-bg gradient">+</a><input style="border-color: #69b440;" contenteditable class="form-control service4" list="service4" autocomplete="off"></td>'+
							'<td style="text-align: center;"><span></span></td>'+
							'<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: center;" class="form-control pourcent" type="number"></td>'+
                            '<td class="colonneassurance"  style="display:none;"><input style="border-color: #69b440;text-align: right;" class="form-control plafond" type="number"></td>'+
							'<td style="text-align: right"><span>0</span><span data-prefix> FCFA</span></td>'+
							'<td style="" ><input style="border-color: #69b440;text-align: center" contenteditable class="form-control qte4" type="number" value="1"></td>'+
							'<td style="text-align: right;"><span>0</span><span data-prefix> FCFA</span></td>';

	return emptyColumn;
 }
 







$(document).on('change', '.service1', function(e) {
	
	var CellTarget=$(this)

	var Mydata = {
		"Nom":$(this).val()
		};
	$.ajax({  
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Service/GetPrice",
		dataType: "json",
		data: JSON.stringify(Mydata),  
		success: function (result) {  
			if (result.nom!=null) {
				//$(CellTarget).closest('td').next('td').find('span').text(result.nom);
				$(CellTarget).closest('td').next('td').find('span').text(result.cote);
				$(CellTarget).closest('td').next('td').next('td').next('td').find('input').first().val(result.montant);
				$(CellTarget).closest('td').next('td').next('td').next('td').next('td').find('span').first().text(SeparateurDeMillier(result.montant));
			}
			
			updateInvoice();
		},  
		error: function (err) {  
			alert("error"); 	
		}  
	});

	for (var a = document.querySelectorAll('table.inventory1 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('.service1');
		qtecells = a[i].querySelectorAll('.qte1');
		if(CellTarget.val()==cells[0].value && a.length>1 && CellTarget.parent().parent().index()!=i){
			qtecells[0].value=parseInt(qtecells[0].value)+1;
			CellTarget.parent().parent().remove();
			
		}
	}
});

$(document).on('change', '.service2', function(e) {
	
	var CellTarget=$(this)

	var Mydata = {
		"Nom":$(this).val()
		};
	$.ajax({  
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Examen/GetPrice",
		dataType: "json",
		data: JSON.stringify(Mydata),  
		success: function (result) {  
			if (result.nom!=null) {
				$(CellTarget).closest('td').next('td').find('span').text(result.cote);
				$(CellTarget).closest('td').next('td').next('td').next('td').find('input').first().val(result.montant);
				$(CellTarget).closest('td').next('td').next('td').next('td').next('td').find('span').first().text(SeparateurDeMillier(result.montant));
			}
			
			updateInvoice();
		},  
		error: function (err) {  
			alert("error"); 	
		}  
	});

	for (var a = document.querySelectorAll('table.inventory2 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('.service2');
		qtecells = a[i].querySelectorAll('.qte2');
		if(CellTarget.val()==cells[0].value && a.length>1 && CellTarget.parent().parent().index()!=i){
			qtecells[0].value=parseInt(qtecells[0].value)+1;
			CellTarget.parent().parent().remove();
			
		}
	}
});


$(document).on('change', '.service3', function(e) {
	
	var CellTarget=$(this)

	var Mydata = {
		"Nom":$(this).val()
		};
	$.ajax({  
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Service/GetPrice",
		dataType: "json",
		data: JSON.stringify(Mydata),  
		success: function (result) {  
			if (result.nom!=null) {
				$(CellTarget).closest('td').next('td').find('span').text(result.cote);
				$(CellTarget).closest('td').next('td').next('td').next('td').find('input').first().val(result.montant);
				$(CellTarget).closest('td').next('td').next('td').next('td').next('td').find('span').first().text(SeparateurDeMillier(result.montant));
			}
			
			updateInvoice();
		},  
		error: function (err) {  
			alert("error"); 	
		}  
	});

	for (var a = document.querySelectorAll('table.inventory3 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('.service3');
		qtecells = a[i].querySelectorAll('.qte3');
		if(CellTarget.val()==cells[0].value && a.length>1 && CellTarget.parent().parent().index()!=i){
			qtecells[0].value=parseInt(qtecells[0].value)+1;
			CellTarget.parent().parent().remove();
			
		}
	}
});

$(document).on('change', '.service4', function(e) {
	
	var CellTarget=$(this)

	var Mydata = {
		"Nom":$(this).val()
		};
	$.ajax({  
		type: "POST",
		contentType: "application/json; charset=utf-8",
		url: "/Stock/GetPrice",
		dataType: "json",
		data: JSON.stringify(Mydata),  
		success: function (result) {  
			if (result.nom_article!=null) {
				$(CellTarget).closest('td').next('td').find('span').text(result.nom_article);
				$(CellTarget).closest('td').next('td').next('td').next('td').find('input').first().val(result.prix_vente);
				$(CellTarget).closest('td').next('td').next('td').next('td').next('td').find('span').first().text(SeparateurDeMillier(result.prix_vente));

			}
			
			updateInvoice();
		},  
		error: function (err) {  
			alert("error"); 	
		}  
	});

	for (var a = document.querySelectorAll('table.inventory4 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('.service4');
		qtecells = a[i].querySelectorAll('.qte4');
		if(CellTarget.val()==cells[0].value && a.length>1 && CellTarget.parent().parent().index()!=i){
			qtecells[0].value=parseInt(qtecells[0].value)+1;
			CellTarget.parent().parent().remove();
			
		}
	}
});



$(document).on('keyup', '.qte1', function(e) {
	updateInvoice();
});
$(document).on('keyup', '.qte2', function(e) {
	updateInvoice();
});
$(document).on('keyup', '.qte3', function(e) {
	updateInvoice();
});
$(document).on('keyup', '.qte4', function(e) {
	updateInvoice();
});
$(document).on('keyup', '.plafond', function(e) {
	updateInvoice();
});
$(document).on('keyup', '.pourcent', function(e) {
	updateInvoice();
});

// $(document).on('keydown', '.service1', function(e) {
	
// 	$(this).val("");
// 	$(this).closest('td').next('td').find('span').text("");
// 	$(this).closest('td').next('td').next('td').next('td').find('input').first().val("");
// 	$(this).closest('td').next('td').next('td').next('td').next('td').find('span').first().text("");
// 	$(this.ancestorQuerySelector('tr')).remove();
// 	updateInvoice();
// });








// $(window).on('load', GetDataList());
      
// function GetDataList() {
          
// }



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







/* Shivving (IE8 is not supported, but at least it won't look as awful)
/* ========================================================================== */

(function (document) {
	var
	head = document.head = document.getElementsByTagName('head')[0] || document.documentElement,
	elements = 'article aside audio bdi canvas data datalist details figcaption figure footer header hgroup mark meter nav output picture progress section summary time video x'.split(' '),
	elementsLength = elements.length,
	elementsIndex = 0,
	element;

	while (elementsIndex < elementsLength) {
		element = document.createElement(elements[++elementsIndex]);
	}

	element.innerHTML = 'x<style>' +
		'article,aside,details,figcaption,figure,footer,header,hgroup,nav,section{display:block}' +
		'audio[controls],canvas,video{display:inline-block}' +
		'[hidden],audio{display:none}' +
		'mark{background:#FF0;color:#000}' +
	'</style>';

	return head.insertBefore(element.lastChild, head.firstChild);
})(document);

/* Prototyping
/* ========================================================================== */

(function (window, ElementPrototype, ArrayPrototype, polyfill) {
	function NodeList() { [polyfill] }
	NodeList.prototype.length = ArrayPrototype.length;

	ElementPrototype.matchesSelector = ElementPrototype.matchesSelector ||
	ElementPrototype.mozMatchesSelector ||
	ElementPrototype.msMatchesSelector ||
	ElementPrototype.oMatchesSelector ||
	ElementPrototype.webkitMatchesSelector ||
	function matchesSelector(selector) {
		return ArrayPrototype.indexOf.call(this.parentNode.querySelectorAll(selector), this) > -1;
	};

	ElementPrototype.ancestorQuerySelectorAll = ElementPrototype.ancestorQuerySelectorAll ||
	ElementPrototype.mozAncestorQuerySelectorAll ||
	ElementPrototype.msAncestorQuerySelectorAll ||
	ElementPrototype.oAncestorQuerySelectorAll ||
	ElementPrototype.webkitAncestorQuerySelectorAll ||
	function ancestorQuerySelectorAll(selector) {
		for (var cite = this, newNodeList = new NodeList; cite = cite.parentElement;) {
			if (cite.matchesSelector(selector)) ArrayPrototype.push.call(newNodeList, cite);
		}

		return newNodeList;
	};

	ElementPrototype.ancestorQuerySelector = ElementPrototype.ancestorQuerySelector ||
	ElementPrototype.mozAncestorQuerySelector ||
	ElementPrototype.msAncestorQuerySelector ||
	ElementPrototype.oAncestorQuerySelector ||
	ElementPrototype.webkitAncestorQuerySelector ||
	function ancestorQuerySelector(selector) {
		return this.ancestorQuerySelectorAll(selector)[0] || null;
	};
})(this, Element.prototype, Array.prototype);

/* Helper Functions
/* ========================================================================== */

function parseFloatHTML(element) {
	return parseInt(element.innerHTML.replace(/\s/g, '')) || 0;
}

function parsePrice(number) {
	return number.toFixed(2).replace(/(\d)(?=(\d\d\d)+([^\d]|$))/g, '$1,');
}

/* Update Number
/* ========================================================================== */

function updateNumber(e) {
	var
	activeElement = document.activeElement,
	value = parseFloat(activeElement.innerHTML),
	wasPrice = activeElement.innerHTML == parsePrice(parseFloatHTML(activeElement));

	if (!isNaN(value) && (e.keyCode == 38 || e.keyCode == 40 || e.wheelDeltaY)) {
		e.preventDefault();

		value += e.keyCode == 38 ? 1 : e.keyCode == 40 ? -1 : Math.round(e.wheelDelta * 0.025);
		value = Math.max(value, 0);

		activeElement.innerHTML = wasPrice ? parsePrice(value) : value;
	}

	//updateInvoice();
}

/* Update Invoice
/* ========================================================================== */

function updateInvoice() {
	var total_ht = 0;
	var cells, price,unitprice, total, a, i;
	var montant_assur_temp = 0;
	var montant_assur = 0;
	var montant_patient_temp = 0;
	var montant_patient = 0;

	// update inventory cells
	// ======================

	for (var a = document.querySelectorAll('table.inventory1 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('span:first-child');
		qtecells = a[i].querySelectorAll('input');
		
		cells[2].innerHTML = SeparateurDeMillier((parseInt(cells[1].innerHTML.replace(/\s/g, ''))* qtecells[3].value).toString());
	    price = parseInt(cells[2].innerHTML.replace(/\s/g, ''));
		unitprice=parseInt(cells[1].innerHTML.replace(/\s/g, ''));

		
		// add price to total
		total_ht += price;
		montant_assur_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*qtecells[1].value*qtecells[3].value):(0.01*qtecells[1].value*qtecells[2].value*qtecells[3].value);
		montant_assur +=montant_assur_temp;
		
		montant_patient_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*(100-qtecells[1].value)*qtecells[3].value):(((0.01*(100-qtecells[1].value)*qtecells[2].value)+(unitprice-qtecells[2].value))*qtecells[3].value);
		montant_patient +=montant_patient_temp;
		console.log(montant_patient_temp)
		
	}
	for (var a = document.querySelectorAll('table.inventory2 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('span:first-child');
		qtecells = a[i].querySelectorAll('input');
		
		cells[2].innerHTML = SeparateurDeMillier((parseInt(cells[1].innerHTML.replace(/\s/g, ''))* qtecells[3].value).toString());
	    price = parseInt(cells[2].innerHTML.replace(/\s/g, ''));
		unitprice=parseInt(cells[1].innerHTML.replace(/\s/g, ''));

		
		// add price to total
		total_ht += price;
		montant_assur_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*qtecells[1].value*qtecells[3].value):(0.01*qtecells[1].value*qtecells[2].value*qtecells[3].value);
		montant_assur +=montant_assur_temp;
		
		montant_patient_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*(100-qtecells[1].value)*qtecells[3].value):(((0.01*(100-qtecells[1].value)*qtecells[2].value)+(unitprice-qtecells[2].value))*qtecells[3].value);
		montant_patient +=montant_patient_temp;
		console.log(montant_patient_temp)
		
	}
	for (var a = document.querySelectorAll('table.inventory3 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('span:first-child');
		qtecells = a[i].querySelectorAll('input');
		
		cells[2].innerHTML = SeparateurDeMillier((parseInt(cells[1].innerHTML.replace(/\s/g, ''))* qtecells[3].value).toString());
	    price = parseInt(cells[2].innerHTML.replace(/\s/g, ''));
		unitprice=parseInt(cells[1].innerHTML.replace(/\s/g, ''));

		
		// add price to total
		total_ht += price;
		montant_assur_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*qtecells[1].value*qtecells[3].value):(0.01*qtecells[1].value*qtecells[2].value*qtecells[3].value);
		montant_assur +=montant_assur_temp;
		
		montant_patient_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*(100-qtecells[1].value)*qtecells[3].value):(((0.01*(100-qtecells[1].value)*qtecells[2].value)+(unitprice-qtecells[2].value))*qtecells[3].value);
		montant_patient +=montant_patient_temp;
		console.log(montant_patient_temp)
	}
	for (var a = document.querySelectorAll('table.inventory4 tbody tr'), i = 0; a[i]; ++i) {
		// get inventory row cells
		cells = a[i].querySelectorAll('span:first-child');
		qtecells = a[i].querySelectorAll('input');
		
		cells[2].innerHTML = SeparateurDeMillier((parseInt(cells[1].innerHTML.replace(/\s/g, ''))* qtecells[3].value).toString());
	    price = parseInt(cells[2].innerHTML.replace(/\s/g, ''));
		unitprice=parseInt(cells[1].innerHTML.replace(/\s/g, ''));

		
		// add price to total
		total_ht += price;
		montant_assur_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*qtecells[1].value*qtecells[3].value):(0.01*qtecells[1].value*qtecells[2].value*qtecells[3].value);
		montant_assur +=montant_assur_temp;
		
		montant_patient_temp=(qtecells[2].value>=unitprice)?(0.01*unitprice*(100-qtecells[1].value)*qtecells[3].value):(((0.01*(100-qtecells[1].value)*qtecells[2].value)+(unitprice-qtecells[2].value))*qtecells[3].value);
		montant_patient +=montant_patient_temp;
		console.log(montant_patient_temp)
		
	}

	
	
	$('#total_ttc').text(SeparateurDeMillier(parseInt(montant_patient+montant_assur).toString()));
	$('#total_ht').text(SeparateurDeMillier(parseInt(montant_patient+montant_assur).toString()));
	$('.total_patient').text(SeparateurDeMillier(parseInt(montant_patient).toString()));
	$('#montant_lettre').text(NumberToLetter(parseInt(montant_patient+montant_assur))+" FCFA");
	$('#esp').text(SeparateurDeMillier((parseInt(montant_patient)).toString()));
	$('.tot').text(SeparateurDeMillier(parseInt(montant_patient+montant_assur).toString()));
	$('.total_ass').text(SeparateurDeMillier((parseInt(montant_assur)).toString()));
	$('#totalassurance').val(parseInt(montant_assur));
	$('#montant_lettre_assurance').val(NumberToLetter(parseInt(montant_assur))+" FCFA");
	$('#montant_lettre_patient').val(NumberToLetter(parseInt(montant_patient))+" FCFA");

	
	//$('#net_a_payer').val(parseInt((total_ht-(total_ht*$('#pr_ass').val()/100))));montant_patient

	
}

/* On Content Load
/* ========================================================================== */

function onContentLoad() {
	

	if ($("#target").val().includes("assurance")) {
		$( "#assur" ).show();
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
		updateInvoice();
	
	}else{
		$( "#assur" ).hide();
		$( ".colonneassurance" ).hide();
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
		updateInvoice();
	}


		

	


	function onEnterCancel(e) {
		e.preventDefault();

		image.classList.add('hover');
	}

	function onLeaveCancel(e) {
		e.preventDefault();

		image.classList.remove('hover');
	}

	function onFileInput(e) {
		image.classList.remove('hover');

		var
		reader = new FileReader(),
		files = e.dataTransfer ? e.dataTransfer.files : e.target.files,
		i = 0;

		reader.onload = onFileLoad;

		while (files[i]) reader.readAsDataURL(files[i++]);
	}

	function onFileLoad(e) {
		var data = e.target.result;

		image.src = data;
	}

	if (window.addEventListener) {
		// document.addEventListener('click', onClick);

		// document.addEventListener('mousewheel', updateNumber);
		// document.addEventListener('keydown', updateNumber);

		// document.addEventListener('keydown', updateInvoice);
		// document.addEventListener('keyup', updateInvoice);

		// input.addEventListener('focus', onEnterCancel);
		// input.addEventListener('mouseover', onEnterCancel);
		// input.addEventListener('dragover', onEnterCancel);
		// input.addEventListener('dragenter', onEnterCancel);

		// input.addEventListener('blur', onLeaveCancel);
		// input.addEventListener('dragleave', onLeaveCancel);
		// input.addEventListener('mouseout', onLeaveCancel);

		// input.addEventListener('drop', onFileInput);
		// input.addEventListener('change', onFileInput);
	}

	

}

// save facture
$(document).on('click', '#saveFacture', function(e) {
	updateInvoice();

			var chaine1="λ";
			var chaine2="λ";
			var chaine3="λ";
			var chaine4="λ";
			//creation de parsseur lamda

			for (var a = document.querySelectorAll('table.inventory1 tbody tr'), i = 0; a[i]; ++i) {
				cells = a[i].querySelectorAll('span:first-child');
				moninput=a[i].querySelectorAll('input');
				chaine1+=moninput[0].value+"λ"+cells[0].innerHTML+"λ"+moninput[1].value+"λ"+moninput[2].value+"λ"+cells[1].innerHTML+"λ"+moninput[3].value+"λ"+cells[2].innerHTML+"λ";
			}
			for (var a = document.querySelectorAll('table.inventory2 tbody tr'), i = 0; a[i]; ++i) {
				cells = a[i].querySelectorAll('span:first-child');
				moninput=a[i].querySelectorAll('input');
				chaine2+=moninput[0].value+"λ"+cells[0].innerHTML+"λ"+moninput[1].value+"λ"+moninput[2].value+"λ"+cells[1].innerHTML+"λ"+moninput[3].value+"λ"+cells[2].innerHTML+"λ";
			}
			for (var a = document.querySelectorAll('table.inventory3 tbody tr'), i = 0; a[i]; ++i) {
				cells = a[i].querySelectorAll('span:first-child');
				moninput=a[i].querySelectorAll('input');
				chaine3+=moninput[0].value+"λ"+cells[0].innerHTML+"λ"+moninput[1].value+"λ"+moninput[2].value+"λ"+cells[1].innerHTML+"λ"+moninput[3].value+"λ"+cells[2].innerHTML+"λ";
			}
			for (var a = document.querySelectorAll('table.inventory4 tbody tr'), i = 0; a[i]; ++i) {
				cells = a[i].querySelectorAll('span:first-child');
				moninput=a[i].querySelectorAll('input');
				chaine4+=moninput[0].value+"λ"+cells[0].innerHTML+"λ"+moninput[1].value+"λ"+moninput[2].value+"λ"+cells[1].innerHTML+"λ"+moninput[3].value+"λ"+cells[2].innerHTML+"λ";
			}

			//collecte des données
			var trump0_1=chaine1;
			var trump0_2=chaine2;
			var trump0_3=chaine3;
			var trump0_4=chaine4;
			var trump1=document.getElementById("montant_lettre").innerHTML;
			var trump1_0=document.getElementById("montant_lettre_assurance").value;
			var trump1_1=document.getElementById("montant_lettre_patient").value;
			var trump2=document.getElementById("prescripteur").value;
			var trump3=document.getElementById("numero_dossier").value;
			//var trump4=document.getElementById("target").value;
			var trump5=document.getElementById("assureur").value;
			//var trump6=isNaN(parseInt(document.getElementById("pr_ass").value))?0:parseInt(document.getElementById("pr_ass").value);
			//var trump7=isNaN(parseInt(document.getElementById("pourcentage_frais").value))?0:parseInt(document.getElementById("pourcentage_frais").value) ;
			var trump8=document.getElementById("total_ht").innerHTML;
			var trump9=isNaN(parseInt(document.getElementById("totalassurance").value))?0:parseInt(document.getElementById("totalassurance").value);
			var trump10=document.getElementById("total_ttc").innerHTML;
			var trump16=document.getElementById("tot_patient").innerHTML;
			var trump11=document.getElementById("identifiant").value;
			var trump12=document.getElementById("titre1").value;
			var trump13=document.getElementById("titre2").value;
			var trump14=document.getElementById("titre3").value;
			var trump15=document.getElementById("titre4").value;
			
			

			var facture = {
				
				"Id": parseInt(trump11),
				"Montant_en_lettre": trump1,
				"Montant_en_lettre_patient": trump1_1,
				"Montant_en_lettre_assurance": trump1_0,
				"Medecin": trump2,
				"Numero_dossier": trump3,
				"Moyen_paiement": "",
				"Assureur": trump5,
				"Pourcentage_Assurance":0,
				"Net_a_payer_assurance": parseInt(trump9),
				"frais_retrait": 0,
				"Nombre_affichage": 0,
				"Total_ht": parseInt(trump8.replace(/\s/g,'')),
				"Total_ttc": parseInt(trump8.replace(/\s/g,'')),
				"Ligne_facturation1": trump0_1,
				"Ligne_facturation2": trump0_2,
				"Ligne_facturation3": trump0_3,
				"Ligne_facturation4": trump0_4,
				"Montant_recu_patient": 0,
				"Especes": 0,
				"Montant_recu_assurance": 0,
				"Net_a_payer_patient": parseInt(trump16.replace(/\s/g,'')),
				"Tva": 0,
				"Titre1": trump12,
				"Titre2": trump13,
				"Titre3": trump14,
				"Titre4": trump15,
				"Type": $("#target").val(),
				"Matricule_patient": $("#a1").val(),
				"MatriculeADH": $("#a2").val(),
				"Societe": $("#a5").val(),
				"assure_prin": $("#a4").val(),
				"Date_entree": new Date($("#a3").val()),
				"Date_sortie": new Date($("#a6").val())
				

			};
			if (facture.Medecin=="" || facture.Medecin==null || (facture.Assureur=="" && $("#target").val().includes("assurance"))) {
				console.log(facture)
			}else{
				$.ajax({
					type: "POST",
					contentType: "application/json; charset=utf-8",
					url: "/Facture/SaveFacture",
					dataType: "json",
					data: JSON.stringify(facture),
					success: function (result) {
						if ($("#target").val().includes("medicament")) {
							location.href = "/Facture/Index3/";
						}else{
							location.href = "/Facture/Index/";
						}
						
					},
					error: function (response) { 
							
					}
				});
			}

			

					

});







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


window.addEventListener && document.addEventListener('DOMContentLoaded', onContentLoad);
