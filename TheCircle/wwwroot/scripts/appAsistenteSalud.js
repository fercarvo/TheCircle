/*
 appMedico v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appAsistente', ['ui.router'])
    .config(["$stateProvider", function ($stateProvider) {
        $stateProvider
            .state('despachar', {
                templateUrl: 'views/asistente/despachar.html',
                controller: 'despachar'
            })
            .state('historial', {
                templateUrl: 'views/asistente/historial.html',
                controller: 'historial'
            })
            .state('stock', {
                templateUrl: 'views/asistente/stock.html',
                controller: 'stock'
            });
    }])
    .run(["$state", function ($state){
        $state.go("despachar");
    }])
    .factory('dataFac', ['$http', function ($http) {
        var dataFactory = {};

        dataFactory.stock = null;
        dataFactory.recetas = null;
        dataFactory.localidad = "CC2";

        dataFactory.getStock = function (localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }

        dataFactory.getRecetas = function (localidad) {
            return $http.get("/api/receta/" + localidad);
        }

        return dataFactory;
    }])
    .controller('despachar', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.recetas = dataFac.recetas;
        $scope.receta = null;
        $scope.despachado = [];

        if (dataFac.recetas === null) {
            dataFac.getRecetas(dataFac.localidad).then(function success(res) {
                dataFac.recetas = res.data;
                $scope.recetas = dataFac.recetas;

            }, function error(err) {
                console.log("error cargar recetas");
                alert("error cargar recetas");
            })
        };

        $scope.select = function (receta) {
            receta.items.map(function (item) {
                item.desactivar = true;
            });
            $scope.receta = receta;

        }

        $scope.activar = function (item) {
            item.desactivar = false;
        }

        $scope.despachar = function (item) {
            var itemDespachado = angular.copy(item);
            $scope.despachado.push(itemDespachado);
        }

        $scope.guardarDespacho = function () {
            if ($scope.despachado.length == 0 || !$scope.despachado) {
                alert("no hay items a despachar");
            } else if ($scope.despachado.length != $scope.receta.items.length) {
                alert("La cantidad de items a despachar son diferentes a la receta")
            } else if ($scope.despachado.length == $scope.receta.items.length) {
                $scope.receta = null;
                $scope.despachado = [];
                alert("despacho exitoso");
            }
        }

        $scope.limpiarDespacho = function () {
            $scope.despachado = [];
        }

    }])
    .controller('historial', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $scope.nuevo_paciente = {};
        $scope.pacientes = {};
        $scope.laboratorios = {};


    }])
    .controller('stock', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {
        $scope.stock = dataFac.stock;

        if (dataFac.stock === null) {
            dataFac.getStock(dataFac.localidad).then(function success(res) {
                dataFac.stock = res.data;
                $scope.stock = dataFac.stock;
            }, function error(err) {
                console.log("error cargar stock");
                alert("error cargar stock");
            })
        };



    }])
