/*
 appContralor v1.0
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
            .state('historial', {
                templateUrl: 'views/contralor/historial.html',
                controller: 'historial'
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
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(true)
        $compileProvider.commentDirectivesEnabled(true)
        $compileProvider.cssClassDirectivesEnabled(true)
    }])
    .run(["$state", "$http", "$templateCache", "dataFac", function ($state, $http, $templateCache, dataFac) {

        checkSession($http)
        loadTemplates($state, "aprobar", $http, $templateCache)
        dataFac.getRecetas({})
        dataFac.getTransferencias({})

    }])
    .factory('dataFac', ['$http', function ($http) {
        
        var dataFac = {
            aprobaciones1: null,
            recetas: null,
            pedidosinternos: null,
            transferencias: null,
            getTransferencias: getTransferencias,
            getRecetas: getRecetas,
            getPedidos: getPedidos,
            getAprobaciones1: getAprobaciones1,
            rechazarRemision: rechazarRemision,
            guardarAprobacion: guardarAprobacion
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
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {

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