<?xml version="1.0" encoding="UTF-8"?>

<!-- Copyright (c) Microsoft Corporation.  All rights reserved. -->

<xsl:stylesheet version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:mtps="http://msdn2.microsoft.com/mtps"
  xmlns:asp="http://msdn2.microsoft.com/asp" 
  xmlns:xhtml="http://www.w3.org/1999/xhtml"
  xmlns:k="urn:mtpg-com:mtps/2004/1/key"
  xmlns:mtps2="urn:msdn-com:public-content-syndication"
  xmlns:hxLink="urn:Link"
  xmlns:xlink="http://www.w3.org/1999/xlink" 
  xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
  xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
  xmlns:se="urn:mtpg-com:mtps/2004/1/search" 
  exclude-result-prefixes="hxLink mtps xhtml mtps2 k asp se">

  <xsl:output method="html" indent="yes" />

  <xsl:param name="locale" />
  <xsl:param name="version" />
  <xsl:param name="contentId" />
  <xsl:param name="output" />
  <xsl:param name="metadata" />
  <xsl:param name="charset" />


  <xsl:template match="xhtml:div[@class='topic']">
<html>
  <!--<meta http-equiv="Content-Type" content="text/html; CHARSET={$charset}" /> -->
<head>
  <title><xsl:value-of select="xhtml:div[@class='title']"/></title>
  <link rel="stylesheet" type="text/css" href="Classic.css" ></link>
</head>
<body>
<xsl:apply-templates />
</body>
</html>
</xsl:template>

  <!-- Self-closing majorTitle divs cause their background color to be applied to
  the entire page. -->
  <xsl:template match ="xhtml:div[@class='majorTitle']">
    <div class="majorTitle">
      <xsl:choose>
        <xsl:when test="child::node() or child::text()">
          <xsl:apply-templates select="@*"/>
          <xsl:apply-templates/>
        </xsl:when>
        <xsl:otherwise>
          <xsl:comment>*</xsl:comment>
        </xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>



  <xsl:template match="xhtml:PRE|xhtml:pre|PRE|pre">
<pre><xsl:apply-templates select="@*"/><xsl:apply-templates/></pre>
</xsl:template>

<xsl:template match="xhtml:CODE|xhtml:code">
<code><xsl:apply-templates select="@*"/><xsl:apply-templates/></code>
</xsl:template>

<xsl:template match="xhtml:SPAN|xhtml:span">
<span><xsl:apply-templates select="@*"/><xsl:apply-templates/></span>
</xsl:template>

<xsl:template match="xhtml:H1|xhtml:h1">
<h1><xsl:apply-templates select="@*"/><xsl:apply-templates/></h1>
</xsl:template>

<xsl:template match="xhtml:H2|xhtml:h2">
<h2><xsl:apply-templates select="@*"/><xsl:apply-templates/></h2>
</xsl:template>

  <!-- Many code blocks contain a self-closing h3, which causes IE to
  render the rest of the file as h3. Remove self-closing h3s. -->
<xsl:template match="xhtml:H3|xhtml:h3">
  <xsl:choose>
    <xsl:when test="child::node() or child::text()">
      <h3>
        <xsl:apply-templates select="@*"/>
        <xsl:apply-templates/>
      </h3>
    </xsl:when>
  </xsl:choose>
</xsl:template>

<xsl:template match="xhtml:H4|xhtml:h4">
  <h4>
    <xsl:apply-templates select="@*"/>
    <xsl:apply-templates/>
  </h4>
</xsl:template>

<xsl:template match="xhtml:BR|xhtml:br">
  <br/>
</xsl:template>

<xsl:template match="xhtml:P|xhtml:p">
<p><xsl:apply-templates select="@*"/><xsl:apply-templates/></p>
</xsl:template>

<xsl:template match="xhtml:HR|xhtml:hr">
<hr><xsl:apply-templates select="@*"/><xsl:apply-templates /></hr>
</xsl:template>

<xsl:template match="xhtml:I|xhtml:i">
<i><xsl:apply-templates select="@*"/><xsl:apply-templates /></i>
</xsl:template>

<xsl:template match="xhtml:B|xhtml:b">
<b><xsl:apply-templates select="@*"/>
  <xsl:apply-templates />
</b>
</xsl:template>

<xsl:template match="xhtml:FONT|xhtml:font">
<font><xsl:apply-templates select="@*"/>
  <xsl:apply-templates />
</font>
</xsl:template>

<xsl:template match="xhtml:CENTER|xhtml:center">
<center><xsl:apply-templates select="@*"/>
  <xsl:apply-templates />
</center>
</xsl:template>

<xsl:template match="xhtml:BLOCKQUOTE|xhtml:blockquote">
<blockquote><xsl:apply-templates select="@*"/><xsl:apply-templates/></blockquote>
</xsl:template>

  <!-- xmlns="" was added to all <a hrefs> in the content cache so needed to add a|A to template. -->

  <xsl:template match="xhtml:a|xhtml:A|a|A">
  <xsl:choose>
    <!-- Some documentation has empty <a name="aName"> tags which end up self-closing, which confuses
    IE and Firefox. The comment within is to prevent a self-closing tag.  -->
    <xsl:when test="@name">
    <a><xsl:apply-templates select="@*" /><xsl:apply-templates /><xsl:comment>*</xsl:comment></a>
    </xsl:when>
    
    <xsl:when test="@href">
    <xsl:choose>
      <xsl:when test="starts-with(@href,'#') or starts-with(@href,'http:')">
        <a>
          <xsl:apply-templates select="@*" />
          <xsl:attribute name="href">
            <xsl:value-of select="@href" />
          </xsl:attribute>
          <xsl:apply-templates/>
        </a>
      </xsl:when>

      <xsl:when test ="starts-with(@href, 'AssetId:')">
        <xsl:variable name="link">
          <xsl:value-of select="hxLink:Resolve(@href, $version, $locale, true())"/>
        </xsl:variable>
        
            <a>
              <xsl:apply-templates select="@*" />
              <xsl:attribute name="href">
                <xsl:choose>
                  <xsl:when test="starts-with($link, 'http:')">
                    <xsl:value-of select="$link" />
                  </xsl:when>
                  <xsl:otherwise>
                    <xsl:text>html/</xsl:text>
                    <xsl:value-of select="$link" />
                    <xsl:text>.htm</xsl:text>
                  </xsl:otherwise>
                </xsl:choose>
              </xsl:attribute>
              <xsl:apply-templates/>
            </a>
        
      </xsl:when>


    </xsl:choose>
  </xsl:when>
  </xsl:choose>
</xsl:template>


<xsl:template match="xhtml:UL|xhtml:ul">
<ul><xsl:apply-templates select="@*"/><xsl:apply-templates/></ul>
</xsl:template>

<xsl:template match="xhtml:OL|xhtml:ol">
<ol><xsl:apply-templates select="@*"/><xsl:apply-templates/></ol>
</xsl:template>

<xsl:template match="xhtml:LI|xhtml:li">
<li><xsl:apply-templates select="@*"/><xsl:apply-templates/></li>
</xsl:template>

  <xsl:template match="xhtml:DL|xhtml:dl">
    <dl>
      <xsl:apply-templates select="@*"/><xsl:apply-templates/>
    </dl>
  </xsl:template>

  <xsl:template match="xhtml:DT|xhtml:dt">
    <dt>
      <xsl:apply-templates select="@*"/><xsl:apply-templates/>
    </dt>
  </xsl:template>

  <xsl:template match="xhtml:TABLE|xhtml:table">
<table><xsl:apply-templates select="@*"/><xsl:apply-templates/></table>
</xsl:template>

<xsl:template match="xhtml:TR|xhtml:tr">
<tr><xsl:apply-templates select="@*"/><xsl:apply-templates/></tr>
</xsl:template>

<xsl:template match="xhtml:TD|xhtml:td">
<td><xsl:apply-templates select="@*"/><xsl:apply-templates/></td>
</xsl:template>

<xsl:template match="xhtml:TH|xhtml:th">
<th><xsl:apply-templates select="@*"/><xsl:apply-templates/></th>
</xsl:template>

<xsl:template match="xhtml:TBODY|xhtml:tbody">
<tbody><xsl:apply-templates select="@*"/><xsl:apply-templates/></tbody>
</xsl:template>

<xsl:template match="xhtml:TFOOT|xhtml:tfoot">
<tfoot><xsl:apply-templates select="@*"/><xsl:apply-templates/></tfoot>
</xsl:template>

<xsl:template match="xhtml:THEAD|xhtml:thead">
<thead><xsl:apply-templates select="@*"/><xsl:apply-templates/></thead>
</xsl:template>

<xsl:template match="xhtml:DIV|xhtml:div">
<div><xsl:apply-templates select="@*"/><xsl:apply-templates/></div>
</xsl:template>

<xsl:template match="xhtml:INPUT|xhtml:input">
<input><xsl:apply-templates select="@*"/><xsl:apply-templates/></input>
</xsl:template>

<xsl:template match="xhtml:IMG|xhtml:img">
<img>
<xsl:apply-templates select="@*" />
<xsl:if test="@src">
<xsl:choose>
<xsl:when test="starts-with(@src,'/')">
<xsl:attribute name="src"><xsl:value-of select="@src" /></xsl:attribute><xsl:apply-templates/>
</xsl:when>
<xsl:when test="starts-with(@src,'http://')">
<xsl:attribute name="src"><xsl:value-of select="@src" /></xsl:attribute><xsl:apply-templates/>
</xsl:when> 
<xsl:otherwise>
<xsl:attribute name="src"><xsl:value-of select="@src" /></xsl:attribute><xsl:apply-templates/>
</xsl:otherwise>
</xsl:choose>
</xsl:if>
</img>
</xsl:template>

<xsl:template match="xhtml:IFRAME|xhtml:iframe">
<iframe><xsl:apply-templates select="@*"/></iframe>
</xsl:template>

<xsl:template match="//SCRIPT|//script">
<script><xsl:apply-templates select="@*"/><xsl:value-of select="." disable-output-escaping="yes" /></script>
</xsl:template>

  <xsl:template match="@*">
    <xsl:attribute name="{name()}"><xsl:value-of select="."/></xsl:attribute>
  </xsl:template>

</xsl:stylesheet>
