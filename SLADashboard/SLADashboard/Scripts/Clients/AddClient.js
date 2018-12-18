$(window).on('load', function () {
	var addClientForm = document.getElementById('AddClientForm');
	var url = $("#btnAddClient").attr('actionurl');
    if (addClientForm !== null) {
        var objAddClient = {

            NewClientName: "",

            submitAddClient: function () {
                var data = {
                    NewClientName: $('#clientNameTextBox').val(),
                };
                $.ajax({
                    type: "POST",
                    url: url,
                    data: { clientData: ko.toJSON(data) },
                    success: function (response) {

                        if (response.Success) {
                            var data = $.parseJSON(response.SuccessMessage);
                            var clientsObj = { ID: data["ID"], Name: data["Description"] };
                            AddClientToSelectFromList(clientsObj);
                            $('#clientNameTextBox').val('');
                            var errDisplay = document.getElementsByClassName('field-validation-valid text-danger');
                            errDisplay[0].innerHTML = ''; //Clear any previous error message
                        }
                        else {
                            var errDisplay = document.getElementsByClassName('field-validation-valid text-danger');
                            errDisplay[0].innerHTML = response.ErrorMessage;
                        }
                    }
                });
            }   
        };

        ko.cleanNode(addClientForm);
        ko.applyBindings(objAddClient, addClientForm);
    }     
});



function ClientMouseOverJS(e) {
	var clientId = e.getAttribute('data-id');
	$('#Client-' + clientId + ' li-icon').remove();
	$('#Client-' + clientId + ' span.artdeco-pill-icons').append('<li-icon aria-hidden="true" type="check-icon" size="small"><svg viewBox="0 0 24 24" width="24px" height="24px" x="0" y="0" preserveAspectRatio="xMinYMin meet" class="artdeco-icon" focusable="false"><path d="M15,3L6.57,13.72A0.7,0.7,0,0,1,6,14a0.72,0.72,0,0,1-.56-0.27L1,8.07,2.36,7,6,11.72,13.68,2Z" class="small-icon" style="fill-opacity: 1"></path></svg></li-icon>');
}

function ClientMouseOutJS(e) {
	var clientId = e.getAttribute('data-id');
	$('#Client-' + clientId + ' li-icon').remove();
	$('#Client-' + clientId + ' span.artdeco-pill-icons').append('<li-icon aria-hidden="true" type="plus-icon" size="small"><svg viewBox="0 0 24 24" width="24px" height="24px" x="0" y="0" preserveAspectRatio="xMinYMin meet" class="artdeco-icon" focusable="false"><path d="M14,9H9v5H7V9H2V7H7V2H9V7h5V9Z" class="small-icon" style="fill-opacity: 1"></path></svg></li-icon>');
}





