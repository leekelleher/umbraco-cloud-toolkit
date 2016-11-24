angular.module("umbraco").controller("UmbracoCloudToolkit.DashboardController", [

    "$scope",
    "notificationsService",
    "UmbracoCloudToolkit.DashboardResources",

    function ($scope, notificationsService, utdResources) {

        $scope.lookupGuid = "";
        $scope.lookupResult = undefined;
        $scope.lookupNodeId = "";
        $scope.foundGuid = undefined;

        $scope.toggle = function (obj, prop) {
            obj[prop] = !obj[prop];
        }

        $scope.lookupEntity = function () {
            $scope.lookupResult = undefined;
            utdResources.lookupEntity($scope.entityId).then(function (data) {
                $scope.lookupResult = data;
            });
        }

        $scope.forceDeploy = function () {
            if (confirm("Are your sure you want to initiate a forced Courier deploy?")) {
                utdResources.forceDeploy().then(function () {
                    notificationsService.success("Deploying", "Force deploy successfully initiated.");
                });
            }
        }

        $scope.rebuild = function () {
            if (confirm("Are your sure you want to initiate a Courier rebuild?")) {
                if (confirm("Seriously, are your sure?")) {
                    utdResources.rebuild().then(function () {
                        notificationsService.success("Rebuilding", "Rebuild successfully initiated.");
                    });
                }
            }
        }

    }
]);
