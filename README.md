# Configuración MSMQ

## Powershell
### Ejecutar los siguientes comandos como ADMIN

1.-

`powershell -Command Enable-WindowsOptionalFeature -Online -FeatureName MSMQ-Server -All`

2.-

`powershell -Command "$ErrorActionPreference = 'Stop';  $ProgressPreference = 'SilentlyContinue';`

## Abrir los siguientes puertos

### Ports

`2103,2105,135,1801,2101`

## Configurar registros en REGEDIT

1.-
`HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters\Security\NewRemoteReadServerAllowNoneSecurityClient registry entry (a DWORD 32) and set it to 1.`

2.-
` the HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters\Security\NewRemoteReadServerDenyWorkgroupClient registry entry (a DWORD 32) and set it to 1.`
