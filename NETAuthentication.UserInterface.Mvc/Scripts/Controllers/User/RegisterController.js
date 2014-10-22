netAuthenticationApp.controller('RegisterUserController', [
    '$scope', '$http', '$window', '$interval', '$modal', function($scope, $http, $window, $interval, $modal) {

        //#region ViewModel

        /*-----------------------------------------------------------------------------------------------------------------------
         *  RegisterViewModel
         *  
         -----------------------------------------------------------------------------------------------------------------------*/
        $scope.registerViewModel = {
            Username: '',
            Password: '',
            Email: '',
            IsRegistered: false,
            HasErrors: false,
            RegisterErrors: '',
            reset: function() {
                this.Username = '';
                this.Password = '';
                this.Email = '';
                this.HasErrors = false;
                this.RegisterErrors = '';
            },
            register: function() {
                this.IsRegistered = true;
                this.HasErrors = false;
                this.RegisterErrors = '';
                // redirect
                $interval(function() {
                    $scope.redirectTimeout--;
                    if ($scope.redirectTimeout <= 0) {
                        $window.location.href = '/User/Login';
                    }
                }, 1000);

            },
            error: function(errorMessage) {
                this.IsRegistered = false;
                this.HasErrors = true;
                this.RegisterErrors = errorMessage;
            }
        };

        $scope.redirectTimeout = 5;
        $scope.loadingPanel = null;

        //#endregion

        /*-----------------------------------------------------------------------------------------------------------------------
         *  Create a new User delegate calling the Secure endpoint
         *  
         -----------------------------------------------------------------------------------------------------------------------*/
        $scope.create = function() {

            jQuery.support.cors = true;
            openLoading();

            $http({
                    method: 'POST',
                    url: netAuthenticationApp.identityApi + 'Account/Register',
                    data: $scope.registerViewModel
                }).
                success(function(data, status, headers, config) {
                    $scope.registerViewModel.register();
                    closeLoading();
                }).
                error(function(data, status, headers, config) {
                    $scope.registerViewModel.error(data);
                    closeLoading();
                });
        };

        /*-----------------------------------------------------------------------------------------------------------------------
         *  Reset the registration form
         *  
         -----------------------------------------------------------------------------------------------------------------------*/
        $scope.reset = function() {
            $scope.registerViewModel.reset();
        };

        /*-----------------------------------------------------------------------------------------------------------------------
         *  loading panel
         *  
         -----------------------------------------------------------------------------------------------------------------------*/
        function openLoading() {
            $scope.loadingPanel = $modal.open(
                {
                    templateUrl: 'myModalContent.html',
                    size: 'sm',
                    controller: 'RegisterUserController',
                    windowClass: 'modal-dialog-center'
        }
            );
        };

        function closeLoading() {
            $scope.loadingPanel.close('finish');
        };
    }
]);