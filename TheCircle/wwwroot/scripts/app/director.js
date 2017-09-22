angular.module('appDirector', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('estadisticas', {
                templateUrl: 'views/director/estadisticas.html',
                controller: 'estadisticas'
            })
            .state('estadisticas.atenciones', {
                templateUrl: 'views/director/estadisticas.atenciones.html',
                controller: 'estadisticas.atenciones'
            })
            .state('estadisticas.remisiones', {
                templateUrl: 'views/director/estadisticas.remisiones.html',
                controller: 'estadisticas.remisiones'
            })
            .state('estadisticas.recetas', {
                templateUrl: 'views/director/estadisticas.recetas.html',
                controller: 'estadisticas.recetas'
            })
            .state('estadisticas.enfermedades', {
                templateUrl: 'views/director/estadisticas.enfermedades.html',
                controller: 'estadisticas.enfermedades'
            });
        
    }])
    .run(["$state", "$rootScope", "$http", function ($state, $rootScope, $http) {

        $state.go("estadisticas")

    }])

    .controller('estadisticas', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En Estadisticas");
        $state.go("estadisticas.atenciones")

    }])
    .controller('estadisticas.atenciones', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En Estadisticas de atenciones");

    }])
    .controller('estadisticas.remisiones', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En Estadisticas de remisiones");

    }])
    .controller('estadisticas.recetas', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En Estadisticas de recetas");

    }])
    .controller('estadisticas.enfermedades', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En Estadisticas de enfermedades");

    }])