angular.module('umbraco.resources').factory('UmbracoCloudToolkit.DashboardResources',
    function ($q, $http, umbRequestHelper) {
        return {
            lookupEntity: function (entityId) {
                var url = "/umbraco/backoffice/UmbracoCloudToolkit/DashboardApi/LookupEntity";
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: url,
                        method: "GET",
                        params: {
                            entityId: entityId
                        }
                    }),
                    'Failed to lookup entity'
                );
            },
            forceDeploy: function () {
                var url = "/umbraco/backoffice/api/CourierAdmin/ForceDeploy";
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: url,
                        method: "GET"
                    }),
                    'Failed to force deploy'
                );
            },
            rebuild: function () {
                var url = "/umbraco/backoffice/api/CourierAdmin/Rebuild";
                return umbRequestHelper.resourcePromise(
                    $http({
                        url: url,
                        method: "GET"
                    }),
                    'Failed to rebuild'
                );
            }
        };
    });
