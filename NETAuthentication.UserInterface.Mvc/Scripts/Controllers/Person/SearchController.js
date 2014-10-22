netAuthenticationApp.controller('SearchPersonController', [
    '$scope', '$http', '$cookies', '$window', function($scope, $http, $cookies, $window) {

        // variables
        $scope.persons = [];
        $scope.queryError = '';
        $scope.isLoading = false;
        $scope.hasErrors = false;

        // the command
        $scope.command = function() {
            // search criteria
            var query = $scope.query;

            if (query == undefined) {
                query = "";
            }

            // call web server
            $scope.isLoading = true;

            jQuery.support.cors = true;

            var bearer = $window.localStorage.getItem("token");

            $http({
                method: 'GET',
                url: netAuthenticationApp.readApi + 'PersonRead',
                headers: { 'Authorization': 'Bearer ' + bearer }
                }).
                success(function(data, status, headers, config) {
                    $scope.persons = data;
                    $scope.isLoading = false;
                    $scope.hasErrors = false;
                }).
                error(function(data, status, headers, config) {
                    $scope.queryError = data || 'An error occurred' + ' Status ' + status;
                    $scope.isLoading = false;
                $scope.hasErrors = true;
            });
        };
    }
]);