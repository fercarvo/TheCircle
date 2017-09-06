/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appCoordinadorCC', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('recetas', {
                templateUrl: 'views/coordinadorCC/recetas.html',
                controller: 'recetas'
            })
            .state('egresos', {
                templateUrl: 'views/coordinadorCC/egresos.html',
                controller: 'egresos'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
    }])
    .run(["$state", "$rootScope", "$cookies", function ($state, $rootScope, $cookies) {
        $rootScope.session_name = (function () {
            var c = $cookies.get('session_nombre')
            if (c) {
                return c
            } return ""
        })() 

        $state.go("recetas");
    }])
    .factory('dataFac', ['$http', function ($http) {
        var dataFactory = {};

        dataFactory.stock = null;
        dataFactory.recetas = null;
        dataFactory.localidad = "CC2";

        return dataFactory;
    }])
    .controller('recetas', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        cosole.log("En Recetas");

    }])
    .controller('egresos', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        console.log("En Egresos");

    }])