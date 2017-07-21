angular.module('appMedico', ['ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('atencionM', {
                url: '/atencion',
                templateUrl: 'html/atencionM/atencion.html',
                controller: 'atencion'
            })
                .state('atencionM.registro', {
                    //url: '/registro',
                    templateUrl: 'html/atencionM/atencion.registro.html',
                    controller: 'atencion.registro'
                })
                .state('atencionM.receta', {
                    //url: '/receta',
                    templateUrl: 'html/atencionM/atencion.receta.html',
                    controller: 'atencion.receta'
                })
                .state('atencionM.remision', {
                    //url: '/remision',
                    templateUrl: 'html/atencionM/atencion.remision.html',
                    controller: 'atencion.remision'
                })
            .state('anulaciones', {
                url: '/anulaciones',
                templateUrl: 'html/anulaciones/index.html',
                controller: 'anulaciones'
            })
            .state('estadisticas', {
                url: '/estadisticas',
                templateUrl: 'html/estadisticas/estadistica.html',
                controller: 'estadisticas'
            })
              .state('estadisticas.1', {
                  url: '/1',
                  templateUrl: 'html/estadisticas/estadistica.1.html',
                  controller: 'estadisticas.1'
              })
              .state('estadisticas.2', {
                  url: '/2',
                  templateUrl: 'html/estadisticas/estadistica.2.html',
                  controller: 'estadisticas.2'
              });
        //$urlRouterProvider.otherwise("/atencion/registro");
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('atencionM.registro');
        });
    })
    .controller('atencion', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};

        $scope.mensaje = "que fue men";

    }])
        .controller('atencion.registro', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
        .controller('atencion.remision', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
        .controller('atencion.receta', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
    .controller('anulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])
    .controller('estadisticas.1', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])
    .controller('estadisticas.2', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])
