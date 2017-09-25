/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('coordinacionSalud', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('validar', {
                templateUrl: 'views/coordinacionSalud/validar.html',
                controller: 'validar'
            })
            .state('historial', {
                templateUrl: 'views/coordinacionSalud/historial.html',
                controller: 'historial'
            });
        //$compileProvider.debugInfoEnabled(false); Activar en modo producción
        //$logProvider.debugEnabled(false); Activar en modo produccion
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", "$templateCache", function ($state, $rootScope, $cookies, $http, $templateCache) {

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
                $state.go("validar") /////////////////////////
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
    .controller('validar', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.remisiones = null;
        $scope.remision = null;

        $http.get("/api/remision").then(function (res) {
            $scope.remisiones = res.data;

        }, function (error) {
            console.log(error);
            notify("Error al cargar remisiones", "danger");
        })

        $scope.ver = function (remision) {
            $("#ver_remision").modal("show");
            $scope.remision = remision;
        }

        $scope.guardar = function (remision, valor, comentario) {

            var data = {
                valor: valor,
                comentario: comentario
            }

            console.log(data);

            $http.put("/api/remision/" + remision + "aprobar").then(function (res) {
                console.log(res.data);

            }, function (error) {
                console.log(error);
                notify("Error al cargar remisiones", "danger");
            })
        }

    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])