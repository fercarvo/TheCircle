angular.module('appSistema', ['ui.router'])
    .config(["$stateProvider", function ($stateProvider) {
        $stateProvider
            .state('activarusuario', {
                templateUrl: 'views/sistema/activarusuario.html',
                controller: 'activarusuario'
            });
    }])
    .config(["$stateProvider", function ($stateProvider) {
        $stateProvider
            .state('desactivarusuario', {
                templateUrl: 'views/sistema/desactivarusuario.html',
                controller: 'desactivarusuario'
            });
    }])
    .config(["$stateProvider", function ($stateProvider) {
        $stateProvider
            .state('modificarusuario', {
                templateUrl: 'views/sistema/modificarusuario.html',
                controller: 'modificarusuario'
            });
    }])
    .run(["$state", function ($state) {
        $state.go("activarusuario");
    }])
    .controller('activarusuario', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "casacasacasa?";
    }])
    .controller('desactivarusuario', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "casacasacasa?";
    }])
    .controller('modificarusuario', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.casa = "casacasacasa?";
    }])