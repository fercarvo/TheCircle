angular.module('appMedico', ['ui.router', 'nvd3'])
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
        atencion.codigo = null;
        atencion.atencion = null;
        atencion.diagnosticos = null;
        atencion.remision = null;
        atencion.receta = [];
        atencion.receta.id = null;
        atencion.status = true;

        return atencion;
    }])
    .controller('atencion', ["$scope", "$state", "$http", "atencionFactory", "disable", function ($scope, $state, $http, atencionFactory, disable) {

        console.log("atencionFactory atencion", atencionFactory);
        $scope.disable = disable.atencion;
        $scope.apadrinado = atencionFactory.apadrinado;
        $scope.foto = atencionFactory.foto;
        $scope.status = atencionFactory.status;

        $scope.$on('disable', function (event, data) {
            $scope.disable = disable.atencion;
        });

        $scope.buscarApadrinado = function (codigo) {
            $http.get("/api/apadrinado/" + codigo).then(function success(res) {

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

        console.log("atencionFactory registro", atencionFactory);

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;

        $scope.activar = function () {
            $(".myselect").select2();
        }


        if (dataFactory.enfermedades == null) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log("error cargar enfermedades");
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
                atencionFactory.atencion = res.data.atencion; //Se guarda la data ingresada en la factory
                atencionFactory.diagnosticos = res.data.diagnosticos; //Se guarda la data ingresada en la factory
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('disable', {}); //evento para desactivar atencion.html
                $state.go('atencion.remision');

            }, function error(err){
              console.log("error atencion");
            });
        }

    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", function ($scope, $state, $http, disable, dataFactory, atencionFactory) {
        console.log("atencionFactory remision", atencionFactory);

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.diagnosticos

        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.instituciones == null) {
            dataFactory.getInstituciones().then(function success(res) {
                dataFactory.instituciones = res.data;
                $scope.instituciones = dataFactory.instituciones;
            }, function error(err) {
                console.log("error cargar instituciones");
            })
        }

        $scope.send = function (remision) {
            var RemisionRequest = {
                atencionM: atencionFactory.atencion.id,
                institucion: remision.institucion,
                monto: remision.monto,
                sintomas: remision.sintomas
            }

            $http.post("/api/remision", RemisionRequest).then(function success(res) {

                console.log("se creo remision", res.data);
                disable.remision = true;
                atencionFactory.remision = res.data; //Se guarda la remision en la factory
                $scope.disable = disable.remision; //Se desactiva atencion.remision.html

            }, function (err) {
                console.log("error crear remision");
            });
        }

    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {
        console.log("atencionFactory receta", atencionFactory);
        
        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.ItemRecetaNuevo = {};
        $scope.editarItem = true;
        $scope.diagnosticos = atencionFactory.diagnosticos;


        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.stock == null && atencionFactory.codigo != null) {
            dataFactory.getStock(atencionFactory.localidad).then(function success(res) {
                dataFactory.stock = res.data;
                $scope.stock = dataFactory.stock;
            }, function error(err) {
                console.log("error cargar itemFarmacia");
            })
        }

        if (atencionFactory.receta.id == null) {
            var RecetaRequest = {
              doctor: atencionFactory.doctor,
              apadrinado: atencionFactory.codigo };

            $http.post("/api/receta", RecetaRequest).then(function success(res) {
                console.log("Se creo receta", res.data);
                atencionFactory.receta.id = res.data.id;
                $scope.receta.id = atencionFactory.receta.id;
            }, function err(err) {
                console.log("No se pudo crear receta");
            });

        }

        $scope.addItenReceta = function (item) {
            $scope.editarItem = true;
            var obj= angular.copy(item);;
            atencionFactory.receta.push(obj);
            $scope.receta = atencionFactory.receta;
            item.diagnostico = {};
            item.cantidad = 0;
            item.posologia = "";
        }

        $scope.eliminarItem = function (receta, index){
            receta.splice(index, 1);
            atencionFactory.receta = receta;
        }

        $scope.select = function (item) {
            $scope.editarItem = false;
            $scope.ItemRecetaNuevo.itemFarmacia = angular.copy(item);
        }

        $scope.guardarReceta = function () {
            var data = { idReceta: atencionFactory.receta.id, items: atencionFactory.receta };

            $http.post("/api/itemsreceta", JSON.parse(angular.toJson(data))).then(function success(res) {
                console.log("Se crearon los items", res.data);
            }, function err(err){
                console.log("No se pudieron crear los items");
            });
        }


    }])
    .controller('anulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        

    }])
    .controller('estadisticas.1', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas.2', ["$scope", "$state", "$http", "atencionFactory", function ($scope, $state, $http, atencionFactory) {

        $scope.generar = function (desde, hasta) {

            var data = {
                desde: desde,
                hasta: hasta,
                apadrinado: atencionFactory.apadrinado.cod
            }

            $http.post("/api/reporte/enfermedad", data).then(function success(res) {

                var arr = [];

                res.data.forEach(function (obj) {
                    arr.push({ key: obj.codigo, y: obj.veces });
                });

                $scope.data = arr;

            }, function error(err) {
                console.log("Error cargar estadisticas");
            });

        }

        $scope.options = {
            chart: {
                type: 'pieChart',
                height: 500,
                x: function (d) { return d.key; },
                y: function (d) { return d.y; },
                showLabels: true,
                duration: 500,
                labelThreshold: 0.01,
                labelSunbeamLayout: true,
                legend: {
                    margin: {
                        top: 5,
                        right: 35,
                        bottom: 5,
                        left: 0
                    }
                }
            }
        };


    }])
