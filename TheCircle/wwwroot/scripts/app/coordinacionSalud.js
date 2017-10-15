﻿/*
    coordinacionSalud v1.0 
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
            .state('remisionesAprobadas', { 
                templateUrl: 'views/coordinacionSalud/remisionesAprobadas.html',
                controller: 'remisionesAprobadas'
            })
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "validar", $http, $templateCache);
    }])
    .factory('dataFac', ['$http', "$rootScope", function ($http, $rootScope) {
        var dataFac = {
            stock: null,
            remisiones: null,
            remisionesAprobadas: null,
            rechazos: null,
            getStock: getStock,
            getRechazos: getRechazos,
            getRemisiones: getRemisiones,
            postAprobacion: postAprobacion,
            guardarAprobacionRechazada: guardarAprobacionRechazada,
            getRemisionesAprobadas: getRemisionesAprobadas
        }

        function getRemisionesAprobadas($scope, data) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/remision/aprobadas",
                params: data
            }).then(function success(res) {
                console.log(res.data)

                res.data.infoAP.forEach(function (aprobacion) {
                    res.data.infoRemision.every(function (remision, index) {
                        if (aprobacion.idRemision == remision.id) {
                            delete aprobacion.idRemision
                            aprobacion.remision = remision
                            return false
                        } else {
                            return true
                        }
                    })
                })

                dataFac.remisionesAprobadas = res.data.infoAP;
                $scope.remisiones = dataFac.remisionesAprobadas;
                console.log(dataFac.remisionesAprobadas)
                NProgress.done();
            }, function error(err) {
                console.log("Error cargar remisiones", error);
                notify("No se pudo cargar las remisiones", "danger")
                NProgress.done();
            })
        }

        function getStock($scope) {
            $http.get("/api/itemfarmacia/report/total").then(function (res) {
                dataFac.stock = res.data;
                $scope.stock = dataFac.stock;                
            }, function (e) {
                console.log("Error cargar Stock", e);
                notify("No se pudo carga el stock de items de farmacia", "danger")
            })
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

        $scope.aprobar = function (remision) {
            refresh.stop(actualizar)
            var data = {
                monto: remision.monto,
                comentario: " "
            }

            dataFac.postAprobacion(remision.id, data).then(function () {
                actualizar = refresh.go(cargar, 1)
            }, function () { })
        }

        $scope.guardar = function (remision, cantidad, comentario) {

            refresh.stop(actualizar)

            var data = {
                monto: cantidad,
                comentario: comentario
            }

            dataFac.postAprobacion(remision, data).then(function () {
                $("#ver_remision").modal("hide")
                actualizar = refresh.go(cargar, 1)
            }, function () { })
        }

    }])
    .controller('stock', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.stock = dataFac.stock
        var actualizar = refresh.go(cargar, 1);

        function cargar() {
            if ($state.includes('stock')) {
                dataFac.getStock($scope)
            } else {
                refresh.stop(actualizar);
            }
        }


    }])
    .controller('transferencias', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.stock = dataFac.stock
        var actualizar = refresh.go(cargar, 1);

        function cargar() {
            if ($state.includes('transferencias')) {
                dataFac.getStock($scope)
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.seleccionar = function (item) {
            $scope.item = item;
            $('#modal_transferencia').modal('show');
            $scope.cantidad = null;
        }

        $scope.solicitar = function (item, cantidad, destino) {
            data = {
                item: item,
                cantidad: cantidad,
                localidad: destino
            }

            NProgress.start();
            $http.post("/api/transferencia", data).then(function success(res) {
                NProgress.done();
                $('#modal_transferencia').modal('hide');
                document.getElementById('transferencia').reset()
                console.log("Se creo la transferencia", res.data);
                notify("Transferencia creada exitosamente", "success");
                actualizar = refresh.go(cargar, 30);
            }, function error(err) {
                NProgress.done();
                console.log("No se pudo crear la transferencia", err);
                notify("No se pudo crear la transferencia", "danger");
                $('#modal_transferencia').modal('hide');
                document.getElementById('transferencia').reset()
                actualizar = refresh.go(cargar, 30);
            })
        }
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
    .controller('remisionesAprobadas', ["$scope", "dataFac", function ($scope, dataFac) {
        $scope.remisiones = dataFac.remisionesAprobadas
        $scope.aprobacion = null

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getRemisionesAprobadas($scope, data)
            
        }

        $scope.select = function (remision) {
            $scope.aprobacion = remision
            $("#modal_aprobacion").modal("show")
        }

    }])