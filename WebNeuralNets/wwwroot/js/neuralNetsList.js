﻿(function (window, document) {

    var baseUrl = document.getElementById('baseUrl').value;
    var table = document.getElementById('tableBody');
    var nameInput = document.getElementById("Name");
    var descriptionInput = document.getElementById("Description");
    var trainingrateInput = document.getElementById("TrainingRate");

    function removeNeuralNetFunctionFactory(id) {
        return function () {
            fetch(baseUrl + "/api/NeuralNet?id=" + id,
                {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    }
                }).then(response => {
                    if (response.status == 204) {
                        document.location.reload();
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }

    function trainNeuralNetFunctionFactory(id) {
        return function () {
            fetch(baseUrl + "/api/NeuralNet/" + id + "/Trains",
                {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    },
                    body: {}
                }).then(response => {
                    if (response.status == 200) {
                        document.location.reload();
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }

    function generateButtonsCell(id) {
        var cell = document.createElement("td");
        var removeButton = document.createElement("button");
        removeButton.classList.add("btn");
        removeButton.classList.add("btn-danger");
        removeButton.innerHTML = "Usuń";
        removeButton.onclick = removeNeuralNetFunctionFactory(neuralNet.id);

        return cell;
    }


    document.getElementById("addButton").onclick = function () {
        var isValid = true;
        if (!nameInput.value) {
            document.getElementById('NameValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('NameValidation').classList.add("hidden");
        }
        if (!trainingrateInput.value) {
            document.getElementById('TrainingRateValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('TrainingRateValidation').classList.add("hidden");
        }

        if (isValid) {
            var name = nameInput.value;
            var description = descriptionInput.value;
            var trainingRate = trainingrateInput.value;

            fetch(baseUrl + "/api/NeuralNet?name=" + name + "&traningRate=" + trainingRate + (description ? ("&description=" + description) : ""),
                {
                    method: "PUT",
                    headers: {
                        "Content-Type": "application/json; charset=utf-8"
                    }
                }).then(response => {
                    if (response.status == 200) {
                        window.location.reload();
                    }
                }).catch(err => {
                    console.error(err);
                });
        }
    }


    fetch(baseUrl + "/api/NeuralNet/").then(response => {
        if (response.status == 200) {
            response.json().then(body => {
                console.log(body);
                for (var i = 0; i < body.length; i++) {
                    var neuralNet = body[i];
                    var row = table.insertRow(i);
                    row.insertCell(0).appendChild(document.createTextNode(neuralNet.id));
                    row.insertCell(1).appendChild(document.createTextNode(neuralNet.name));
                    row.insertCell(2).appendChild(document.createTextNode(neuralNet.description ? neuralNet.description : ''));
                    row.insertCell(3).appendChild(document.createTextNode(neuralNet.trainingRate));
                    row.insertCell(4).appendChild(document.createTextNode(neuralNet.layersCount));
                    row.insertCell(5).appendChild(document.createTextNode(neuralNet.neuronsCount));
                    row.insertCell(6).appendChild(document.createTextNode(neuralNet.training));
                    row.insertCell(7).appendChild(document.createTextNode(neuralNet.trainingIterations));
                    row.appendChild(generateButtonsCell());
                }
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));