; Check arguments.
!ifndef VERSION
	!error "The /DVersion=x.y.z.w argument must be specified."
!endif

!define BUILDDIR "..\..\build"
!define RELEASEDIR "${BUILDDIR}\release"

; Define your application name
!define APPNAME "MbUnit"
!define APPNAMEANDVERSION "MbUnit v${VERSION}"
!define PRODUCT_WEB_SITE "http://www.mbunit.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\XsdTidy.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"

; Main Install settings
Name "${APPNAMEANDVERSION}"
InstallDir "$PROGRAMFILES\MbUnit"
OutFile "${RELEASEDIR}\MbUnit-${VERSION}-Setup.exe"

BrandingText "mbunit.com"

ShowInstDetails show
ShowUnInstDetails show

; Modern interface settings
!define MUI_COMPONENTSPAGE_SMALLDESC ; Put description on bottom.
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"
!include "MUI.nsh"

; Installer pages
!insertmacro MUI_PAGE_WELCOME
Page custom AddRemovePageEnter AddRemovePageLeave
!insertmacro MUI_PAGE_LICENSE "${BUILDDIR}\MbUnit License.txt"
Page custom UserSelectionPageEnter UserSelectionPageLeave
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

; Set languages (first is default language)
!insertmacro MUI_LANGUAGE "English"
!insertmacro MUI_RESERVEFILE_LANGDLL

; Store installation options in the Reserve data block for
; startup efficiency.
ReserveFile "AddRemovePage.ini"
ReserveFile "UserSelectionPage.ini"
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
Var INI_VALUE

; Stores "all" if installing for all users, else "current"
Var UserContext

Section "MainSection" Main
	SectionIn RO
	SetOverwrite on

	SetOutPath "$INSTDIR"
	File "${BUILDDIR}\ASL - Apache Software Foundation License.txt"
	File "${BUILDDIR}\MbUnit License.txt"
	File "${BUILDDIR}\Release Notes.txt"

	File "${BUILDDIR}\XsdTidy.exe"
	File "${BUILDDIR}\TestDriven.Framework.dll"
	File "${BUILDDIR}\Refly.dll"
	File "${BUILDDIR}\Refly.xml"
	File "${BUILDDIR}\QuickGraph.dll"
	File "${BUILDDIR}\QuickGraph.xml"
	File "${BUILDDIR}\QuickGraph.Algorithms.Graphviz.dll"
	File "${BUILDDIR}\QuickGraph.Algorithms.Graphviz.xml"
	File "${BUILDDIR}\QuickGraph.Algorithms.dll"
	File "${BUILDDIR}\QuickGraph.Algorithms.xml"
	File "${BUILDDIR}\NGraphviz.Layout.dll"
	File "${BUILDDIR}\NGraphviz.Helpers.dll"
	File "${BUILDDIR}\NGraphviz.dll"
	File "${BUILDDIR}\TestFu.dll" 
	File "${BUILDDIR}\TestFu.xml" 
	File "${BUILDDIR}\MbUnit.Framework.dll"
	File "${BUILDDIR}\MbUnit.Framework.xml"
	File "${BUILDDIR}\MbUnit.Framework.2.0.dll"
	File "${BUILDDIR}\MbUnit.Framework.2.0.xml"
	File "${BUILDDIR}\MbUnit.Tasks.dll"
	File "${BUILDDIR}\MbUnit.GUI.exe.config"
	File "${BUILDDIR}\MbUnit.GUI.exe"
	File "${BUILDDIR}\MbUnit.Cons.exe"
	File "${BUILDDIR}\MbUnit.Cons.exe.config"
	File "${BUILDDIR}\MbUnit.AddIn.dll"
	File "${BUILDDIR}\MbUnit.MSBuild.Tasks.dll"
	File "MbUnit.url"
	File "MbUnit Offline Documentation.url"
	File "MbUnit Online Documentation.url"

	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "" "10"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "AssemblyPath" "$PROGRAMFILES\MbUnit\MbUnit.AddIn.dll"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TypeName" "MbUnit.AddIn.MbUnitTestRunner"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TargetFrameworkAssemblyName" "MbUnit.Framework"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "Application" "$PROGRAMFILES\MbUnit\MbUnit.GUI.exe"
	
	WriteRegStr SHCTX "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit" "" "$PROGRAMFILES\MbUnit\"

	SetOutPath "$INSTDIR\VSSnippets\MbUnitCSharpSnippets"
	SetOverwrite try
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\autorunner.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\datafixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\model.snippet"  
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\rowtest.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\state.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\submodel.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\test.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\testexpectedexception.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\testfixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\testsuitefixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\typefixturewithproviderfactory.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\typefixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\usingmbunit.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\combinatorialtest.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitCSharpSnippets\processtestfixture.snippet"

	SetOutPath "$INSTDIR\VSSnippets\MbUnitVBSnippets"
	SetOverwrite try

	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\autorunner.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\datafixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\model.snippet"  
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\rowtest.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\state.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\submodel.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\test.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\testexpectedexception.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\testfixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\testsuitefixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\typefixturewithproviderfactory.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\typefixture.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\usingmbunit.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\combinatorialtest.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitVBSnippets\processtestfixture.snippet"
	
	SetOutPath "$INSTDIR\VSSnippets\MbUnitXMLSnippets"
	SetOverwrite try

	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitXMLSnippets\msbuild.snippet"
	File "${BUILDDIR}\Snippets\VSSnippets\MbUnitXMLSnippets\nant.snippet"
	
	;Start menu items
	CreateDirectory "$SMPROGRAMS\${APPNAME}" 
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit.GUI.lnk" "$INSTDIR\MbUnit.GUI.exe" 
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit Offline Documentation.lnk" "$INSTDIR\MbUnit Offline Documentation.url"
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit Online Documentation.lnk" "$INSTDIR\MbUnit Online Documentation.url"
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit Website.lnk" "$INSTDIR\MbUnit.url"
	CreateShortCut "$SMPROGRAMS\${APPNAME}\Uninstall.lnk" "$INSTDIR\uninstall.exe"
	
	;Register file association. 
	!define Index "Line${__LINE__}" 
	ReadRegStr $1 HKCR ".mbunit" "" 
	StrCmp $1 "" "${Index}-NoBackup" 
	StrCmp $1 "MbUnit" "${Index}-NoBackup" 
	WriteRegStr HKCR ".mbunit" "backup_val" $1 
	"${Index}-NoBackup:" 
	WriteRegStr HKCR ".mbunit" "" "MbUnit" 
	ReadRegStr $0 HKCR "MbUnit" "" 
	StrCmp $0 "" 0 "${Index}-Skip" 
	WriteRegStr HKCR "MbUnit" "" "MbUnit Project File" 
	WriteRegStr HKCR "MbUnit\shell" "" "open" 
	WriteRegStr HKCR "MbUnit\DefaultIcon" "" "$INSTDIR\MbUnit.GUI.exe,0" 
	"${Index}-Skip:" 
	WriteRegStr HKCR "MbUnit\shell\open\command" "" '$INSTDIR\MbUnit.GUI.exe "%1"' 

	System::Call 'Shell32::SHChangeNotify(i 0x8000000, i 0, i 0, i 0)' 
	!undef Index 
	
SectionEnd

Function un.onUninstSuccess
	HideWindow
	MessageBox MB_ICONINFORMATION|MB_OK "MbUnit was successfully removed from your computer."
FunctionEnd

; Initialization code
Function .onInit
	; Extract install option pages.
	!insertmacro MUI_INSTALLOPTIONS_EXTRACT "AddRemovePage.ini"
	!insertmacro MUI_INSTALLOPTIONS_EXTRACT "UserSelectionPage.ini"
FunctionEnd

; Uninstaller initialization code.
Function un.onInit
	ClearErrors
	ReadRegStr $0 HKCU "Software\${APPNAME}" ""
	IfErrors NotInstalledForUser
		SetShellVarContext current
		StrCpy $UserContext "current"
		StrCpy $INSTDIR $0
		Goto Installed
	NotInstalledForUser:

	ClearErrors
	ReadRegStr $0 HKLM "Software\${APPNAME}" ""
	IfErrors NotInstalledForSystem
		SetShellVarContext all
		StrCpy $UserContext "all"
		StrCpy $INSTDIR $0
		Goto Installed
	NotInstalledForSystem:

	MessageBox MB_OK "MbUnit does not appear to be installed!  Abandoning uninstallation."
	Abort

	Installed:	
FunctionEnd

Section -FinishSection
	WriteRegStr SHCTX "Software\${APPNAME}" "" "$INSTDIR"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "DisplayName" "${APPNAME}"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "UninstallString" "$INSTDIR\uninstall.exe"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "InstallLocation" "$INSTDIR"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "HelpLink" "http://www.mbunit.com/"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLUpdateInfo" "http://www.mbunit.com/"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLInfoAbout" "http://www.mbunit.com/"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "URLInfoAbout" "http://www.mbunit.com/"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoModify" "1"
	WriteRegStr SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}" "NoRepair" "1"
	WriteUninstaller "$INSTDIR\uninstall.exe"
SectionEnd

Section Uninstall
	DeleteRegKey SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit"
	DeleteRegKey SHCTX "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit"
	
	DeleteRegKey SHCTX "${PRODUCT_UNINST_KEY}"
	DeleteRegKey SHCTX "${PRODUCT_DIR_REGKEY}"
	
	
	;Unregister file association 
	!define Index "Line${__LINE__}" 
	ReadRegStr $1 HKCR ".mbunit" "" 
	StrCmp $1 "OptionsFile" 0 "${Index}-NoOwn" ; only do this if we own it 
	ReadRegStr $1 HKCR ".mbunit" "backup_val" 
	StrCmp $1 "" 0 "${Index}-Restore" ; if backup="" then delete the whole key 
	DeleteRegKey HKCR ".mbunit" 
	Goto "${Index}-NoOwn" 
	"${Index}-Restore:" 
	WriteRegStr HKCR ".mbunit" "" $1 
	DeleteRegValue HKCR ".mbunit" "backup_val" 

	DeleteRegKey HKCR "MbUnit" ;Delete key with association settings 

	System::Call 'Shell32::SHChangeNotify(i 0x8000000, i 0, i 0, i 0)' 
	"${Index}-NoOwn:" 
	!undef Index 

	; Delete Shortcuts
	RMDir /r "$SMPROGRAMS\${APPNAME}"

	; Remove from registry...
	DeleteRegKey SHCTX "Software\Microsoft\Windows\CurrentVersion\Uninstall\${APPNAME}"
	DeleteRegKey SHCTX "SOFTWARE\${APPNAME}"

	; Delete self
	Delete "$INSTDIR\uninstall.exe"

	; Remove all remaining contents
	RMDir /r "$INSTDIR"
SectionEnd

; Add-remove page.
Var OLD_INSTALL_DIR
Function AddRemovePageEnter
	ClearErrors
	ReadRegStr $OLD_INSTALL_DIR HKCU "Software\${APPNAME}" ""
	IfErrors 0 AlreadyInstalled
	ReadRegStr $OLD_INSTALL_DIR HKLM "Software\${APPNAME}" ""
	IfErrors 0 AlreadyInstalled
	Return

	AlreadyInstalled:
	!insertmacro MUI_HEADER_TEXT "Installation Type" "Please select whether to upgrade or remove the currently installed version."
	!insertmacro MUI_INSTALLOPTIONS_DISPLAY "AddRemovePage.ini"
FunctionEnd

Function AddRemovePageLeave
	!insertmacro MUI_INSTALLOPTIONS_READ $INI_VALUE "AddRemovePage.ini" "Field 2" "State"

	MessageBox MB_OKCANCEL "Uninstall the current version?" IDOK Uninstall
	Abort

	Uninstall:
	ExecWait '"$OLD_INSTALL_DIR\uninstall.exe" /S _?=$OLD_INSTALL_DIR' $0
	DetailPrint "Uninstaller returned $0"

	IntCmp $INI_VALUE 1 Upgrade
	Quit

	Upgrade:
FunctionEnd

; User-selection page.
Function UserSelectionPageEnter
	!insertmacro MUI_HEADER_TEXT "Installation Options" "Please select for which users to install MbUnit."
	!insertmacro MUI_INSTALLOPTIONS_DISPLAY "UserSelectionPage.ini"
FunctionEnd

Function UserSelectionPageLeave
	!insertmacro MUI_INSTALLOPTIONS_READ $INI_VALUE "UserSelectionPage.ini" "Field 2" "State"
	IntCmp $INI_VALUE 0 CurrentUserOnly
		SetShellVarContext all
		StrCpy $UserContext "all"
		Goto Done
	CurrentUserOnly:
		SetShellVarContext current
		StrCpy $UserContext "current"
	Done:
FunctionEnd

