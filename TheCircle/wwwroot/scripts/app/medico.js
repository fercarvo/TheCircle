/*
 appMedico v1.0
 Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
 Children International
*/

angular.module('appMedico', ['ui.router', 'nvd3'])
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
            .state('pedidos.generar', {
                templateUrl: 'views/medico/pedidos.generar.html',
                controller: 'pedidos.generar'
            })
            .state('pedidos.receptar', {
                templateUrl: 'views/medico/pedidos.receptar.html',
                controller: 'pedidos.receptar'
            });

        //False en modo de produccion
        $compileProvider.debugInfoEnabled(false)
        $compileProvider.commentDirectivesEnabled(false)
        $compileProvider.cssClassDirectivesEnabled(false)
    }])
    .run(["$state", "$http", "$templateCache", function ($state, $http, $templateCache) {

        checkSession($http);

        loadTemplates($state, "atencion", $http, $templateCache);
    }])
    .factory('dataFactory', ['$http', '$rootScope', function ($http, $rootScope) {

        var dataFactory = {
            enfermedades: null,
            instituciones: null,
            stockChildren: null,
            stock: null,
            stockInsumos: null,
            recetas: null,
            estadisticas: {},
            pedidosInternos: null,
            tipos: ["curativo", "seguimiento", "control"],
            getStockChildren: getStockChildren,
            getInstituciones: getInstituciones,
            getEnfermedades: getEnfermedades,
            getStock: getStock,
            getStockInsumos: getStockInsumos,
            getRecetas: getRecetas,
            getApadrinado: getApadrinado,
            postRemision: postRemision,
            getPedidosAReceptar: getPedidosAReceptar
        }

        function getPedidosAReceptar($scope) {
            $http.get("/api/pedidointerno/despachadas").then(function (res) {
                console.log("Pedidos internos", res.data)
                dataFactory.pedidosInternos = res.data;
                $scope.pedidosInternos = dataFactory.pedidosInternos
            }, function (error) {
                console.log("Error pedidos: ", error)
                notify("No se pudo cargar pedidos internos", "danger")
            })
        }

        function getStockChildren() {
            $http.get("/api/itemfarmacia/report").then(function success(res) {
                dataFactory.stockChildren = res.data;
                $rootScope.$broadcast('dataFactory.stockChildren');
            }, function error(e) {
                console.log("Error cargar Stock", e);
            })
        }

        function getStockInsumos() {
            $http.get("/api/itemfarmacia/insumos").then(function success(res) {
                dataFactory.stockInsumos = res.data;
                $rootScope.$broadcast('dataFactory.stockInsumos');
            }, function error(e) {
                console.log("Error cargar Stock de farmacia", e);
            })
        }

        function getInstituciones() {
            return $http.get("/api/institucion", { cache: true }).then(function (res) {
                dataFactory.instituciones = res.data;
                $rootScope.$broadcast('dataFactory.instituciones');
            }, function (error) {
                console.log("Error cargar instituciones", error);
            })
        }

        function getEnfermedades($scope) {
            $http.get("/api/enfermedad").then(function (res) {
                //dataFactory.enfermedades = res.data;
                $scope.enfermedades = res.data;
            }, function (err) {
                console.log("Error cargar enfermedades", err);
            })
        }

        function getStock() {
            $http.get("/api/itemfarmacia").then(function success(res) {
                console.log("Actualizando Stock by localidad", res.data);

                var filtrado = [];

                res.data.forEach(function (item) {
                    if (item.grupo === "MED") {
                        filtrado.push(item)
                    }
                })

                dataFactory.stock = filtrado;
                console.log(dataFactory.stock);
                $rootScope.$broadcast('dataFactory.stock'); //Se informa a los controladores que cambio stock
            }, function error(err) {
                console.log("Error cargar Stock de farmacia", err);
            })
        }

        function getRecetas() {
            $http.get("api/receta/medico/activas").then(function success(res) {
                console.log("recetas by status", res.data);
                dataFactory.recetas = res.data.filter(data=> !data.receta.eliminada) //Se muestran solo las no eliminadas
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

        function postRemision(remision) {
            NProgress.start();
            var promise = $http.post("/api/remision", remision)

            promise.then(function (res) {
                NProgress.done()
                console.log("se creo remision", res.data)
                notify("Se creo la remision exitosamente", "success")
            }, function (err) {
                NProgress.done();
                console.log("error crear remision", err);
                notify("No se pudo generar la remision, por favor verifique los datos", "danger");
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
    .factory('atencionFactory', ["$http", function ($http) { //factory donde se guarda toda la data ingresada
        var atencionFactory = {
            apadrinado : {},
            foto : "/images/foto.png",
            codigo : null,
            atencion : null,
            diagnosticos : null,
            remision : null,
            receta: {
                items: [],
                id: null
            },
            status: true,
            monto: null,
            getMonto: getMonto
        }

        function getMonto($scope) {
            $http.get("/api/remision/gastado").then(function (res) {
                console.log("Monto", res.data)
                atencionFactory.monto = res.data
                $scope.monto = atencionFactory.monto
            }, function () { })
        }

        return atencionFactory
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

        $scope.imc = function (peso, talla) {

            if (!peso || !talla) {
                return ""
            }

            var imc = peso * 10 / (talla / 100 * talla / 100) / 10;

            if (imc < 18.5) {
                return "Bajo peso"
            } else if (imc < 25) {
                return "Normal"
            } else if (imc < 30) {
                return "Sobrepeso"
            } else if (imc < 35) {
                return "Obesidad I"
            } else if (imc < 40) {
                return "Obesidad II"
            } else if (imc < 50) {
                return "Obesidad III"
            } else if (imc >= 50) {
                return "Obesidad IV"
            } else {
                return ""
            }
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

                switch (res.data.status) {
                    case "D":
                    case "E":
                        $scope.status = false;
                        break;
                    default:
                        $scope.status = true;
                        break;
                }

                $scope.foto = "/api/apadrinado/" + codigo + "/foto";
                $scope.apadrinado = res.data;

            }, function error() {

                $scope.foto = "/images/foto.png";
                $scope.status = true
                $scope.codigo = null;
                $scope.apadrinado = {};
            })
        } 

    }])
    .controller('atencion.registro', ["$scope", "$state", "$http", "dataFactory", "atencionFactory", "disable", function ($scope, $state, $http, dataFactory, atencionFactory, disable) {

        $scope.disable = disable.atencion;
        $scope.enfermedades = null;
        $scope.tipos = dataFactory.tipos;
        $scope.atencion = atencionFactory.atencion;

        dataFactory.getEnfermedades($scope);

        /*if (dataFactory.enfermedades === null) {
            dataFactory.getEnfermedades().then(function success(res) {
                dataFactory.enfermedades = res.data;
                $scope.enfermedades = dataFactory.enfermedades;
            }, function error(err) {
                console.log("Error cargar enfermedades", err);
            })
        }*/


        $scope.reset = function () {
            $scope.atencion = {};
        };

        $scope.$watch('atencion', function() {
            atencionFactory.atencion = $scope.atencion;
        });

        $scope.send = function () {
            var data = {
                apadrinado: atencionFactory.codigo,
                tipo: atencionFactory.atencion.tipo,
                diagnosticos: [atencionFactory.atencion.diagp,
                  atencionFactory.atencion.diag1,
                  atencionFactory.atencion.diag2],
                peso: atencionFactory.atencion.peso,
                talla: atencionFactory.atencion.talla
            }

            NProgress.start();
            $http.post("/api/atencion", data).then(function success(res){

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
    .controller('atencion.remision', ["$scope", "$state", "disable", "dataFactory", "atencionFactory", function ($scope, $state, disable, dataFactory, atencionFactory) {

        $scope.disable = disable.remision;
        $scope.remision = atencionFactory.remision; //se guarda todo lo ingresado en remision
        $scope.instituciones = dataFactory.instituciones;
        $scope.diagnosticos = atencionFactory.diagnosticos
        $scope.monto = atencionFactory.monto

        if ($scope.monto === null) {
            atencionFactory.getMonto($scope)
        }

        if (dataFactory.instituciones === null) {
            dataFactory.getInstituciones()
        }

        $scope.$on('dataFactory.instituciones', function() { $scope.instituciones = dataFactory.instituciones })

        $scope.send = function (remision) {
            var data = {
                atencionM: atencionFactory.atencion.id,
                institucion: remision.institucion,
                monto: remision.monto,
                sintomas: remision.sintomas
            }

            dataFactory.postRemision(data)
                .then(function (res) {
                    disable.remision = true;
                    atencionFactory.remision = $scope.remision; //Se guarda la remision en la factory
                    $scope.disable = disable.remision; //Se desactiva atencion.remision.html
                    //window.open("/api/remision/" + res.data.id + "/imprimir");
                    openURL("/api/remision/" + res.data.id + "/imprimir")
                }, function() { })
        }

    }])
    .controller('atencion.receta', ["$scope", "$state", "$http", "disable", "dataFactory", "atencionFactory", function ($scope, $state, $http, disable, dataFactory, atencionFactory) {

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
            $scope.ItemRecetaNuevo.diagnostico = null;
            $scope.ItemRecetaNuevo.cantidad = 1;
            $scope.ItemRecetaNuevo.posologia = null;
        }

        $scope.guardarReceta = function () {
            var items = atencionFactory.receta.items;

            NProgress.start();
            $http.post("/api/receta/apadrinado/" + atencionFactory.codigo, JSON.parse(angular.toJson(items))).then(function (res) {
                
                NProgress.done();
                console.log("Creo la receta", res.data);
                notify("Se creo la receta exitosamente", "success");
                atencionFactory.receta.id = res.data.id;
                $scope.receta.id = atencionFactory.receta.id;
                disable.receta = true;
                $scope.disable = disable.receta; //Se desactiva atencion.receta.html
                refresh.stop(actualizar); //Se detiene la actualizacion de receta
                cargar(); //Se carga por ultima vez la data
                openURL("/api/receta/" + res.data.id + "/imprimir")
                //window.open("/api/receta/" + res.data.id + "/imprimir");

            }, function err(err){
                NProgress.done();
                console.log("Error al crear receta", err);
                notify("No se ha podido crear la receta, por favor verifique los items", "danger");
            })
        }

    }])
    .controller('anulaciones', ["$scope", "$state", "$http", "dataFactory", function ($scope, $state, $http,  dataFactory) {

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
    .controller('estadisticas', ["$state", function ($state) {
        $state.go('estadisticas.enfermedades');

    }])
    .controller('estadisticas.atenciones', ["$scope", "$state", "$http", "dataFactory", function ($scope, $state, $http, dataFactory) {
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
    .controller('estadisticas.remisiones', ["$scope", "$state", "$http", "dataFactory", function ($scope, $state, $http, dataFactory) {
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
    .controller('estadisticas.recetas', ["$scope", "$state", "$http", "dataFactory", function ($scope, $state, $http, dataFactory) {
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
    .controller('estadisticas.enfermedades', ["$scope", "$state", "$http", "dataFactory", function ($scope, $state, $http, dataFactory) {

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
                legendPosition: "top",
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
    .controller('pedidos', ["$state", function ($state) {
        $state.go('pedidos.generar');
    }])
    .controller('pedidos.generar', ['$scope', '$http', '$state', 'dataFactory', function ($scope, $http, $state, dataFactory) {
        console.log("En generar")
        $scope.stock = dataFactory.stockInsumos;
        $scope.item = null;

        var actualizar = refresh.go(cargar, 1);

        $scope.$on('dataFactory.stockInsumos', function () {
            $scope.stock = dataFactory.stockInsumos
        })

        function cargar() {
            if ($state.includes('pedidos.generar')) {
                dataFactory.getStockInsumos();
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.seleccionar = function (item) {
            $scope.item = item;
            $('#despachar').modal('show');
            $scope.cantidad = null;
        }

        $scope.solicitar = function (cantidad) {
            data = {
                item: $scope.item.id,
                cantidad: cantidad
            }

            refresh.stop(actualizar)
            NProgress.start()

            $http.post("/api/pedidointerno", data).then(function (res) {                    
                console.log("Se creo el pedido interno", res);
                notify("El pedido interno se creo exitosamente", "success");
            }, function (e) {
                console.log("No se pudo crear el pedido interno", e);
                notify("No se pudo crear el pedido interno", "danger");                    
            })
            .finally(function () {
                NProgress.done();
                $('#despachar').modal('hide');
                cantidad = 1;
                actualizar = refresh.go(cargar, 30)
            })
        }
    }])
    .controller('pedidos.receptar', ["$scope", "$state", "dataFactory", "$http", function ($scope, $state, dataFactory, $http) {
        $scope.pedidosInternos = dataFactory.pedidosInternos;
        $scope.pedido = null;

        var actualizar = refresh.go(cargar, 1);

        function cargar() {
            if ($state.includes('pedidos.receptar')) {
                dataFactory.getPedidosAReceptar($scope);
            } else {
                refresh.stop(actualizar);
            }
        }

        $scope.ver = function (pedido) {
            $scope.pedido = pedido
            $scope.comentario = null
        }

        $scope.guardarComentario = function (pedido, comentario) {
            refresh.stop(actualizar);
            NProgress.start();
            $http.put("/api/pedidointerno/" + pedido + "/receptar", {comentario: comentario}).then(function () {
                $('#ver_pedido').modal('hide');
                actualizar = refresh.go(cargar, 1);
                notify("Pedido receptado exitosamente", "success");
                NProgress.done();
            }, function error(e) {
                $('#ver_pedido').modal('hide');
                actualizar = refresh.go(cargar, 1);
                console.log("No se recepto", e)
                notify("No se pudo receptar el pedido", "danger")
                NProgress.done();
            })
        }   
    }])