(function (window, document) {

    var layout = document.getElementById('layout');
    var menu = document.getElementById('menu');
    var menuLink = document.getElementById('menuLink');
    var content = document.getElementById('main');

    var translations = [];

    function toggleClass(element, className) {
        var classes = element.className.split(/\s+/);
        var length = classes.length;

        for (var i = 0; i < length; i++) {
            if (classes[i] === className) {
                classes.splice(i, 1);
                break;
            }
        }
        // The className is not found
        if (length === classes.length) {
            classes.push(className);
        }

        element.className = classes.join(' ');
    }

    function translateAll() {
        let toTranslate = document.getElementsByClassName("translate");

        for (let i = 0; i < toTranslate.length; i++) {
            let elem = toTranslate[i];
            var key = elem.innerHTML;
            if (!key) {
                key = elem.value;
            }
            if (translations[key]) {
                elem.innerHTML = translations[key];
            } else {
                fetch("https://localhost:44357/api/Translation/" + key + "?language=" + getLanugage()).then(response => {
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
            }
        }
    }

    function toggleAll(e) {
        var active = 'active';

        e.preventDefault();
        toggleClass(layout, active);
        toggleClass(menu, active);
        toggleClass(menuLink, active);
    }

    function getCookie(name) {
        var value = "; " + document.cookie;
        var parts = value.split("; " + name + "=");
        if (parts.length == 2) {
            return parts.pop().split(";").shift();
        }
        return null;
    }

    function getLanugage() {
        let language = getCookie("lang");
        if (language)
            return language;
        return "ENG";
    }

    function setLanguage(language) {
        language = language.toUpperCase();
        fetch("https://localhost:44357/api/Account/Language?language=" + language, {
            method: "POST",
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            },
            body: JSON.stringify(language)
        }).then(response => {
            document.location.reload();
        }).catch(err => {
            console.error(err);
        });
    }

    function logOut() {
        fetch("https://localhost:44357/api/Account/Logout").then(response => {
            document.location.reload();
        }).catch(err => {
            console.error(err);
        });
    }

    menuLink.onclick = function (e) {
        toggleAll(e);
    };

    content.onclick = function (e) {
        if (menu.className.indexOf('active') !== -1) {
            toggleAll(e);
        }
    };

    document.getElementById("logoutLink").onclick = logOut;

    document.getElementById("langPL").onclick = function () { return setLanguage("PL") };
    document.getElementById("langENG").onclick = function () { return setLanguage("ENG") };

    if (getCookie("id")) {
        document.getElementById('loggedIn').classList.remove("hidden");
        document.getElementById('footerLoggedIn').classList.remove("hidden");
    } else {
        document.getElementById('notLoggedIn').classList.remove("hidden");
    }

    translateAll();

}(this, this.document));
