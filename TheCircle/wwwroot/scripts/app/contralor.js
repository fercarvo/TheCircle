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
            });
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(true)
        $compileProvider.commentDirectivesEnabled(true)
        $compileProvider.cssClassDirectivesEnabled(true)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http)
        loadTemplates($state, "aprobar", $http, $templateCache)

    }])
    .factory('dataFac', ['$http', function ($http) {
        
        var dataFac = {
            aprobaciones1: null,
            getAprobaciones1: getAprobaciones1,
            rechazarRemision: rechazarRemision
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
            $http.put("/api/remision/aprobacion1/" + id + "/rechazar", comentario).then(function (res) {
                console.log("Se rechazo con exito", res)
                getAprobaciones1($scope)
                NProgress.done()
            }, function (err) {
                NProgress.done()
                console.log("Error en rechazar AP1", err)
                notify("No se pudo rechazar la remision medica", "danger")
            })
        }

        return dataFac

    }])
    .controller('aprobar', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.aprobaciones1 = dataFac.aprobaciones1
        $scope.aprobacion = null

        dataFac.getAprobaciones1($scope)

        $scope.aprobar = function (aprobacion) {
            $scope.aprobacion = aprobacion
        }

        $scope.rechazar = function (aprobacion) {
            $scope.aprobacion = aprobacion
            $("#modal_rechazar").modal("show")
            $scope.comentarioRechazo = null
        }

        $scope.guardarRechazo = function (remision, comentario) {
            dataFac.rechazarRemision(remision, comentario, $scope)
        }








    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])