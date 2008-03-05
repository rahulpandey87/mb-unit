; Check arguments.
!ifndef VERSION
	!error "The /DVersion=x.y.z.w argument must be specified."
!endif
!ifndef ROOTDIR
	!error "The /DRootDir=... argument must be specified."
!endif

; Common directories
!define BUILDDIR "${ROOTDIR}\build"
!define TARGETDIR "${BUILDDIR}\target"
!define RELEASEDIR "${BUILDDIR}\release"

; Define your application name
!define APPNAME "MbUnit"
!define APPNAMEANDVERSION "MbUnit v${VERSION}"

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
!insertmacro MUI_PAGE_LICENSE "${TARGETDIR}\MbUnit License.txt"
Page custom UserSelectionPageEnter UserSelectionPageLeave
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
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

Section "!MbUnit v2" MbUnit2Section
	SectionIn RO
	SetOverwrite on

	SetOutPath "$INSTDIR"
	File /r "${TARGETDIR}\*.*"

	File "MbUnit.url"
	File "MbUnit Online Documentation.url"

	; Test Driven .Net
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "" "10"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "AssemblyPath" "$INSTDIR\bin\MbUnit.AddIn.dll"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TypeName" "MbUnit.AddIn.MbUnitTestRunner"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TargetFrameworkAssemblyName" "MbUnit.Framework"
	WriteRegStr SHCTX "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "Application" "$INSTDIR\bin\MbUnit.GUI.exe"
	
	; Register the folder so that Visual Studio Add References can find it
	WriteRegStr SHCTX "SOFTWARE\Microsoft\.NETFramework\AssemblyFolders\MbUnit" "" "$INSTDIR\bin"
	WriteRegStr SHCTX "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit" "" "$INSTDIR\bin"

	;Start menu items
	CreateDirectory "$SMPROGRAMS\${APPNAME}" 
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit.GUI.lnk" "$INSTDIR\bin\MbUnit.GUI.exe" 
	CreateShortCut "$SMPROGRAMS\${APPNAME}\MbUnit Offline Documentation.lnk" "$INSTDIR\docs\MbUnit.chm"
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
	WriteRegStr HKCR "MbUnit\DefaultIcon" "" "$INSTDIR\bin\MbUnit.GUI.exe,0" 
	"${Index}-Skip:" 
	WriteRegStr HKCR "MbUnit\shell\open\command" "" '$INSTDIR\bin\MbUnit.GUI.exe "%1"' 

	System::Call 'Shell32::SHChangeNotify(i 0x8000000, i 0, i 0, i 0)' 
	!undef Index 	
SectionEnd

Section "MbUnit v2 Visual Studio 2005 Templates" MbUnit2VS2005TemplatesSection
	; Set Section properties
	SetOverwrite on

	ClearErrors
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\VisualStudio\8.0" "InstallDir"
	IfErrors SkipVS2005Templates

        ; C# Item Templates
	SetOutPath "$0\ItemTemplates\CSharp\Test"
	File "${TARGETDIR}\extras\Templates\VS2005\ItemTemplates\CSharp\Test\MbUnit2.TestFixtureTemplate.CSharp.zip"

	; C# Project Templates
	SetOutPath "$0\ProjectTemplates\CSharp\Test"
	File "${TARGETDIR}\extras\Templates\VS2005\ProjectTemplates\CSharp\Test\MbUnit2.TestProjectTemplate.CSharp.zip"

        ; VB Item Templates
	SetOutPath "$0\ItemTemplates\VisualBasic\Test"
	File "${TARGETDIR}\extras\Templates\VS2005\ItemTemplates\VisualBasic\Test\MbUnit2.TestFixtureTemplate.VisualBasic.zip"

	; VB Project Templates
	SetOutPath "$0\ProjectTemplates\VisualBasic\Test"
	File "${TARGETDIR}\extras\Templates\VS2005\ProjectTemplates\VisualBasic\Test\MbUnit2.TestProjectTemplate.VisualBasic.zip"

	; Run DevEnv /setup to register the templates.
	ExecWait '"$0\devenv.exe" /setup'

	SkipVS2005Templates:
SectionEnd

Section "MbUnit v2 Visual Studio 2008 Templates" MbUnit2VS2008TemplatesSection
	; Set Section properties
	SetOverwrite on

	ClearErrors
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\VisualStudio\9.0" "InstallDir"
	IfErrors SkipVS2008Templates

        ; C# Item Templates
	SetOutPath "$0\ItemTemplates\CSharp\Test"
	File "${TARGETDIR}\extras\Templates\VS2008\ItemTemplates\CSharp\Test\MbUnit2.TestFixtureTemplate.CSharp.zip"

	; C# Project Templates
	SetOutPath "$0\ProjectTemplates\CSharp\Test"
	File "${TARGETDIR}\extras\Templates\VS2008\ProjectTemplates\CSharp\Test\MbUnit2.TestProjectTemplate.CSharp.zip"
	File "${TARGETDIR}\extras\Templates\VS2008\ProjectTemplates\CSharp\Test\MbUnit2.MvcWebApplicationTestProjectTemplate.CSharp.zip"

	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\C#" "Path" "CSharp\Test"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\C#" "Template" "MbUnit2.MvcWebApplicationTestProjectTemplate.CSharp.zip"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\C#" "TestFrameworkName" "MbUnit v2"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\C#" "AdditionalInfo" "http://www.mbunit.com/"

        ; VB Item Templates
	SetOutPath "$0\ItemTemplates\VisualBasic\Test"
	File "${TARGETDIR}\extras\Templates\VS2008\ItemTemplates\VisualBasic\Test\MbUnit2.TestFixtureTemplate.VisualBasic.zip"

	; VB Project Templates
	SetOutPath "$0\ProjectTemplates\VisualBasic\Test"
	File "${TARGETDIR}\extras\Templates\VS2008\ProjectTemplates\VisualBasic\Test\MbUnit2.TestProjectTemplate.VisualBasic.zip"
	File "${TARGETDIR}\extras\Templates\VS2008\ProjectTemplates\VisualBasic\Test\MbUnit2.MvcWebApplicationTestProjectTemplate.VisualBasic.zip"

	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\VB" "Path" "VisualBasic\Test"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\VB" "Template" "MbUnit2.MvcWebApplicationTestProjectTemplate.VisualBasic.zip"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\VB" "TestFrameworkName" "MbUnit v2"
	WriteRegStr HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\VB" "AdditionalInfo" "http://www.mbunit.com/"

	; Run DevEnv /setup to register the templates.
	ExecWait '"$0\devenv.exe" /setup'

	SkipVS2008Templates:
SectionEnd

Function un.onUninstSuccess
	HideWindow
	MessageBox MB_ICONINFORMATION|MB_OK "MbUnit was successfully removed from your computer."
FunctionEnd

; Component descriptions
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
	!insertmacro MUI_DESCRIPTION_TEXT ${MbUnit2Section} "Installs the MbUnit v2 test framework."
	!insertmacro MUI_DESCRIPTION_TEXT ${MbUnit2VS2005TemplatesSection} "Installs the MbUnit v2 Visual Studio 2005 templates."
	!insertmacro MUI_DESCRIPTION_TEXT ${MbUnit2VS2008TemplatesSection} "Installs the MbUnit v2 Visual Studio 2008 templates."
!insertmacro MUI_FUNCTION_DESCRIPTION_END

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
	DeleteRegKey SHCTX "SOFTWARE\Microsoft\.NETFramework\AssemblyFolders\MbUnit"
	DeleteRegKey SHCTX "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit"
	
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

	; Uninstall the Visual Studio 2005 templates
	ClearErrors
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\VisualStudio\8.0" "InstallDir"
	IfErrors SkipVS2005Templates

	Delete "$0\ItemTemplates\CSharp\Test\MbUnit2.TestFixtureTemplate.CSharp.zip"
	Delete "$0\ProjectTemplates\CSharp\Test\MbUnit2.TestProjectTemplate.CSharp.zip"

	Delete "$0\ItemTemplates\VisualBasic\Test\MbUnit2.TestFixtureTemplate.VisualBasic.zip"
	Delete "$0\ProjectTemplates\VisualBasic\Test\MbUnit2.TestProjectTemplate.VisualBasic.zip"

	; Run DevEnv /setup to unregister the templates.
	ExecWait '"$0\devenv.exe" /setup'

	SkipVS2005Templates:

	; Uninstall the Visual Studio 2008 templates
	ClearErrors
	ReadRegStr $0 HKLM "SOFTWARE\Microsoft\VisualStudio\9.0" "InstallDir"
	IfErrors SkipVS2008Templates

	Delete "$0\ItemTemplates\CSharp\Test\MbUnit2.TestFixtureTemplate.CSharp.zip"
	Delete "$0\ProjectTemplates\CSharp\Test\MbUnit2.TestProjectTemplate.CSharp.zip"
	Delete "$0\ProjectTemplates\CSharp\Test\MbUnit2.MvcWebApplicationTestProjectTemplate.CSharp.zip"
	DeleteRegKey HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\C#"

	Delete "$0\ItemTemplates\VisualBasic\Test\MbUnit2.TestFixtureTemplate.VisualBasic.zip"
	Delete "$0\ProjectTemplates\VisualBasic\Test\MbUnit2.TestProjectTemplate.VisualBasic.zip"
	Delete "$0\ProjectTemplates\VisualBasic\Test\MbUnit2.MvcWebApplicationTestProjectTemplate.VisualBasic.zip"
	DeleteRegKey HKLM "SOFTWARE\Microsoft\VisualStudio\9.0\MVC\TestProjectTemplates\MbUnit2\VB"

	; Run DevEnv /setup to unregister the templates.
	ExecWait '"$0\devenv.exe" /setup'

	SkipVS2008Templates:

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

	; Note: We don't uninstall silently anymore because it takes too
	;       long and it sucks not to get any feedback during the process.
	ExecWait '"$OLD_INSTALL_DIR\uninstall.exe" _?=$OLD_INSTALL_DIR' $0
	IntCmp $0 0 Ok
	MessageBox MB_OK "Cannot proceed because the old version was not successfully uninstalled."
	Abort

	Ok:
	IntCmp $INI_VALUE 1 Upgrade
	Quit

	Upgrade:
	BringToFront
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

