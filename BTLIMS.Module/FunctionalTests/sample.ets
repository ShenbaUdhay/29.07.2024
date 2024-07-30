;#DropDB BTLIMSEasyTest

#Application BTLIMS.Win
#Application BTLIMS.Web
#IfDef BTLIMS.Win

*Action About...
 
*Action OK

#EndIf

#IfDef BTLIMS.Web
!Action Navigation(My Details)
#EndIf
