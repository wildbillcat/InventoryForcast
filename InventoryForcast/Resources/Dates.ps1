$file = Get-Content -Path "C:\Users\wildbillcat\Documents\SpiderOak Hive\WebSpecs\ReportRefactor.csv"

$headers = $file[0].Split(',')

$Dates = @()
for($i = 1; $i -lt $headers.length; $i++){
    $Dates += get-date ("1-"+$headers[$i])
}
$Dates
#$file.length
for($i = 1; $i -lt $file.length; $i++){
    $Values = $file[$i].Split(",")
    $Sku = $Values[0]
    for($j = 1; $j -lt $Values.Length; $j++){
        $Month_Id = ($Dates[$j-1].Month + 12*$Dates[$j-1].Year)
        $Quantity_Sold = $Values[$j]
        $Date = $Dates[$j-1]
        $SeasonModifier = 1 #This is to start, case statement will change it to the expected value
        switch($Date.Month){
            1 { $SeasonModifier = 1 }
            2 { $SeasonModifier = 1 }
            3 { $SeasonModifier = 1.15 }
            9 { $SeasonModifier = 1.1385 }
            10 { $SeasonModifier = 1.1385 }
            11 { $SeasonModifier = 1.1385 }
            12 { $SeasonModifier = 1.02465 }
            default { $SeasonModifier = 1.265 } #April, May, June, July, August
        }        
        $Absolute_Quantity_Sold = $Quantity_Sold / $SeasonModifier
        $Sku + " " + $Quantity_Sold + " " + $Month_Id + " " + $Date
        Invoke-RestMethod -Method Post -Uri http://localhost:61154/api/MonthlyTotals -Body @{
            SKU = $Sku 
            Month_Id = $Month_Id
            Quantity_Sold = $Quantity_Sold
            Absolute_Quantity_Sold = $Absolute_Quantity_Sold
            Date = $Date
        }
    }
}
