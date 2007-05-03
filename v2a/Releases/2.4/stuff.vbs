Const ForReading = 1

dim ArgObj, var1


Set ArgObj = WScript.Arguments 

var1 = ArgObj(0)

Set objRegEx = CreateObject("VBScript.RegExp")
objRegEx.Pattern = "{V}"

Set fSO = CreateObject("Scripting.FileSystemObject")
Set tFile = fSO.OpenTextFile("MbUnitBuild.nsi.new", ForReading)
Set nFile = fso.CreateTextFile("MbUnitBuild2.nsi", True)

Do Until tFile.AtEndOfStream
    strSearchString = tFile.ReadLine  
   
     ReplacedString = objRegEx.Replace(strSearchString, var1)
     
     nFile.WriteLine(ReplacedString)

Loop

tFile.Close
nFile.Close
