/*
 appContralor v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appContralor', ['ui.router', 'ngCookies'])
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
    .run(["$state", "$rootScope", "$cookies", "$http", "refresh", function ($state, $rootScope, $cookies, $http, refresh) {

        refresh.go(function () {
            $http.get("session").then(function () { console.log("Session valida") }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('/login');
                }
            })
        }, 20) //minutos


        var promises = []
        var states = $state.get()
        NProgress.start()

        for (i = 1; i < states.length; i++) {
            var p = $http.get(states[i].templateUrl, { cache: $templateCache })
            promises.push(p)
            p.then(function () { }, function (error) { console.log("Error template: ", error) })
        }

        Promise.all(promises)
            .then(function () { }).catch(function () { }).then(function () {
                NProgress.done()
                $state.go("aprobar") /////////////////////////
            })

        $rootScope.session_name = $cookies.get('session_name')
        $rootScope.session_email = $cookies.get('session_email')
        $rootScope.session_photo = $cookies.get('session_photo')   
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