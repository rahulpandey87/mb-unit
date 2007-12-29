; Check arguments.
!ifndef VERSION
	!error "The /DVersion=x.y.z.w argument must be specified."
!endif

!define BUILDDIR "..\..\build"
!define RELEASEDIR "${BUILDDIR}\release"

; HM NIS Edit Wizard helper defines
!define PRODUCT_NAME "MbUnit"
!define PRODUCT_VERSION "${VERSION}"
!define PRODUCT_WEB_SITE "http://www.mbunit.com"
!define PRODUCT_DIR_REGKEY "Software\Microsoft\Windows\CurrentVersion\App Paths\XsdTidy.exe"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"

; MUI 1.67 compatible ------
!include "MUI.nsh"

; MUI Settings
!define MUI_ABORTWARNING
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\modern-install.ico"
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\modern-uninstall.ico"

; Welcome page
!insertmacro MUI_PAGE_WELCOME
; Instfiles page
!insertmacro MUI_PAGE_INSTFILES
; Finish page
!insertmacro MUI_PAGE_FINISH

; Uninstaller pages
!insertmacro MUI_UNPAGE_INSTFILES

; Language files
!insertmacro MUI_LANGUAGE "English"

; MUI end ------

Name "${PRODUCT_NAME} v${PRODUCT_VERSION}"
OutFile "${RELEASEDIR}\MbUnit-${VERSION}-Setup.exe"

BrandingText "mbunit.com"

InstallDir "$PROGRAMFILES\MbUnit"
InstallDirRegKey HKLM "${PRODUCT_DIR_REGKEY}" ""
ShowInstDetails show
ShowUnInstDetails show

Section "MainSection" SEC01
  SetShellVarContext all
  SetOutPath "$INSTDIR"
  SetOverwrite on
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

  WriteRegStr HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "" "10"
  WriteRegStr HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "AssemblyPath" "$PROGRAMFILES\MbUnit\MbUnit.AddIn.dll"
  WriteRegStr HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TypeName" "MbUnit.AddIn.MbUnitTestRunner"
  WriteRegStr HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TargetFrameworkAssemblyName" "MbUnit.Framework"
  WriteRegStr HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "Application" "$PROGRAMFILES\MbUnit\MbUnit.GUI.exe"
  
  WriteRegStr HKLM "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "" "10"
  WriteRegStr HKLM "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "AssemblyPath" "$PROGRAMFILES\MbUnit\MbUnit.AddIn.dll"
  WriteRegStr HKLM "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TypeName" "MbUnit.AddIn.MbUnitTestRunner"
  WriteRegStr HKLM "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "TargetFrameworkAssemblyName" "MbUnit.Framework"
  WriteRegStr HKLM "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit" "Application" "$PROGRAMFILES\MbUnit\MbUnit.GUI.exe"
  
  WriteRegStr HKCU "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit" "" "$PROGRAMFILES\MbUnit\"

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
  CreateDirectory "$SMPROGRAMS\${PRODUCT_NAME}" 
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\MbUnit.GUI.lnk" "$INSTDIR\MbUnit.GUI.exe" 
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\MbUnit Offline Documentation.lnk" "$INSTDIR\MbUnit Offline Documentation.url"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\MbUnit Online Documentation.lnk" "$INSTDIR\MbUnit Online Documentation.url"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\MbUnit Website.lnk" "$INSTDIR\MbUnit.url"
  CreateShortCut "$SMPROGRAMS\${PRODUCT_NAME}\Uninstall.lnk" "$INSTDIR\uninst.exe"
  
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

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr HKLM "${PRODUCT_DIR_REGKEY}" "" "$INSTDIR\XsdTidy.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayIcon" "$INSTDIR\XsdTidy.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
SectionEnd


Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "MbUnit was successfully removed from your computer."
FunctionEnd

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "Are you sure you want to completely remove MbUnit and all of its components?" IDYES +2
  Abort
FunctionEnd

Section Uninstall
  SetShellVarContext all

  RMDir /r "$SMPROGRAMS\${PRODUCT_NAME}"
  RMDir /r "$INSTDIR"

  DeleteRegKey HKCU "SOFTWARE\MutantDesign\TestDriven.NET\TestRunners\MbUnit"
  DeleteRegKey HKCU "SOFTWARE\Microsoft\.NETFramework\v2.0.50727\AssemblyFoldersEx\MbUnit"
  
  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  DeleteRegKey HKLM "${PRODUCT_DIR_REGKEY}"
  
  
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
  
  
  SetAutoClose true
SectionEnd
