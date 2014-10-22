console.log('AppStart');

// angular application
var netAuthenticationApp = angular.module('netAuthenticationApp', [
    'ngCookies',
    'ngStorage',
    'angularModalService',
    'ui.bootstrap'
]);

// angular constants
netAuthenticationApp.readApi = 'http://localhost:10004/';
netAuthenticationApp.writeApi = 'http://localhost:10003/';
netAuthenticationApp.identityApi = 'http://localhost:10002/';

// popup service
netAuthenticationApp.service('popupService', function() {
    this.open = function(url, title, w, h) {
        var left = (screen.width / 2) - (w / 2);
        var top = (screen.height / 2) - (h / 2);
        var targetWin = window.open(url, title,
            'toolbar=no, location=no, ' +
            'directories=no, status=no, ' +
            'menubar=no, scrollbars=no, ' +
            'resizable=no, copyhistory=no, ' +
            'width=' + w + ', height=' + h + ', ' +
            'top=' + top + ', left=' + left);
        return targetWin;
    };
});

// session manager service
netAuthenticationApp.service('authenticationService', function ($sessionStorage) {

    // authenticate a user and its token in the local session
    this.authenticate = function(username, token) {
        $sessionStorage.username = username;
        $sessionStorage.token = token;
    };

    // logout the user
    this.logout = function() {
        $sessionStorage.$reset();
    };

    // verify if a user is logged in
    this.isAuthenticated = function() {
        if ($sessionStorage.username != ''
            && $sessionStorage.username != undefined
            && $sessionStorage.token != ''
            && $sessionStorage.token != undefined
            ) {
            return true;
        }

        return false;
    };

    // return the user logged in
    this.getUsername = function () {
        return $sessionStorage.username;
    };

    // return the token of the user logged in
    this.getToken = function () {
        return $sessionStorage.token;
    };
});


