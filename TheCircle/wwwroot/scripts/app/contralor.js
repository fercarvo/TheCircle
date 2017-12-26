/*
    contralor v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/
angular.module('contralor', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('aprobar', {
                templateUrl: 'views/contralor/aprobar.html',
                controller: 'aprobar'
            })
            .state('inconsistencias', {
                templateUrl: 'views/contralor/inconsistencias.html',
                controller: 'inconsistencias'
            })
            .state('inconsistencias.receta', {
                templateUrl: 'views/contralor/inconsistencias.receta.html',
                controller: 'inconsistencias.receta'
            })
            .state('inconsistencias.pedidointerno', {
                templateUrl: 'views/contralor/inconsistencias.pedidointerno.html',
                controller: 'inconsistencias.pedidointerno'
            })
            .state('inconsistencias.transferencia', {
                templateUrl: 'views/contralor/inconsistencias.transferencia.html',
                controller: 'inconsistencias.transferencia'
            })
            .state('historial', {
                templateUrl: 'views/contralor/historial.html',
                controller: 'historial'
            })
            .state('historial.cambiosStock', {
                templateUrl: 'views/contralor/historial.cambiosStock.html',
                controller: 'historial.cambiosStock'
            })
            .state('historial.remisiones', {
                templateUrl: 'views/contralor/historial.remisiones.html',
                controller: 'historial.remisiones'
            })
            .state('editarStock', {
                templateUrl: 'views/contralor/editarStock.html',
                controller: 'editarStock'
            })
            .state('reportes', {
                templateUrl: 'views/contralor/reportes.html',
                controller: 'reportes'
            })
            .state('reportes.egresos', {
                templateUrl: 'views/contralor/reportes.egresos.html',
                controller: 'reportes.egresos'
            })
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
    }])
    .run(["$state", "$http", "$templateCache", "dataFac", function ($state, $http, $templateCache, dataFac) {

        checkSession($http)
        loadTemplates($state, "reportes", $http, $templateCache)
        dataFac.getRecetas({})
        dataFac.getTransferencias({})

    }])
    .factory('dataFac', ['$http', function ($http) {
        
        var dataFac = {
            aprobaciones1: null,
            recetas: null,
            pedidosinternos: null,
            transferencias: null,
            remisionesAprobadas: {
                desde: null,
                hasta: null,
                data: null
            },
            cambiosStock: {
                desde: null,
                hasta: null,
                data: null
            },
            reporteEgresos: {
                desde: null,
                hasta: null,
                data: null
            },
            stock: null,
            getCambiosStock: getCambiosStock,
            getStock: getStock,
            getTransferencias: getTransferencias,
            getRecetas: getRecetas,
            getPedidos: getPedidos,
            getAprobaciones1: getAprobaciones1,
            rechazarRemision: rechazarRemision,
            guardarAprobacion: guardarAprobacion,
            getRemisionesAprobadas: getRemisionesAprobadas,
            postCambio: postCambio,
            getReporteEgresos
        }

        function getReporteEgresos(desde, hasta, $scope) {
            NProgress.start()
            $http({
                method: "GET",
                url: `/api/reporte/egresos/`,
                params: { desde, hasta }
            }).then(function (res) {
                console.log("Egresos", res.data)
                $scope.reportes.data = res.data.map(r => { r.personal = uppers(r.personal); return r })
                NProgress.done();
            }, function (error) {
                console.log("Error egresos", error);
                notify("No se pudo obtener los egresos", "danger")
                NProgress.done();
            })                      
        }

        function getCambiosStock($scope, data) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/itemfarmacia/report/cambios",
                params: data
            }).then(function (res) {
                console.log("getCambiosStock", res.data)
                $scope.cambios.data = res.data
                NProgress.done();
            }, function (error) {
                console.log("Error getCambiosStock", error);
                notify("No se pudo cargar la información", "danger")
                NProgress.done();
            })
        }

        function postCambio(id, cantidad, comentario, $scope) {
            NProgress.start()
            $http({
                method: "PUT",
                url: `/api/itemfarmacia/${id}`,
                params: {cantidad, comentario}
            }).then(function (res) {
                getStock($scope);
                console.log("Se actualizo con exito", res.data)
                NProgress.done();
                notify("El item se actualizo correctamente", "success")
                $('#modal_alterar').modal('hide')
            }, function (error) {
                console.log("Error cambiar stock", error);
                notify("No se pudo alterar el stock", "danger")
                NProgress.done();
                $('#modal_alterar').modal('hide')
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

        function getRemisionesAprobadas($scope, data) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/remision/aprobadas",
                params: data
            }).then(function success(res) {
                console.log("Info inicial", res.data)

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

                $scope.remisiones.data = res.data.infoAP;
                console.log("Despues populate", res.data.infoAP)
                NProgress.done();
            }, function error(err) {
                console.log("Error cargar remisiones", error);
                notify("No se pudo cargar las remisiones", "danger")
                NProgress.done();
            })
        }

        function getTransferencias($scope) {
            $http.get("/api/transferencia/inconsistente").then(function (res) {
                console.log("transferencias", res.data)
                dataFac.transferencias = res.data
                $scope.transferencias = dataFac.transferencias
            }, function (error) {
                console.log("Error transferencias", error)
                notify("Ha ocurrido un error al cargar transferencias", "danger")
            })
        }

        function getPedidos($scope) {
            $http.get("/api/pedidointerno/inconsistentes").then(function (res) {
                console.log("pedidos", res.data)
                dataFac.pedidosinternos = res.data
                $scope.pedidos = dataFac.pedidosinternos
            }, function (error) {
                console.log("Error pedidos", error)
                notify("Ha ocurrido un error al cargar pedidos internos", "danger")
            })
        }

        function getRecetas($scope) {
            $http.get("/api/receta/inconsistente").then(function (res) {
                console.log("recetas", res.data)
                dataFac.recetas = res.data
                $scope.recetas = dataFac.recetas
            }, function (error) {
                console.log("Error recetas", error)
                notify("Ha ocurrido un error al cargar recetas", "danger")
            })
        }

        function guardarAprobacion(remision, comentario, $scope) {
            NProgress.start()
            $http.post("/api/remision/" + remision + "/aprobacioncontralor", comentario).then(function (res) {
                console.log("AP contralor", res)
                getAprobaciones1($scope)
                $('#modal_aprobar').modal('hide')
                notify("La remision ha sido aprobada exitosamente", "success")
                NProgress.done()
            }, function (err) {
                NProgress.done()
                console.log("Error APcontralor", err)
                $('#modal_aprobar').modal('hide')
                notify("No se pudo guardar la aprobación", "danger")
            })
        }

        function getAprobaciones1($scope) {
            $http.get("/api/remision/aprobacion1").then(function (res) {
                console.log("Aprobaciones 1", res.data)
                dataFac.aprobaciones1 = res.data
                $scope.aprobaciones1 = dataFac.aprobaciones1
            }, function (error) {
                console.log("AP! error", error)
                notify("Error al cargar remisiones", "danger")
            })
        }

        function rechazarRemision(id, comentario, $scope) {
            NProgress.start()
            $http.put("/api/remision/aprobacion1/" + id + "/rechazar", { monto: 0, comentario: comentario }).then(function (res) {
                console.log("Se rechazo con exito", res)
                $("#modal_rechazar").modal("hide")
                getAprobaciones1($scope)
                notify("La remision se ha rechazado exitosamente", "success")
                NProgress.done()
            }, function (err) {
                NProgress.done()
                console.log("Error en rechazar AP1", err)
                notify("No se pudo rechazar la remision medica", "danger")
            })
        }

        return dataFac

    }])
    .controller('aprobar', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.aprobaciones1 = dataFac.aprobaciones1
        $scope.aprobacion = null

        dataFac.getAprobaciones1($scope)

        $scope.ver = function (aprobacion) {
            $scope.aprobacion = aprobacion
            $("#modal_ver").modal("show")
        }

        $scope.rechazar = function (aprobacion) {
            $scope.aprobacion = aprobacion
            $("#modal_rechazar").modal("show")
            $scope.comentarioRechazo = null
        }

        $scope.guardarRechazo = function (remision, comentario) {
            dataFac.rechazarRemision(remision, comentario, $scope)
        }

        $scope.guardarAprobacion = function (remision) {
            dataFac.guardarAprobacion(remision, "", $scope)
        }
    }])
    .controller('inconsistencias', ["$state", function ($state) {
        $state.go('inconsistencias.receta')

    }])
    .controller('inconsistencias.receta', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.recetas = dataFac.recetas
        $scope.receta = null

        dataFac.getRecetas($scope)

        $scope.select = function (receta) {
            $scope.receta = receta
        }

    }])
    .controller('inconsistencias.pedidointerno', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.pedidos = dataFac.pedidosinternos
        $scope.pedido = null

        dataFac.getPedidos($scope)

        $scope.select = function (pedido) {
            $scope.pedido = pedido
        }

    }])
    .controller('inconsistencias.transferencia', ["$scope", "$state", "dataFac", function ($scope, $state, dataFac) {
        $scope.transferencias = dataFac.transferencias
        $scope.transferencia = null

        dataFac.getTransferencias($scope)

        $scope.select = function (transferencia) {
            $scope.transferencia = transferencia
        }
    }])
    .controller('historial', ['$state', function ($state) {
        $state.go("historial.cambiosStock")
    }])
    .controller('historial.cambiosStock', ['$scope', 'dataFac', function ($scope, dataFac) {
        $scope.cambios = dataFac.cambiosStock
        $scope.item = null;

        $scope.$watch("cambios", function () {
            dataFac.cambiosStock = $scope.cambios
        })

        $scope.ver = function (item) {
            $scope.item = item
        }

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }
            dataFac.getCambiosStock($scope, data)
        }
    }])
    .controller('historial.remisiones', ["$scope", "dataFac", function ($scope, dataFac) {
        $scope.remisiones = dataFac.remisionesAprobadas
        $scope.aprobacion = null

        $scope.$watch("remisiones", function () {
            dataFac.remisionesAprobadas = $scope.remisiones
        })

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
    .controller('editarStock', ['$scope', 'dataFac', function ($scope, dataFac) {
        $scope.stock = dataFac.stock
        $scope.item = null;

        dataFac.getStock($scope)

        $scope.select = function (item) {
            $scope.item = item
            $("#modal_alterar").modal("show")
            $scope.cantidad = null;
        }

        $scope.guardarUpdate = function (idItem, cantidad, comentario) {
            dataFac.postCambio(idItem, cantidad, comentario, $scope);
        }
    }])
    .controller('reportes', ["$state", function ($state) { $state.go('reportes.egresos') } ])
    .controller('reportes.egresos', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) { //reportes.egresos

        $scope.reportes = dataFac.reportes

        $scope.$watch('reportes', ()=> { dataFac.reportes = $scope.reportes })

        $scope.generar = function (desde, hasta) {
            dataFac.getReporteEgresos(desde, hasta, $scope)
        }


    }])