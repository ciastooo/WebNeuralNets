(function (window, document) {

    var layout = document.getElementById('layout');
    var menu = document.getElementById('menu');
    var menuLink = document.getElementById('menuLink');
    var content = document.getElementById('main');
    var menuLoggedInLinks = document.getElementById('loggedIn');
    var menuNotLoggedInLinks = document.getElementById('notLoggedIn');

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
    };

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

    console.log("logged in", getCookie("id"));
    if (getCookie("id")) {
        menuLoggedInLinks.classList.remove("hidden");
    } else {
        menuNotLoggedInLinks.classList.remove("hidden");
    }

}(this, this.document));
