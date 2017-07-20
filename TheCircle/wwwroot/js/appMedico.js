angular.module('appMedico', ['ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('atencionM', {
                //url: '/pacientes',
                templateUrl: 'html/atencionM/atencion.html',
                controller: 'ctrAtencionM'
            })
                .state('atencionM.registro', {
                    //url: '/pacientes',
                    templateUrl: 'html/atencionM/atencion.registro.html',
                    controller: 'ctrAtencionM.registro'
                })
                .state('atencionM.receta', {
                    //url: '/pacientes',
                    templateUrl: 'html/atencionM/atencion.receta.html',
                    controller: 'ctrAtencionM.receta'
                })
                .state('atencionM.remision', {
                    //url: '/pacientes',
                    templateUrl: 'html/atencionM/atencion.remision.html',
                    controller: 'ctrAtencionM.remision'
                })
            .state('anulaciones', {
                //url: '/muestras',
                templateUrl: 'html/anulaciones/index.html',
                controller: 'ctrAnulaciones'
            })
            .state('estadisticas', {
                //url: '/muestras',
                templateUrl: 'html/estadisticas/index.html',
                controller: 'ctrEstadisticas'
            });
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('atencionM.registro');
        });
    })
    .controller('ctrAtencionM', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
        .controller('ctrAtencionM.registro', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
        .controller('ctrAtencionM.remision', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
        .controller('ctrAtencionM.receta', ["$scope", "$state", "$http", function ($scope, $state, $http) {
            $scope.nuevo_paciente = {};
            $scope.pacientes = {};
            $scope.laboratorios = {};


        }])
    .controller('ctrAnulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])
    .controller('ctrEstadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.pacientes = {};
        $scope.laboratorios = {};
        $scope.centros = {};

    }])