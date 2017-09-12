/*
 appMedico v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/
angular.module('appMedico', ['ui.router', 'nvd3', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
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
            })
            .state('pedidos', {
                templateUrl: 'views/medico/pedidos.html',
                controller: 'pedidos'
            })
            .state('pedidos.transferencia', {
                templateUrl: 'views/medico/pedidos.transferencia.html',
                controller: 'pedidos.transferencia'
            })
            .state('pedidos.interno', {
                templateUrl: 'views/medico/pedidos.interno.html',
                controller: 'pedidos.interno'
            });
        $compileProvider.debugInfoEnabled(true); //false en modo de produccion
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", "refresh", function ($state, $rootScope, $cookies, $http, refresh) {

        refresh.goTime(function () {
            $http.get("login").then(function () {
            }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('logout');
                }
            })
        }, 1000 * 60 * 20)       

        $rootScope.session_name = (function () {
            var c = $cookies.get('session_name')
            if (c) {
                return c
            } return ""
        })() 

        $rootScope.session_email = (function () {
            var c = $cookies.get('session_email')
            if (c) {
                return c
            } return ""
        })()

        $rootScope.session_photo = (function () {
            var c = $cookies.get('session_photo')
            if (c) {
                return c
            } return "/images/ci.png"
        })()

        $state.go("atencion");
    }])
    .factory('date', [function () {
        return function (date) {
            var format = new Date(date);
            var day = format.getDate();
            var month = format.getMonth() + 1;
            var year = format.getFullYear();

            return day + '/' + month + '/' + year;
        };
    }])
    .factory('dataFactory', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFactory = {
            enfermedades: null,
            instituciones: null,
            stock: null,
            recetas: null,
            estadisticas: {},
            tipos: ["curativo", "seguimiento", "control"],
            getInstituciones: getInstituciones,
            getEnfermedades: getEnfermedades,
            getStock: getStock,
            getRecetas: getRecetas,
            getApadrinado: getApadrinado
        }

        function getInstituciones() {
            return $http.get("/api/institucion", {cache: true});
        }

        function getEnfermedades() {
            return $http.get("/api/enfermedad", {cache: true});
        }

        function getStock() {
            //NProgress.start();
            $http.get("/api/itemfarmacia").then(function success(res) {
                console.log("Actualizando Stock by localidad");
                dataFactory.stock = res.data;
                $rootScope.$broadcast('dataFactory.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                console.log("Error cargar Stock de farmacia", err);
            })
        }

        function getRecetas() {
            $http.get("api/receta/medico/activas").then(function success(res) {
                console.log("recetas by status", res.data);
                dataFactory.recetas = res.data;
                $rootScope.$broadcast('dataFactory.recetas'); //Se informa a los controladores que cambio recetas
            }, function error(err) {
                console.log("error cargar recetas", err);
            })
        }

        function getApadrinado(codigo) {
            NProgress.start();
            var promise = $http.get("/api/apadrinado/" + codigo, {cache: true});            

            promise.then(function(){
                NProgress.done();
            }, function(err){
                console.log("No existe apadrinado", err);
                NProgress.done();
            })

            return promise;
        }

        return dataFactory;
    }])
    .factory('disable', [function () {
        return {
            atencion : false,
            remision : false,
            receta : false
        }
    }])
    .factory('notify', [function () {
        return function (mensaje, tipo) {

            var icono = "";

            if (tipo === "success") {
                icono = "glyphicon glyphicon-saved";
            } else if (tipo === "danger") {
                icono = "glyphicon glyphicon-ban-circle"
            }

            return $.notify(
                {
                    icon: icono,
                    message: mensaje,
                    url: '#',
                    target: '_blank'
                },
                {
                    element: 'body',
                    position: null,
                    showProgressbar: false,
                    type: tipo,
                    allow_dismiss: true,
                    newest_on_top: false,
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
        }
    }])
    .factory('atencionFactory', [function () { //factory donde se guarda toda la data ingresada
        return {
            apadrinado : {},
            foto : "/images/ci.png",
            codigo : null,
            atencion : null,
            diagnosticos : null,
            remision : null,
            receta: {
                items: [],
                id: null
            },
            status : true
        }
    }])
    .factory('refresh', [function () { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            console.log("Go refresh");
            return setInterval(fn, 10000);
        }

        function goTime(fn, time) {
            fn();
            console.log("Go refresh by ", time);
            return setInterval(fn, time);
        }

        function stop(repeater) {
            console.log("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop,
            goTime: goTime
        }
    }])
    .controller('atencion', ["$scope", "$state", "atencionFactory", "dataFactory", "disable", function ($scope, $state, atencionFactory, dfac, disable) {

        $state.go('atencion.registro');
        $scope.codigo = atencionFactory.codigo;
        $scope.disable = disable.atencion;        
        $scope.apadrinado = atencionFactory.apadrinado;
        $scope.foto = atencionFactory.foto;
        $scope.status = atencionFactory.status;

        if (disable.atencion === false) {
            cargar();
        }

        function cargar() {
            //Se desactiva el codigo de apadrinado y se bloquea la informacion del mismo.
            var unregister1 = $scope.$on('guardar', function () {
                $scope.disable = disable.atencion;
                unregister1(); //Deja de escuchar
                unregister2(); //Deja de escuchar 
            })

            var unregister2 = $scope.$watch('apadrinado', function () {
                atencionFactory.apadrinado = $scope.apadrinado;
                atencionFactory.status = $scope.status;
                atencionFactory.codigo = $scope.codigo;
                atencionFactory.foto = $scope.foto;
            })
        }



        $scope.buscarApadrinado = function (codigo) {
            dfac.getApadrinado(codigo).then(function success(res) {

                if (res.data.status === "D" || res.data.status === "E") {
                    $scope.status = false;
                } else {
                    $scope.status = true;
                }
                $scope.foto = "/api/apadrinado/" + codigo + "/foto";
                $scope.apadrinado = res.data;

            }, function error(err) {

                $scope.foto = "/images/ci.png";
                $scope.status = true
                $scope.codigo = null;
                $scope.apadrinado = {};
            })
        } 

    }])
    .controller('atencion.registro', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", "disable", "notify", function ($scope, $state, $http, dataFactory, atencionFactory, disable, notify) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = dataFactory.enfermedades;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;

        if (dataFactory.enfermedades === null) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log("Error cargar enfermedades", err);
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
                apadrinado: atencionFactory.codigo,
                tipo: atencionFactory.atencion.tipo,
                diagnosticos: [atencionFactory.atencion.diagp,
                  atencionFactory.atencion.diag1,
                  atencionFactory.atencion.diag2],
                peso: atencionFactory.atencion.peso,
                talla: atencionFactory.atencion.talla
            }

            NProgress.start();
            $http.post("/api/atencion", AtencionNueva).then(function success(res){

                NProgress.done();
                console.log("Se creo atencion", res.data);
                disable.atencion = true;
                atencionFactory.atencion = res.data.atencion; //Se guarda la data ingresada en la factory
                atencionFactory.diagnosticos = res.data.diagnosticos; //Se guarda la data ingresada en la factory
                $scope.disable = disable.atencion; //Se desactiva atencion.registro.html
                $scope.$emit('guardar'); //Guarda y bloquea la atencion medica
                notify("Atención creada satisfactoriamente", "success");
                $state.go('atencion.receta');

            }, function error(err) {
                NProgress.done();
                notify("Intento fallido de atencion medica", "danger");
                console.log("error atencion", err);
            });
        }

    }])
    .controller('atencion.remision', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory","notify", function ($scope, $state, $http, disable, dataFactory, atencionFactory,notify) {

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.diagnosticos

        if (dataFactory.instituciones === null) {
            dataFactory.getInstituciones().then(function success(res) {
                dataFactory.instituciones = res.data;
                $scope.instituciones = dataFactory.instituciones;
            }, function error(err) {
                console.log("error cargar instituciones", err);
            })
        }

        $scope.send = function (remision) {
            var RemisionRequest = {
                atencionM: atencionFactory.atencion.id,
                institucion: remision.institucion,
                monto: remision.monto,
                sintomas: remision.sintomas
            }

            NProgress.start();
            $http.post("/api/remision", RemisionRequest).then(function success(res) {

                NProgress.done();
                console.log("se creo remision", res.data);
                disable.remision = true;
                atencionFactory.remision = $scope.remision; //Se guarda la remision en la factory
                $scope.disable = disable.remision; //Se desactiva atencion.remision.html
                notify("Se creo la remision exitosamente", "success");

            }, function (err) {
                NProgress.done();
                console.log("error crear remision", err);
                notify("No se pudo generar la remision, por favor verifique los datos", "danger");
            });
        }

    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", "notify", "refresh", function ($scope, $state, $http, disable, dataFactory, atencionFactory, notify, refresh) {

        $scope.disable = disable.receta;
        $scope.stock = dataFactory.stock;
        $scope.receta = atencionFactory.receta;
        $scope.ItemRecetaNuevo = {};
        $scope.editarItem = true;
        $scope.diagnosticos = atencionFactory.diagnosticos;
        var actualizar = refresh.go(cargar);


        $scope.$on('dataFactory.stock', function () {
            $scope.stock = dataFactory.stock;
        })

        function cargar() {
            if ($state.includes('atencion.receta') && atencionFactory.codigo !== null) {
                dataFactory.getStock();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.$watch('receta.items', function() { //Cada vez que cambia la receta, se guarda en atencionFactory y se refresca la misma
            atencionFactory.receta.items = $scope.receta.items;
            $scope.receta.items = atencionFactory.receta.items;
        })

        if (atencionFactory.receta.id === null && atencionFactory.codigo !== null) {
            var apadrinado = atencionFactory.codigo;

            $http.post("/api/receta/apadrinado/" + apadrinado).then(function success(res) {
                console.log("Se creo receta", res.data);
                atencionFactory.receta.id = res.data.id;
                $scope.receta.id = atencionFactory.receta.id;
            }, function (err) {
                console.log("No se creo receta", err);
                notify("No se pudo crear ID de la receta", "danger");
            })

        }

        $scope.addItenReceta = function (item) {
            $('#modal_crearItem').modal('hide'); //Se cierra el modal
            actualizar = refresh.go(cargar); //Se empiezan a actualizar las recetas
            $scope.receta.items.push(angular.copy(item)); //Se actualiza la receta con el nuevo item
        }

        $scope.eliminarItem = function (itemsReceta, index) {
            console.log("Eliminando item");
            itemsReceta.splice(index, 1); //Se elimina el item de $scope.receta.items
        }

        $scope.select = function (item) {
            refresh.stop(actualizar);
            $scope.ItemRecetaNuevo.itemFarmacia = angular.copy(item);
            $scope.ItemRecetaNuevo.diagnostico = {};
            $scope.ItemRecetaNuevo.cantidad = 1;
            $scope.ItemRecetaNuevo.posologia = "";
        }

        $scope.guardarReceta = function () {
            //var data = { idReceta: atencionFactory.receta.id, items: atencionFactory.receta.items };
            var idReceta = atencionFactory.receta.id;
            var items = atencionFactory.receta.items;

            //console.log("Items a enviar", JSON.parse(angular.toJson(items)));
            NProgress.start();
            $http.post("/api/receta/" + idReceta, JSON.parse(angular.toJson(items))).then(function success(res) {
                
                NProgress.done();
                console.log("Se crearon los items", res.data);
                notify("Se creo la receta exitosamente", "success");
                disable.receta = true;
                $scope.disable = disable.receta; //Se desactiva atencion.receta.html
                refresh.stop(actualizar); //Se detiene la actualizacion de receta
                cargar(); //Se carga por ultima vez la data
            }, function err(err){
                NProgress.done();
                console.log("No se pudieron crear los items", err);
                notify("No se pudo crear la receta", "danger");
            })
        }

    }])
    .controller('anulaciones', ["$scope", "$state", "$http", "atencionFactory", "notify", "dataFactory", "refresh", function ($scope, $state, $http, atencionFactory, notify, dataFactory, refresh) {

        $scope.recetas = dataFactory.recetas;
        $scope.receta = null;
        var actualizar = refresh.go(cargar); //cada 10 segundos

        $scope.$on('dataFactory.recetas', function(){
            $scope.recetas = dataFactory.recetas;
        })

        function cargar() {
            if ($state.includes('anulaciones')) {
                dataFactory.getRecetas();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.select = function (receta) {
            refresh.stop(actualizar);
            $scope.receta = receta;
        }

        $scope.cancelar = function () {
            actualizar = refresh.go(cargar);
        }

        $scope.eliminarReceta = function (receta) {

            $http.delete("/api/receta/" + receta.receta.id).then(function success(res) {
                actualizar = refresh.go(cargar);
                console.log("Receta eliminada con exito", receta, res);
                notify("Receta eliminada con exito", "success");
            }, function error(err) {
                actualizar = refresh.go(cargar);
                console.log("No se pudo eliminar receta", err);
                notify("Receta no se pudo eliminar", "danger");
            });
        }



    }])
    .controller('estadisticas', ["$scope", "$state", "$http", function ($scope, $state, $http) {
        $state.go('estadisticas.enfermedades');

    }])
    .controller('estadisticas.atenciones', ["$scope", "$state", "$http", "dataFactory", "notify", "date", function ($scope, $state, $http, dataFactory, notify, date) {
        $scope.atenciones = dataFactory.estadisticas.atenciones;

        $scope.$watch('atenciones', function () {
            dataFactory.estadisticas.atenciones = $scope.atenciones;
        })

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta)
            }

            NProgress.start();

            $http({
                method: "GET",
                url: "/api/atencion/medico",
                params: data
            }).then(function success(res) {
                $scope.atenciones.all = res.data;
                console.log($scope.atenciones.all);
                NProgress.done();

            }, function error(err) {
                console.log("error cargar atenciones", err);
                notify("No se pudo cargar atenciones médicas", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('estadisticas.remisiones', ["$scope", "$state", "$http", "dataFactory", "notify", "date", function ($scope, $state, $http, dataFactory, notify, date) {
        $scope.remisiones = dataFactory.estadisticas.remisiones;

        $scope.$watch('remisiones', function () {
            dataFactory.estadisticas.remisiones = $scope.remisiones;
        })        

        $scope.generar = function (desde, hasta) {

            var data = {
                desde: date(desde),
                hasta: date(hasta)
            }

            NProgress.start();

            $http({
                method: "GET",
                url: "/api/reporte/remision/date",
                params: data
            }).then(function success(res) {
                $scope.remisiones.all = res.data;
                NProgress.done();
            }, function error(err) {
                console.log("error cargar remisiones", err);
                notify("No se pudo cargar remisiones", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('estadisticas.recetas', ["$scope", "$state", "$http", "dataFactory", "notify", "date", function ($scope, $state, $http, dataFactory, notify, date) {
        $scope.recetas = dataFactory.estadisticas.recetas;
        
        $scope.$watch('recetas', function () {
            dataFactory.estadisticas.recetas = $scope.recetas;
        })
        

        $scope.select = function (receta) {
            $scope.receta = receta;
        }

        $scope.generar = function (desde, hasta) {
            var data = {
                desde: date(desde),
                hasta: date(hasta)
            }
            NProgress.start()
            $http({
                method: "GET",
                url: "/api/receta/medico/fecha",
                params: data
            }).then(function success(res) {
                NProgress.done();
                $scope.recetas.all = res.data;
            }, function error(err) {
                console.log("error cargar recetas")
                notify("No se pudo cargar recetas", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('estadisticas.enfermedades', ["$scope", "$state", "$http", "dataFactory", "date", "notify", function ($scope, $state, $http, dataFactory, date, notify) {

        $scope.enfermedades = dataFactory.estadisticas.enfermedades;

        $scope.$watch('enfermedades', function () {
            dataFactory.estadisticas.enfermedades = $scope.enfermedades;
        })

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

        var color = ["#901F61", "#009877", "#D64227", "#FED115", "#ADBF2B"];

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
    .controller('pedidos', ['$scope', "$state", '$http', function ($scope,$state, $http) {
        $state.go('pedidos.interno');
    }])
    .controller('pedidos.transferencia', ['$scope', '$http', function ($scope, $http) {

    }])
    .controller('pedidos.interno', ['$scope', '$http', '$state', 'dataFactory', 'refresh', 'notify', function ($scope, $http, $state, dataFactory, refresh, notify) {

        $scope.stock = dataFactory.stock;
        $scope.item = null;

        var actualizar = refresh.go(cargar, 30000);

        $scope.$on('dataFactory.stock', function () {
            $scope.stock = dataFactory.stock;
        })

        function cargar() {
            if ($state.includes('pedidos.interno')) {
                dataFactory.getStock();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.seleccionar = function (item) {
            $scope.item = item;
            $('#despachar').modal('show');
        }

        $scope.solicitar = function (cantidad) {
            data = {
                item: $scope.item,
                cantidad: cantidad
            }

            NProgress.start();
            $http.post("/api/pedidointerno", data).then(function success(res) {
                NProgress.done();
                $('#despachar').modal('hide');
                console.log("Se creo el pedido interno", res);
                notify("El pedido interno se creo exitosamente", "success");
                actualizar = refresh.go(cargar, 30000);
            }, function error(e) {
                NProgress.done();
                console.log("No se pudo crear el pedido interno", e);
                notify("No se pudo crear el pedido interno", "danger");
                $('#despachar').modal('hide');
                actualizar = refresh.go(cargar, 30000);
            })
        }
    }])
