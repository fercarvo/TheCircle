/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('coordinacionSalud', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('validar', {
                templateUrl: 'views/coordinacionSalud/validar.html',
                controller: 'validar'
            })
            .state('historial', {
                templateUrl: 'views/coordinacionSalud/historial.html',
                controller: 'historial'
            })
            .state('stock', {
                templateUrl: 'views/coordinacionSalud/stock.html',
                controller: 'stock'
            })
            .state('transferencias', {
                templateUrl: 'views/coordinacionSalud/transferencias.html',
                controller: 'transferencias'
            })
            .state('remisionesRechazadas', {
                templateUrl: 'views/coordinacionSalud/remisionesRechazadas.html',
                controller: 'remisionesRechazadas'
            })
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(true)
        $compileProvider.commentDirectivesEnabled(true)
        $compileProvider.cssClassDirectivesEnabled(true)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "validar", $http, $templateCache);
    }])
    .factory('dataFac', ['$http', "$rootScope", function ($http, $rootScope) {
        var dataFac = {
            remisiones: null,
            rechazos: null,
            getRechazos: getRechazos,
            getRemisiones: getRemisiones,
            postAprobacion: postAprobacion,
            guardarAprobacionRechazada: guardarAprobacionRechazada
        }

        function guardarAprobacionRechazada(remision, comentario, monto, $scope) {
            var data = {
                monto: monto,
                comentario: comentario
            }

            NProgress.start();
            $http.put("/api/remision/aprobacion1/" + remision, data).then(function (res) {
                $('#modal_aprobarRechazo').modal('hide')
                console.log("Aprobacion1 re-enviada", res)
                notify("Se aprobó la remision satisfactoriamente", "success")
                getRechazos($scope)
                NProgress.done()
            }, function (error) {
                $('#modal_aprobarRechazo').modal('hide')
                console.log("Error rechazos", error)
                notify("No se pudo cargar los rechazos de aprobacion1", "danger")
                NProgress.done()
            })

        }

        function getRechazos($scope) {
            $http.get("/api/remision/aprobacion1/rechazada").then(function (res) {
                console.log("Rechazos", res.data)
                dataFac.rechazos = res.data
                $scope.rechazos = dataFac.rechazos
            }, function (error) {
                console.log("Error rechazos", error)
                notify("No se pudo cargar los rechazos de aprobacion1", "danger")
            })
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
        $scope.valor = null;
        $scope.comentario = null;
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
            $scope.cantidad = null;
            $scope.comentario = null;
        }

        $scope.guardar = function (remision, cantidad, comentario) {

            refresh.stop(actualizar)

            var data = {
                monto: cantidad,
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
    .controller('stock', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])
    .controller('transferencias', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])
    .controller('remisionesRechazadas', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.rechazos = dataFac.rechazos
        $scope.aprobacion = null

        dataFac.getRechazos($scope)

        $scope.select = function (aprobacion) {
            $scope.aprobacion = aprobacion
            $("#modal_aprobarRechazo").modal("show")
            $scope.comentario = null
            $scope.monto = null
        }

        $scope.guardarAprobacion = function (remision, comentario, monto) {
            dataFac.guardarAprobacionRechazada(remision, comentario, monto, $scope)
        }
    }])