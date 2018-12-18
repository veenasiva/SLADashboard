$(window).on('load', function () {
	var addProfileForm = document.getElementById('AddProfileForm');
	var url = $("#btnAddProfile").attr('actionurl');
    if (addProfileForm !== null) {

        var objAddProfile = {
            NewProfileName: "",
            NewProfileDescription: "",
			NewProfileIDPrefix: "",
            submitAddProfile: function () {
                var data = {
                     Client: { ID: $('#Client_ID').val() } ,
                    NewProfileName: $('#profileNameTextBox').val(),
					NewProfileDescription: $('#profileDescriptionTextBox').val(),
					NewProfileIDPrefix: $('#profileIDPrefixTextBox').val()
				};
				var errDisplay = document.getElementsByClassName('field-validation-valid text-danger');
				errDisplay[0].innerHTML = '';
				errDisplay[1].innerHTML = '';
				errDisplay[2].innerHTML = '';
                $.ajax({
                    type: "POST",
                    url: url,
                    data: { profileData: ko.toJSON(data) },
                    success: function (response) {
						var errDisplayField = document.getElementById(response.ErrorKey);
						if (response.Success) {
							var data = $.parseJSON(response.SuccessMessage);
                            $('#profileNameTextBox').val('');
							$('#profileDescriptionTextBox').val('');
							$('#profileIDPrefixTextBox').val('');
                            $('#profilesList').append('<div id=' + data["ID"] + ' class="row profile-listing" onclick="HandleProfileClick(this)">'
                                + '<div class= "col-md-3">'
                                + '<div class="profile-image"><img src="/Content/Images/Client_Profile.png" /></div>'
                                + '</div>'
                                + '<div class="col-md-9">'
                                + '<div class="desc">'
                                + '<p class="title"><span>Name: <strong>' + data["Name"] + '</strong></span></p>'
								+ '<span>Description: <strong>' + data["Description"] + '</strong></span>'
								+ '<br /><span>SLA ID Prefix: ' + data["SLAIDPrefix"] + '</span>'
                                + '</div></div></div>');

                            let element = document.getElementById(data["ID"]);

                            element.addEventListener("click", function () {
                                HandleProfileClick(this);
                            });

                            element.addEventListener("mouseover", function () {
                                $("#" + data["ID"]).addClass('profile-glow');
                            });

                            element.addEventListener("mouseout", function () {
                                $("#" + data["ID"]).removeClass('profile-glow');
                            });
                        }
						else {
							
                            //errDisplay[0].innerHTML = response.ErrorMessage;
							errDisplayField.innerHTML = response.ErrorMessage;
                        }
                    }
                });
            }
        };

        ko.cleanNode(addProfileForm);
        ko.applyBindings(objAddProfile, addProfileForm);
    }
});
