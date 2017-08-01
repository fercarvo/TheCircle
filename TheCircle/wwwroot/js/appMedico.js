angular.module('appMedico', ['ui.router'])
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
        var dataFactory = {};

        dataFactory.enfermedades = null;
        dataFactory.instituciones = null;
        dataFactory.stock = null;

        dataFactory.tipos = ["curativo", "seguimiento", "control"];

        dataFactory.getInstituciones = function () {
            return $http.get("/api/institucion");
        }

        dataFactory.getEnfermedades = function () {
            return $http.get("/api/enfermedad");
        }

        dataFactory.getStock = function (localidad) {
            return $http.get("/api/itemfarmacia/" + localidad);
        }

        return dataFactory;
    }])
    .factory('disable', [function () {
        var disable = {};

        disable.atencion = false;
        disable.remision = false;

        return disable;
    }])
    .factory('atencionFactory', [function () { //factory donde se guarda toda la data ingresada
        var atencion = {};
        atencion.doctor = 705565656;
        atencion.localidad = "CC2";
        atencion.apadrinado = {};
        atencion.foto = "/images/ci.png";
        atencion.codigo = {};
        atencion.atencion = {};
        atencion.atencion.diag1 = null;
        atencion.atencion.diag2 = null;
        atencion.atencion.diagp = null;
        atencion.remision = {};
        atencion.receta = [];
        atencion.receta.id = null;
        atencion.status = false;

        return atencion;
    }])
    .controller('atencion', ["$scope", "$state", "$http", "atencionFactory", "disable", function ($scope, $state, $http, atencionFactory, disable) {

        $scope.disable = disable.atencion;
        $scope.apadrinado = atencionFactory.apadrinado;
        $scope.foto = atencionFactory.foto;

        $scope.$on('disable', function (event, data) {
            $scope.disable = disable.atencion;
        });

        $scope.buscarApadrinado = function (codigo) {
            $http.get("/api/apadrinado/" + codigo)
                .then(function success(res) {
                    if (res.data) {
                        if (res.data.status == "D" || res.data.status == "E") {
                            $scope.status = false;
                            atencionFactory.status = false;
                        } else {
                            $scope.status = true;
                        }

                        $scope.foto = "/api/apadrinado/foto/" + codigo;
                        atencionFactory.apadrinado = res.data;
                        $scope.apadrinado = atencionFactory.apadrinado;
                        atencionFactory.codigo = codigo;

                    } else {
                        atencionFactory.apadrinado = {};
                        $scope.apadrinado = atencionFactory.apadrinado;
                        atencionFactory.foto = "/images/ci.png";
                        $scope.foto = atencionFactory.foto;
                        atencionFactory.status = true;
                        $scope.status = atencionFactory.status;
                        atencionFactory.codigo = {};
                        $scope.codigo = atencionFactory.codigo;
                    }

                }, function error(err, status) {
                    console.log(err, status);
                    atencionFactory.apadrinado = {};
                    $scope.apadrinado = atencionFactory.apadrinado;
                    atencionFactory.foto = "/images/ci.png";
                    $scope.foto = atencionFactory.foto;
                    atencionFactory.status = true;
                    $scope.status = atencionFactory.status;
                    atencionFactory.codigo = {};
                    $scope.codigo = atencionFactory.codigo;
                });
        };

    }])
    .controller('atencion.registro', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", "disable", function ($scope, $state, $http, dataFactory, atencionFactory, disable) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;


        $scope.activar = function () {
            $(".myselect").select2();
        }


        if (dataFactory.enfermedades==null) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log(err);
            })
        }


        $scope.reset = function () {
            $scope.atencion = {};
        };

        $scope.$watch('atencion', function() {
            atencionFactory.atencion = $scope.atencion;
        });

        $scope.send = function () {
            var AtencionNueva = {
                doctor: atencionFactory.doctor,
                apadrinado: atencionFactory.codigo,
                tipo: atencionFactory.atencion.tipo,
                diagnosticos: [atencionFactory.atencion.diagp,
                  atencionFactory.atencion.diag1,
                  atencionFactory.atencion.diag2]
            }

            $http.post("/api/atencion", AtencionNueva).then(function success(res){

                console.log("se creo atencion", res.data);

                disable.atencion = true;
                atencionFactory.atencion = res.data; //Se guarda la data ingresada en la factory
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('disable', {}); //evento para desactivar atencion.html
                $state.go('atencion.remision');

            }, function (err, status){
              console.log("error atencion", err, status);
            });
        }


    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", function ($scope, $state, $http, disable, dataFactory, atencionFactory) {


        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.atencion.diagnosticos


        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.instituciones==null) {
            dataFactory.getInstituciones().then(function success(res) {
                dataFactory.instituciones = res.data;
                $scope.instituciones = dataFactory.instituciones;
            }, function error(err) {
                console.log("error al cargar instituciones", err);
            })
        }

        $scope.send = function (remision) {
            var RemisionNueva = {
                atencionM: atencionFactory.atencion.atencion.id,
                institucion: remision.institucion,
                monto: remision.monto,
                sintomas: remision.sintomas
            }

            $http.post("/api/remision", RemisionNueva).then(function success(res) {

                console.log("se creo remision", res);
                disable.remision = true;
                atencionFactory.remision = $scope.remision; //Se guarda la data ingresada en la factory
                $scope.disable = disable.remision; //Se desactiva atencion.remision.html

            }, function (err, status) {
                console.log("error crear remision", err, status);
            });
        }



    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {

        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.ItemRecetaNuevo = {};
        $scope.editarItem = true;
        $scope.diagnosticos = atencionFactory.atencion.diagnosticos;

        if (dataFactory.stock==null) {
            dataFactory.getStock(atencionFactory.localidad).then(function success(res) {
                dataFactory.stock = res.data;
                $scope.stock = dataFactory.stock;
            }, function error(err) {
                console.log(err);
            })
        }

        if (atencionFactory.receta==null) {

            var RecetaNueva = { idDoctor: atencionFactory.doctor, idApadrinado: atencionFactory.apadrinado.cod };

            $http.post("/api/receta", RecetaNueva).then(function success(res) {
                console.log("Se creo receta", res.data);
                atencionFactory.receta.id = res.data.id;
                $scope.receta.id = atencionFactory.receta.id;
            }, function err(err) {
                console.log("No se pudo crear receta", err);
            });

        }

        $scope.addItenReceta = function (item) {
            $scope.editarItem = true;
            atencionFactory.receta.push(item);
            $scope.receta = atencionFactory.receta;
        }

        $scope.eliminarItem = function (receta, index){
            receta.splice(index, 1);
            atencionFactory.receta = array;
        }

        $scope.select = function (item) {
            $scope.editarItem = false;
            $scope.ItemRecetaNuevo.itemFarmacia = item
        }

        $scope.guardarReceta = function () {
            var data = { idReceta: atencionFactory.receta.id, items: atencionFactory.receta };

            console.log("data a enviar", JSON.parse(angular.toJson(data)));

            $http.post("/api/itemsreceta", JSON.parse(angular.toJson(data))).then(function success(res) {
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
