(function (window, document) {


    fetch(baseUrl + "/api/Translation/" + key + "?language=" + getLanugage()).then(response => {
        if (response.status == 200) {
            response.text().then(text => {
                if (text) {
                    translations[key] = text;
                    if (elem.innerHTML) {
                        elem.innerHTML = text;
                    } else {
                        elem.value = text;
                    }
                }
            });
        }
    }).catch(err => {
        console.error(err);
    });

}(this, this.document));
