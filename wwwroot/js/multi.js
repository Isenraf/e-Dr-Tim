$(document).ready(function() {

	$(".js-select2-multi").select2();
  
	$(".js-select2-multi").select2({
	  closeOnSelect: false,
	  scrollAfterSelect: true
	});
  
	$("#checkbox").click(function(){
	  if($("#checkbox").is(':checked') ){
		
		$(".js-select2-multi > option").prop("selected","selected");
		$(".js-select2-multi").trigger("change");
	  }else{
		$(".js-select2-multi > option").removeAttr("selected");
		$(".js-select2-multi").trigger("change");
	  }
	});

	$(".js-select3-multi").select2({
		closeOnSelect: false,
		scrollAfterSelect: true
	  });
	
	  $("#checkbox2").click(function(){
		if($("#checkbox").is(':checked') ){
		  $(".js-select3-multi > option").prop("selected","selected");
		  $(".js-select3-multi").trigger("change");
		}else{
		  $(".js-select3-multi > option").removeAttr("selected");
		  $(".js-select3-multi").trigger("change");
		}
	  });

  });
  
  