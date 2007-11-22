using System;

using Microsoft.Tools.WindowsInstallerXml.Serialize;

namespace QuickWix
{
    public class WixVisitor
    {
        public virtual void VisitWix(Wix wix)
        {
            if (wix == null)
                throw new ArgumentNullException("wix");
            VisitItem(wix.Item);
            VisitFragmentArray(wix.Fragment);
        }

        public virtual void VisitFragmentArray(Fragment[] nodes)
        {
            if (nodes == null)
                return;
            foreach (Fragment node in nodes)
                VisitFragment(node);
        }

        public virtual void VisitFragment(Fragment fragment)
        {
            VisitItemArray(fragment.Items);
        }

        public virtual void VisitItemArray(Object[] items)
        {
            if (items == null)
                return;
            foreach (object item in items)
                VisitItem(item);
        }

        public virtual void VisitItem(Object item)
        {
            if (item == null)
                return;

            Module module = item as Module;
            if (module != null)
            {
                VisitModule(module);
                return;
            }
            Product product = item as Product;
            if (product != null)
            {
                VisitProduct(product);
                return;
            }
            Feature feature = item as Feature;
            if (feature != null)
            {
                VisitFeature(feature);
                return;
            }
            AdvtExecuteSequence advtExecuteSequence = item as AdvtExecuteSequence;
            if (advtExecuteSequence != null)
            {
                VisitAdvtExecuteSequence(advtExecuteSequence);
                return;
            }
            InstallUISequence installUISequence = item as InstallUISequence;
            if (installUISequence != null)
            {
                VisitInstallUISequence(installUISequence);
                return;
            }
            User user = item as User;
            if (user != null)
            {
                VisitUser(user);
                return;
            }
            Upgrade upgrade = item as Upgrade;
            if (upgrade != null)
            {
                VisitUpgrade(upgrade);
                return;
            }
            Directory directory = item as Directory;
            if (directory != null)
            {
                VisitDirectory(directory);
                return;
            }
            PropertyRef propertyRef = item as PropertyRef;
            if (propertyRef != null)
            {
                VisitPropertyRef(propertyRef);
                return;
            }
            WebSite webSite = item as WebSite;
            if (webSite != null)
            {
                VisitWebSite(webSite);
                return;
            }
            AdminUISequence adminUISequence = item as AdminUISequence;
            if (adminUISequence != null)
            {
                VisitAdminUISequence(adminUISequence);
                return;
            }
            CustomAction customAction = item as CustomAction;
            if (customAction != null)
            {
                VisitCustomAction(customAction);
                return;
            }
            DirectoryRef directoryRef = item as DirectoryRef;
            if (directoryRef != null)
            {
                VisitDirectoryRef(directoryRef);
                return;
            }
            AppId appId = item as AppId;
            if (appId != null)
            {
                VisitAppId(appId);
                return;
            }
            Media media = item as Media;
            if (media != null)
            {
                VisitMedia(media);
                return;
            }
            CustomTable customTable = item as CustomTable;
            if (customTable != null)
            {
                VisitCustomTable(customTable);
                return;
            }
            Condition condition = item as Condition;
            if (condition != null)
            {
                VisitCondition(condition);
                return;
            }
            SFPCatalog sFPCatalog = item as SFPCatalog;
            if (sFPCatalog != null)
            {
                VisitSFPCatalog(sFPCatalog);
                return;
            }
            UI ui = item as UI;
            if (ui != null)
            {
                VisitUI(ui);
                return;
            }
            FragmentRef fragmentRef = item as FragmentRef;
            if (fragmentRef != null)
            {
                VisitFragmentRef(fragmentRef);
                return;
            }
            Icon icon = item as Icon;
            if (icon != null)
            {
                VisitIcon(icon);
                return;
            }
            Property property = item as Property;
            if (property != null)
            {
                VisitProperty(property);
                return;
            }
            FeatureRef featureRef = item as FeatureRef;
            if (featureRef != null)
            {
                VisitFeatureRef(featureRef);
                return;
            }
            WebDirProperties webDirProperties = item as WebDirProperties;
            if (webDirProperties != null)
            {
                VisitWebDirProperties(webDirProperties);
                return;
            }
            ComplianceCheck complianceCheck = item as ComplianceCheck;
            if (complianceCheck != null)
            {
                VisitComplianceCheck(complianceCheck);
                return;
            }
            InstallExecuteSequence installExecuteSequence = item as InstallExecuteSequence;
            if (installExecuteSequence != null)
            {
                VisitInstallExecuteSequence(installExecuteSequence);
                return;
            }
            AdminExecuteSequence adminExecuteSequence = item as AdminExecuteSequence;
            if (adminExecuteSequence != null)
            {
                VisitAdminExecuteSequence(adminExecuteSequence);
                return;
            }
            Binary binary = item as Binary;
            if (binary != null)
            {
                VisitBinary(binary);
                return;
            }
            Group group = item as Group;
            if (group != null)
            {
                VisitGroup(group);
                return;
            }
            WebApplication webApplication = item as WebApplication;
            if (webApplication != null)
            {
                VisitWebApplication(webApplication);
                return;
            }
            ActionSequenceType actionSequenceType = item as ActionSequenceType;
            if (actionSequenceType != null)
            {
                VisitActionSequenceType(actionSequenceType);
                return;
            }
            ActionModuleSequenceType actionModuleSequenceType = item as ActionModuleSequenceType;
            if (actionModuleSequenceType != null)
            {
                VisitActionModuleSequenceType(actionModuleSequenceType);
                return;
            }
            BillboardAction billboardAction = item as BillboardAction;
            if (billboardAction != null)
            {
                VisitBillboardAction(billboardAction);
                return;
            }
            Error error = item as Error;
            if (error != null)
            {
                VisitError(error);
                return;
            }
            Dialog dialog = item as Dialog;
            if (dialog != null)
            {
                VisitDialog(dialog);
                return;
            }
            ProgressText progressText = item as ProgressText;
            if (progressText != null)
            {
                VisitProgressText(progressText);
                return;
            }
            TextStyle textStyle = item as TextStyle;
            if (textStyle != null)
            {
                VisitTextStyle(textStyle);
                return;
            }
            ListBox listBox = item as ListBox;
            if (listBox != null)
            {
                VisitListBox(listBox);
                return;
            }
            ListView listView = item as ListView;
            if (listView != null)
            {
                VisitListView(listView);
                return;
            }
            ComboBox comboBox = item as ComboBox;
            if (comboBox != null)
            {
                VisitComboBox(comboBox);
                return;
            }
            UIText uIText = item as UIText;
            if (uIText != null)
            {
                VisitUIText(uIText);
                return;
            }
            RadioGroup radioGroup = item as RadioGroup;
            if (radioGroup != null)
            {
                VisitRadioGroup(radioGroup);
                return;
            }
            IniFileSearch iniFileSearch = item as IniFileSearch;
            if (iniFileSearch != null)
            {
                VisitIniFileSearch(iniFileSearch);
                return;
            }
            RegistrySearch registrySearch = item as RegistrySearch;
            if (registrySearch != null)
            {
                VisitRegistrySearch(registrySearch);
                return;
            }
            ComponentSearch componentSearch = item as ComponentSearch;
            if (componentSearch != null)
            {
                VisitComponentSearch(componentSearch);
                return;
            }
            FileSearch fileSearch = item as FileSearch;
            if (fileSearch != null)
            {
                VisitFileSearch(fileSearch);
                return;
            }
            DirectorySearch directorySearch = item as DirectorySearch;
            if (directorySearch != null)
            {
                VisitDirectorySearch(directorySearch);
                return;
            }
            File file = item as File;
            if (file != null)
            {
                VisitFile(file);
                return;
            }
            Component component = item as Component;
            if (component != null)
            {
                VisitComponent(component);
                return;
            }
            Merge merge = item as Merge;
            if (merge != null)
            {
                VisitMerge(merge);
                return;
            }
            Custom custom = item as Custom;
            if (custom != null)
            {
                VisitCustom(custom);
                return;
            }
            WebError webError = item as WebError;
            if (webError != null)
            {
                VisitWebError(webError);
                return;
            }
            WebVirtualDir webVirtualDir = item as WebVirtualDir;
            if (webVirtualDir != null)
            {
                VisitWebVirtualDir(webVirtualDir);
                return;
            }
            WebDir webDir = item as WebDir;
            if (webDir != null)
            {
                VisitWebDir(webDir);
                return;
            }
            WebFilter webFilter = item as WebFilter;
            if (webFilter != null)
            {
                VisitWebFilter(webFilter);
                return;
            }
            MergeRef mergeRef = item as MergeRef;
            if (mergeRef != null)
            {
                VisitMergeRef(mergeRef);
                return;
            }
            Subscribe subscribe = item as Subscribe;
            if (subscribe != null)
            {
                VisitSubscribe(subscribe);
                return;
            }
            Publish publish = item as Publish;
            if (publish != null)
            {
                VisitPublish(publish);
                return;
            }
            TypeLib typeLib = item as TypeLib;
            if (typeLib != null)
            {
                VisitTypeLib(typeLib);
                return;
            }
            Shortcut shortcut = item as Shortcut;
            if (shortcut != null)
            {
                VisitShortcut(shortcut);
                return;
            }
            ODBCTranslator oDBCTranslator = item as ODBCTranslator;
            if (oDBCTranslator != null)
            {
                VisitODBCTranslator(oDBCTranslator);
                return;
            }
            Permission permission = item as Permission;
            if (permission != null)
            {
                VisitPermission(permission);
                return;
            }
            Class _class = item as Class;
            if (_class != null)
            {
                VisitClass(_class);
                return;
            }
            CopyFile copyFile = item as CopyFile;
            if (copyFile != null)
            {
                VisitCopyFile(copyFile);
                return;
            }
            Patch patch = item as Patch;
            if (patch != null)
            {
                VisitPatch(patch);
                return;
            }
            ODBCDriver oDBCDriver = item as ODBCDriver;
            if (oDBCDriver != null)
            {
                VisitODBCDriver(oDBCDriver);
                return;
            }
            PerfCounter perfCounter = item as PerfCounter;
            if (perfCounter != null)
            {
                VisitPerfCounter(perfCounter);
                return;
            }
            FileShare fileShare = item as FileShare;
            if (fileShare != null)
            {
                VisitFileShare(fileShare);
                return;
            }
            Certificate certificate = item as Certificate;
            if (certificate != null)
            {
                VisitCertificate(certificate);
                return;
            }
            Category category = item as Category;
            if (category != null)
            {
                VisitCategory(category);
                return;
            }
            WebAppPool webAppPool = item as WebAppPool;
            if (webAppPool != null)
            {
                VisitWebAppPool(webAppPool);
                return;
            }
            SqlString sqlString = item as SqlString;
            if (sqlString != null)
            {
                VisitSqlString(sqlString);
                return;
            }
            ServiceControl serviceControl = item as ServiceControl;
            if (serviceControl != null)
            {
                VisitServiceControl(serviceControl);
                return;
            }
            IsolateComponent isolateComponent = item as IsolateComponent;
            if (isolateComponent != null)
            {
                VisitIsolateComponent(isolateComponent);
                return;
            }
            ServiceConfig serviceConfig = item as ServiceConfig;
            if (serviceConfig != null)
            {
                VisitServiceConfig(serviceConfig);
                return;
            }
            WebProperty webProperty = item as WebProperty;
            if (webProperty != null)
            {
                VisitWebProperty(webProperty);
                return;
            }
            SqlScript sqlScript = item as SqlScript;
            if (sqlScript != null)
            {
                VisitSqlScript(sqlScript);
                return;
            }
            SqlDatabase sqlDatabase = item as SqlDatabase;
            if (sqlDatabase != null)
            {
                VisitSqlDatabase(sqlDatabase);
                return;
            }
            WebLockdown webLockdown = item as WebLockdown;
            if (webLockdown != null)
            {
                VisitWebLockdown(webLockdown);
                return;
            }
            Extension extension = item as Extension;
            if (extension != null)
            {
                VisitExtension(extension);
                return;
            }
            ReserveCost reserveCost = item as ReserveCost;
            if (reserveCost != null)
            {
                VisitReserveCost(reserveCost);
                return;
            }
            RemoveFile removeFile = item as RemoveFile;
            if (removeFile != null)
            {
                VisitRemoveFile(removeFile);
                return;
            }
            ProgId progId = item as ProgId;
            if (progId != null)
            {
                VisitProgId(progId);
                return;
            }
            Microsoft.Tools.WindowsInstallerXml.Serialize.Environment environment = item as
                Microsoft.Tools.WindowsInstallerXml.Serialize.Environment;
            if (environment != null)
            {
                VisitEnvironment(environment);
                return;
            }
            ServiceInstall serviceInstall = item as ServiceInstall;
            if (serviceInstall != null)
            {
                VisitServiceInstall(serviceInstall);
                return;
            }
            IniFile iniFile = item as IniFile;
            if (iniFile != null)
            {
                VisitIniFile(iniFile);
                return;
            }
            Registry registry = item as Registry;
            if (registry != null)
            {
                VisitRegistry(registry);
                return;
            }
            CreateFolder createFolder = item as CreateFolder;
            if (createFolder != null)
            {
                VisitCreateFolder(createFolder);
                return;
            }
            MIME mIME = item as MIME;
            if (mIME != null)
            {
                VisitMIME(mIME);
                return;
            }
            Verb verb = item as Verb;
            if (verb != null)
            {
                VisitVerb(verb);
                return;
            }
        }
        public virtual void VisitVerb(Verb node)
        {
        }
        public virtual void VisitMIME(MIME node)
        {
        }
        public virtual void VisitCreateFolder(CreateFolder node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitRegistry(Registry node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitIniFile(IniFile node)
        {
        }
        public virtual void VisitServiceInstall(ServiceInstall node)
        {
        }
        public virtual void VisitEnvironment(Microsoft.Tools.WindowsInstallerXml.Serialize.Environment node)
        {
        }
        public virtual void VisitCategory(Category node)
        {
        }
        public virtual void VisitWebAppPool(WebAppPool node)
        {
        }
        public virtual void VisitSqlString(SqlString node)
        {
        }
        public virtual void VisitServiceControl(ServiceControl node)
        {
        }
        public virtual void VisitIsolateComponent(IsolateComponent node)
        {
        }
        public virtual void VisitServiceConfig(ServiceConfig node)
        {
        }
        public virtual void VisitWebProperty(WebProperty node)
        {
        }
        public virtual void VisitSqlScript(SqlScript node)
        {
        }
        public virtual void VisitSqlDatabase(SqlDatabase node)
        {
        }
        public virtual void VisitWebLockdown(WebLockdown node)
        {
        }
        public virtual void VisitExtension(Extension node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitReserveCost(ReserveCost node)
        {
        }
        public virtual void VisitRemoveFile(RemoveFile node)
        {
        }
        public virtual void VisitProgId(ProgId node)
        {
        }
        public virtual void VisitCertificate(Certificate node)
        {
        }
        public virtual void VisitFileShare(FileShare node)
        {
        }
        public virtual void VisitPerfCounter(PerfCounter node)
        {
        }
        public virtual void VisitODBCDataSourceArray(ODBCDataSource[] nodes)
        {
            if (nodes == null)
                return;
            foreach (ODBCDataSource node in nodes)
                VisitODBCDataSource(node);
        }
        public virtual void VisitODBCDataSource(ODBCDataSource node)
        {
        }
        public virtual void VisitODBCDriver(ODBCDriver node)
        {
        }
        public virtual void VisitPatch(Patch node)
        {
        }
        public virtual void VisitCopyFile(CopyFile node)
        {
        }
        public virtual void VisitClass(Class node)
        {
        }
        public virtual void VisitPermission(Permission node)
        {
        }
        public virtual void VisitODBCTranslator(ODBCTranslator node)
        {
        }
        public virtual void VisitShortcut(Shortcut node)
        {
        }
        public virtual void VisitTypeLib(TypeLib node)
        {
        }
        public virtual void VisitPublish(Publish node)
        {
        }
        public virtual void VisitSubscribe(Subscribe node)
        {
        }
        public virtual void VisitMergeRef(MergeRef node)
        {
        }
        public virtual void VisitWebError(WebError node)
        {
        }
        public virtual void VisitWebFilter(WebFilter node)
        {
        }
        public virtual void VisitWebDir(WebDir node)
        {
        }
        public virtual void VisitWebVirtualDir(WebVirtualDir node)
        {
        }
        public virtual void VisitCustom(Custom node)
        {
        }
        public virtual void VisitMerge(Merge node)
        {
        }
        public virtual void VisitComponent(Component node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitColumnArray(Column[] columns)
        {
            if (columns == null)
                return;
            foreach (Column column in columns)
                VisitColumn(column);
        }
        public virtual void VisitColumn(Column node)
        {
        }
        public virtual void VisitRowArray(Row[] rows)
        {
            if (rows == null)
                return;
            foreach (Row row in rows)
                VisitRow(row);
        }
        public virtual void VisitDataArray(Data[] nodes)
        {
            if (nodes == null)
                return;
            foreach (Data node in nodes)
                VisitData(node);
        }
        public virtual void VisitData(Data node)
        {
        }
        public virtual void VisitRow(Row node)
        {
            VisitDataArray(node.Data);
        }
        public virtual void VisitFile(File node)
        {
            VisitItem(node.Items);
        }
        public virtual void VisitIniFileSearch(IniFileSearch node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitRegistrySearch(RegistrySearch node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitComponentSearch(ComponentSearch node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitFileSearch(FileSearch node)
        {
        }
        public virtual void VisitDirectorySearch(DirectorySearch node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitRadioButtonArray(RadioButton[] nodes)
        {
            if (nodes == null)
                return;
            foreach (RadioButton node in nodes)
                VisitRadioButton(node);
        }
        public virtual void VisitRadioButton(RadioButton node)
        {
        }
        public virtual void VisitRadioGroup(RadioGroup node)
        {
            VisitRadioButtonArray(node.RadioButton);
        }
        public virtual void VisitUIText(UIText node)
        {
        }
        public virtual void VisitComboBox(ComboBox node)
        {
            VisitListItemArray(node.ListItem);
        }
        public virtual void VisitListView(ListView node)
        {
            VisitListItemArray(node.ListItem);
        }
        public virtual void VisitListItemArray(ListItem[] nodes)
        {
            if (nodes == null)
                return;
            foreach (ListItem node in nodes)
                VisitListItem(node);
        }
        public virtual void VisitListItem(ListItem node)
        {
        }
        public virtual void VisitListBox(ListBox node)
        {
            VisitListItemArray(node.ListItem);
        }
        public virtual void VisitTextStyle(TextStyle node)
        {
        }
        public virtual void VisitProgressText(ProgressText node)
        {
        }
        public virtual void VisitControlArray(Control[] nodes)
        {
            if (nodes == null)
                return;
            foreach (Control node in nodes)
                VisitControl(node);
        }
        public virtual void VisitControl(Control node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitDialog(Dialog node)
        {
            VisitControlArray(node.Control);
        }
        public virtual void VisitError(Error node)
        {
        }
        public virtual void VisitBillboardArray(Billboard[] nodes)
        {
            if (nodes == null)
                return;
            foreach (Billboard node in nodes)
                VisitBillboard(node);
        }
        public virtual void VisitBillboard(Billboard node)
        {
        }
        public virtual void VisitBillboardAction(BillboardAction node)
        {
            VisitBillboardArray(node.Billboard);
        }
        public virtual void VisitFeatureArray(Feature[] features)
        {
            if (features == null)
                return;
            foreach (Feature feature in features)
                VisitFeature(feature);
        }
        public virtual void VisitFeature(Feature node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitAdvtExecuteSequence(AdvtExecuteSequence node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitInstallUISequence(InstallUISequence node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitUser(User node)
        {
        }
        public virtual void VisitUpgrade(Upgrade node)
        {
            VisitUpgradeVersionArray(node.UpgradeVersion);
        }
        public virtual void VisitUpgradeVersionArray(UpgradeVersion[] upgradeVersions)
        {
            if (upgradeVersions==null)
                return;
            foreach (UpgradeVersion uv in upgradeVersions)
                VisitUpgradeVersion(uv);
        }
        public virtual void VisitUpgradeVersion(UpgradeVersion node)
        {
        }
        public virtual void VisitDirectory(Directory node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitPropertyRef(PropertyRef node)
        {
        }
        public virtual void VisitWebSite(WebSite node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitAdminUISequence(AdminUISequence node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitCustomAction(CustomAction node)
        {
        }
        public virtual void VisitDirectoryRef(DirectoryRef node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitAppId(AppId node)
        {
        }
        public virtual void VisitMedia(Media node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitCustomTable(CustomTable node)
        {
            VisitColumnArray(node.Column);
            VisitRowArray(node.Row);
        }
        public virtual void VisitSFPCatalog(SFPCatalog node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitUI(UI node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitFragmentRef(FragmentRef node)
        {
        }
        public virtual void VisitIcon(Icon node)
        {
        }
        public virtual void VisitProperty(Property node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitFeatureRef(FeatureRef node)
        {
            VisitComponentRefArray(node.ComponentRef);
            VisitFeatureArray(node.Feature);
        }
        public virtual void VisitComponentRefArray(ComponentRef[] node)
        {
            foreach (ComponentRef cref in node)
                VisitComponentRef(cref);
        }
        public virtual void VisitComponentRef(ComponentRef node)
        {
        }
        public virtual void VisitWebDirProperties(WebDirProperties node)
        {
        }
        public virtual void VisitCondition(Condition node)
        {
            VisitConditionAction(node.Action);
        }
        public virtual void VisitConditionAction(ConditionAction node)
        {
        }
        public virtual void VisitComplianceCheck(ComplianceCheck node)
        {
            VisitItem(node.Item);
        }
        public virtual void VisitInstallExecuteSequence(InstallExecuteSequence node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitAdminExecuteSequence(AdminExecuteSequence node)
        {
            VisitItemArray(node.Items);
        }
        public virtual void VisitBinary(Binary node)
        {
        }
        public virtual void VisitGroup(Group node)
        {
        }

        public virtual void VisitWebApplication(WebApplication node)
        {
        }
        public virtual void VisitProduct(Product product)
        {
            VisitPackage(product.Package);
            VisitItemArray(product.Items);
        }
        public virtual void VisitModule(Module module)
        {
            VisitPackage(module.Package);
            VisitItemArray(module.Items);
        }
        public virtual void VisitPackage(Package package)
        {
        }
        public virtual void VisitActionModuleSequenceType(ActionModuleSequenceType node)
        {
        }
        public virtual void VisitActionSequenceType(ActionSequenceType node)
        {
        }
    }
}
