angular.module('appMedico', ['ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('atencionM', {
                //url: '/pacientes',
                templateUrl: 'html/atencionM/pacientes.html',
                controller: 'controllerPacientes'
            })
            .state('anulaciones', {
                //url: '/muestras',
                templateUrl: 'views/operario/muestras.html',
                controller: 'controllerMuestras'
            })
            .state('estadisticas', {
                //url: '/muestras',
                templateUrl: 'views/operario/muestras.html',
                controller: 'controllerMuestras'
            });
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('pacientes');
        });
    })
    .controller('controllerPacientes', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
    .controller('controllerMuestras', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])