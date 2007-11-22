// QuickGraph Library 
// 
// Copyright (c) 2004 Jonathan de Halleux
//
// This software is provided 'as-is', without any express or implied warranty. 
// 
// In no event will the authors be held liable for any damages arising from 
// the use of this software.
// Permission is granted to anyone to use this software for any purpose, 
// including commercial applications, and to alter it and redistribute it 
// freely, subject to the following restrictions:
//
//		1. The origin of this software must not be misrepresented; 
//		you must not claim that you wrote the original software. 
//		If you use this software in a product, an acknowledgment in the product 
//		documentation would be appreciated but is not required.
//
//		2. Altered source versions must be plainly marked as such, and must 
//		not be misrepresented as being the original software.
//
//		3. This notice may not be removed or altered from any source 
//		distribution.
//		
//		QuickGraph Library HomePage: http://mbunit.tigris.org
//		Author: Jonathan de Halleux

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Permissions;
using System.Runtime.InteropServices;

//
// Les informations générales relatives à un assembly dépendent de 
// l'ensemble d'attributs suivant. Pour modifier les informations
// associées à un assembly, changez les valeurs de ces attributs.
//
[assembly: AssemblyTitle("")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]		
[assembly: CLSCompliant(true)]
[assembly: ComVisible(true)]

//
// Les informations de version pour un assembly se composent des quatre valeurs suivantes :
//
//      Version principale
//      Version secondaire 
//      Numéro de build
//      Révision
//
// Vous pouvez spécifier toutes les valeurs ou indiquer des numéros de révision et de build par défaut 
// en utilisant '*', comme ci-dessous :

[assembly: AssemblyVersion("2.21.1.0")]

//
// Pour signer votre assembly, vous devez spécifier la clé à utiliser. Consultez 
// la documentation Microsoft .NET Framework pour plus d'informations sur la signature d'un assembly.
//
// Utilisez les attributs ci-dessous pour contrôler la clé utilisée lors de la signature. 
//
// Remarques : 
//   (*) Si aucune clé n'est spécifiée, l'assembly n'est pas signé.
//   (*) KeyName fait référence à une clé installée dans le fournisseur de
//       services cryptographiques (CSP) de votre ordinateur. KeyFile fait référence à un fichier qui contient
//       une clé.
//   (*) Si les valeurs de KeyFile et de KeyName sont spécifiées, le 
//       traitement suivant se produit :
//       (1) Si KeyName se trouve dans le CSP, la clé est utilisée.
//       (2) Si KeyName n'existe pas mais que KeyFile existe, la clé 
//           de KeyFile est installée dans le CSP et utilisée.
//   (*) Pour créer KeyFile, vous pouvez utiliser l'utilitaire sn.exe (Strong Name, Nom fort).
//        Lors de la spécification de KeyFile, son emplacement doit être
//        relatif au répertoire de sortie du projet qui est
//       %Project Directory%\obj\<configuration>. Par exemple, si votre KeyFile se trouve
//       dans le répertoire du projet, vous devez spécifier l'attribut 
//       AssemblyKeyFile sous la forme [assembly: AssemblyKeyFile("..\\..\\mykey.snk")]
//   (*) DelaySign (signature différée) est une option avancée. Pour plus d'informations, consultez la
//       documentation Microsoft .NET Framework.
//
[assembly: AssemblyDelaySign(false)]
#if STRONGLY_NAMED
[assembly: AssemblyKeyFile(@"..\..\..\PublicPrivateQuickGraph.snk")]
#else
[assembly: AssemblyKeyFile("")]
#endif
[assembly: AssemblyKeyName("")]
