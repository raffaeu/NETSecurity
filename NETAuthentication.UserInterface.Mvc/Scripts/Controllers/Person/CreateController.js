netAuthenticationApp.controller('CreatePersonController', [
    '$scope', '$http', function($scope, $http) {

        // variables
        $scope.isLoading = false;
        $scope.hasErrors = false;
        $scope.personCreated = false;
        $scope.queryError = '';

        $scope.newPerson = {
            FirstName: '',
            LastName: ''
        };

        $scope.finalPerson = {
            Id: '',
            FirstName: '',
            LastName: ''
        };

        // commands
        $scope.create = function() {
            $scope.isLoading = true;
            $scope.personCreated = false;

            jQuery.support.cors = true;

            $http({
                    method: 'POST',
                    url: netAuthenticationApp.writeApi + 'PersonWrite',
                    data: $scope.newPerson
                }).
                success(function(data, status, headers, config) {
                    $scope.isLoading = false;
                    $scope.hasErrors = false;
                    $scope.personCreated = true;
                    $scope.finalPerson = {
                        Id: data,
                        FirstName: $scope.newPerson.FirstName,
                        LastName: $scope.newPerson.LastName
                    };

                }).
                error(function(data, status, headers, config) {
                    $scope.queryError = data || 'An error occurred' + ' Status ' + status;
                    $scope.isLoading = false;
                    $scope.hasErrors = true;
                });
        };

        $scope.reset = function() {
            $scope.newPerson.FirstName = '';
            $scope.newPerson.LastName = '';
        };
    }
]);