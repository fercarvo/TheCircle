/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appCoordinador', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('validar', {
                templateUrl: 'views/coordinador/validar.html',
                controller: 'validar'
            })
            .state('historial', {
                templateUrl: 'views/coordinador/historial.html',
                controller: 'historial'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .run(["$state", "$rootScope", "$cookies", function ($state, $rootScope, $cookies) {
        $rootScope.session_name = (function () {
            var c = $cookies.get('session_nombre')
            if (c) {
                return c
            } return ""
        })() 

        $state.go("validar");
    }])
    .factory('dataFac', ['$http', function ($http) {
        var dataFactory = {};

        dataFactory.stock = null;
        dataFactory.recetas = null;
        dataFactory.localidad = "CC2";

        dataFactory.getStock = function (localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }


        return dataFactory;
    }])
    .controller('validar', ["$log", "$scope", "$state", "$http", "dataFac", function ($log, $scope, $state, $http, dataFac) {
        $log.info("En Validar");

    }])
    .controller('historial', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En historial");

    }])