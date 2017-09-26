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
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(true)
        $compileProvider.commentDirectivesEnabled(true)
        $compileProvider.cssClassDirectivesEnabled(true)
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
    .factory('dataFac', ['$http', "$rootScope", function ($http, $rootScope) {
        var dataFac = {
            remisiones: null,
            getRemisiones: getRemisiones,
            postAprobacion: postAprobacion
        }

        function postAprobacion(remision, data) {
            NProgress.start();

            var promise = $http.post("/api/remision/" + remision + "/aprobacion1", data)

            promise.then(function (res) {
                console.log("Aprobacion1 creada", res.data);
                NProgress.done()
                notify("Remision aprobada exitosamente", "success");
            }, function (error) {
                console.log(error);
                notify("Error al aprobar la remision", "danger");
                NProgress.done()
            })

            return promise
        }

        function getRemisiones() {
            var promise = $http.get("/api/remision")

            promise.then(function (res) {
                dataFac.remisiones = res.data;
                console.log("Remisiones", res.data)
                $rootScope.$broadcast('dataFac.remisiones');

            }, function (error) {
                console.log(error);
                notify("Error al cargar remisiones", "danger")
            })

            return promise
        }

        return dataFac
    }])
    .controller('validar', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.remisiones = dataFac.remisiones
        $scope.remision = null
        var actualizar = refresh.go(cargar, 1) //cada 1/2 minuto

        $scope.$on('dataFac.remisiones', function () { $scope.remisiones = dataFac.remisiones })

        function cargar() {
            if ($state.includes("validar")) {
                dataFac.getRemisiones()
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.ver = function (remision) {
            $("#ver_remision").modal("show");
            $scope.remision = remision;
        }

        $scope.guardar = function (remision, valor, comentario) {

            refresh.stop(actualizar)

            var data = {
                monto: valor,
                comentario: comentario
            }

            dataFac.postAprobacion(remision, data).then(function () {
                $("#ver_remision").modal("hide")
                cargar()
            }, function () { })
        }

    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])