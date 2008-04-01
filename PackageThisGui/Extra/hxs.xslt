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
  xmlns:an="urn:mtpg-com:mtps/2007/1/annotations"
  exclude-result-prefixes="hxLink mtps xhtml mtps2 k asp se an">

  <xsl:output method="xml" encoding="utf-8" indent="yes" />

  <xsl:param name="locale" />
  <xsl:param name="version" />
  <xsl:param name="contentId" />
  <xsl:param name="output" />
  <xsl:param name="metadata" />
  <xsl:param name="annotations" />

  <xsl:template match="xhtml:div[@class='topic']">
    <html>
      <head>
        <title>
          <xsl:value-of select="xhtml:div[@class='title']"/>
        </title>
        <xml>
          <xsl:apply-templates mode="msh" select="$metadata"/>
        </xml>
        <link rel="stylesheet" type="text/css" href="ms-help://Hx/HxRuntime/HxLink.css" >
        </link>
        <link rel="stylesheet" type="text/css" href="../Classic.css" ></link>

      </head>
      <body>
        <xsl:apply-templates />
        <xsl:if test="$annotations//an:annotation">
          <xsl:apply-templates mode="annotations" select="$annotations" />
        </xsl:if>
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
    <pre>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </pre>
  </xsl:template>

  <xsl:template match="xhtml:CODE|xhtml:code|CODE|code">
    <code>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </code>
  </xsl:template>

  <xsl:template match="xhtml:SPAN|xhtml:span|SPAN|span">
    <span>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </span>
  </xsl:template>

  <xsl:template match="xhtml:H1|xhtml:h1|H1|h1">
    <h1>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </h1>
  </xsl:template>

  <xsl:template match="xhtml:H2|xhtml:h2|H2|h2">
    <h2>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </h2>
  </xsl:template>

  <!-- Many code blocks contain a self-closing h3, which causes IE to
  render the rest of the file as h3. Remove self-closing h3s. -->
  <xsl:template match="xhtml:H3|xhtml:h3|H3|h3">
    <xsl:choose>
      <xsl:when test="child::node() or child::text()">
        <h3>
          <xsl:apply-templates select="@*"/>
          <xsl:apply-templates/>
        </h3>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="xhtml:H4|xhtml:h4|H4|h4">
    <h4>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </h4>
  </xsl:template>

  <xsl:template match="xhtml:BR|xhtml:br|BR|br">
    <br/>
  </xsl:template>

  <xsl:template match="xhtml:P|xhtml:p|P|p">
    <p>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </p>
  </xsl:template>

  <xsl:template match="xhtml:HR|xhtml:hr|HR|hr">
    <hr>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates />
    </hr>
  </xsl:template>

  <xsl:template match="xhtml:I|xhtml:i|i|I">
    <i>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </i>
  </xsl:template>

  <xsl:template match="xhtml:B|xhtml:b|b|B">
    <b>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </b>
  </xsl:template>

  <xsl:template match="xhtml:FONT|xhtml:font|FONT|font">
    <font>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </font>
  </xsl:template>

  <xsl:template match="xhtml:CENTER|xhtml:center|CENTER|center">
    <center>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </center>
  </xsl:template>

  <xsl:template match="xhtml:BLOCKQUOTE|xhtml:blockquote">
    <blockquote>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </blockquote>
  </xsl:template>

  <!-- xmlns="" was added to all <a hrefs> in the content cache so needed to add a|A to template. -->
  <xsl:template match="xhtml:a|xhtml:A|a|A">
    <xsl:choose>
      <!-- Some documentation has empty <a name="aName"> tags which end up self-closing, which confuses
    IE and Firefox. The comment within is to prevent a self-closing tag. 
    -->
      <xsl:when test="@name">
        <a>
          <xsl:apply-templates select="@*" />
          <xsl:apply-templates />
          <xsl:comment>*</xsl:comment>
        </a>
      </xsl:when>

      <xsl:when test="@href">
        <xsl:choose>
          <xsl:when test="starts-with(@href,'#')">
            <a>
              <xsl:apply-templates select="@*" />
              <xsl:attribute name="href">
                <xsl:value-of select="@href" />
              </xsl:attribute>
              <xsl:apply-templates/>
            </a>
          </xsl:when>
          <xsl:when test="starts-with(@href,'http:')">
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
              <xsl:value-of select="hxLink:Resolve(@href, $version, $locale, false())"/>
            </xsl:variable>

            <xsl:choose>
              <xsl:when test="starts-with($link, 'http:') or $link=''">
                <a>
                  <xsl:apply-templates select="@*" />
                  <xsl:attribute name="href">
                    <xsl:value-of select="$link" />
                  </xsl:attribute>
                  <xsl:apply-templates/>
                </a>
              </xsl:when>
              <xsl:otherwise>
                <xsl:element name="MSHelp:link">
                  <xsl:attribute name="keywords">
                    <xsl:value-of select="$link"/>
                  </xsl:attribute>
                  <xsl:attribute name="tabindex">0</xsl:attribute>
                  <xsl:apply-templates/>
                </xsl:element>
              </xsl:otherwise>
            </xsl:choose>

          </xsl:when>
        </xsl:choose>
      </xsl:when>
    </xsl:choose>
  </xsl:template>


  <xsl:template match="xhtml:UL|xhtml:ul|UL|ul">
    <ul>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </ul>
  </xsl:template>

  <xsl:template match="xhtml:OL|xhtml:ol|OL|ol">
    <ol>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </ol>
  </xsl:template>

  <xsl:template match="xhtml:LI|xhtml:li|LI|li">
    <li>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </li>
  </xsl:template>

  <xsl:template match="xhtml:DL|xhtml:dl|DL|dl">
    <dl>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </dl>
  </xsl:template>

  <xsl:template match="xhtml:DT|xhtml:dt|DT|dt">
    <dt>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </dt>
  </xsl:template>

  <xsl:template match="xhtml:TABLE|xhtml:table|TABLE|table">
    <table>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </table>
  </xsl:template>

  <xsl:template match="xhtml:TR|xhtml:tr|TR|tr">
    <tr>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </tr>
  </xsl:template>

  <xsl:template match="xhtml:TD|xhtml:td|TD|td">
    <td>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </td>
  </xsl:template>

  <xsl:template match="xhtml:TH|xhtml:th|TH|th">
    <th>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </th>
  </xsl:template>

  <xsl:template match="xhtml:TBODY|xhtml:tbody|TBODY|tbody">
    <tbody>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </tbody>
  </xsl:template>

  <xsl:template match="xhtml:TFOOT|xhtml:tfoot|TFOOT|tfoot">
    <tfoot>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </tfoot>
  </xsl:template>

  <xsl:template match="xhtml:THEAD|xhtml:thead|THEAD|thead">
    <thead>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </thead>
  </xsl:template>

  <xsl:template match="xhtml:DIV|xhtml:div|DIV|div">
    <div>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </div>
  </xsl:template>

  <xsl:template match="xhtml:INPUT|xhtml:input|INPUT|input">
    <input>
      <xsl:apply-templates select="@*"/>
      <xsl:apply-templates/>
    </input>
  </xsl:template>

  <xsl:template match="xhtml:IMG|xhtml:img|IMG|img">
    <img>
      <xsl:apply-templates select="@*" />
      <xsl:if test="@src">
        <xsl:choose>
          <xsl:when test="starts-with(@src,'/')">
            <xsl:attribute name="src">
              <xsl:value-of select="@src" />
            </xsl:attribute>
            <xsl:apply-templates/>
          </xsl:when>
          <xsl:when test="starts-with(@src,'http://')">
            <xsl:attribute name="src">
              <xsl:value-of select="@src" />
            </xsl:attribute>
            <xsl:apply-templates/>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="src">
              <xsl:value-of select="@src" />
            </xsl:attribute>
            <xsl:apply-templates/>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
    </img>
  </xsl:template>

  <xsl:template match="xhtml:IFRAME|xhtml:iframe">
    <iframe>
      <xsl:apply-templates select="@*"/>
    </iframe>
  </xsl:template>

  <xsl:template match="//SCRIPT|//script">
    <script>
      <xsl:apply-templates select="@*"/>
      <xsl:value-of select="." disable-output-escaping="yes" />
    </script>
  </xsl:template>


  <xsl:template mode="msh" match="xhtml:meta">
    <xsl:if test="@name='MSHKeywordA'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">A</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHKeywordB'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">B</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHKeywordF'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">F</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHKeywordK'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">K</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHKeywordN'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">N</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHKeywordS'">
      <xsl:element name="MSHelp:Keyword">
        <xsl:attribute name="Index">S</xsl:attribute>
        <xsl:attribute name="Term">
          <xsl:value-of select="@content"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
    <xsl:if test="@name='MSHAttr'">
      <xsl:element name="MSHelp:Attr">
        <xsl:attribute name="Name">
          <xsl:value-of select="substring-before(@content,':')"/>
        </xsl:attribute>
        <xsl:attribute name="Value">
          <xsl:value-of select="substring-after(@content,':')"/>
        </xsl:attribute>
      </xsl:element>
    </xsl:if>
  </xsl:template>

  <xsl:template mode="annotations" match="//an:annotations">
    <table class="ccHeader" cellpadding="0" cellspacing="0" border="0" width="100%" style="margin:5px 0px 0px 0px; border">
      <tr style="height:66px;max-height:66px;margin:0px 0px 0px 0px">
        <td style="width:30%;padding:0px 0px 0px 0px">
          <div class="cch_left">
            <div class="cch_title">
              <span>Community Content</span>
            </div>
          </div>
        </td>
        <td style="width:2px;padding:0px 0px 0px 0px">
          <div class="cch_middle">&#160;</div>
        </td>
        <td style="width:100%;padding:0px 0px 0px 0px">
          <div class="cch_right">&#160;</div>
        </td>
      </tr>
    </table>

    <xsl:apply-templates mode="annotations" select="*" />


  </xsl:template>

  <xsl:template mode="annotations" match="an:annotation">
    <div class="wkViewerContainer">
      <table width="100%">
        <tr>
          <td align="left">
            <span class="wkViewerHeader_Title">
              <xsl:apply-templates mode="annotations" select="an:title" />
            </span>
          </td>
          <td>
            <xsl:apply-templates mode="annotations" select="an:lastEditedUTC" /> UTC
          </td>
          <td align="right">
            <xsl:apply-templates mode="annotations" select="an:user" />
          </td>
        </tr>
        <tr>
          <td colspan="3">
            <xsl:apply-templates mode="annotations" select="an:body" />
          </td>
        </tr>
      </table>
    </div>
  </xsl:template>

  <xsl:template mode="annotations" match="an:body">
    <xsl:apply-templates />
  </xsl:template>


  <xsl:template match="@*">
    <xsl:attribute name="{name()}">
      <xsl:value-of select="."/>
    </xsl:attribute>
  </xsl:template>

</xsl:stylesheet>
