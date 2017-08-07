angular.module('appAsistente', ['ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('despachar', {
                //url: '/atencion',
                templateUrl: 'html/asistente/despachar.html',
                controller: 'despachar'
            })
            .state('historial', {
                //url: '/registro',
                templateUrl: 'html/asistente/historial.html',
                controller: 'historial'
            })
            .state('stock', {
                //url: '/2',
                templateUrl: 'html/asistente/stock.html',
                controller: 'stock'
            });
        //$urlRouterProvider.otherwise("/atencion/registro");
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('despachar');
        });
    })
    .controller('despachar', ["$scope", "$state", "$http", function ($scope, $state, $http) {

        $scope.apadrinado = {};
        $scope.apadrinado.cod = "";

    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
    .controller('stock', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
