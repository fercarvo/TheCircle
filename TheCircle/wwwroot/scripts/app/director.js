﻿/*
    director v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/

angular.module('director', ['ui.router', 'nvd3'])
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
            })
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
        
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);
        loadTemplates($state, "estadisticas", $http, $templateCache);

    }])
    .factory("dataFac", ["$http", function ($http) {

        var dataFac = {
            atenciones: null,
            getAtenciones: getAtenciones
        }

        function getAtenciones(data) {
            NProgress.start();

            var promise = $http({
                method: "GET",
                url: "/api/atencion/report/",
                params: data
            })

            promise.then(function success(res) {
                console.log("Atenciones", res.data)
                NProgress.done();
            }, function error(err) {
                console.log("Error cargar estadisticas", err);
                notify("No se pudo cargar las estadisticas", "danger");
                NProgress.done();
            })

            return promise
        }

        return dataFac;
        
    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $state.go("estadisticas.atenciones")

    }])
    .controller('estadisticas.atenciones', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

        $scope.atenciones = dataFac.atenciones;

        var color = ["#901F61", "#009877", "#D64227", "#FED115", "#ADBF2B"];

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getAtenciones(data).then(function (res) {
                $scope.data = [];

                res.data.forEach(function (obj, index, array) {
                    $scope.data.push({
                        key: obj.localidad,
                        y: obj.cantidad,
                        color: color[index]
                    })
                })
            }, function () { })
        }

        $scope.options = {
            chart: {
                type: 'pieChart',
                height: 340,
                x: function (d) { return d.key; },
                y: function (d) { return d.y; },
                showLabels: false,
                duration: 500,
                labelThreshold: 0,
                labelSunbeamLayout: true,
                legendPosition: "right",
                legend: {
                    margin: {
                        top: 5,
                        right: 35,
                        bottom: 5,
                        left: 0
                    }
                }
            }
        }

    }])
    .controller('estadisticas.remisiones', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])
    .controller('estadisticas.recetas', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])
    .controller('estadisticas.enfermedades', ["$scope", "$state", "$http", function ($scope, $state, $http) {

    }])