angular.module('app', []);

angular
    .module('app')
    .controller('SkuPurchaseController', SkuPurchaseController);

function SkuPurchaseController($http) {
    var vm = this;
    vm.skuPurchases = {};
    vm.skuTotalQty = 0;
    vm.page = 1;
    vm.displayQty = 100;
    
    /*API Functions*/
    vm.getSkuPurchases = function () {
        $http.get('/api/SKUPurchases?Quantity=' + vm.displayQty + '&Page=' + vm.page).
            success(function (data) {
                vm.skuPurchases = data;              
            });
    };

    vm.getSkuPurchase = function ($SKUPurchase) {
        $http.get('/api/SKUPurchases/' + $SKUPurchase.Id + '?PurchaseID=' + $SKUPurchase.PurchaseID).
            success(function (data) {
                var index = vm.skuPurchases.indexOf($SKUPurchase)
                if (index > -1) {
                    vm.skuPurchases.splice(index, 1);
                }
                vm.skuPurchases.push(data);
            });
    };

    vm.deleteSkuPurchase = function ($SKUPurchase) {
        console.log("Application Initialized");
        $http.delete('/api/SKUPurchases/' + $SKUPurchase.Id + '?PurchaseID=' + $SKUPurchase.PurchaseID).
            success(function (data) {
                var index = vm.skuPurchases.indexOf($SKUPurchase)
                if (index > -1) {
                    vm.skuPurchases.splice(index, 1);
                }
            });
    };

    vm.getSkuPurchaseQuantity = function ($SKUPurchase) {
        $http.get('/api/SKUPurchases/GetSKUPurchaseQuantity').
            success(function (data) {
                vm.skuTotalQty = data;
            });
    };
    
    /*Page initialization*/
    vm.getSkuPurchaseQuantity();
    vm.getSkuPurchases();

    /*Pagination Functions*/
    vm.firstPage = function () {
        if (vm.page > 1) {
            vm.page = 1;
            vm.getSkuPurchases();;
        }
    }

    vm.nextPage = function () {
        var lastPage = Math.round(vm.skuTotalQty / vm.displayQty);
        if (vm.skuTotalQty % vm.displayQty != 0) {
            lastPage++;
        }
        if (vm.page < lastPage) {
            vm.page++;
        }
        vm.getSkuPurchases();
    }

    vm.previousPage = function () {
        if (vm.page > 1) {
            vm.page--;
        }
        vm.getSkuPurchases();
    }

    vm.lastPage = function () {
        vm.page = Math.round(vm.skuTotalQty / vm.displayQty)
        if (vm.skuTotalQty % vm.displayQty != 0) {
            vm.page++;
        }
        vm.getSkuPurchases();
    }
    
}

