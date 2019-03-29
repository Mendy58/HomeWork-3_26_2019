$(() => {
	console.log(`Hi`);
	$("#new-simcha").click(function () {
	//	$("#new-simcha-modal").prop("display", block);
	//	$("#new-simcha-modal").prop("aria-hidden", false)
	  $("#new-simcha-modal").modal('show');
	});
})