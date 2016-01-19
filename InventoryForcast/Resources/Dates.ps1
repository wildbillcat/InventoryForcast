$file = Get-Content -Path "C:\Users\wildbillcat\Sync\WebSpecs\ReportRefactor.csv"

$headers = $file[0].Split(',')

$Dates = @()
for($i = 1; $i -lt $headers.length; $i++){
    $Dates += get-date ("1-"+$headers[$i])
    "SKU, Month_Id, Quantity_Sold, Absolute_Quantity_Sold, Date" >> ("C:\Users\wildbillcat\Sync\WebSpecs\SKUs\" + $Dates[$j-1].ToString("MM-yyyy") + ".csv")
}

for($i = 1; $i -lt $file.length; $i++){
    $Values = $file[$i].Split(",")
    $Sku = $Values[0]
    for($j = 1; $j -lt $Values.Length; $j++){
        $Month_Id = ($Dates[$j-1].Month + 12*$Dates[$j-1].Year)
        $Quantity_Sold = $Values[$j]
        $Date = $Dates[$j-1]       
        #$Absolute_Quantity_Sold = $Quantity_Sold / $SeasonModifier
        $Sku + " " + $Quantity_Sold + " " + $Month_Id + " " + $Date
        "$Sku, $Month_Id, $Quantity_Sold, $Quantity_Sold, $Date" >> ("C:\Users\wildbillcat\Sync\WebSpecs\SKUs\" + $Dates[$j-1].ToString("MM-yyyy") + ".csv")
    }
}
"SKU, Month_Id, Quantity_Sold, Absolute_Quantity_Sold, Date"