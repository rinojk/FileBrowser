var app = angular.module('fileBrowserApp', []);

        app.controller('mainController', function ($scope, $http) {

            //Function to init controller. Getting list of all drives.
            $scope.init = function() { 
                $http.get("/api/FileBrowser/AllDrives").then(function (response) {
                    $scope.disks = response.data;
                    $scope.isDiskList = true; 
                });
            }

            var requestCount = 0; //Total count of working requests. Variable for fixing duplicated http-requests
            //Function to load new folder
            function goToPath($path) {
                requestCount++;
                var currentRequest = requestCount; //Current request number
                //Getting list of all folders and files in target folder
                $http.get("/api/FileBrowser/GetFolder?folderPath=" + $path).then(function (response) {
                    $scope.data = response.data;
                    $scope.isDiskList = false;
                    },
                function (response) {
                    alert(response.data.exceptionMessage);
                    return;
                });


                //Getting information about subfiles of target folder
                $scope.fileLess10MbCount = $scope.file10To50MbCount = $scope.fileMore100MbCount = "Loading";
                $http.get("/api/FileBrowser/GetFilesCount?folderPath=" + $path).then(function (response) {
                    if (currentRequest == requestCount) { //show result only from last request
                        $scope.fileLess10MbCount = response.data.fileLess10MbCount;
                        $scope.file10To50MbCount = response.data.file10To50MbCount;
                        $scope.fileMore100MbCount = response.data.fileMore100MbCount;
                        requestCount = currentRequest = 0;
                    }
                });
            }

            $scope.goToPath = function ($folderPath) {
                goToPath($folderPath);
            }

            $scope.goToPrevFolder = function () {

                //Show list of disks if there is no parent folder (root directory)
                if ($scope.data.parentFolder == null) { 
                    $http.get("/api/FileBrowser/Alldrives").then(function (response) {
                        $scope.disks = response.data;
                        $scope.isDiskList = true;
                    });
                }
                else {
                    goToPath($scope.data.parentFolder);
                }
            }
        });

