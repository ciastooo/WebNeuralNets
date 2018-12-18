(function (window, document) {
    var loginInput = document.getElementById("Username");
    var passwordInput = document.getElementById("Password");
    var baseUrl = document.getElementById('baseUrl').value;

    document.getElementById("submit").onclick = function () {
        var isValid = true;
        if (!loginInput.value) {
            document.getElementById('UsernameValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('UsernameValidation').classList.add("hidden");
        }
        if (!passwordInput.value) {
            document.getElementById('PasswordValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('PasswordValidation').classList.add("hidden");
            if (passwordInput.value.length < 6) {
                document.getElementById('PasswordValidationLength').classList.remove("hidden");
                isValid = false;
            } else {
                document.getElementById('PasswordValidationLength').classList.add("hidden");
            }
        }

        if (isValid) {
            console.log("OK");
            var postBody = {
                username: loginInput.value,
                password: passwordInput.value
            }
            fetch(baseUrl + "/api/Account/Login",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    body: JSON.stringify(postBody)
                }).then(response => {
                    if (response.status == 200) {
                        window.location.replace(baseUrl + "/Home");
                    } else {
                        document.getElementById("loginFailed").classList.remove("hidden");
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }

}(this, this.document));
