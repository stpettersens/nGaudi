# nGaudi installation script (NSIS) for Windows
#
# This script uses EnvVarUpdate.nsh from:
# http://nsis.sourceforge.net/Environmental_Variables:_append,_prepend,_and_remove_entries
#
# This script also uses the NSISArray plug-in from:
# http://nsis.sourceforge.net/Arrays_in_NSIS
#
# NB: Convert tabs->spaces if you make alterations to this file!

Name nGaudi

# General Symbol Definitions
!define REGKEY "SOFTWARE\$(^Name)"
!define VERSION 0.1.0.0
!define COMPANY "Sam Saint-Pettersen"
!define URL http://stpettersens.github.com/nGaudi
!define DESC "nGaudi platform agnostic build tool"
!define COPYRIGHT "(c) 2010 Sam Saint-Pettersen"

# MUI Symbol Definitions
!define MUI_WELCOMEFINISHPAGE_BITMAP "${NSISDIR}\Contrib\Graphics\Wizard\orange.bmp" ; Change later to custom
!define MUI_ICON "${NSISDIR}\Contrib\Graphics\Icons\orange-install.ico"
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_LICENSEPAGE_CHECKBOX
!define MUI_STARTMENUPAGE_REGISTRY_ROOT HKLM
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_REGISTRY_KEY ${REGKEY}
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME StartMenuGroup
!define MUI_STARTMENUPAGE_DEFAULTFOLDER Gaudi
!define MUI_UNICON "${NSISDIR}\Contrib\Graphics\Icons\orange-uninstall.ico"
!define MUI_UNFINISHPAGE_NOAUTOCLOSE

# Included files
!include Sections.nsh
!include MUI2.nsh
!include LogicLib.nsh
!include StrFunc.nsh
; " " Quoted includes do not ship with NSIS by default
!include "EnvVarUpdate.nsh" 
!include "NSISArray.nsh"

# Variables
Var StartMenuGroup
Var lib
Var libsFound
Var indx
Var libs

# Installer pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE license.txt
!insertmacro MUI_PAGE_COMPONENTS
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuGroup
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

# Installer languages
!insertmacro MUI_LANGUAGE English

# Installer attributes
OutFile nGaudi_setup.exe
InstallDir $PROGRAMFILES\nGaudi
CRCCheck on
XPStyle on
ShowInstDetails show
VIProductVersion 0.1.0.0
VIAddVersionKey ProductName nGaudi
VIAddVersionKey ProductVersion "${VERSION}"
VIAddVersionKey CompanyName "${COMPANY}"
VIAddVersionKey CompanyWebsite "${URL}"
VIAddVersionKey FileVersion "${VERSION}"
VIAddVersionKey FileDescription "${DESC}"
VIAddVersionKey LegalCopyright "${COPYRIGHT}"
InstallDirRegKey HKLM "${REGKEY}" Path
ShowUninstDetails show

# Create libraries ("libs") arrays and array functions for use later
# <BEGIN>
${Array} libs 1 20
${ArrayFunc} Read
${ArrayFunc} Write
${ArrayFunc} FreeUnusedMem

${Array} libsRedun 1 20
${ArrayFunc} Read
${ArrayFunc} Push
${ArrayFunc} FreeUnusedMem

Section
    ${libs->Init}
    ${libsRedun->Init}
    ${libs->Write} 0 "Jayrock.Json.dll"
    ${libs->Write} 1 "IronPython.dll"
    ${libs->Write} 2 "IronPython.Modules.dll"
    ${libs->Write} 3 "Microsoft.Scripting.dll"
    ${libs->Write} 4 "Microsoft.Dynamic.dll"
    ${libs->Write} 5 "Ionic.Zip.dll"
    ${libs->FreeUnusedMem}
    ${libsRedun->FreeUnusedMem}
SectionEnd
# <END>

# Detect presence of a suitable CLR Framework on system
# That is, that it exists and is at least version 4.0+ capable
Function detectCLR
    SetOutPath .
    File CLRCheck.exe ; Extract small CLR version checker program
    ; Attempt to execute CLR version checker program to get version
    ; Also does a suitable CLR (.NET/Mono Framework) even exist?
    nsExec::ExecToStack `CLRCheck.exe 4.0` 
    Pop $0 ; Pop return code from program from stack
    Pop $1 ; Pop stdout from program from stack
    ${If} $0 == "error" ; Error occurs when a CLR cannot be found...
        DetailPrint "Fail: No CLR (.NET/Mono Framework) detected!"
        ${If} ${Cmd} `MessageBox MB_YESNO|MB_ICONQUESTION "No CLR Framework was detected. Download one now?" IDYES`
        downloadJVM:
        ; Go to download site for Microsoft .NET Framework
        DetailPrint "Opening Microsoft .NET page in your web browser."
        ExecShell "open" "http://www.microsoft.com/net" 
        GoTo badCLR
        ${Else}
            GoTo badCLR
        ${EndIf}
    ${EndIf}
    DetailPrint "Detected CLR (.NET/Mono Framework) version: $1" ; Display detected version 
    # Check this CLR meets minimum version requirement (4.0.x)
    ${If} $0 == "1"
        DetailPrint "Pass: CLR reports suitable version (4.0+)"
    ${ElseIf} $0 == "0"
        DetailPrint "Fail: CLR reports unsuitable version (< 4.0)"
        DetailPrint "Please update it."
        GoTo downloadJVM
        badCLR: ; Done with CLR check; failed
        DetailPrint "------------------------------------------------------------------------------------------"
        DetailPrint "CLR (.NET/Mono Framework) requirement was not met, so installation was aborted."
        DetailPrint "Please download and/or install the latest .NET or Mono Framework and run this setup again."
        DetailPrint "------------------------------------------------------------------------------------------"
        Delete CLRCheck.exe ; Done with the checker program, delete it
        Abort
    ${EndIf}
FunctionEnd

# Detect if third-party libraries Gaudi requires are
# present on system by looking in the system's CLASSPATH
;Function detectTPLibs
    ;File FindInVar.class ; Extract small FindInVar program
    ;IntOp $libs $libs + 5 ; Assign 5 as number of libraries
    ;IntOp $libsFound $libsFound + 0 ; Set libraries found to 0
    ;IntOp $indx $indx + 0 ; Set loop index to 0
    ;${DoUntil} $indx == $libs
        ;${libs->Read} $lib $indx ; Read indexed library as current library  to check for
        ; Execute FindInVar program to check for current library in CLASSPATH
        ;nsExec::ExecToStack `java -classpath . FindInVar CLASSPATH $lib` 
        ;Pop $0 ; Pop return code from program from stack
        ;Pop $1 ; Pop stdout from program from stack (unused, but clears the stack)
        ;${If} $0 == "1"
            ;DetailPrint "Found library: $lib" ; Print that found library
            ;IntOp $libsFound $libsFound + 1 ; Increment number of found libraries
            ;${libsRedun->Push} $lib ; Add found library to libraries erase after copying
        ;${ElseIf} $0 == "0"
            ;DetailPrint "Did not find library: $lib" ; Otherwise, print that did not find library
        ;${EndIf}
        ;IntOp $indx $indx + 1
    ;${Loop}
    ;DetailPrint "Found $libsFound of 4 libraries already installed."
    ;${If} $libsFound < 4:
        ;DetailPrint "Warning: Libraries are missing. This may be a problem if you are not installing them."
    ;${EndIf}
    ;Delete FindInVar.class ; Done with this program, delete it
    ;${libs->Delete} ; Delete first array, done with
;FunctionEnd

;# Remove installed libraries that were found in CLASSPATH before
;# installation and therefore are unneeded
;Function removeDuplicates
    ;DetailPrint "Removing any duplicate libraries..."
    ;StrCpy $lib "x" ; Make lib variable not blank initially so that loop works
    ;IntOp $indx $indx - 4 ; Reset loop index to 0
    ;${DoUntil} $lib == ""
        ;${libsRedun->Read} $lib $indx ; Get each duplicate library
        ;Delete $INSTDIR\lib\$lib ; Delete each duplicate library from $INSTDIR
        ;IntOp $indx $indx + 1 ; Increment index
    ;${Loop}
    ;${libsRedun->Delete} ; Delete second array, done with
;FunctionEnd

# Installer sections
Section -Main SEC0000
    Call detectCLR
    ;Call detectTPLibs
    SetOverwrite on
SectionEnd

# Component selection:
# Core program 
InstType /COMPONENTSONLYONCUSTOM
Section "Gaudi" GaudiTool
    SectionIn 1 RO
    SetOutPath $INSTDIR
    File nGaudi.exe
    File license.txt
SectionEnd

# Plug-ins for core program]
;Section "Plug-ins" Plugins
    ;SetOutPath $INSTDIR\plugins
    ; TODO: Change to packaged plug-in files:
    ;File plugins\ExamplePluginA.gpod
	;File plugins\ExamplePluginB.gpod
;SectionEnd

# Library dependencies for core program
Section "Third-party libraries" TPLibs
    ; Prompt user if they want the installable libraries to be  
    ; available to other CLR-based programs
    ${If} ${Cmd} `MessageBox MB_YESNO|MB_ICONQUESTION "Install libraries for all CLR applications?" IDYES`
        ; Use FindInVar program again to find 1st path in GAC
        ;SetOutPath .
        ;File FindInVar.exe
        ;nsExec::ExecToStack `FindInVar.exe`
        ;Pop $0 ; Pop exit code for program from stack (unused)
        ;Pop $1 ; Pop stdout for program from stack (should be 1st path)
        ;Delete FindInVar.class ; Done with FindInPath program, delete it
        ;SetOutPath $1 ; Use this path to install the libraries into
    ${Else}
        SetOutPath $INSTDIR
        StrCpy $R8 "true"
    ${EndIf}
    File Jayrock.Json.dll
    File IronPython.dll
    File IronPython.Modules.dll
    File Microsoft.Scripting.dll
    File Microsoft.Dynamic.dll
    File Ionic.Zip.dll
    StrCmpS $R8 "true" 0 skip
    ;Call removeDuplicates
    skip:
SectionEnd

LangString DESC_GaudiTool ${LANG_ENGLISH} "nGaudi executable (required)."
;LangString DESC_Plugins ${LANG_ENGLISH} "Plug-ins for Gaudi (recommended)."
LangString DESC_TPLibs ${LANG_ENGLISH} "Third party libraries nGaudi depends on."
!insertmacro MUI_FUNCTION_DESCRIPTION_BEGIN
  !insertmacro MUI_DESCRIPTION_TEXT ${GaudiTool} $(DESC_GaudiTool)
  ;!insertmacro MUI_DESCRIPTION_TEXT ${Plugins} $(DESC_Plugins)
  !insertmacro MUI_DESCRIPTION_TEXT ${TPLibs} $(DESC_TPLibs)
!insertmacro MUI_FUNCTION_DESCRIPTION_END

Section -post SEC0001
    WriteRegStr HKLM "${REGKEY}\Components" Main 1
    WriteRegStr HKLM "${REGKEY}" Path $INSTDIR
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\uninstall.exe
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\Uninstall $(^Name).lnk" $INSTDIR\uninstall.exe
    !insertmacro MUI_STARTMENU_WRITE_END
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayName "$(^Name)"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayVersion "${VERSION}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" Publisher "${COMPANY}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" URLInfoAbout "${URL}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayIcon $INSTDIR\uninstall.exe
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" UninstallString $INSTDIR\uninstall.exe
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoModify 1
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoRepair 1 
    ${EnvVarUpdate} $0 "PATH" "A" "HKLM" $INSTDIR ; Append to PATH
SectionEnd

# Macro for selecting uninstaller sections
!macro SELECT_UNSECTION SECTION_NAME UNSECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 next${UNSECTION_ID}
    !insertmacro SelectSection "${UNSECTION_ID}"
    GoTo done${UNSECTION_ID}
next${UNSECTION_ID}:
    !insertmacro UnselectSection "${UNSECTION_ID}"
done${UNSECTION_ID}:
    Pop $R0
!macroend

# Uninstaller sections
Section /o -un.Main UNSEC0000
    Delete $INSTDIR\nGaudi.exe
    Delete $INSTDIR\license.txt
    Delete $INSTDIR\*.dll
    Delete $INSTDIR\*.log ; TODO: Change to point to final log dir when implemented in nGaudi!!!
    Delete $INSTDIR\plugins\* 
    DeleteRegValue HKLM "${REGKEY}\Components" Main
SectionEnd

Section -un.post UNSEC0001
    DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)"
    Delete "$SMPROGRAMS\$StartMenuGroup\Uninstall $(^Name).lnk"
    Delete $INSTDIR\uninstall.exe
    DeleteRegValue HKLM "${REGKEY}" StartMenuGroup
    DeleteRegValue HKLM "${REGKEY}" Path
    DeleteRegKey /IfEmpty HKLM "${REGKEY}\Components"
    DeleteRegKey /IfEmpty HKLM "${REGKEY}"
    RmDir $SMPROGRAMS\$StartMenuGroup
    RmDir $INSTDIR
    ${un.EnvVarUpdate} $0 "PATH" "R" "HKLM" $INSTDIR ; Remove from PATH
SectionEnd

# Installer functions
Function .onInit
    InitPluginsDir
FunctionEnd

# Uninstaller functions
Function un.onInit
    ReadRegStr $INSTDIR HKLM "${REGKEY}" Path
    !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuGroup
    !insertmacro SELECT_UNSECTION Main ${UNSEC0000}
FunctionEnd
