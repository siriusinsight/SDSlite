Invoke-WebRequest $args[0] -OutFile netcdf.msi
msiexec -q .\netcdf.msi