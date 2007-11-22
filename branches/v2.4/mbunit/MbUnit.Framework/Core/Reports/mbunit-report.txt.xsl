<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text"/>
	<xsl:template match="/">
		<xsl:apply-templates/>
	</xsl:template>
	<xsl:template match="counter">
		<xsl:text>Tests run: </xsl:text>
		<xsl:value-of select="@run-count"/>
		<xsl:text>, Failures: </xsl:text>
		<xsl:value-of select="@failure-count"/>
		<xsl:text>, Skipped: </xsl:text>
		<xsl:value-of select="@skip-count"/>
		<xsl:text>, Ignored: </xsl:text>
		<xsl:value-of select="@ignore-count"/>
		<xsl:text>
</xsl:text>
	</xsl:template>
	<xsl:template match="assemblies">
		<xsl:if test="//run[@result='failure']">
			<xsl:text>Failures:
</xsl:text>
		<xsl:apply-templates select="//run[@result='failure']"></xsl:apply-templates>
		</xsl:if>
		<xsl:if test="//run[@result='skip']">
			<xsl:text>Skipped:
</xsl:text>
		<xsl:apply-templates select="//run[@result='skip']"></xsl:apply-templates>
		</xsl:if>
		<xsl:if test="//run[@result='ignore']">
			<xsl:text>Ignored:
</xsl:text>
		<xsl:apply-templates select="//run[@result='ignore']"></xsl:apply-templates>
		</xsl:if>
	</xsl:template>
	<xsl:template match="run">
		<xsl:value-of select="position()" />
		<xsl:text>) </xsl:text>
		<xsl:value-of select="../../../../@name" />
		<xsl:text>.</xsl:text>
		<xsl:value-of select="@name" />
		<xsl:text> :
</xsl:text>
		<xsl:value-of select="child::node()/message" />
		<xsl:text>
</xsl:text>
	<xsl:if test="@result='failure'">
		<xsl:value-of select="child::node()/stack-trace" />
		<xsl:text>
</xsl:text>
	</xsl:if>
	</xsl:template>
</xsl:stylesheet>
