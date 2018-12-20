"use strict";

(function (window, document) {

    var baseUrl = document.getElementById('baseUrl').value;

    var nameInput = document.getElementById("Name");
    var nameHeader = document.getElementById("NameHeader");
    var descriptionInput = document.getElementById("Description");
    var trainingRateInput = document.getElementById("TrainingRate");

    var input1Input = document.getElementById("Input1");
    var input2Input = document.getElementById("Input2");
    var outputInput = document.getElementById("Output");

    var path = window.location.pathname.split("/");
    var paramId = path[path.length - 1];

    document.getElementById("editButton").onclick = function () {
        var isValid = true;
        if (!nameInput.value) {
            document.getElementById('NameValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('NameValidation').classList.add("hidden");
        }
        if (!trainingRateInput.value) {
            document.getElementById('TrainingRateValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('TrainingRateValidation').classList.add("hidden");
            if (trainingRateInput.value <= 0) {
                document.getElementById('TrainingRateTooLowValidation').classList.remove("hidden");
                isValid = false;
            } else {
                document.getElementById('TrainingRateValidation').classList.add("hidden");
            }
        }

        if (isValid) {
            var body =
            {
                id: paramId,
                name: nameInput.value,
                description: descriptionInput.value,
                trainingRate: trainingRateInput.value
            }
            fetch(baseUrl + "/api/NeuralNet",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    body: JSON.stringify(body)
                }).then(response => {
                    if (response.status == 200) {
                        window.location.reload();
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }

    document.getElementById("propagateButton").onclick = function () {
        var isValid = true;
        if (!input1Input.value) {
            document.getElementById('Input1Validation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('Input1Validation').classList.add("hidden");
        }
        if (!input2Input.value) {
            document.getElementById('Input2Validation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('Input2Validation').classList.add("hidden");
        }

        if (isValid) {
            fetch(baseUrl + "/api/NeuralNet/" + paramId + "/Propagate?input=" + input1Input.value + "&input=" + input2Input.value).then(response => {
                    if (response.status == 200) {
                        response.json().then(array => {
                            outputInput.value = array[0];
                        })
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }


    fetch(baseUrl + "/api/NeuralNet/" + paramId).then(response => {
        if (response.status == 200) {
            response.json().then(body => {
                console.log(body);
                nameInput.value = body.name;
                descriptionInput.value = body.description;
                trainingRateInput.value = body.trainingRate;
                nameHeader.innerHTML = body.name;
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));
