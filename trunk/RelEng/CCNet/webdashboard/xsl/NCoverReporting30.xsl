<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt">
	<xsl:output method="html" doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd" doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"/>

	<!--<xsl:key name="classes" match="//class" use="."/>-->
	<!--This gets passed in from the transform so we are able to determine the type-->
	<xsl:variable name="root" select="//trendcoveragedata" />
	<xsl:variable name="reportType" select="//./params[@name = 'reporttype']/@value" />
	<xsl:variable name="date" select="//./params[@name = 'date']/@value" />
	<xsl:variable name="time" select="//./params[@name = 'time']/@value" />
	<xsl:variable name="fulltags" select="string(//./params[@name = 'ishtml']/@value) = '1'" />
	<!--
	The width of the widest graph in pixels
	The Class in .8 of that and methods are .7
	-->
	<xsl:variable name="graphscale">75</xsl:variable>
	<xsl:variable name="projectstats" select="$root/stats[last()]"></xsl:variable>
	<xsl:variable name="execution" select="$root/execution[last()]"></xsl:variable>
	<xsl:variable name="acceptable" select="$execution/satisfactorycoveragethreshold"></xsl:variable>
	<xsl:variable name="acceptableFunction" select="$execution/satisfactoryfunctionthreshold"></xsl:variable>
	<xsl:variable name="reportName" xml:space="preserve"><xsl:choose>
																												<xsl:when test="string(//./params[@name = 'reportname']/@value) != ''"><xsl:value-of select="//./params[@name = 'reportname']/@value" /></xsl:when>
																												<xsl:otherwise><xsl:value-of select="$execution/projectname" /></xsl:otherwise></xsl:choose></xsl:variable>
	<xsl:variable name="isFunctionReport" select="$reportType = 'MethodModuleNamespaceClass'
																								or $reportType = 'MethodModuleNamespaceClassMethod'
																								or $reportType = 'MethodSourceCode'
																								or $reportType = 'MethodSourceCodeClass'
																								or $reportType = 'MethodSourceCodeClassMethod'
																								or $reportType = 'MethodCCModuleClassFailedCoverageTop'
																								or $reportType = 'MethodCCModuleClassCoverageTop'"/>
	<xsl:variable name="isDocumentReport" select="$reportType = 'SymbolSourceCode'
																								or $reportType = 'SymbolSourceCodeClass'
																								or $reportType = 'SymbolSourceCodeClassMethod'
																								or $reportType = 'MethodSourceCode'
																								or $reportType = 'MethodSourceCodeClass'
																								or $reportType = 'MethodSourceCodeClassMethod'"/>
	<xsl:variable name="reportBranchPoints" select="$projectstats/@vbp" />
	<xsl:variable name="reportVisitCounts" select="$isFunctionReport and $projectstats/@svc" />
	<!--We have specific function CC reports that need the extra column-->
	<xsl:variable name="pointCCReport" select="$reportType = 'SymbolCCModuleClassFailedCoverageTop'
																					or $reportType = 'SymbolCCModuleClassCoverageTop'" />
	<xsl:variable name="functionalCCReport" select="$reportType = 'SymbolCCModuleClassFailedCoverageTop'
																								or $reportType = 'SymbolCCModuleClassCoverageTop'" />
	<xsl:variable name="ccnodeExists" select="$projectstats/@ccavg"  />
	<xsl:variable name="ccdataExists" select="$projectstats/@ccavg > 0"  />
	<xsl:variable name="reportCC" xml:space="preserve" select="($functionalCCReport and $ccdataExists)
																												or	($projectstats/@ccavg and $ccdataExists and not ($isFunctionReport))" />
	<xsl:variable name="vcColumnCount" xml:space="preserve"><xsl:choose>
																														<xsl:when test="$reportVisitCounts">2</xsl:when>
																														<xsl:otherwise>0</xsl:otherwise></xsl:choose></xsl:variable>
	<xsl:variable name="ccClassCount" xml:space="preserve"><xsl:choose>
																														<xsl:when test="$reportType = 'SymbolCCModuleClassFailedCoverageTop'">20</xsl:when>
																														<xsl:when test="$reportType = 'SymbolCCModuleClassCoverageTop'">20</xsl:when>
																														<xsl:otherwise>20</xsl:otherwise></xsl:choose></xsl:variable>
	<xsl:variable name="ccColumnCount" xml:space="preserve"><xsl:choose>
																														<xsl:when test="$reportCC">1</xsl:when>
																														<xsl:otherwise>0</xsl:otherwise></xsl:choose></xsl:variable>
	<xsl:variable name="tablecolumns" xml:space="preserve"><xsl:choose>
																	<xsl:when test="not ($isFunctionReport) and $reportBranchPoints"><xsl:value-of select="9 + $ccColumnCount" /></xsl:when>
																	<xsl:otherwise><xsl:value-of select="5 + $ccColumnCount + $vcColumnCount" /></xsl:otherwise></xsl:choose></xsl:variable>
	<xsl:variable name="reportTitle">
		<xsl:choose>
			<xsl:when test="$reportType = 'MethodModuleNamespaceClass'">Method Module, Namespace and Classes</xsl:when>
			<xsl:when test="$reportType = 'MethodModuleNamespaceClassMethod'">Method Module, Namespace, Class and Methods</xsl:when>
			<xsl:when test="$reportType = 'MethodSourceCode'">Method Source File</xsl:when>
			<xsl:when test="$reportType = 'MethodSourceCodeClass'">Method Source File and Class</xsl:when>
			<xsl:when test="$reportType = 'MethodSourceCodeClassMethod'">Method Source Code, Class and Methods</xsl:when>
			<xsl:when test="$reportType = 'SymbolModule'">Symbol Module</xsl:when>
			<xsl:when test="$reportType = 'SymbolModuleNamespace'">Symbol Module and Namespace</xsl:when>
			<xsl:when test="$reportType = 'SymbolModuleNamespaceClass'">Symbol Module, Namespace and Class</xsl:when>
			<xsl:when test="$reportType = 'SymbolModuleNamespaceClassMethod'">Symbol Module, Namespace, Class and Methods</xsl:when>
			<xsl:when test="$reportType = 'SymbolSourceCode'">Source Code</xsl:when>
			<xsl:when test="$reportType = 'SymbolSourceCodeClass'">Source Code and Class</xsl:when>
			<xsl:when test="$reportType = 'SymbolSourceCodeClassMethod'">Source Code, Class and Methods</xsl:when>
			<!--We may need to change this-->
			<xsl:when test="$reportType = 'SymbolCCModuleClassFailedCoverageTop'">
				Cyclomatic Complexity Coverage Top <xsl:value-of select="$ccClassCount" /> Failing Coverage
			</xsl:when>
			<xsl:when test="$reportType = 'SymbolCCModuleClassCoverageTop'">
				Cyclomatic Complexity Coverage Top <xsl:value-of select="$ccClassCount" />
			</xsl:when>
			<xsl:when test="$reportType = 'MethodCCModuleClassFailedCoverageTop'">
				Cyclomatic Complexity Function Coverage Top <xsl:value-of select="$ccClassCount" /> Failing Coverage
			</xsl:when>
			<xsl:when test="$reportType = 'MethodCCModuleClassCoverageTop'">
				Cyclomatic Complexity Function Coverage Top <xsl:value-of select="$ccClassCount" />
			</xsl:when>
			<xsl:otherwise></xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="vFilterStyle" select="//./params[@name = 'filterstyle']/@value" />
	<xsl:variable name="vSortStyle">
		<xsl:choose>
			<xsl:when test="$pointCCReport or $functionalCCReport">CC</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="//./params[@name = 'sortstyle']/@value" />
			</xsl:otherwise>
		</xsl:choose>

	</xsl:variable>

	<xsl:variable name="filterStyle">
		<xsl:choose>
			<xsl:when test="$vFilterStyle = 'HideFullyCovered'">Hide Fully Covered</xsl:when>
			<xsl:when test="$vFilterStyle = 'HideThresholdCovered'">Hide Threshold Covered</xsl:when>
			<xsl:when test="$vFilterStyle = 'HideUnvisited'">Hide Unvisited</xsl:when>
			<xsl:when test="$vFilterStyle = 'None'">None</xsl:when>
			<xsl:otherwise>Unknown</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:variable name="sortStyle">
		<xsl:choose>
			<xsl:when test="$vSortStyle = 'ClassLine'">Class Line</xsl:when>
			<xsl:when test="$vSortStyle = 'CoveragePercentageAscending'">Coverage Percentage Ascending</xsl:when>
			<xsl:when test="$vSortStyle = 'CoveragePercentageDescending'">Coverage Percentage Descending</xsl:when>
			<xsl:when test="$vSortStyle = 'FunctionCoverageAscending'">Function Coverage Ascending</xsl:when>
			<xsl:when test="$vSortStyle = 'FunctionCoverageDescending'">Function Coverage Descending</xsl:when>
			<xsl:when test="$vSortStyle = 'Name'">Name</xsl:when>
			<xsl:when test="$vSortStyle = 'UnvisitedSequencePointsAscending'">Unvisited Sequence Point Ascending</xsl:when>
			<xsl:when test="$vSortStyle = 'UnvisitedSequencePointsDescending'">Unvisited Sequence Point Descending</xsl:when>
			<xsl:when test="$vSortStyle = 'VisitCountAscending'">Visit Count Ascending</xsl:when>
			<xsl:when test="$vSortStyle = 'VisitCountDescending'">Visit Count Descending</xsl:when>
			<xsl:when test="$vSortStyle = 'CC'">Cyclomatic Complexity Descending</xsl:when>
			<xsl:otherwise>Unknown</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	<xsl:template match="/">
		<xsl:choose>
			<xsl:when test="$fulltags">
				<html>
					<head>
						<title>
							<xsl:value-of select="$reportTitle"/> - <xsl:value-of select="$filterStyle"/>  - <xsl:value-of select="$sortStyle"/>
						</title>
						<xsl:comment>NCover 3.0 Template</xsl:comment>
						<xsl:comment>Generated by the Joe Feser joe@ncover.com on the NCover team and inspired by the original template by Grant Drake</xsl:comment>
						<xsl:call-template name="style" />
					</head>
					<body>
						<table class="coverageReportTable" cellpadding="2" cellspacing="0">
							<tbody>
								<xsl:apply-templates select="$root" />
							</tbody>
						</table>
					</body>
				</html>
			</xsl:when>
			<xsl:otherwise>
				<xsl:call-template name="style" />
				<xsl:apply-templates select="$root" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!-- Main Project Section -->
	<xsl:template match="trendcoveragedata">
		<xsl:variable name="threshold">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">
					<xsl:value-of select="$acceptableFunction" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$acceptable" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="unvisitedTitle">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">U.V.</xsl:when>
				<xsl:otherwise>U.V.</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="excludedTitle">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">Functions</xsl:when>
				<xsl:otherwise>SeqPts</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="coverageTitle">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">Function Coverage</xsl:when>
				<xsl:otherwise>Coverage</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:call-template name="header" />

		<xsl:call-template name="projectSummary">
			<xsl:with-param name="threshold" select="$threshold" />
			<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
			<xsl:with-param name="coverageTitle" select="$coverageTitle" />
		</xsl:call-template>

		<!--SymbolCCModuleClassFailedCoverageTop we show everything sorted by cc desc and coverage asc-->
		<xsl:if test="$reportType = 'SymbolCCModuleClassFailedCoverageTop' 
						or $reportType = 'SymbolCCModuleClassCoverageTop'
						or $reportType = 'MethodCCModuleClassFailedCoverageTop'
						or $reportType = 'MethodCCModuleClassCoverageTop'">
			<xsl:call-template name="cyclomaticComplexitySummaryCommon">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<!--Output the top level module information moduleSummary-->
		<xsl:if test="$reportType = 'SymbolModule'">
			<xsl:call-template name="moduleSummary">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<!--Output the top level module information moduleSummary-->
		<xsl:if test="$reportType = 'SymbolSourceCode' or $reportType = 'MethodSourceCode'">
			<xsl:call-template name="documentSummary">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<!--Output the top level module information moduleSummary-->
		<xsl:if test="$reportType = 'SymbolSourceCodeClass' 
								or $reportType = 'MethodSourceCodeClass'
								or $reportType = 'SymbolSourceCodeClassMethod' 
								or $reportType = 'MethodSourceCodeClassMethod'">
			<xsl:call-template name="documentSummaryCommon">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
				<xsl:with-param name="printMethods">
					<xsl:choose xml:space="preserve">
						<xsl:when test="$reportType = 'SymbolSourceCodeClassMethod' or $reportType = 'MethodSourceCodeClassMethod'">True</xsl:when>
						<xsl:otherwise>False</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<!--if the report is namespace based, then output the namespace information.-->
		<xsl:if test="$reportType = 'SymbolModuleNamespace'">
			<xsl:call-template name="summaryCommon">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">False</xsl:with-param>
				<xsl:with-param name="printMethods">False</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<xsl:if test="($reportType = 'SymbolModuleNamespaceClass') or ($reportType = 'MethodModuleNamespaceClass')">
			<xsl:call-template name="summaryCommon">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
				<xsl:with-param name="printMethods">False</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<xsl:if test="($reportType = 'SymbolModuleNamespaceClassMethod') or ($reportType = 'MethodModuleNamespaceClassMethod')">
			<xsl:call-template name="summaryCommon">
				<xsl:with-param name="threshold" select="$threshold" />
				<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="printClasses">True</xsl:with-param>
				<xsl:with-param name="printMethods">True</xsl:with-param>
			</xsl:call-template>
		</xsl:if>

		<xsl:if test="count($root/exclusions/exclusion) != 0">
			<xsl:call-template name="exclusionsSummary" >
				<xsl:with-param name="excludedTitle" select="$excludedTitle" />
			</xsl:call-template>
		</xsl:if>

		<xsl:call-template name="footer2" />
	</xsl:template>

	<!-- Report Header -->
	<xsl:template name="header">
		<tr>
			<td class="projectTable reportHeader" colspan="{$tablecolumns}">
				<table width="100%">
					<tbody>
						<tr>
							<td valign="top">
								<h1 class="titleText">
									NCover Report - <xsl:value-of select="$reportName" />&#160;-&#160;<xsl:value-of select="$reportTitle" />
								</h1>
								<table cellpadding="1" class="subtitleText">
									<tbody>
										<tr>
											<td class="heading">Report generated on:</td>
											<td>
												<xsl:value-of select="$date" />&#160;at&#160;<xsl:value-of select="$time" />
											</td>
										</tr>
										<tr>
											<td class="heading">NCover Profiler version:</td>
											<td>
												<xsl:value-of select="$execution/profilerversion" />
											</td>
										</tr>
										<tr>
											<td class="heading">Filtering / Sorting:</td>
											<td>
												<xsl:value-of select="$filterStyle" />&#160;/&#160;<xsl:value-of select="$sortStyle" />
											</td>
										</tr>
										<tr>
											<td class="heading">Report version:</td>
											<td>
												2009.02.11.14.28
											</td>
										</tr>										
									</tbody>
								</table>
							</td>
							<td class="projectStatistics" align="right" valign="top">
								<table cellpadding="1">
									<tbody>
										<tr>
											<td rowspan="4" valign="top" nowrap="true" class="heading">Project Statistics:</td>
											<td align="right" class="heading">Files:</td>
											<td align="right">
												<xsl:value-of select="count($root/doc)" />
											</td>
											<td rowspan="4">&#160;</td>
											<td align="right" class="heading">NCLOC:</td>
											<td align="right">
												<xsl:value-of select="$projectstats/@ul + $projectstats/@vl" />
											</td>
										</tr>
										<tr>
											<td align="right" class="heading">Classes:</td>
											<td align="right">
												<xsl:value-of select="count($root/mod/ns/class)" />
											</td>
											<td align="right" class="heading">&#160;</td>
											<td align="right">&#160;</td>
										</tr>
										<tr>
											<td align="right" class="heading">Functions:</td>
											<td align="right">
												<xsl:value-of select="$projectstats/@vm + $projectstats/@um" />
											</td>
											<td align="right" class="heading">Unvisited:</td>
											<td align="right">
												<xsl:value-of select="$projectstats/@um" />
											</td>
										</tr>
										<tr>
											<td align="right" class="heading">Seq Pts:</td>
											<td align="right">
												<xsl:value-of select="$projectstats/@usp + $projectstats/@vsp" />
											</td>
											<td align="right" class="heading">Unvisited:</td>
											<td align="right">
												<xsl:value-of select="$projectstats/@usp" />
											</td>
										</tr>

										<xsl:if test="$ccnodeExists and not ($ccdataExists)">
											<tr>
												<td colspan="6" align="right" class="heading">Cyclomatic complexity data does not exist</td>
											</tr>
										</xsl:if>

                                        <xsl:if test="count($root/doc[@id != 0]) = 0 or $projectstats/@usp + $projectstats/@vsp = 0">
                                            <tr>
                                                <td colspan="6" align="right" class="heading">Symbols were not available for this execution.<br />The sequence points coverage data will not be available.</td>
                                            </tr>
                                        </xsl:if>
									</tbody>
								</table>
							</td>
						</tr>

						<xsl:if test="name(.) != 'trendcoveragedata' or not ($root)">
							<tr>
								<td>
									<b>
										The input file used to generate the report must start with 'trendcoveragedata' and the file provided started with '<xsl:value-of select="name(.)"/>'
									</b>
								</td>
							</tr>
						</xsl:if>
						<xsl:if test="name(.) = 'coverage'">
							<tr>
								<td>
									<b>
										The input file used to generate the report is a coverage file and not a report file. Please use ncover.reporting to generate an xml file format that may be used with this xsl file.
									</b>
								</td>
							</tr>
						</xsl:if>

					</tbody>
				</table>
			</td>
		</tr>
	</xsl:template>

	<!-- Project Summary -->
	<xsl:template name="projectSummary">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle" />
		<!--The Total points or methods-->
		<xsl:variable name="total">
			<xsl:call-template name="gettotal">
				<xsl:with-param name="stats" select="$projectstats" />
			</xsl:call-template>
		</xsl:variable>

		<xsl:call-template name="sectionTableHeader">
			<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
			<xsl:with-param name="coverageTitle" select="$coverageTitle" />
			<xsl:with-param name="sectionTitle">Project</xsl:with-param>
			<xsl:with-param name="cellClass">projectTable</xsl:with-param>
			<xsl:with-param name="showThreshold">True</xsl:with-param>
		</xsl:call-template>

		<xsl:call-template name="coverageDetail">
			<xsl:with-param name="name">
				<xsl:choose>
					<xsl:when test="string-length($reportName) > 0">
						<xsl:value-of select="$reportName" />
					</xsl:when>
					<xsl:otherwise>&#160;</xsl:otherwise>
				</xsl:choose>
			</xsl:with-param>
			<xsl:with-param name="stats" select="$projectstats" />
			<xsl:with-param name="showThreshold">True</xsl:with-param>
			<xsl:with-param name="showBottomLine">True</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<!--Build the header for each of the sections.
	It removes the complexity of building the multi table headers for the project and then the modules-->
	<xsl:template name="sectionTableHeader">
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle" />
		<xsl:param name="sectionTitle" />
		<xsl:param name="cellClass" />
		<xsl:param name="showThreshold" />
		<!--white space-->
		<tr>
			<td colspan="{$tablecolumns}">&#160;</td>
		</tr>
		<!--Top row showing SP and BP Data-->
		<xsl:if test="not ($isFunctionReport)">
			<tr>
				<td class="{$cellClass} mtHdLeftTop">&#160;</td>
				<td class="{$cellClass} mtHdRightTop">
					<xsl:attribute name="colspan" xml:space="preserve"><xsl:choose><xsl:when test="$showThreshold = 'True'">4</xsl:when><xsl:otherwise>3</xsl:otherwise></xsl:choose></xsl:attribute>Sequence Points
				</td>
				<xsl:if test="$reportBranchPoints">
					<td class="{$cellClass} mtHdRightTop">
						<xsl:attribute name="colspan" xml:space="preserve"><xsl:choose><xsl:when test="$showThreshold = 'True'">4</xsl:when><xsl:otherwise>3</xsl:otherwise></xsl:choose></xsl:attribute>Branch Points
					</td>
				</xsl:if>
				<xsl:if test="$reportCC">
					<td class="{$cellClass} mtHdRightTop">C. C.</td>
				</xsl:if>
			</tr>
		</xsl:if>
		<tr>
			<td>
				<xsl:attribute xml:space="preserve" name="class"><xsl:value-of select="$cellClass" /><xsl:choose><xsl:when test="$isFunctionReport"> mtHdLeftFunc</xsl:when><xsl:otherwise> mtHdLeft</xsl:otherwise></xsl:choose></xsl:attribute>
				<xsl:value-of select="$sectionTitle" disable-output-escaping="yes" />
			</td>
			<td class="{$cellClass} mtHd">Acceptable</td>
			<td class="{$cellClass} mtHd">
				<xsl:value-of select="$unvisitedTitle" />
			</td>
			<!--This cell contains both the text and the chart.-->
			<td class="{$cellClass} mtgHd" style="text-align: center;" colspan="2">
				<xsl:value-of select="$coverageTitle" />
			</td>

			<xsl:if test="not ($isFunctionReport) and $reportBranchPoints">
				<td class="{$cellClass} mtHd">Acceptable</td>
				<td class="{$cellClass} mtHd">
					<xsl:value-of select="$unvisitedTitle" />
				</td>
				<!--This cell contains both the text and the chart.-->
				<td class="{$cellClass} mtgHd" style="text-align: center;" colspan="2">
					<xsl:value-of select="$coverageTitle" />
				</td>
			</xsl:if>
			<xsl:if test="$reportCC">
				<td class="{$cellClass} mtHd">
					<xsl:if test="$functionalCCReport">
						C.C.<br />
					</xsl:if> Avg / Max
				</td>
			</xsl:if>
			<xsl:if test="$reportVisitCounts">
				<td class="{$cellClass} mtHd">Visit Count</td>
				<td class="{$cellClass} mtHd">V.C. %</td>
			</xsl:if>
		</tr>
	</xsl:template>

	<!-- Modules Summary -->
	<xsl:template name="moduleSummary">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle" />

		<xsl:call-template name="sectionTableHeader">
			<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
			<xsl:with-param name="coverageTitle" select="$coverageTitle" />
			<xsl:with-param name="sectionTitle">Modules</xsl:with-param>
			<xsl:with-param name="cellClass">primaryTable</xsl:with-param>
			<xsl:with-param name="showThreshold">True</xsl:with-param>
		</xsl:call-template>

		<xsl:for-each select="$root/mod[string(stats/@ex) != '1']">
			<xsl:variable name="stats" select="stats[last()]" />
			<!--The Total points or methods-->
			<xsl:variable name="total">
				<xsl:call-template name="gettotal">
					<xsl:with-param name="stats" select="$stats" />
				</xsl:call-template>
			</xsl:variable>
			<xsl:call-template name="coverageDetail">
				<xsl:with-param name="name" select="assembly" />
				<xsl:with-param name="stats" select="$stats" />
				<xsl:with-param name="showThreshold">True</xsl:with-param>
				<xsl:with-param name="showBottomLine">True</xsl:with-param>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<!--Document Summary-->

	<xsl:template name="documentSummary">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle" />

		<xsl:call-template name="sectionTableHeader">
			<xsl:with-param name="unvisitedTitle" select="$unvisitedTitle" />
			<xsl:with-param name="coverageTitle" select="$coverageTitle" />
			<xsl:with-param name="sectionTitle">Documents</xsl:with-param>
			<xsl:with-param name="cellClass">primaryTable</xsl:with-param>
			<xsl:with-param name="showThreshold">True</xsl:with-param>
		</xsl:call-template>

		<xsl:for-each select="$root/doc[string(stats/@ex) != '1' and string(@id) != '0']">
			<!--<xsl:sort select="en"/>-->
			<xsl:variable name="stats" select="stats[last()]" />
			<xsl:call-template name="coverageDetail">
				<xsl:with-param name="name" select="en" />
				<xsl:with-param name="stats" select="$stats" />
				<xsl:with-param name="showThreshold">True</xsl:with-param>
				<xsl:with-param name="showBottomLine">True</xsl:with-param>
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<!--Helper templates-->

	<!-- Coverage detail row in main grid displaying a name, statistics and graph bar -->
	<xsl:template name="coverageDetail">
		<xsl:param name="name" />
		<xsl:param name="stats" />
		<xsl:param name="showThreshold" />
		<xsl:param name="showBottomLine" />
		<!--By default this needs to be unvisited count-->
		<xsl:param name="countText">
			<xsl:call-template name="unvisiteditems">
				<xsl:with-param name="stats" select="$stats" />
			</xsl:call-template>
		</xsl:param>
		<xsl:param name="styleTweak" />
		<xsl:param name="scale" select="$graphscale" />
		<xsl:variable name="total">
			<xsl:call-template name="gettotal">
				<xsl:with-param name="stats" select="$stats" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="totalbranch">
			<xsl:call-template name="gettotal">
				<xsl:with-param name="stats" select="$stats" />
				<xsl:with-param name="branch" select="'True'" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="unvisitedcount">
			<xsl:call-template name="unvisiteditems">
				<xsl:with-param name="stats" select="$stats" />
			</xsl:call-template>
		</xsl:variable>
		<xsl:variable name="unvisitedcountbranch">
			<xsl:value-of select="$stats/@ubp" />
		</xsl:variable>
		<xsl:variable name="coverage">
			<xsl:choose>
				<xsl:when test="$total = 0">N/A</xsl:when>
				<xsl:when test="$isFunctionReport">
					<xsl:value-of select="($stats/@vm div $total) * 100" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="($stats/@vsp div $total) * 100" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="coveragebranch">
			<xsl:choose>
				<xsl:when test="$totalbranch = 0">N/A</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="($stats/@vbp div $totalbranch) * 100" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:variable name="threshold">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">
					<xsl:value-of select="$stats/@afp" />
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$stats/@acp" />
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<tr>
			<xsl:choose>
				<xsl:when test="$showThreshold='True'">
					<td>
						<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcItem</xsl:attribute>
						<xsl:if test="$styleTweak != ''" xml:space="preserve"><xsl:attribute name="style"><xsl:value-of select="$styleTweak"/></xsl:attribute></xsl:if>
						<div class="mtDiv">
							<xsl:value-of select="$name" />&#160;
						</div>
					</td>
					<td>
						<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
						<xsl:value-of select="concat(format-number($threshold,'#0.0'), ' %')" />
					</td>
				</xsl:when>
				<xsl:otherwise>
					<td colspan="2">
						<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcItem</xsl:attribute>
						<xsl:if test="$styleTweak != ''" xml:space="preserve"><xsl:attribute name="style"><xsl:value-of select="$styleTweak"/></xsl:attribute></xsl:if>
						<div class="mtDiv">
							<xsl:value-of select="$name" />
						</div>
					</td>
				</xsl:otherwise>
			</xsl:choose>
			<td>
				<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
				<xsl:value-of select="$countText" />
			</td>
			<td>
				<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcPercent</xsl:attribute>
				<xsl:call-template name="printcoverage">
					<xsl:with-param name="coverage" select="$coverage"></xsl:with-param>
				</xsl:call-template>
			</td>
			<td>
				<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>tmcGraph</xsl:attribute>
				<xsl:call-template name="detailPercent">
					<xsl:with-param name="notVisited" select="$unvisitedcount" />
					<xsl:with-param name="total" select="$total" />
					<xsl:with-param name="threshold" select="$threshold" />
					<xsl:with-param name="scale" select="$scale" />
				</xsl:call-template>
			</td>
			<!--This section can be hard coded to branch points since that is all that it does.-->
			<xsl:if test="not ($isFunctionReport) and $reportBranchPoints">
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
					<xsl:value-of select="concat(format-number($threshold,'#0.0'), ' %')" />
				</td>
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
					<xsl:value-of select="$unvisitedcountbranch" />
				</td>
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcPercent</xsl:attribute>
					<xsl:call-template name="printcoverage">
						<xsl:with-param name="coverage" select="$coveragebranch"></xsl:with-param>
					</xsl:call-template>
				</td>
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>tmcGraph</xsl:attribute>
					<xsl:call-template name="detailPercent">
						<xsl:with-param name="notVisited" select="$unvisitedcountbranch" />
						<xsl:with-param name="total" select="$totalbranch" />
						<xsl:with-param name="threshold" select="$threshold" />
						<xsl:with-param name="scale" select="$scale" />
					</xsl:call-template>
				</td>
			</xsl:if>
			<xsl:if test="$reportCC">
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
					<xsl:value-of select="$stats/@ccavg" /> / <xsl:value-of select="$stats/@ccmax" />
				</td>
			</xsl:if>
			<xsl:if test="$reportVisitCounts">
				<xsl:variable name="count" select="$stats/@svc" />
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
					<xsl:choose>
						<xsl:when test="$count > 0">
							<xsl:value-of select="$count" />
						</xsl:when>
						<xsl:otherwise>-</xsl:otherwise>
					</xsl:choose>
				</td>
				<td>
					<xsl:attribute xml:space="preserve" name="class"><xsl:if test="$showBottomLine = 'True'">mtcBtm </xsl:if>mtcDR</xsl:attribute>
					<xsl:choose>
						<xsl:when test="$count > 0">
							<xsl:call-template name="printcoverage">
								<xsl:with-param name="coverage" select="($count div $projectstats/@svc) * 100"></xsl:with-param>
								<xsl:with-param name="twoplaces" select="'1'" />
							</xsl:call-template>
						</xsl:when>
						<xsl:otherwise>-</xsl:otherwise>
					</xsl:choose>
				</td>
			</xsl:if>
		</tr>
	</xsl:template>

	<!-- Exclusions Summary Not supported at this time.-->
	<xsl:template name="exclusionsSummary">
		<xsl:param name="excludedTitle" />
		<!--<tr>
			<td colspan="{$tablecolumns}">&#160;</td>
		</tr>
		<tr>
			<td class="exclusionTable mtHdLeft" colspan="1">Exclusion In Module</td>
			<td class="exclusionTable mtgHd" colspan="3">Item Excluded</td>
			<td class="exclusionTable mtgHd" colspan="1">All Code Within</td>
			<td class="exclusionTable mtgHd2" colspan="1">
				<xsl:value-of select="$excludedTitle" />
			</td>
		</tr>
		<xsl:for-each select="./exclusions/exclusion">
			<tr>
				<td class="mtcBtm etcItem exclusionCell" colspan="1">
					<xsl:value-of select="@m" />
				</td>
				<td class="mtcBtm tmcGraph exclusionCell" colspan="3">
					<xsl:value-of select="@n" />
				</td>
				<td class="mtcBtm tmcGraph exclusionCell" colspan="1">
					<xsl:value-of select="@cat" />
				</td>
				<td class="mtcBtm mtcData exclusionCell" colspan="1">
					<xsl:value-of select="@fp" />
				</td>
			</tr>
		</xsl:for-each>
		<tr>
			<td colspan="4">&#160;</td>
			<td class="exclusionTable mtHdLeft">Total</td>
			<td class="exclusionTable mtgHd2">
				<xsl:value-of select="sum(./exclusions/exclusion/@fp)"/>
			</td>
		</tr>-->
	</xsl:template>

	<!-- Footer -->
	<xsl:template name="footer2">
		<tr>
			<td colspan="{$tablecolumns}">&#160;</td>
		</tr>
	</xsl:template>

	<!-- Draw % Green/Red/Yellow Bar -->
	<xsl:template name="detailPercent">
		<xsl:param name="notVisited" />
		<xsl:param name="total" />
		<xsl:param name="threshold" />
		<xsl:param name="scale" />
		<xsl:variable name="nocoverage" select="$total = 0 and $notVisited = 0" />
		<xsl:variable name="visited" select="$total - $notVisited" />
		<xsl:variable name="coverage" select="$visited div $total * 100"/>
		<!--Fix rounding issues in the graph since we can only do whole numbers-->
		<xsl:variable name="right" select="format-number($coverage div 100 * $scale, '0') - 1" />
		<xsl:variable name="left" select="format-number($scale - $right, '0')" />
		<xsl:choose>
			<xsl:when test="$nocoverage">
				&#160;
			</xsl:when>
			<xsl:otherwise>
				<xsl:if test="$notVisited = 0">
					<img src="g.png" height="14" width="{format-number($scale, '0')}" />
				</xsl:if>
				<xsl:if test="($visited != 0) and ($notVisited != 0)">
					<img src="g.png" height="14" width="{$right}" />
				</xsl:if>
				<xsl:if test="$notVisited != 0">
					<xsl:if test="$coverage &gt;= $threshold">
						<img src="y.png" height="14" width="{$left}" />
					</xsl:if>
					<xsl:if test="$coverage &lt; $threshold">
						<img src="r.png" height="14" width="{$left}" />
					</xsl:if>
				</xsl:if>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--Summary for document reports that contain methods-->
	<xsl:template name="cyclomaticComplexitySummaryCommon">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle">Coverage</xsl:param>
		<xsl:param name="printClasses" />
		<xsl:param name="printMethods" />

		<xsl:for-each select="$root/mod[string(stats/@ex) != '1']">
			<xsl:sort select="stats[last()]/@ccmax" order="descending" data-type ="number" />
			<xsl:variable name="docstats" select="stats[last()]" />
			<xsl:variable name="documentID" select="@id" />
			<!--
			To make a nice clean report we want to see if we actually have a count before we continue.
			-->
			<xsl:variable name="classcount">
				<xsl:choose xml:space="preserve">
					<xsl:when test="$reportType = 'SymbolCCModuleClassFailedCoverageTop'"><xsl:value-of select="count(ns/class[string(stats/@ex) != '1' and stats/@ccmax > 0 and
										(
										(stats/@vsp div (stats/@usp + stats/@vsp)) * 100 &lt; stats/@acp
										or (stats/@vbp div (stats/@ubp + stats/@vbp)) * 100 &lt; stats/@acp
										)
										])"/></xsl:when>
					<xsl:when test="$reportType = 'MethodCCModuleClassFailedCoverageTop'"><xsl:value-of select="count(ns/class[string(stats/@ex) != '1' and stats/@ccmax > 0 and
										(stats/@vm div (stats/@um + stats/@vm)) * 100 &lt; stats/@afp])" /></xsl:when>
					<xsl:when test="$reportType = 'MethodCCModuleClassCoverageTop'"><xsl:value-of select="count(ns/class[string(stats/@ex) != '1' and stats/@ccmax > 0])" /></xsl:when>
					<xsl:otherwise>1</xsl:otherwise>
				</xsl:choose>
			</xsl:variable>

			<!--If we have something to show then show it-->
			<xsl:if test="$classcount > 0">
				<xsl:call-template name="sectionTableHeader">
					<xsl:with-param name="unvisitedTitle" select="'U.V.'" />
					<xsl:with-param name="coverageTitle" select="$coverageTitle" />
					<xsl:with-param name="sectionTitle">Module</xsl:with-param>
					<xsl:with-param name="cellClass">secondaryTable</xsl:with-param>
					<xsl:with-param name="showThreshold">True</xsl:with-param>
				</xsl:call-template>

				<xsl:call-template name="coverageDetail">
					<xsl:with-param name="name" select="assembly" />
					<xsl:with-param name="stats" select="$docstats" />
					<xsl:with-param name="showThreshold">True</xsl:with-param>
				</xsl:call-template>
				<tr>
					<td class="secondaryChildTable ctHeader" colspan="{$tablecolumns}">
						Classes <xsl:if test="$printMethods = 'True'"> / Methods</xsl:if>
					</td>
				</tr>
				<xsl:choose>
					<xsl:when test="$reportType = 'SymbolCCModuleClassFailedCoverageTop'">
						<!--Grab the first 10 classes where the sp or bp % is less than acceptable-->
						<xsl:for-each select="ns/class[string(stats/@ex) != '1' and stats/@ccmax > 0 and
										(
										(stats/@vsp div (stats/@usp + stats/@vsp)) * 100 &lt; stats/@acp
										or (stats/@vbp div (stats/@ubp + stats/@vbp)) * 100 &lt; stats/@acp
										)
										]">
							<xsl:sort select="stats[last()]/@ccmax" order="descending" data-type ="number" />
							<!--Not sure why the position above works so we are going to limit it here-->
							<xsl:if test="position() &lt; $ccClassCount">
								<xsl:call-template name="classCommon">
									<xsl:with-param name="printMethods" select="$printMethods" />
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:when>
					<xsl:when test="$reportType = 'MethodCCModuleClassFailedCoverageTop'">
						<!--Grab the first 10 classes where the sp or bp % is less than acceptable-->
						<xsl:for-each select="ns/class[string(stats/@ex) != '1' and stats/@ccmax > 0 and
										(stats/@vm div (stats/@um + stats/@vm)) * 100 &lt; stats/@afp]">
							<xsl:sort select="stats[last()]/@ccmax" order="descending" data-type ="number" />
							<!--Not sure why the position above works so we are going to limit it here-->
							<xsl:if test="position() &lt; $ccClassCount">
								<xsl:call-template name="classCommon">
									<xsl:with-param name="printMethods" select="$printMethods" />
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:when>
					<!--<xsl:when test="$reportType = 'SymbolCCModuleClassCoverageTop'">
				
				</xsl:when>-->
					<xsl:otherwise>
						<!--Grab the first 10 classes where the sp or bp %-->
						<xsl:for-each select="ns/class[string(stats/@ex) != '1']">
							<xsl:sort select="stats[last()]/@ccmax" order="descending" data-type ="number" />
							<!--Not sure why the position above works so we are going to limit it here-->
							<xsl:if test="position() &lt; $ccClassCount and stats[last()]/@ccmax > 0">
								<xsl:call-template name="classCommon">
									<xsl:with-param name="printMethods" select="$printMethods" />
								</xsl:call-template>
							</xsl:if>
						</xsl:for-each>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<!--Summary for document reports that contain methods-->
	<xsl:template name="documentSummaryCommon">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle">Coverage</xsl:param>
		<xsl:param name="printClasses" />
		<xsl:param name="printMethods" />

		<xsl:for-each select="$root/doc[string(stats/@ex) != '1' and string(@id) != '0']">
			<xsl:variable name="docstats" select="stats[last()]" />
			<xsl:variable name="documentID" select="@id" />

			<xsl:call-template name="sectionTableHeader">
				<xsl:with-param name="unvisitedTitle" select="'U.V.'" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="sectionTitle">Document</xsl:with-param>
				<xsl:with-param name="cellClass">secondaryTable</xsl:with-param>
				<xsl:with-param name="showThreshold">True</xsl:with-param>
			</xsl:call-template>

			<xsl:call-template name="coverageDetail">
				<xsl:with-param name="name" select="en" />
				<xsl:with-param name="stats" select="$docstats" />
				<xsl:with-param name="showThreshold">True</xsl:with-param>
			</xsl:call-template>
			<tr>
				<td class="secondaryChildTable ctHeader" colspan="{$tablecolumns}">
					Classes <xsl:if test="$printMethods = 'True'"> / Methods</xsl:if>
				</td>
			</tr>

			<!--For each class that is used for the document [string(stats/@ex) != '1']-->
			<xsl:for-each select="$root/mod/ns/class[string(stats/@ex) != '1']">
				<xsl:variable name="doc" select="stats/doc = $documentID" />
				<xsl:if test="$doc">
					<xsl:call-template name="classCommon">
						<xsl:with-param name="printMethods" select="$printMethods" />
					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
		</xsl:for-each>
	</xsl:template>

	<!-- Common Summary Header information -->
	<xsl:template name="summaryCommon">
		<xsl:param name="threshold" />
		<xsl:param name="unvisitedTitle" />
		<xsl:param name="coverageTitle">Coverage</xsl:param>
		<xsl:param name="printClasses" />
		<xsl:param name="printMethods" />
		<xsl:for-each select="$root/mod[string(stats/@ex) != '1']">
			<xsl:variable name="modstats" select="stats[last()]" />

			<xsl:call-template name="sectionTableHeader">
				<xsl:with-param name="unvisitedTitle" select="'U.V.'" />
				<xsl:with-param name="coverageTitle" select="$coverageTitle" />
				<xsl:with-param name="sectionTitle">Module</xsl:with-param>
				<xsl:with-param name="cellClass">secondaryTable</xsl:with-param>
				<xsl:with-param name="showThreshold">True</xsl:with-param>
			</xsl:call-template>

			<xsl:call-template name="coverageDetail">
				<xsl:with-param name="name" select="assembly" />
				<xsl:with-param name="stats" select="$modstats" />
				<xsl:with-param name="showThreshold">True</xsl:with-param>
			</xsl:call-template>
			<tr>
				<td class="secondaryChildTable ctHeader" colspan="{$tablecolumns}">
					Namespace <xsl:if test="$printClasses = 'True'"> / Classes</xsl:if>
				</td>
			</tr>
			<!--print the Namespaces classes and methods-->
			<xsl:call-template name="namespaceClassMethodSummary">
				<xsl:with-param name="printClasses" select="$printClasses" />
				<xsl:with-param name="printMethods" select="$printMethods" />
			</xsl:call-template>
		</xsl:for-each>
	</xsl:template>

	<!--print the Namespaces classes and methods-->
	<xsl:template name="namespaceClassMethodSummary">
		<xsl:param name="printClasses" />
		<xsl:param name="printMethods" />
		<xsl:for-each select="ns[string(stats/@ex) != '1']">
			<xsl:variable name="nsstats" select="stats[last()]" />
			<!--Modified to support threshold on the node-->
			<xsl:call-template name="coverageDetail">
				<xsl:with-param name="name" select="name" />
				<xsl:with-param name="stats" select="$nsstats" />
				<xsl:with-param name="styleTweak">padding-left:20px;font-weight:bold</xsl:with-param>
				<xsl:with-param name="showThreshold">True</xsl:with-param>
				<xsl:with-param name="showBottomLine">True</xsl:with-param>
			</xsl:call-template>
			<xsl:if test="$printClasses = 'True'">
				<xsl:for-each select="class[string(stats/@ex) != '1']">
					<xsl:call-template name="classCommon">
						<xsl:with-param name="printMethods" select="$printMethods" />
					</xsl:call-template>
				</xsl:for-each>
			</xsl:if>
		</xsl:for-each>
	</xsl:template>

	<!--Print the class and method as needed.-->
	<xsl:template name="classCommon">
		<xsl:param name="printMethods" />
		<xsl:variable name="clsstats" select="stats[last()]" />
		<xsl:call-template name="coverageDetail">
			<xsl:with-param name="name" select="name" />
			<xsl:with-param name="stats" select="$clsstats" />
			<xsl:with-param name="styleTweak">padding-left:30px</xsl:with-param>
			<xsl:with-param name="scale">
				<xsl:value-of select ="$graphscale * .8" />
			</xsl:with-param>
			<xsl:with-param name="showThreshold">True</xsl:with-param>
			<xsl:with-param name="showBottomLine">True</xsl:with-param>
		</xsl:call-template>
		<xsl:if test="$printMethods = 'True'">
			<xsl:for-each select="method[string(stats/@ex) != '1']">
				<xsl:call-template name="methodDetails" />
			</xsl:for-each>
		</xsl:if>
	</xsl:template>

	<xsl:template name="methodDetails">
		<xsl:variable name="methstats" select="stats[last()]" />
		<xsl:variable name="countTextDisplay">
			<xsl:choose>
				<xsl:when test="$isFunctionReport">-</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$methstats/@usp"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:call-template name="coverageDetail">
			<xsl:with-param name="name" select="name" />
			<xsl:with-param name="stats" select="$methstats" />
			<xsl:with-param name="countText" select="$countTextDisplay" />
			<xsl:with-param name="styleTweak">padding-left:40px;font-style:italic</xsl:with-param>
			<xsl:with-param name="scale">
				<xsl:value-of select ="$graphscale * .7" />
			</xsl:with-param>
			<xsl:with-param name="showBottomLine">True</xsl:with-param>
		</xsl:call-template>
	</xsl:template>

	<xsl:template name="printcoverage">
		<xsl:param name="coverage" />
		<xsl:param name="twoplaces" select="'0'" />
		<xsl:choose>
			<xsl:when test="$coverage = 'N/A'">
				<xsl:value-of select="$coverage" />
			</xsl:when>
			<xsl:when test="string($twoplaces) = '1'">
				<xsl:value-of select="concat(format-number($coverage,'#0.00'), ' %')" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="concat(format-number($coverage,'#0.0'), ' %')" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--Get the total items based on the report-->
	<xsl:template name="gettotal">
		<xsl:param name="stats" />
		<xsl:param name="branch" />
		<xsl:choose>
			<xsl:when test="$isFunctionReport">
				<xsl:value-of select="$stats/@um + $stats/@vm" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$branch = 'True'">
						<xsl:value-of select="$stats/@ubp + $stats/@vbp" />
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$stats/@usp + $stats/@vsp" />
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--Get the total visited items based on the report-->
	<xsl:template name="visiteditems">
		<xsl:param name="stats" />
		<xsl:choose>
			<xsl:when test="$isFunctionReport">
				<xsl:value-of select="$stats/@vm" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$stats/@vsp" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<!--Get the total visited items based on the report-->
	<xsl:template name="unvisiteditems">
		<xsl:param name="stats" />
		<xsl:choose>
			<xsl:when test="$isFunctionReport">
				<xsl:value-of select="$stats/@um" />
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$stats/@usp" />
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template name="style">
		<style>
			<xsl:text disable-output-escaping="yes">
					body                    { font: small verdana, arial, helvetica; color:#000000;	}
					.coverageReportTable    { font-size: 9px; }
					.reportHeader 				{ padding: 5px 8px 5px 8px; font-size: 12px; border: 1px solid; margin: 0px;	}
					.titleText					{ font-weight: bold; font-size: 12px; white-space: nowrap; padding: 0px; margin: 1px; }
					.subtitleText 				{ font-size: 9px; font-weight: normal; padding: 0px; margin: 1px; white-space: nowrap; }
					.projectStatistics			{ font-size: 10px; border-left: #649cc0 1px solid; white-space: nowrap;	width: 40%;	}
					.heading					{ font-weight: bold; }
					
					.mtHdLeftTop 	{
													border-top: #dcdcdc 1px solid;
													border-left: #dcdcdc 1px solid;  
													border-right: #dcdcdc 1px solid; 
													font-weight: bold;
													padding-left: 5px;
												}
					.mtHdRightTop 	{
													border-top: #dcdcdc 1px solid;
													border-right: #dcdcdc 1px solid; 
													font-weight: bold;
													padding-left: 5px;
													text-align: center;
												}
					.mtHdLeftFunc { 
													border: #dcdcdc 1px solid; 
													font-weight: bold;	
													padding-left: 5px; 
												}
					.mtHdLeft 		{
													border-bottom: #dcdcdc 1px solid;
													border-left: #dcdcdc 1px solid;  
													border-right: #dcdcdc 1px solid; 
													font-weight: bold;
													padding-left: 5px;
												}
					.mtHd 			{ border-bottom: 1px solid; border-top: 1px solid; border-right: 1px solid;	text-align: center;	}
					
					.mtHdLeft2 		{ font-weight: bold;	padding-left: 5px; }
					.mtHd2 			{ text-align: center;	}
					
					.mtgHd			{ border-bottom: 1px solid; border-top: 1px solid; border-right: 1px solid;	text-align: left; font-weight: bold; }
					.mtgHd2 		{ border-bottom: 1px solid; border-top: 1px solid; border-right: 1px solid;	text-align: center; font-weight: bold; }
					.mtDiv			{width:500px;overflow: hidden;}
					
					.mtcItem 			{ background: #ffffff; border-left: #dcdcdc 1px solid; border-right: #dcdcdc 1px solid; padding-left: 10px; padding-right: 10px; font-weight: bold; font-size: 10px; }
					.mtcData 			{ background: #ffffff; border-right: #dcdcdc 1px solid;	text-align: center;	white-space: nowrap; }
					.mtcDR { background: #ffffff; border-right: #dcdcdc 1px solid;	text-align: right;	white-space: nowrap; }
					.mtcPercent 		{ background: #ffffff; font-weight: bold; white-space: nowrap; text-align: right; padding-left: 10px; }
					.tmcGraph 		{ background: #ffffff; border-right: #dcdcdc 1px solid; padding-right: 5px; }
					.mtcBtm		{ border-bottom: #dcdcdc 1px solid;	}
					.ctHeader 			{ border-top: 1px solid; border-bottom: 1px solid; border-left: 1px solid; border-right: 1px solid;	font-weight: bold; padding-left: 10px; }
					.ctciItem { background: #ffffff; border-left: #dcdcdc 1px solid; border-right: #dcdcdc 1px solid; padding-right: 10px; font-size: 10px; }
					.etcItem 	{ background: #ffffff; border-left: #dcdcdc 1px solid; border-right: #dcdcdc 1px solid; padding-left: 10px; padding-right: 10px; }
					.projectTable				{ background: #a9d9f7; border-color: #649cc0; }
					.primaryTable				{ background: #d7eefd; border-color: #a4dafc; }
					.secondaryTable 			{ background: #f9e9b7; border-color: #f6d376; }
					.secondaryChildTable 		{ background: #fff6df; border-color: #f5e1b1; }
					.exclusionTable				{ background: #e1e1e1; border-color: #c0c0c0; }
					.exclusionCell				{ background: #f7f7f7; }
					.graphBarNotVisited			{ font-size: 2px; border:#9c9c9c 1px solid; background:#df0000; }
					.graphBarSatisfactory		{ font-size: 2px; border:#9c9c9c 1px solid;	background:#f4f24e; }
					.graphBarVisited			{ background: #00df00; font-size: 2px; border-left:#9c9c9c 1px solid; border-top:#9c9c9c 1px solid; border-bottom:#9c9c9c 1px solid; }
					.graphBarVisitedFully		{ background: #00df00; font-size: 2px; border:#9c9c9c 1px solid; }
        </xsl:text>
		</style>
	</xsl:template>

</xsl:stylesheet>
