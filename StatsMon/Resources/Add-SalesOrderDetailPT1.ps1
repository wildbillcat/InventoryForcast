$Sales = get-content -Path .\Sales_SalesOrderDetailPT1.csv

for($i = 1; $i -lt $Sales.Length; $i++){
    $SaleOrderArray = ($Sales[$i].Replace('$', '')).Split(',')
    Invoke-RestMethod -Method Post -Uri http://localhost:6220/api/SalesOrderDetails -Body @{
        SalesOrderID = $SaleOrderArray[0] 
        SalesOrderDetailID = $SaleOrderArray[1]
        CarrierTrackingNumber = $SaleOrderArray[2]
        OrderQty = $SaleOrderArray[3]
        ProductID = $SaleOrderArray[4]
        SpecialOfferID = $SaleOrderArray[5]
        UnitPrice = $SaleOrderArray[6]
        UnitPriceDiscount = $SaleOrderArray[7]
    }
}


