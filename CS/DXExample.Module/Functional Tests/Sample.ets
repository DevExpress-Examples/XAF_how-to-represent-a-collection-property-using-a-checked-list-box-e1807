;#DropDB DXExampleEasyTest

#Application DXExampleWin
#Application DXExampleWeb

*Action Navigation(User)

*ProcessRecord
 Full Name = ''

#IfDef DXExampleWeb
*Action Edit
#EndIf
 
*FillForm
 Full Name = Test User
 
*Action Cancel

#IfDef DXExampleWin
*HandleDialog
 Respond = Yes
#EndIf

*CheckFieldValues
 Full Name = ''
