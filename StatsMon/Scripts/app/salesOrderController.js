angular.module('app', []);

angular
    .module('app')
    .controller('SalesOrderController', SalesOrderController);

function SalesOrderController($http) {
    var vm = this;
    vm.salesOrders = {};
    vm.salesOrderTotalQty = 0;
    vm.page = 1;
    vm.displayQty = 100;
    console.log("Start Sales Order Controller")
    /*API Functions*/
    vm.getSalesOrders = function () {
        console.log("Start Data Fetch")
        $http.get('/api/SalesOrders?Quantity=' + vm.displayQty + '&Page=' + vm.page).
            success(function (data) {
                vm.salesOrders = data;
                console.log("Data Fetched")
            });
    };

    vm.getSalesOrder = function ($SalesOrder) {
        console.log("Fetch Single Order")
        $http.get('/api/SalesOrders/' + $SalesOrder.SalesOrderID).
            success(function (data) {
                var index = vm.salesOrders.indexOf($SalesOrder)
                if (index > -1) {
                    vm.salesOrders.splice(index, 1);
                }
                vm.salesOrders.push(data);
            });
    };

    vm.deleteSalesOrder = function ($SalesOrder) {
        console.log("Application Initialized");
        $http.delete('/api/SalesOrders/' + $SalesOrder.SalesOrderID).
            success(function (data) {
                var index = vm.salesOrders.indexOf($SalesOrder)
                if (index > -1) {
                    vm.salesOrders.splice(index, 1);
                }
            });
    };

    vm.getSalesOrderQuantity = function ($SalesOrder) {
        $http.get('/api/SalesOrders/GetSalesOrderQuantity').
            success(function (data) {
                vm.salesOrderTotalQty = data;
            });
    };
    
    /*Page initialization*/
    vm.getSalesOrderQuantity();
    vm.getSalesOrders();

    /*Pagination Functions*/
    vm.firstPage = function () {
        if (vm.page > 1) {
            vm.page = 1;
            vm.getSalesOrders();;
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
        vm.getSalesOrders();
    }

    vm.previousPage = function () {
        if (vm.page > 1) {
            vm.page--;
        }
        vm.getSalesOrders();
    }

    vm.lastPage = function () {
        vm.page = Math.round(vm.salesOrderTotalQty / vm.displayQty)
        if (vm.salesOrderTotalQty % vm.displayQty != 0) {
            vm.page++;
        }
        vm.getSalesOrders();
    }
    
}

