netAuthenticationApp.controller('ShellController', [
    '$scope', '$http', '$window','authenticationService',
    function ($scope, $http, $window, authenticationService) {

        //#region ViewModel

        $scope.shellViewModel = {
            navigateLogin: function() {
                $window.location.href = '/User/Login';
            },
            navigateRegister: function() {
                $window.location.href = '/User/Register';
            },
            navigateUserProfile: function() {
                $window.location.href = '/User/Details';
            },
            logout: function() {

                $http({
                    method: 'POST',
                    url: netAuthenticationApp.identityApi + 'Account/Logout'
                }).
                    success(function(data, status, headers, config) {
                        authenticationService.logout();
                        $scope.shellViewModel.navigateLogin();
                    }).
                    error(function(data, status, headers, config) {
                        console.log('error while logging out ' + data);
                    });
            },
            authenticated: function() {
                return authenticationService.isAuthenticated();
            },
            username: function() {
                return authenticationService.getUsername();
            }
    };

        //#endregion

    }
]);