angular.module('appAsistente', ['ui.router'])
    .config(function ($stateProvider, $urlRouterProvider) {
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
        //$urlRouterProvider.otherwise("/atencion/registro");
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('despachar');
        });
    })
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

        if (dataFac.recetas === null) {
            dataFac.getRecetas(dataFac.localidad).then(function success(res) {
                dataFac.recetas = res.data;
                $scope.recetas = dataFac.recetas;
            }, function error(err) {
                console.log("error cargar recetas");
            })
        };

        $scope.select = function (receta) {
            $scope.receta = receta;

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
            })
        };
        


    }])
