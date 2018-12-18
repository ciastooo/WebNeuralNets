(function (window, document) {


    fetch(baseUrl + "/api/NeuralNet/").then(response => {
        if (response.status == 200) {
            response.json().then(body => {
                console.log(body);
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));
