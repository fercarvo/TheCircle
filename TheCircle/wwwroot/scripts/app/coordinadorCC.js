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
    .run(["$state", "$rootScope", "$cookies", "$http", "refresh", function ($state, $rootScope, $cookies, $http, refresh) {

        refresh.goTime(function () {
            $http.get("login").then(function () {
            }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('logout');
                }
            })
        }, 1000 * 60 * 20)

        $rootScope.session_name = (function () {
            var c = $cookies.get('session_name')
            if (c) {
                return c
            } return ""
        })()

        $rootScope.session_email = (function () {
            var c = $cookies.get('session_email')
            if (c) {
                return c
            } return ""
        })()

        $rootScope.session_photo = (function () {
            var c = $cookies.get('session_photo')
            if (c) {
                return c
            } return "/images/ci.png"
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
    .factory('refresh', [function () { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            console.log("Go refresh");
            return setInterval(fn, 10000);
        }

        function goTime(fn, time) {
            fn();
            console.log("Go refresh by ", time);
            return setInterval(fn, time);
        }

        function stop(repeater) {
            console.log("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop,
            goTime: goTime
        }
    }])
    .controller('recetas', ["$log", "$scope", "$state", "$http", "dataFac", function ($log, $scope, $state, $http, dataFac) {
        cosole.log("En Recetas");

    }])
    .controller('egresos', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        console.log("En Egresos");

    }])