$file = Get-Content -Path "C:\Users\wildbillcat\Sync\WebSpecs\ReportRefactor.csv"

$headers = $file[0].Split(',')

$Dates = @()
for($i = 1; $i -lt $headers.length; $i++){
    Set-Content -Value $file -Path ("C:\Users\wildbillcat\Sync\WebSpecs\SKUs\" + (get-date ("1-"+$headers[$i])).ToString("MM-dd-yyyy") + ".csv")
}