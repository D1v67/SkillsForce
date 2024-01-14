$(document).ready(function () {

    $("#loginForm").submit(function (e) {
        e.preventDefault();

        $(".error-message").remove();
        $(".form-control").removeClass("is-invalid");

        var emailAddress = $("#emailAddress").val();
        var password = $("#password").val();

        // Validate Email Address
        if (!emailAddress) {
            $("#emailAddress").addClass("is-invalid");
            $("#emailAddress").after('<div class="invalid-feedback error-message">Email is required.</div>');
        }

        // Validate Password
        if (!password) {
            $("#password").addClass("is-invalid");
            $("#password").after('<div class="invalid-feedback error-message">Password is required.</div>');
        }

        if (!emailAddress || !password) {
            return;
        }

        var authObj = { Email: emailAddress, Password: password };

        // Show spinner while waiting for the response
        //   $(".spinner-wrapper").show();


        $.ajax({
            type: "POST",
            url: "/Account/Authenticate",
            data: authObj,
            dataType: "json",
            success: function (response) {
                try {
                    if (response.result) {

                        setTimeout(function () {
                            //$(".spinner-wrapper").hide();
                            toastr.success("Login Successful...Redirecting to your Home page...");
                            setTimeout(function () {
                                window.location.href = response.url;
                            }, 500);
                        }, 500);

                    } else {
                        toastr.error('Invalid Email or Password');
                        // $(".spinner-wrapper").hide();
                    }
                } catch (error) {
                    console.log(error)
                    toastr.error('aaaaaaaaaaaaa');
                }

            },
            error: function (xhr, textStatus, errorThrown) {
                console.log("Error:", xhr);
                console.log("Status:", textStatus);
                console.log("Error Thrown:", errorThrown);

                toastr.error('Something went wrong. Please try again later.');

                // $(".spinner-wrapper").hide();
            },
            //complete: function () {
            //    // Hide spinner after a delay of 2 seconds
            //    setTimeout(function () {
            //        $(".spinner-wrapper").hide();
            //    }, 1000);
            //}
        });
    });

    $("#btnRegister").click(function () {
        window.location = '/Account/Register';
    });

    // Show Password functionality
    $("#showPassword").click(function () {
        var passwordInput = $("#password");
        var passwordFieldType = passwordInput.attr("type");
        passwordInput.attr("type", passwordFieldType === "password" ? "text" : "password");
    });


});