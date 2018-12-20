"use strict";

(function (window, document) {

    var baseUrl = document.getElementById('baseUrl').value;
    var table = document.getElementById('tableBody');
    var nameInput = document.getElementById("Name");
    var input1Input = document.getElementById("Input1");
    var input2Input = document.getElementById("Input2");
    var outputInput = document.getElementById("Output");

    var path = window.location.pathname.split("/");
    var paramId = path[path.length - 1];

    function removeTrainingDataFunctionFactory(id) {
        return function () {
            fetch(baseUrl + "/api/NeuralNet/" + paramId + "/TrainingData/" + id,
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

    function generateButtonsCell(id) {
        var cell = document.createElement("td");
        var parentDiv = document.createElement("div");
        parentDiv.classList.add("btn-group");
        parentDiv.classList.add("pull-right");
        cell.appendChild(parentDiv);

        var removeButton = document.createElement("button");
        removeButton.classList.add("btn");
        removeButton.classList.add("btn-danger");
        removeButton.innerHTML = "Usuń";
        removeButton.onclick = removeTrainingDataFunctionFactory(id);
        parentDiv.appendChild(removeButton);

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
        if (!outputInput.value) {
            document.getElementById('OutputValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('OutputValidation').classList.add("hidden");
        }

        if (isValid) {
            var body = {
                Name: nameInput.value,
                TrainingSet: [{
                    Input: [input1Input.value, input2Input.value],
                    Output: [outputInput.value]
                }]
            }

            fetch(baseUrl + "/api/NeuralNet/" + paramId + "/TrainingData",
                {
                    method: "PUT",
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


    fetch(baseUrl + "/api/NeuralNet/" + paramId + "/TrainingData").then(response => {
        if (response.status == 200) {
            response.json().then(body => {
                console.log(body);
                for (var i = 0; i < body.length; i++) {
                    var trainingData = body[i];
                    var row = table.insertRow(i);
                    row.insertCell(0).appendChild(document.createTextNode(trainingData.id));
                    row.insertCell(1).appendChild(document.createTextNode(trainingData.name));
                    row.insertCell(2).appendChild(document.createTextNode(trainingData.trainingSet[0].input[0]));
                    row.insertCell(3).appendChild(document.createTextNode(trainingData.trainingSet[0].input[1]));
                    row.insertCell(4).appendChild(document.createTextNode(trainingData.trainingSet[0].output[0]));
                    row.appendChild(generateButtonsCell(trainingData.id));
                }
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));
