/*
    coordinadorCC v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/

angular.module('coordinadorCC', ['ui.router'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('recetas', {
                templateUrl: 'views/coordinadorCC/recetas.html',
                controller: 'recetas'
            })
            .state('egresos', {
                templateUrl: 'views/coordinadorCC/egresos.html',
                controller: 'egresos'
            });
        //False en modo de produccion
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "recetas", $http, $templateCache);   
    }])
    .factory('dataFac', ['$http', function ($http) {
        var dataFac = {
            recetas: {},
            egresos: {},
            stock: null,
            getRecetas: getRecetas,
            getEgresos: getEgresos,
            getStock: null
        }

        
        function getRecetas(data) {

            var noti = notify("Cargando recetas", "success", true)
            NProgress.start()

            var promise = $http({
                method: "GET",
                url: "/api/receta/localidad/fecha",
                params: data
            })

            promise.then(function (res) {
                NProgress.done();
                console.log("recetas", res.data)
                noti.close();
            }, function (err) {
                console.log("error cargar recetas", err)
                notify("No se pudo cargar recetas", "danger")
                NProgress.done();
                noti.close();
            })

            return promise;
        }

        function getEgresos(data, $scope) {
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/itemfarmacia/report/egresos",
                params: data
            }).then(function (res) {
                NProgress.done();
                console.log("Egresos", res.data)
                $scope.egresos.data = res.data
            }, function (err) {
                console.log("error cargar egresos", err)
                notify("No se pudo cargar los egresos", "danger");
                NProgress.done();
            })
        }

        return dataFac;
    }])
    .controller('recetas', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

        $scope.recetas = dataFac.recetas
        $scope.receta = null

        $scope.$watch("recetas", function () {
            dataFac.recetas = $scope.recetas
        })

        $scope.select = function (receta) {
            $scope.receta = receta
        }

        $scope.generar = function (desde, hasta) {
            var fecha = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getRecetas(fecha).then(function (res) {
                $scope.recetas.all = res.data
            }, function () { })

        }

    }])
    .controller('egresos', ["$scope", "dataFac", function ($scope, dataFac) {

        $scope.egresos = dataFac.egresos;

        $scope.$watch("egresos", function () { dataFac.egresos = $scope.egresos })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getEgresos(data, $scope);
        }

    }])