for($i = 1; $i -lt 50; $i++){
    Invoke-RestMethod -Uri ("http://localhost:61154/api/SingleLinearForcasts/" + $i)
}