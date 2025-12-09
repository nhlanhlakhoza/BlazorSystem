window.authStore = {
    get: function () {
        return localStorage.getItem("authToken");
    },
    set: function (token) {
        localStorage.setItem("authToken", token);
    },
    remove: function () {
        localStorage.removeItem("authToken");
    }
};
