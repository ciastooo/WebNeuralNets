(function (window, document) {

    var baseUrl = document.getElementById('baseUrl').value;

    var idInput = document.getElementById("id");
    var nameInput = document.getElementById("Name");
    var nameHeader = document.getElementById("NameHeader");
    var descriptionInput = document.getElementById("Description");
    var trainingRateInput = document.getElementById("TrainingRate");

    var input1Input = document.getElementById("Input1");
    var input2Input = document.getElementById("Input2");

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
                id: idInput.value,
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

    fetch(baseUrl + "/api/NeuralNet/" + paramId).then(response => {
        if (response.status == 200) {
            response.json().then(body => {
                console.log(body);
                idInput.value = body.id;
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
