angular.module('app', []);

angular
    .module('app')
    .controller('SalesOrderDetailController', SalesOrderDetailController);

function SalesOrderDetailController($http) {
    var vm = this;
    vm.SalesOrderDetails = {};
    vm.salesOrderTotalQty = 0;
    vm.page = 1;
    vm.displayQty = 100;
    
    /*API Functions*/
    vm.getSalesOrderDetails = function () {
        $http.get('/api/SalesOrderDetails?Quantity=' + vm.displayQty + '&Page=' + vm.page).
            success(function (data) {
                vm.SalesOrderDetails = data;
            });
    };

    vm.getSalesOrder = function ($SalesOrder) {
        $http.get('/api/SalesOrderDetails/' + $SalesOrder.SalesOrderDetailID + '?SalesOrderID=' + $SKUPurchase.SalesOrderID).
            success(function (data) {
                var index = vm.SalesOrderDetails.indexOf($SalesOrder)
                if (index > -1) {
                    vm.SalesOrderDetails.splice(index, 1);
                }
                vm.SalesOrderDetails.push(data);
            });
    };

    vm.deleteSalesOrder = function ($SalesOrder) {
        console.log("Application Initialized");
        $http.delete('/api/SalesOrderDetails/' + $SalesOrder.SalesOrderDetailID + '?SalesOrderID=' + $SKUPurchase.SalesOrderID).
            success(function (data) {
                var index = vm.SalesOrderDetails.indexOf($SalesOrder)
                if (index > -1) {
                    vm.SalesOrderDetails.splice(index, 1);
                }
            });
    };

    vm.getSalesOrderQuantity = function ($SalesOrder) {
        $http.get('/api/SalesOrderDetails/GetSalesOrderQuantity').
            success(function (data) {
                vm.salesOrderTotalQty = data;
            });
    };
    
    /*Page initialization*/
    vm.getSalesOrderQuantity();
    vm.getSalesOrderDetails();

    /*Pagination Functions*/
    vm.firstPage = function () {
        if (vm.page > 1) {
            vm.page = 1;
            vm.getSalesOrderDetails();;
        }
    }

    vm.nextPage = function () {
        var lastPage = Math.round(vm.salesOrderTotalQty / vm.displayQty);
        if (vm.salesOrderTotalQty % vm.displayQty != 0) {
            lastPage++;
        }
        if (vm.page < lastPage) {
            vm.page++;
        }
        vm.getSalesOrderDetails();
    }

    vm.previousPage = function () {
        if (vm.page > 1) {
            vm.page--;
        }
        vm.getSalesOrderDetails();
    }

    vm.lastPage = function () {
        vm.page = Math.round(vm.salesOrderTotalQty / vm.displayQty)
        if (vm.salesOrderTotalQty % vm.displayQty != 0) {
            vm.page++;
        }
        vm.getSalesOrderDetails();
    }
    
}

