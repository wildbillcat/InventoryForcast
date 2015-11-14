$Sales = get-content -Path .\Sales_SalesOrderHeaderPT1.csv

for($i = 0; $i -lt $Sales.Length; $i++){
    $SaleOrderArray = ($Sales[$i].Replace('$', '')).Split(',')
    Invoke-RestMethod -Method Post -Uri http://localhost:6220/api/SalesOrders -Body @{
        SalesOrderID = $SaleOrderArray[0] 
        OrderDate = $SaleOrderArray[2]
        ShipDate = $SaleOrderArray[4]
        Status = $SaleOrderArray[5]
        SalesOrderNumber = $SaleOrderArray[7]
        PurchaseOrderNumber = $SaleOrderArray[8]
        AccountNumber = $SaleOrderArray[9]
        CustomerID = $SaleOrderArray[10]
        TerritoryID = $SaleOrderArray[12]
        BillToAddressID = $SaleOrderArray[13]
        ShipToAddressID = $SaleOrderArray[14]
        ShipMethodID = $SaleOrderArray[15]
        CreditCardID = $SaleOrderArray[16]
        CreditCardApprovalCode = $SaleOrderArray[17]
        CurrencyRateID = $SaleOrderArray[18]
        SubTotal = $SaleOrderArray[19]
        TaxAmt = $SaleOrderArray[20]
        Freight = $SaleOrderArray[21]
        Comment = $SaleOrderArray[23]
    }
}


