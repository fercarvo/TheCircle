angular.module('appMedico', ['ui.router', "ui.select"])
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
    .factory('dataFactory', ['$http', function ($http) {
        var fac = {};

        fac.getInstituciones = function () {
            return $http.get("/api/institucion");
        }

        fac.getEnfermedades = function () {
            return $http.get("/api/enfermedad");
        }

        fac.getStock = function (localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }

        return fac;
    }])
    .factory('disable', [function () {
        var disable = {};

        disable.atencion = false;
        disable.remision = false;

        return disable;
    }])
    .factory('atencionFactory', [function () { //factory donde se guarda toda la data ingresada
        var atencion = {};
        atencion.doctor = "705565656";
        atencion.localidad = "CC2";
        atencion.apadrinado = {};
        atencion.apadrinado.foto = "/images/ci.png";
        atencion.atencion = {};
        atencion.remision = {};
        atencion.receta = [];
        atencion.receta.id = {};
        atencion.apadrinado.status = true;

        return atencion;
    }])
    .controller('atencion', ["$scope", "$state", "$http", "atencionFactory", "disable", function ($scope, $state, $http, atencionFactory, disable) {

        $scope.disable = disable.atencion;
        $scope.apadrinado = atencionFactory.apadrinado;

        $scope.$on('disable', function (event, data) {
            $scope.disable = disable.atencion;
        });

        $scope.buscarApadrinado = function () {
            $http.get("/api/apadrinado/" + $scope.apadrinado.cod)
                .then(function success(res) {

                    console.log(res.data)

                    if (res.data.length === 0) {
                        $scope.apadrinado = {};
                        $scope.apadrinado.foto = "/images/ci.png";
                        $scope.apadrinado.status = false;
                        atencionFactory.apadrinado.status = false;
                        atencionFactory.apadrinado.cod = $scope.apadrinado.cod;
                    } else {

                        if (res.data[0].status == "D" || res.data[0].status == "E") {
                            $scope.apadrinado.status = false;
                            atencionFactory.apadrinado.status = false;
                        } else {
                            $scope.apadrinado.status = true;
                        }

                        console.log($scope.apadrinado.status);
                        $scope.apadrinado.foto = "/api/apadrinado/foto/" + $scope.apadrinado.cod;

                        $scope.apadrinado.nombres = res.data[0].nombres;
                        $scope.apadrinado.apellidos = res.data[0].apellidos;
                        $scope.apadrinado.sector = res.data[0].sector;
                        $scope.apadrinado.hogar = res.data[0].posesionHogar;
                        $scope.apadrinado.numPer = res.data[0].numPer;
                        $scope.apadrinado.income = res.data[0].income;
                        $scope.apadrinado.numBeds = res.data[0].numBeds;
                        $scope.apadrinado.edad = res.data[0].edad;

                        atencionFactory.apadrinado = $scope.apadrinado;
                    }

                }, function error(err, status) {
                    console.log(err, status);
                });
        };

    }])
    .controller('atencion.registro', ["$scope", "$http", "dataFactory", "atencionFactory", "disable", function ($scope, $http, dataFactory, atencionFactory, disable) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = ["curativo", "seguimiento", "control"];
        $scope.atencion = atencionFactory.atencion;


        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (!dataFactory.enfermedades) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log(err);
            })
        }



        $scope.reset = function () {
            $scope.atencion.tipo = {};
            $scope.atencion.diagp = {};
            $scope.atencion.diag1 = {};
            $scope.atencion.diag2 = {};
        };

        $scope.send = function () {
            var data = {
                doctor: atencionFactory.doctor,
                apadrinado: atencionFactory.apadrinado.cod,
                tipo: $scope.atencion.tipo,
                diagp: $scope.atencion.diagp,
                diag1: $scope.atencion.diag1,
                diag2: $scope.atencion.diag2
            }

            $http.post("/api/atencion", data).then(function success(res){

                console.log("se creo", res.data);

                disable.atencion = true;
                atencionFactory.atencion = $scope.atencion; //Se guarda la data ingresada en la factory
                atencionFactory.atencion.id = res.data[0].id
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('disable', {}); //evento para desactivar atencion.html
                $state.go('atencion.remision');

            }, function (err, status){
              console.log("error", err, status);
            });
        }


    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", function ($scope, $state, $http, disable, dataFactory, atencionFactory) {

        //console.log("atencionFactory", atencionFactory);

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = [
            atencionFactory.atencion.diagp,
            atencionFactory.atencion.diag1,
            atencionFactory.atencion.diag2]


        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (!dataFactory.instituciones) {
            dataFactory.getInstituciones().then(function success(res) {
                dataFactory.instituciones = res.data;
                $scope.instituciones = dataFactory.instituciones;
            }, function error(err) {
                console.log("error al cargar instituciones", err);
            })
        }



        $scope.send = function () {
            var data = {
                atencionM: atencionFactory.atencion.id,
                institucion: $scope.remision.institucion,
                monto: $scope.remision.monto
            }

            $http.post("/api/remision", data).then(function success(res) {

                console.log("se creo", res);
                disable.remision = true;
                atencionFactory.remision = $scope.remision; //Se guarda la data ingresada en la factory
                $scope.disable = disable.remision; //Se desactiva atencion.remision.html
                console.log("atencionFactory after remision", atencionFactory);

            }, function (err, status) {
                console.log("error", err, status);
            });
        }



    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {

        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.itemReceta = {};

        $scope.diagnosticos = [
            atencionFactory.atencion.diagp,
            atencionFactory.atencion.diag1,
            atencionFactory.atencion.diag2
        ]

        if (!dataFactory.stock) {
            dataFactory.getStock(atencionFactory.localidad).then(function success(res) {
                dataFactory.stock = res.data;
                $scope.stock = dataFactory.stock;
            }, function error(err) {
                console.log(err);
            })
        }

        $scope.creatReceta = function () {
            var data = {idDoctor: atencionFactory.doctor, idApadrinado: atencionFactory.apadrinado.cod};

            $http.post("/api/receta", data).then(function success(res) {
                console.log("Se creo receta", res.data[0]);
                atencionFactory.receta.id = res.data[0].id;
                $scope.receta.id = atencionFactory.receta.id;
            }, function err(err){
                console.log("No se pudo crear receta", err);
            });

        }

        $scope.addItenReceta = function () {
            $scope.editarItem = true;
            atencionFactory.receta.push($scope.itemReceta);
            $scope.receta = atencionFactory.receta;
        }

        $scope.eliminarItem = function (array, index){
            array.splice(index, 1);
            atencionFactory.splice(index, 1);
        }

        $scope.select = function (item) {
            $scope.editarItem = false;;
            $scope.itemReceta.id = item.id;
            $scope.itemReceta.nombre = item.nombre;
        }

        $scope.guardarReceta = function () {
            var data = {idReceta: atencionFactory.receta.id, items: atencionFactory.receta};
            console.log("data a enviar", data);

            $http.post("/api/itemsreceta", data).then(function success(res) {
                console.log("Se crearon los items", res.data);
            }, function err(err){
                console.log("No se pudieron crear los items", err);
            });
        }


    }])
    .controller('anulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas.1', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas.2', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
