
var selectedId = 0;

$(document).ready(function () {
	$("#deleteForm").hide();
});
//jQueryUI method to create dialog box
$("#createForm").dialog({
	autoOpen: false,
	modal: true,
	width: 650,
	title: "New SLA Item"
});
//jQueryUI method to create dialog box
$("#editForm").dialog({
	autoOpen: false,
	modal: true,
	width: 650,
	title: "Edit Selected SLA Item"
});
//jQueryUI method to create dialog box
$("#deleteForm").dialog({
	autoOpen: false,
	modal: true,
	title: "Delete Selected SLA"
});

$(".buttonCreate").button().click(function () {
	let clientId = $('#SelectedClient').val();
	let profileId = $('#SelectedProfile').val();
	var url = $("#btnAdd").attr('request-url');
	$.ajax({
		// Call CreatePartialView action method
		url: url ,
		type: 'Get',
		data: {
			clientId: clientId,
			profileID: profileId
		},
		success: function (data) {
			$("#createForm").dialog("open");
			$("#createForm").empty().append(data);
			$("#editForm").hide();
		},
		error: function () {
			alert("something seems wrong");
		}
	});
});

function loadDatabases(profileId, url) {
	var token = $("#__AjaxAntiForgeryForm input").val();
	var dataObject = {
		__RequestVerificationToken: token
	};
	
	$.ajax({
		url: url ,
		type: 'POST',
		data: {
			profileID: profileId, __RequestVerificationToken: token
		},
		headers: dataObject,
		success: function (data) {
			$('#partial_SLADefinition').html(data);
		},
		failure: function (response) {
			alert(response);
		},
		error: function (request, status, error) {
			alert(status + ' ' + error + ' ' + request.responseText);
		}
	});
}


$(".buttonEdit").button().click(function () {
	// Get the Id if selected sla and assign in selectedId variable 
	//alert($(this).closest("tr").find("td:nth-child(1)").text())
	var selectedId = $(this).closest("tr").find("td:nth-child(1)").text();
	var url = $("#btnEdit").attr('request-url');
	$.ajax({
		// Call EditPartialView action method
		url: url,
		data: { id: selectedId },
		type: 'Get',
		success: function (msg) {
			$("#editForm").dialog("open");
			$("#editForm").empty().append(msg);
			$("#createForm").hide();
		},
		error: function () {
			alert("something seems wrong");
		}
	});
});



$(".buttonDelete").button().click(function () {
	//Get the slaId
	selectedId = $(this).closest("tr").find("td:nth-child(1)").text();
	//update div tag
	var selectedName = $(this).closest("tr").find("td:nth-child(2)").text();
	$('#SLAIDToDelete').text(selectedName);
	//Open the dialog box
	$("#deleteForm").dialog("open");

});

$(".okDelete").button().click(function (e) {
	//disable the button so that its not firing the event twice
	$('#okDelete').attr('disabled', 'disabled');
	// Close the dialog box on Yes button is clicked
	$("#deleteForm").dialog("close");
	var url = $("#btnDelete").attr('request-url');
	var partialviewurl = $("#btnDelete").attr('partialview-url');
	let profileId = $('#SelectedProfile').val();
	
	$.ajax({
		// Call Delete action method
		url: url,
		data: { id: selectedId },
		type: 'Get',
		success: function (response) {
			$('#okDelete').removeAttr('disabled');
			if (response.Success) {
				loadDatabases(profileId, partialviewurl);
			}
			else {
				alert(response.ErrorMessage);
			}
		},
		error: function (response) {
			alert("something seems wrong");

		}
	});
	e.stopImmediatePropagation();
});


