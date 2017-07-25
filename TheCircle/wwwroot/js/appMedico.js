angular.module('appMedico', ['ui.router', "ngSanitize", "ui.select"])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('atencion', {
                //url: '/atencion',
                templateUrl: 'html/medico/atencion.html',
                controller: 'atencion'
            })
            .state('atencion.registro', {
                //url: '/registro',
                templateUrl: 'html/medico/atencion.registro.html',
                controller: 'atencion.registro'
            })
            .state('atencion.receta', {
                //url: '/receta',
                templateUrl: 'html/medico/atencion.receta.html',
                controller: 'atencion.receta'
            })
            .state('atencion.remision', {
                //url: '/remision',
                templateUrl: 'html/medico/atencion.remision.html',
                controller: 'atencion.remision'
            })
            .state('anulaciones', {
                //url: '/anulaciones',
                templateUrl: 'html/medico/anulaciones.html',
                controller: 'anulaciones'
            })
            .state('estadisticas', {
                //url: '/estadisticas',
                templateUrl: 'html/medico/estadistica.html',
                controller: 'estadisticas'
            })
            .state('estadisticas.1', {
                //url: '/1',
                templateUrl: 'html/medico/estadistica.1.html',
                controller: 'estadisticas.1'
            })
            .state('estadisticas.2', {
                //url: '/2',
                templateUrl: 'html/medico/estadistica.2.html',
                controller: 'estadisticas.2'
            });
        //$urlRouterProvider.otherwise("/atencion/registro");
        $urlRouterProvider.otherwise(function ($injector) {
            var $state = $injector.get('$state');
            $state.go('atencion.registro');
        });
    })
    .controller('atencion', ["$scope", "$state", "$http", function ($scope, $state, $http) {

        $scope.apadrinado = {};
        $scope.apadrinado.cod = "";

        $scope.url = "/images/ci.png";
        
        $scope.buscarApadrinado = function () {
            $http.get("/api/apadrinado/" + $scope.apadrinado.cod)
                .then(function success(res) {

                    if (res.data.length === 0) {
                        $scope.apadrinado = {};
                        $scope.url = "/images/ci.png";
                    } else {

                        $scope.url = "/api/Foto/" + $scope.apadrinado.cod;

                        $scope.apadrinado.nombres = res.data[0].nombres;
                        $scope.apadrinado.apellidos = res.data[0].apellidos;
                        $scope.apadrinado.sector = res.data[0].sector;
                        $scope.apadrinado.hogar = res.data[0].posesionHogar;
                        $scope.apadrinado.numPer = res.data[0].numPer;
                        $scope.apadrinado.income = res.data[0].income;
                        $scope.apadrinado.numBeds = res.data[0].numBeds;
                        $scope.apadrinado.edad = res.data[0].edad;
                    }                 

                }, function error() {
                    console.log("error de conexion");
                });
        };

    }])
    .controller('atencion.registro', ["$scope", "$state", "$http", function ($scope, $state, $http) {

        $scope.activar = function () {
            $(".myselect").select2();
        }

        $scope.itemArray = [
            { id: 1, name: 'first' },
            { id: 2, name: 'second' },
            { id: 3, name: 'third' },
            { id: 4, name: 'fourth' },
            { id: 5, name: 'fifth' },
        ];

        $scope.selected = { value: $scope.itemArray[0] };


    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", function ($scope, $state, $http) {



    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", function ($scope, $state, $http) {



    }])
    .controller('anulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas.1', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas.2', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
