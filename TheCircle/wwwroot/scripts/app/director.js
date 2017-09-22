angular.module('director', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('estadisticas', {
                templateUrl: 'views/director/estadisticas.html',
                controller: 'estadisticas'
            })
            .state('estadisticas.atenciones', {
                templateUrl: 'views/director/estadisticas.atenciones.html',
                controller: 'estadisticas.atenciones'
            })
            .state('estadisticas.remisiones', {
                templateUrl: 'views/director/estadisticas.remisiones.html',
                controller: 'estadisticas.remisiones'
            })
            .state('estadisticas.recetas', {
                templateUrl: 'views/director/estadisticas.recetas.html',
                controller: 'estadisticas.recetas'
            })
            .state('estadisticas.enfermedades', {
                templateUrl: 'views/director/estadisticas.enfermedades.html',
                controller: 'estadisticas.enfermedades'
            });
        
    }])
    .run(["$state", "$rootScope", "$http", function ($state, $rootScope, $http) {

        $state.go("estadisticas")

    }])

    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $state.go("estadisticas.atenciones")

    }])
    .controller('estadisticas.atenciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $log.info("En Estadisticas de atenciones")

        $scope.atenciones = null;

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta)
            }
            NProgress.start();

            $http({
                method: "GET",
                url: "/api/reporte/enfermedad/date",
                params: data
            }).then(function success(res) {
                $scope.data = [];

                for (i = 0; i < res.data.length; i++) {
                    $scope.data.push({ key: res.data[i].codigo + ' ' + res.data[i].nombre, y: res.data[i].veces, color: color[i] });
                }
                NProgress.done();
            }, function error(err) {
                console.log("Error cargar estadisticas", err);
                notify("No se pudo cargar las enfermedades", "danger");
                NProgress.done();
            })

        }

    }])
    .controller('estadisticas.remisiones', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $log.info("En Estadisticas de remisiones");

    }])
    .controller('estadisticas.recetas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $log.info("En Estadisticas de recetas");

    }])
    .controller('estadisticas.enfermedades', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $log.info("En Estadisticas de enfermedades");

    }])