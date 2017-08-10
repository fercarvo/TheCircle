angular.module('appMedico', ['ui.router', 'nvd3'])
    .config(function ($stateProvider, $urlRouterProvider) {
        $stateProvider
            .state('atencion', {
                templateUrl: 'views/medico/atencion.html',
                controller: 'atencion'
            })
            .state('atencion.registro', {
                templateUrl: 'views/medico/atencion.registro.html',
                controller: 'atencion.registro'
            })
            .state('atencion.receta', {
                templateUrl: 'views/medico/atencion.receta.html',
                controller: 'atencion.receta'
            })
            .state('atencion.remision', {
                templateUrl: 'views/medico/atencion.remision.html',
                controller: 'atencion.remision'
            })
            .state('anulaciones', {
                templateUrl: 'views/medico/anulaciones.html',
                controller: 'anulaciones'
            })
            .state('estadisticas', {
                templateUrl: 'views/medico/estadistica.html',
                controller: 'estadisticas'
            })
            .state('estadisticas.atenciones', {
                templateUrl: 'views/medico/estadistica.atenciones.html',
                controller: 'estadisticas.atenciones'
            })
            .state('estadisticas.remisiones', {
                templateUrl: 'views/medico/estadistica.remisiones.html',
                controller: 'estadisticas.remisiones'
            })
            .state('estadisticas.recetas', {
                templateUrl: 'views/medico/estadistica.recetas.html',
                controller: 'estadisticas.recetas'
            })
            .state('estadisticas.enfermedades', {
                templateUrl: 'views/medico/estadistica.enfermedades.html',
                controller: 'estadisticas.enfermedades'
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
        dataFactory.estadisticas = {};

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
    .factory('notify', [function () {

        var notify = {};

        notify = function (titulo, mensaje, tipo) {

            var icono;

            if (tipo === "success") {
                icono = "glyphicon glyphicon-saved";
            } else if (tipo == "danger") {
                icono = "glyphicon glyphicon-ban-circle"
            }

            $.notify(
                {
                    icon: icono,
                    title: titulo,
                    message: mensaje,
                    url: '#',
                    target: '_blank'
                },
                {
                    element: 'body',
                    position: null,
                    showProgressbar: true,
                    type: tipo,
                    allow_dismiss: true,
                    newest_on_top: false,
                    showProgressbar: false,
                    placement: {
                        from: "top",
                        align: "right"
                    },
                    offset: {x:20,y:70},
                    spacing: 10,
                    z_index: 1031,
                    delay: 1000,
                    timer: 1000,
                    url_target: '_blank',
                    mouse_over: "pause",
                    animate: {
                        enter: 'animated bounceIn',
                        exit: 'animated bounceOut'
                    },
                    onShow: null,
                    onShown: null,
                    onClose: null,
                    onClosed: null,
                    icon_type: 'class'
                });
        };

        return notify;
    }])
    .factory('atencionFactory', [function () { //factory donde se guarda toda la data ingresada
        var atencion = {};
        atencion.doctor = 908362247;
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

        $state.go('atencion.registro');
        $scope.disable = disable.atencion;
        $scope.apadrinado = atencionFactory.apadrinado;
        $scope.foto = atencionFactory.foto;
        $scope.status = atencionFactory.status;

        $scope.$on('disable', function (event, data) {
            $scope.disable = disable.atencion;
        });

        $scope.buscarApadrinado = function (codigo) {
            $http.get("/api/apadrinado/" + codigo).then(function success(res) {

                if (res.data.status === "D" || res.data.status === "E") {
                    $scope.status = false;
                    atencionFactory.status = false;
                } else {
                    $scope.status = true;
                }
                $scope.foto = "/api/apadrinado/foto/" + codigo;
                atencionFactory.apadrinado = res.data;
                $scope.apadrinado = atencionFactory.apadrinado;
                atencionFactory.codigo = codigo;

            }, function error(err) {

                console.log(err);
                atencionFactory.apadrinado = {};
                $scope.apadrinado = atencionFactory.apadrinado;
                atencionFactory.foto = "/images/ci.png";
                $scope.foto = atencionFactory.foto;
                atencionFactory.status = true;
                $scope.status = atencionFactory.status;
                atencionFactory.codigo = null;
                $scope.codigo = atencionFactory.codigo;
            });
        };

    }])
    .controller('atencion.registro', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", "disable", "notify", function ($scope, $state, $http, dataFactory, atencionFactory, disable, notify) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;

        $scope.activar = function () {
            $(".myselect").select2();
        }


        if (dataFactory.enfermedades === null) {
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
                  atencionFactory.atencion.diag2],
                localidad: atencionFactory.localidad,
                peso: atencionFactory.atencion.peso,
                talla: atencionFactory.atencion.talla
            }

            $http.post("/api/atencion", AtencionNueva).then(function success(res){

                console.log("se creo atencion", res.data);
                disable.atencion = true;
                atencionFactory.atencion = res.data.atencion; //Se guarda la data ingresada en la factory
                atencionFactory.diagnosticos = res.data.diagnosticos; //Se guarda la data ingresada en la factory
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('disable', {}); //evento para desactivar atencion.html
                $state.go('atencion.remision');
                notify("Exito", "Apadrinado creado satisfactoriamente", "success");

            }, function error(err) {
                notify("Error", "Intento fallido de atencion medica", "danger");
                console.log("error atencion");
            });
        }

    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory","notify", function ($scope, $state, $http, disable, dataFactory, atencionFactory,notify) {
        console.log("atencionFactory remision", atencionFactory);

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.diagnosticos

        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.instituciones === null) {
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
                notify("Exito", "Se creo la remision exitosamente", "success");

            }, function (err) {
                console.log("error crear remision");
                notify("Error", "No se pudo generar la remision", "danger");
            });
        }

    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "dataFactory", "atencionFactory","notify", function ($scope, $state, $http, dataFactory, atencionFactory,notify) {

        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.ItemRecetaNuevo = {};
        $scope.editarItem = true;
        $scope.diagnosticos = atencionFactory.diagnosticos;


        $scope.activar = function () {
            $(".myselect").select2();
        }

        if (dataFactory.stock === null && atencionFactory.codigo !== null) {
            dataFactory.getStock(atencionFactory.localidad).then(function success(res) {
                dataFactory.stock = res.data;
                $scope.stock = dataFactory.stock;
            }, function error(err) {
                console.log("error cargar itemFarmacia");
            })
        };

        if (atencionFactory.receta.id === null) {
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
                notify("Exito", "Se creo la receta exitosamente", "success");
            }, function err(err){
                console.log("No se pudieron crear los items");
                notify("Error", "No se pudo crear la receta", "danger");
            });
        }


    }])
    .controller('anulaciones', ["$scope", "$state", "$http", function ($scope, $state, $http) {


    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $state.go('estadisticas.enfermedades');

    }])
    .controller('estadisticas.atenciones', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {
        $scope.atenciones = dataFactory.estadisticas.atenciones;

        $scope.$watch('atenciones', function () {
            dataFactory.estadisticas.atenciones = $scope.atenciones;
        });

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta,
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/atencion", data).then(function success(res) {
                $scope.atenciones.all = res.data;
            }, function error(err) {
                console.log("error cargar atenciones")
                alert("error cargar atenciones")
            });
        }

    }])
    .controller('estadisticas.remisiones', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {
        $scope.remisiones = dataFactory.estadisticas.remisiones;
        $scope.$watch('remisiones', function () {
            dataFactory.estadisticas.remisiones = $scope.remisiones;
        });

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta,
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/remision", data).then(function success(res) {
                $scope.remisiones.all = res.data;
            }, function error(err) {
                console.log("error cargar remisiones");
                alert("error cargar remisiones")
            });
        }

    }])
    .controller('estadisticas.recetas', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {
        $scope.recetas = dataFactory.estadisticas.recetas;
        $scope.$watch('recetas', function () {
            dataFactory.estadisticas.recetas = $scope.recetas;
        });

        $scope.activar = function (index) {
            index = ""

        }

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta,
                doctor: atencionFactory.doctor
            }

            $http.post("/api/reporte/receta", data).then(function success(res) {

                dataFactory.estadisticas.recetas.all = res.data;
                $scope.recetas.all = dataFactory.estadisticas.recetas.all;

            }, function error(err) {
                console.log("error cargar recetas")
                alert("error cargar recetas")
            });
        }

    }])
    .controller('estadisticas.enfermedades', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", function ($scope, $state, $http, dataFactory, atencionFactory) {

        $scope.enfermedades = dataFactory.estadisticas.enfermedades;
        $scope.data = [];

        $scope.$watch('enfermedades', function () {
            dataFactory.estadisticas.enfermedades = $scope.enfermedades;
        });

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: desde,
                hasta: hasta,
                localidad: atencionFactory.localidad
            }
            $http.post("/api/reporte/enfermedad", data).then(function success(res) {
                //var arr = [];

                for (i = 0; i < res.data.length; i++) {
                    $scope.data.push({ key: res.data[i].codigo + ' ' + res.data[i].nombre, y: res.data[i].veces, color: color[i] });
                }

                /*$scope.data = res.data.map(function (obj) {
                    return { key: obj.codigo + ' ' + obj.nombre, y: obj.veces, color: "red" };
                });*/
                //$scope.data = arr;
            }, function error(err) {
                console.log("Error cargar estadisticas");
                alert("Error cargar estadisticas");
            });

        }

        var color = ["#901F61", "#009877", "#D64227", "#FED115", "#ADBF2B"];

        $scope.options = {
            chart: {
                type: 'pieChart',
                height: 500,
                x: function (d) { return d.key; },
                y: function (d) { return d.y; },
                showLabels: false,
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
