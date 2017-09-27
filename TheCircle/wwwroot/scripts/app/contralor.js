/*
 appContralor v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appContralor', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", "$logProvider", function ($stateProvider, $compileProvider, $logProvider) {
        $stateProvider
            .state('aprobar', {
                templateUrl: 'views/contralor/aprobar.html',
                controller: 'aprobar'
            })
            .state('historial', {
                templateUrl: 'views/contralor/historial.html',
                controller: 'historial'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .run(["$state", "$http", "refresh", function ($state, $http, refresh) {

        checkSession($http);

        loadTemplates($state, "aprobar", $http, $templateCache);
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
    .controller('aprobar', ["$log", "$scope", "$state", "$http", "dataFac", function ($log, $scope, $state, $http, dataFac) {
        $log.info("En aprobar");

    }])
    .controller('historial', ["$log", "$scope", "$state", "$http", function ($log, $scope, $state, $http) {
        $log.info("En historial");

    }])