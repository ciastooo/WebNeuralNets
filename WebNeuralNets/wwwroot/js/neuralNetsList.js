"use strict";

(function (window, document) {

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
            fetch(baseUrl + "/api/NeuralNet/" + id + "/Train",
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

    function detailsNeuralNetFunctionFactory(id) {
        return function () {
            window.location.replace(baseUrl + "/Home/NeuralNetDetails/" + id);
        }
    }

    function trainingDataNeuralNetFunctionFactory(id) {
        return function () {
            window.location.replace(baseUrl + "/Home/TrainingDataList/" + id);
        }
    }

    function downloadJSONNeuralNetFunctionFactory(id) {
        return function () {
            fetch(baseUrl + "/api/Export/json/" + id).then(response => {
                if (response.status == 200) {
                    console.log(response);
                    response.blob().then(blob => {
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = "neuralnet_" + id + ".json";
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    })
                }
            }).catch(err => {
                console.error(err);
            });
        }
    }

    function downloadXMLNeuralNetFunctionFactory(id) {
        return function () {
            fetch(baseUrl + "/api/Export/xml/" + id).then(response => {
                if (response.status == 200) {
                    console.log(response);
                    response.blob().then(blob => {
                        var link = document.createElement('a');
                        link.href = window.URL.createObjectURL(blob);
                        link.download = "neuralnet_" + id + ".xml";
                        document.body.appendChild(link);
                        link.click();
                        document.body.removeChild(link);
                    })
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

        var trainButton = document.createElement("button");
        trainButton.classList.add("btn");
        trainButton.classList.add("btn-primary");
        trainButton.innerHTML = "Trenuj";
        trainButton.onclick = trainNeuralNetFunctionFactory(id);
        parentDiv.appendChild(trainButton);

        var detailsButton = document.createElement("button");
        detailsButton.classList.add("btn");
        detailsButton.classList.add("btn-default");
        detailsButton.innerHTML = "Szczegóły";
        detailsButton.onclick = detailsNeuralNetFunctionFactory(id);
        parentDiv.appendChild(detailsButton);

        var trainingDataButton = document.createElement("button");
        trainingDataButton.classList.add("btn");
        trainingDataButton.classList.add("btn-default");
        trainingDataButton.innerHTML = "Dane treningowe";
        trainingDataButton.onclick = trainingDataNeuralNetFunctionFactory(id);
        parentDiv.appendChild(trainingDataButton);

        var jsonDataButton = document.createElement("button");
        jsonDataButton.classList.add("btn");
        jsonDataButton.classList.add("btn-default");
        jsonDataButton.innerHTML = "JSON";
        jsonDataButton.onclick = downloadJSONNeuralNetFunctionFactory(id);
        parentDiv.appendChild(jsonDataButton);

        var xmlDataButton = document.createElement("button");
        xmlDataButton.classList.add("btn");
        xmlDataButton.classList.add("btn-default");
        xmlDataButton.innerHTML = "XML";
        xmlDataButton.onclick = downloadXMLNeuralNetFunctionFactory(id);
        parentDiv.appendChild(xmlDataButton);

        var removeButton = document.createElement("button");
        removeButton.classList.add("btn");
        removeButton.classList.add("btn-danger");
        removeButton.innerHTML = "Usuń";
        removeButton.onclick = removeNeuralNetFunctionFactory(id);
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
        if (!trainingrateInput.value) {
            document.getElementById('TrainingRateValidation').classList.remove("hidden");
            isValid = false;
        } else {
            document.getElementById('TrainingRateValidation').classList.add("hidden");
            if (trainingrateInput.value <= 0) {
                document.getElementById('TrainingRateTooLowValidation').classList.remove("hidden");
                isValid = false;
            } else {
                document.getElementById('TrainingRateValidation').classList.add("hidden");
            }
        }

        if (isValid) {
            var name = nameInput.value;
            var description = descriptionInput.value;
            var trainingRate = trainingrateInput.value;

            fetch(baseUrl + "/api/NeuralNet?name=" + name + "&trainingRate=" + trainingRate + (description ? ("&description=" + description) : ""),
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
                    row.appendChild(generateButtonsCell(neuralNet.id));
                }
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));
