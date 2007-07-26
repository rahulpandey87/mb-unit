<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.1">

	<xsl:param name="project" select="string('project')" />

	<xsl:output method="HelpCollection" encoding="utf-8" doctype-system="MS-Help://Hx/Resources/HelpCollection.dtd" indent="yes" />

	<xsl:template match="/">
		<HelpCollection DTDVersion="1.0" FileVersion="08.00.50720.2102" LangId="1033" Title="DocProject-Generated">
			<xsl:element name="CompilerOptions">
				<xsl:attribute name="OutputFile">
					<xsl:value-of select="concat($project, '.HxS')" />
				</xsl:attribute>
				<xsl:attribute name="CreateFullTextIndex">Yes</xsl:attribute>
				<xsl:attribute name="CompileResult">Hxs</xsl:attribute>
				<IncludeFile File="files.HxF" />
			</xsl:element>
			<xsl:element name="TOCDef">
				<xsl:attribute name="File">
					<xsl:value-of select="concat($project, '.HxT')" />
				</xsl:attribute>
			</xsl:element>
			<KeywordIndexDef File="index_A.HxK" />
			<KeywordIndexDef File="index_K.HxK" />
			<KeywordIndexDef File="index_F.HxK" />
			<KeywordIndexDef File="index_N.HxK" />
			<KeywordIndexDef File="index_S.HxK" />
			<KeywordIndexDef File="index_B.HxK" />
			<ItemMoniker Name="!DefaultTOC" ProgId="HxDs.HxHierarchy" InitData="AnyString" />
			<ItemMoniker Name="!DefaultFullTextSearch" ProgId="HxDs.HxFullTextSearch" InitData="AnyString" />
			<ItemMoniker Name="!DefaultAssociativeIndex" ProgId="HxDs.HxIndex" InitData="A" />
			<ItemMoniker Name="!DefaultKeywordIndex" ProgId="HxDs.HxIndex" InitData="K" />
			<ItemMoniker Name="!DefaultContextWindowIndex" ProgId="HxDs.HxIndex" InitData="F" />
			<ItemMoniker Name="!DefaultNamedUrlIndex" ProgId="HxDs.HxIndex" InitData="NamedUrl" />
			<ItemMoniker Name="!DefaultSearchWindowIndex" ProgId="HxDs.HxIndex" InitData="S" />
			<ItemMoniker Name="!DefaultDynamicLinkIndex" ProgId="HxDs.HxIndex" InitData="B" />
		</HelpCollection>
	</xsl:template>

</xsl:stylesheet>
