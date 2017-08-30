angular.module('appSistema', ['ui.router'])
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
        //$compileProvider.debugInfoEnabled(false); //Activar en modo producción
    }])
    .run(["$state", function ($state) {
        $state.go("activarusuario");
    }])
    .factory('usuarios', ['$http', '$rootScope', function ($http, $rootScope) {

        var usuarios = {
            inactivos: null,
            activos : null,
            all : null,
            getInactivos: getInactivos,
            getActivos: getActivos,
            getAll: getAll
        }

        function getInactivos() {
            $http.get("api/user/inactivos").then(function success(res) {
                console.log("usuarios.inactivos", res.data);
                usuarios.inactivos = res.data;
                $rootScope.$broadcast('usuarios.inactivos'); //Se informa a los controladores que cambio usuarios.inactivos
            }, function error(err) {
                console.log("error cargar usuarios.inactivos", err);
            })
        }

        function getActivos() {
            $http.get("api/user/activos").then(function success(res) {
                console.log("usuarios.activos", res.data);
                usuarios.activos = res.data;
                $rootScope.$broadcast('usuarios.activos'); //Se informa a los controladores que cambio usuarios.activos
            }, function error(err) {
                console.log("error cargar usuarios.activos", err);
            })
        }

        function getAll() {
            $http.get("api/user").then(function success(res) {
                console.log("usuarios.all", res.data);
                usuarios.all = res.data;
                $rootScope.$broadcast('usuarios.all'); //Se informa a los controladores que cambio usuarios.all
            }, function error(err) {
                console.log("error cargar usuarios.all", err);
            })
        }

        return usuarios;
    }])
    .controller('activarusuario', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {

        $scope.usuarios = usuarios.inactivos;
        $scope.usuario = null;
        $scope.index = null;

        usuarios.getInactivos();

        $scope.$on('usuarios.inactivos', function () {
            $scope.usuarios = usuarios.inactivos;
        })

        $scope.$watch('usuarios', function () {
            usuarios.inactivos = $scope.usuarios;
        })

        $scope.seleccionar = function(usuario, index){
            $scope.usuario = usuario;
            $scope.index = index;
        }

        $scope.aceptar = function () {
            $('#modal_activar').modal('hide');

            $http.put("api/user/" + $scope.usuario.cedula + "/activar").then(function success(res) {
                console.log("Activado con exito");
                $scope.usuarios.splice($scope.index, 1);
            }, function error(err) {
                console.log("error al activar");
            })
        }

    }])
    .controller('desactivarusuario', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {
        $scope.usuarios = usuarios.activos;
        $scope.usuario = null;
        $scope.index = null;

        usuarios.getActivos();

        $scope.$on('usuarios.activos', function () {
            $scope.usuarios = usuarios.activos;
        })

        $scope.$watch('usuarios', function () {
            usuarios.activos = $scope.usuarios;
        })

        $scope.seleccionar = function (usuario, index) {
            $scope.index = index;
            $scope.usuario = usuario;
        }

        $scope.aceptar = function () {
            $('#modal_desactivar').modal('hide');

            $http.put("api/user/" + $scope.usuario.cedula + "/desactivar").then(function success(res) {
                console.log("desActivado con exito");
                $scope.usuarios.splice($scope.index, 1);
            }, function error(err) {
                console.log("error al desactivar");
            })
        }
    }])
    .controller('cambiarclave', ["$scope", "$state", "$http", "usuarios", function ($scope, $state, $http, usuarios) {
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

            $http.put("api/user/" + $scope.usuario.cedula + "/clave/set").then(function success(res) {
                console.log("Cambio de clave exitoso", res.data);
                $scope.clave = res.data.clave;
                $('#nueva_clave').modal('show');
            }, function error(err) {
                console.log("error cambio de clave");
            })
        }
    }])