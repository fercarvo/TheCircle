/*
    sistema v1.0 
    Edgar Fernando Carvajal Ulloa efcarvaj@espol.edu.ec
    Children International
*/
angular.module('sistema', ['ui.router', 'ngCookies'])
    .config(["$stateProvider", "$compileProvider", function ($stateProvider, $compileProvider) {
        $stateProvider
            .state('activarusuario', {
                templateUrl: 'views/sistema/activarusuario.html',
                controller: 'activarusuario'
            })
            .state('desactivarusuario', {
                templateUrl: 'views/sistema/desactivarusuario.html',
                controller: 'desactivarusuario'
            })
            .state('cambiarclave', {
                templateUrl: 'views/sistema/cambiarclave.html',
                controller: 'cambiarclave'
            });
        $compileProvider.debugInfoEnabled(true); //False en modo de produccion
    }])
    .run(["$state", "$rootScope", "$cookies", "$http", "refresh", function ($state, $rootScope, $cookies, $http, refresh) {

        /*
        Cada X segundos se ejecuta un HTTP GET a /login con el fin de determinar 
        si esta activa o no la session, en caso de estar inactiva se redirecciona /logout
        */
        refresh.goTime(function () {
            $http.get("login").then(function () {
            }, function (response) {
                if (response.status === 401) {
                    alert("Su sesion ha caducado");
                    document.location.replace('logout');
                }
            })
        }, 1000 * 60 * 20)

        //Se carga desde los cookies el nombre de la persona con la session
        $rootScope.session_name = (function () {
            var c = $cookies.get('session_name')
            if (c) {
                return c
            } return ""
        })()

        //Se carga desde las cookies el email de la persona con la session
        $rootScope.session_email = (function () {
            var c = $cookies.get('session_email')
            if (c) {
                return c
            } return ""
        })()

        //Se carga desde las cookies el link para obtener la foto de la persona con la session
        $rootScope.session_photo = (function () {
            var c = $cookies.get('session_photo')
            if (c) {
                return c
            } return "/images/ci.png"
        })()

        //Esta inicial de la app web
        $state.go("activarusuario");
    }])
    .factory('refresh', [function () { //Sirve para ejecutar una funcion cada cierto tiempo y detenerla cuando se requiera.

        function go(fn) {
            fn();
            //console.log("Go refresh");
            return setInterval(fn, 10000);
        }

        function goTime(fn, time) {
            fn();
            //console.log("Go refresh by ", time);
            return setInterval(fn, time);
        }

        function stop(repeater) {
            //console.log("Stop refresh");
            clearInterval(repeater);
        }

        return {
            go: go,
            stop: stop,
            goTime: goTime
        }
    }])
    /**
     * @description
     *
     * Esta factory retorna una funcin encargada de mostrar las notificaciones
     * de la aplicacion
     *
     * @param {mensaje} Mensaje que será mostrado en la notificacion
     * @param {tipo} success o danger, dependiendo de esos dos parametros cambiará de color.
     * @returns {promise} Promise con la cual se puede terminar la notificación antes de que esta
     * culmine por ejm
     * var not = notify.close();
     * not();
     */
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
                    offset: { x: 20, y: 70 },
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
                })
        }
    }])
        /**
     * @description
     *
     * Esta factory un objeto que contiene una serie de información a guardar, asi como funciones
     * a ser utilizadas por la app
     *
     */
    .factory('usuarios', ['$http', '$rootScope', function ($http, $rootScope) {

        var usuarios = {
            inactivos: null,
            activos : null,
            all : null,
            getInactivos: getInactivos,
            getActivos: getActivos,
            getAll: getAll
        }

        //Se obtienen todos los usuarios inactivos que existen en el sistema
        function getInactivos() {
            $http.get("api/user/inactivos").then(function success(res) {
                //console.log("usuarios.inactivos", res.data);
                usuarios.inactivos = res.data;
                $rootScope.$broadcast('usuarios.inactivos'); //Se informa a los controladores que cambio usuarios.inactivos
            }, function error(err) {
                //console.log("error cargar usuarios.inactivos", err);
            })
        }

        //Se obtienen todos los usuarios activos que existen en el sistema
        function getActivos() {
            $http.get("api/user/activos").then(function success(res) {
                //console.log("usuarios.activos", res.data);
                usuarios.activos = res.data;
                $rootScope.$broadcast('usuarios.activos'); //Se informa a los controladores que cambio usuarios.activos
            }, function error(err) {
                //console.log("error cargar usuarios.activos", err);
            })
        }

        //Se obtienen todos los usuarios activos y no activos del sistema, excepto los que tienen
        //Como rol "sistema"
        function getAll() {
            $http.get("api/user").then(function success(res) {
                //console.log("usuarios.all", res.data);
                usuarios.all = res.data;
                $rootScope.$broadcast('usuarios.all'); //Se informa a los controladores que cambio usuarios.all
            }, function error(err) {
                //console.log("error cargar usuarios.all", err);
            })
        }

        return usuarios;
    }])
    .controller('activarusuario', ["$scope", "$state", "$http", "usuarios", "notify", function ($scope, $state, $http, usuarios, notify) {

        $scope.usuarios = usuarios.inactivos;
        $scope.usuario = null;

        usuarios.getInactivos();

        $scope.$on('usuarios.inactivos', function () {
            $scope.usuarios = usuarios.inactivos;
        })

        $scope.$watch('usuarios', function () {
            usuarios.inactivos = $scope.usuarios;
        })

        $scope.seleccionar = function(usuario){
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#modal_activar').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/activar").then(function success(res) {
                //console.log("Activado con exito");
                $scope.usuarios = res.data;
                notify("Usuario activado exitosamente", "success");
                NProgress.done();
            }, function error(err) {
                //console.log("error al activar");
                notify("No se ha podido activar el usuario", "danger");
                NProgress.done();
            })
        }

    }])
    .controller('desactivarusuario', ["$scope", "$state", "$http", "usuarios", "notify", function ($scope, $state, $http, usuarios, notify) {
        $scope.usuarios = usuarios.activos;
        $scope.usuario = null;

        usuarios.getActivos();

        $scope.$on('usuarios.activos', function () {
            $scope.usuarios = usuarios.activos;
        })

        $scope.$watch('usuarios', function () {
            usuarios.activos = $scope.usuarios;
        })

        $scope.seleccionar = function (usuario) {
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#modal_desactivar').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/desactivar").then(function success(res) {
                //console.log("desActivado con exito");
                $scope.usuarios = res.data;
                notify("No se ha podido activar el usuario", "success");
                NProgress.done();
            }, function error(err) {
                //console.log("error al desactivar");
                notify("No se ha podido activar el usuario", "danger");
                NProgress.done();
            })
        }
    }])
    .controller('cambiarclave', ["$scope", "$state", "$http", "usuarios", "notify", function ($scope, $state, $http, usuarios, notify) {
        $scope.usuarios = usuarios.all;
        $scope.usuario = null;
        $scope.clave = null;

        usuarios.getAll();

        $scope.$on('usuarios.all', function () {
            $scope.usuarios = usuarios.all;
        })

        $scope.seleccionar = function (usuario) {
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#cambiar_clave').modal('hide');
            NProgress.start();
            $http.put("api/user/" + $scope.usuario.cedula + "/clave/set").then(function success(res) {
                NProgress.done();
                //console.log("Cambio de clave exitoso", res.data);
                $scope.clave = res.data.clave;
                $('#nueva_clave').modal('show');
                notify("Reseteo de clave exitoso", "success");
            }, function error(err) {
                //console.log("error cambio de clave");
                NProgress.done();
                notify("No se ha podido cambiar la clave del usuario", "danger");
            })
        }
    }])