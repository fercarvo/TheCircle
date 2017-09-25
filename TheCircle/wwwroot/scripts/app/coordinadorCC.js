/*
 appCoordinador v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/

angular.module('coordinadorCC', ['ui.router', 'ngCookies'])
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
        $compileProvider.debugInfoEnabled(true); //Activar en modo producción
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", "$templateCache", function ($state, $rootScope, $cookies, $http, $templateCache) {

        refresh.go(function () {
            $http.get("session").then(function () { console.log("Session valida") }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('/login');
                }
            })
        }, 20) //minutos


        var promises = []
        var states = $state.get()
        NProgress.start()

        for (i = 1; i < states.length; i++) {
            var p = $http.get(states[i].templateUrl, { cache: $templateCache })
            promises.push(p)
            p.then(function () { }, function (error) { console.log("Error template: ", error) })
        }

        Promise.all(promises)
            .then(function () { }).catch(function () { }).then(function () {
                NProgress.done()
                $state.go("recetas") /////////////////////////
            })

        $rootScope.session_name = $cookies.get('session_name')
        $rootScope.session_email = $cookies.get('session_email')
        $rootScope.session_photo = $cookies.get('session_photo')        
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

        function getEgresos(data) {
            NProgress.start()
            var promise = $http({
                method: "GET",
                url: "/api/itemfarmacia/report/egresos",
                params: data
            })

            promise.then(function (res) {
                NProgress.done();
                console.log("Egresos", res.data)
            }, function (err) {
                console.log("error cargar egresos", err)
                notify("No se pudo cargar los egresos", "danger");
                NProgress.done();
            })

            return promise;
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
    .controller('egresos', ["$scope", "$state", "$http", "dataFac", function ($scope, $state, $http, dataFac) {

        $scope.egresos = dataFac.egresos;

        $scope.$watch("egresos", function () {
            dataFac.egresos = $scope.egresos
        })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta
            }

            dataFac.getEgresos(data).then(function (res) {
                $scope.egresos = res.data
            }, function () { })
        }

    }])