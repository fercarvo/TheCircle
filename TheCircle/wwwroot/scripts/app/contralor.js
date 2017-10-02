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
            getAprobaciones1: getAprobaciones1
        }

        function getAprobaciones1($scope) {
            $http.get("/api/remision/aprobacion1").then(function (res) {
                console.log("Aprobaciones 1", res.data)
                dataFac.aprobaciones1 = res.data
                $scope.aprobaciones1 = dataFac.aprobaciones1
            }, function (error) {
                console.log("AP! error", e)
                notify("Error al cargar remisiones", "danger")
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
        }









    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])