angular.module('appSistema', ['ui.router'])
    .config(["$stateProvider", function ($stateProvider) {
        $stateProvider
            .state('personal', {
                templateUrl: 'views/sistema/personal.html',
                controller: 'personal'
            });
    }])
    .run(["$state", function ($state) {
        $state.go("personal");
    }])
    .controller('personal', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "casacasacasa?";
    }])