using System;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Globalization;

namespace YTech.IM.SenseCity.Web.Controllers.Helper
{
    public class ReportViewerHelper
    {
        private XmlDocument oXmlDoc;

        const double dCM2Inches = 2.54000000d;


        private SizeF sizePage;
        private SizeF sizeReport;
        private PointF pointMarginsLT;
        private PointF pointMarginsRB;


        private SizeF sizePageOld;
        private PointF pointMarginsLTOld;
        private PointF pointMarginsRBOld;
        private SizeF sizeReportOld;


        private NumberFormatInfo oNumInfo;


        public SizeF PageSize
        {
            get { return sizePage; }
            set { sizePage = value; }
        }

        public SizeF ReportSize
        {
            get { return sizeReport; }
            set { sizeReport = value; }
        }

        public PointF MarginsLT
        {
            get { return pointMarginsLT; }
            set { pointMarginsLT = value; }
        }

        public PointF MarginsRB
        {
            get { return pointMarginsRB; }
            set { pointMarginsRB = value; }
        }


        public ReportViewerHelper(string sXMLData)
        {
            oXmlDoc = new XmlDocument();

            oXmlDoc.LoadXml(sXMLData);

            //Need to create a custom format due to other cultures      
            this.oNumInfo = new NumberFormatInfo();

            this.oNumInfo.NumberDecimalSeparator = ".";
            this.oNumInfo.CurrencyDecimalDigits = 2;
            this.oNumInfo.CurrencyDecimalSeparator = ".";
            this.oNumInfo.NegativeSign = "-";
            this.oNumInfo.PositiveSign = "";


            this.PageSize = new SizeF(0, 0);
            this.MarginsLT = new PointF(0, 0);
            this.MarginsRB = new PointF(0, 0);

            this.sizePageOld = new SizeF(0, 0);

            this.pointMarginsLTOld = new PointF(0, 0);
            this.pointMarginsRBOld = new PointF(0, 0);

            this.sizeReport = new SizeF(0, 0);
            this.sizeReportOld = new SizeF(0, 0);
        }

        public void SetPageSize(SizeF page, PointF marginLT, PointF marginRB)
        {
            //Page and Margin are PrinterSettings and in 100th of inch     

            //Convert 100th of inch to cm     
            this.PageSize = new SizeF((float)Math.Round(((double)page.Width) * dCM2Inches, 1),
                                      (float)Math.Round(((double)page.Height) * dCM2Inches, 1));


            this.MarginsLT = new PointF((float)Math.Round(((double)marginLT.X) * dCM2Inches, 2),
                                            (float)Math.Round(((double)marginLT.Y) * dCM2Inches, 2));

            this.MarginsRB = new PointF((float)Math.Round(((double)marginRB.X ) * dCM2Inches, 2),
                                            (float)Math.Round(((double)marginRB.Y) * dCM2Inches, 2));

            this.ReportSize = new SizeF(this.PageSize.Width - this.MarginsLT.X - this.MarginsRB.X, this.PageSize.Height - this.MarginsLT.Y - this.MarginsRB.Y);


            XmlNode oRoot = oXmlDoc.DocumentElement;

            //XmlNode reportNode = GetNode("Report", oRoot);
            XmlNode pageNode = GetNode("Page", oRoot);
            XmlNode oNode;

            CultureInfo ci = CultureInfo.CurrentCulture;

            //Save the current values for the PageWidht, PageHeight and Margins     
            //Only supports values in CM  
            //PageWidth is in Page node, not in root node
            oNode = GetNode("PageWidth", pageNode);
            this.sizePageOld.Width = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

            oNode.InnerText = this.PageSize.Width.ToString(".0", this.oNumInfo) + "cm";


            oNode = GetNode("PageHeight", pageNode);
            this.sizePageOld.Height = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

            oNode.InnerText = this.PageSize.Height.ToString(".0", this.oNumInfo) + "cm";

            oNode = GetNode("LeftMargin", pageNode);
            if (oNode != null)
            {
                this.pointMarginsLTOld.X = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

                oNode.InnerText = this.MarginsLT.X.ToString(".0", this.oNumInfo) + "cm";
            }
            oNode = GetNode("RightMargin", pageNode);
            if (oNode != null)
            {
                this.pointMarginsRBOld.X = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

                oNode.InnerText = this.MarginsRB.X.ToString(".0", this.oNumInfo) + "cm";
            }

            oNode = GetNode("TopMargin", pageNode);
            if (oNode != null)
            {
                this.pointMarginsLTOld.Y = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

                oNode.InnerText = this.MarginsLT.Y.ToString(".0", this.oNumInfo) + "cm";
            }

            oNode = GetNode("BottomMargin", pageNode);
            if (oNode != null)
            {
                this.pointMarginsRBOld.Y = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);

                oNode.InnerText = this.MarginsRB.Y.ToString(".0", this.oNumInfo) + "cm";
            }

            oNode = GetNode("Width", oRoot);
            if (oNode != null)
            {
                this.sizeReportOld.Width = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);
            }

            oNode = GetNode("Body", oRoot);

            if (oNode != null)
            {
                oNode = GetNode("Height", oNode);

                if (oNode != null)
                {
                    this.sizeReportOld.Height = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);
                }
            }

        }

        public void SetAllFieldWidth(bool bFixHeight)
        {
            XmlNode oRoot = oXmlDoc.DocumentElement;


            XmlNode oNode = GetNode("ReportItems", oRoot, true);

            //Only change items in ReportItems     
            if (oNode != null)
            {
                FixNodeWidth(oNode);
            }

            if (bFixHeight)
            {
                //Change the design area     
                SetWidthHeight(GetNode("Width", oRoot), GetNode("Height", oRoot));
            }
            else
            {
                //Change the design area     
                SetWidthHeight(GetNode("Width", oRoot), null);
            }
        }

        private void FixNodeWidth(XmlNode oRoot)
        {
            //check all nodes for a Width element     
            foreach (XmlNode oNode in oRoot.ChildNodes)
            {
                //Hack to able to also scale item in the middle of the page on the same line     
                //Set in the Label field to ScaleLeft     
                XmlNode oNodeLabel = GetNode("Label", oNode);


                if ((oNodeLabel != null) && (oNodeLabel.InnerText.IndexOf("ScaleHeight") != -1))
                {
                    //Change the width and height as a percentage of the orginal     
                    SetWidthHeight(GetNode("Width", oNode), GetNode("Height", oNode));
                }
                else
                {
                    SetWidthHeight(GetNode("Width", oNode), null);
                }


                if ((oNodeLabel != null) && (oNodeLabel.InnerText.IndexOf("ScaleLeft") != -1))
                {
                    //Change the Left as a percentage of the orginal     
                    SetLeft(GetNode("Left", oNode));
                }

                if ((oNodeLabel != null) && (oNodeLabel.InnerText.IndexOf("ScaleTop") != -1))
                {
                    //Change the Left as a percentage of the orginal     
                    SetTop(GetNode("Top", oNode));
                }

                if ((oNodeLabel != null) && (oNodeLabel.InnerText.IndexOf("HeightFull") != -1))
                {
                    //Change the Left as a percentage of the orginal     
                    SetHeightFullPage(GetNode("Top", oNode), GetNode("Height", oNode));
                }


                if (oNode.HasChildNodes)
                {
                    FixNodeWidth(oNode);
                }
            }
        }

        private void SetLeft(XmlNode oNodeWidth)
        {
            if (oNodeWidth != null)
            {
                float fNewWidth = GetNewWidth(float.Parse(oNodeWidth.InnerText.Replace("cm", ""), this.oNumInfo));

                if (fNewWidth > 0)
                {
                    oNodeWidth.InnerText = fNewWidth.ToString(".0", this.oNumInfo) + "cm";
                }
            }
        }

        private void SetTop(XmlNode oNodeHeight)
        {
            if (oNodeHeight != null)
            {
                float fNewHeight = GetNewHeight(float.Parse(oNodeHeight.InnerText.Replace("cm", ""), this.oNumInfo));


                if (fNewHeight > 0)
                {
                    oNodeHeight.InnerText = fNewHeight.ToString(".0", this.oNumInfo) + "cm";
                }


            }
        }

        private void SetWidthHeight(XmlNode oNodeWidth, XmlNode oNodeHeight)
        {
            SetLeft(oNodeWidth);
            SetTop(oNodeHeight);
        }

        private void SetHeightFullPage(XmlNode oNode, XmlNode oNodeHeight)
        {
            if (oNode != null)
            {
                float fCurrent = float.Parse(oNode.InnerText.Replace("cm", ""), this.oNumInfo);


                float fNew = this.PageSize.Height - fCurrent - this.MarginsLT.Y - this.MarginsRB.Y;

                if (fNew > 0)
                {
                    oNodeHeight.InnerText = fNew.ToString(".0", this.oNumInfo) + "cm";
                }
            }
        }

        public SizeF GetItemSize(string sName)
        {
            SizeF oSize = new SizeF();

            XmlNode oNode = FindNodeName(sName, GetNode("ReportItems", oXmlDoc.DocumentElement, true));

            XmlNode oNodeWidth = GetNode("Width", oNode);
            XmlNode oNodeHeight = GetNode("Height", oNode);

            if (oNodeWidth != null)
            {
                oSize.Width = float.Parse(oNodeWidth.InnerText.Replace("cm", ""), this.oNumInfo);
            }

            if (oNodeHeight != null)
            {
                oSize.Height = float.Parse(oNodeHeight.InnerText.Replace("cm", ""), this.oNumInfo);
            }

            return oSize;
        }

        public SizeF GetItemPos(string sName)
        {
            SizeF oSize = new SizeF();

            XmlNode oNode = FindNodeName(sName, GetNode("ReportItems", oXmlDoc.DocumentElement, true));

            XmlNode oNodeWidth = GetNode("Left", oNode);
            XmlNode oNodeHeight = GetNode("Top", oNode);

            if (oNodeWidth != null)
            {
                oSize.Width = float.Parse(oNodeWidth.InnerText.Replace("cm", ""), this.oNumInfo);
            }

            if (oNodeHeight != null)
            {
                oSize.Height = float.Parse(oNodeHeight.InnerText.Replace("cm", ""), this.oNumInfo);

            }

            return oSize;
        }

        public void SetItemSize(string sName, SizeF oSize)
        {
            XmlNode oNode = FindNodeName(sName, GetNode("ReportItems", oXmlDoc.DocumentElement, true));

            XmlNode oNodeWidth = GetNode("Width", oNode);
            XmlNode oNodeHeight = GetNode("Height", oNode);

            if (oNodeWidth != null)
            {
                oNodeWidth.InnerText = oSize.Width.ToString(".0", this.oNumInfo) + "cm";
            }

            if (oNodeHeight != null)
            {
                oNodeHeight.InnerText = oSize.Height.ToString(".0", this.oNumInfo) + "cm";
            }
        }

        public void SetItemPos(string sName, SizeF oPos)
        {
            XmlNode oNode = FindNodeName(sName, GetNode("ReportItems", oXmlDoc.DocumentElement, true));

            XmlNode oNodeLeft = GetNode("Left", oNode);
            XmlNode oNodeTop = GetNode("Top", oNode);

            if (oNodeLeft != null)
            {
                oNodeLeft.InnerText = oPos.Width.ToString(".0", this.oNumInfo) + "cm";
            }

            if (oNodeTop != null)
            {
                oNodeTop.InnerText = oPos.Height.ToString(".0", this.oNumInfo) + "cm";
            }
        }

        private XmlNode GetNode(string sName, XmlNode oRoot)
        {
            return GetNode(sName, oRoot, false);
        }

        //Method to find the XML elements     
        //This may not be the fastest way to do this?     
        private XmlNode GetNode(string sName, XmlNode oRoot, bool bRecursive)
        {
            bool bFlag = false;
            int iCount = 0;
            XmlNode oNode = null;

            while ((!bFlag) && (iCount < oRoot.ChildNodes.Count))
            {
                oNode = oRoot.ChildNodes.Item(iCount);

                if (oNode.Name == sName)
                {
                    bFlag = true;
                }
                else if (bRecursive)
                {
                    if (oNode.HasChildNodes)
                    {
                        oNode = GetNode(sName, oNode, true);

                        if (oNode != null)
                        {
                            bFlag = true;
                        }
                    }
                }

                iCount++;
            }

            if (!bFlag)
            {
                oNode = null;
            }

            return oNode;
        }

        private XmlNode FindNodeName(string sName, XmlNode oRoot)
        {
            bool bFlag = false;
            int iCount = 0;
            XmlNode oNode = null;

            while ((!bFlag) && (iCount < oRoot.ChildNodes.Count))
            {
                oNode = oRoot.ChildNodes.Item(iCount);

                if (oNode.Attributes["Name"] != null)
                {
                    if (oNode.Attributes["Name"].InnerText == sName)
                    {
                        bFlag = true;
                    }
                }

                iCount++;
            }

            if (!bFlag)
            {
                return null;
            }

            return oNode;
        }

        //Save the changed report     
        public Stream GetReport()
        {
            MemoryStream ms = new MemoryStream();
            // Create Xml      
            XmlTextWriter writer = new XmlTextWriter(ms,
            System.Text.Encoding.UTF8);

            oXmlDoc.Save(writer);

            ms.Position = 0;

            return ms;
        }

        public string GetReportStr()
        {
            return oXmlDoc.InnerXml;
        }

        private float GetNewWidth(float fWidth)
        {
            return (fWidth / (this.sizeReportOld.Width)) * (this.ReportSize.Width);
        }

        private float GetNewHeight(float fHeight)
        {
            return (fHeight / (this.sizeReportOld.Height)) * (this.ReportSize.Height);
        }

        public XmlNode GetReportHeader()
        {
            return GetNode("PageHeader", oXmlDoc.DocumentElement, true);
        }


        public XmlNode GetReportFooter()
        {
            return GetNode("PageFooter", oXmlDoc.DocumentElement, true);
        }

        public XmlNode GetReportImages()
        {
            return GetNode("EmbeddedImages", oXmlDoc.DocumentElement, true);
        }

        public void SetReportNode(string sName, XmlNode oSourceNode)
        {
            XmlNode oNode = GetNode(sName, oXmlDoc.DocumentElement, true);

            if (oNode != null)
            {
                oNode.RemoveAll();

                foreach (XmlNode oChild in oSourceNode.ChildNodes)
                {
                    oNode.AppendChild(oXmlDoc.ImportNode(oChild, true));
                }
            }
            else
            {
                oXmlDoc.DocumentElement.AppendChild(oXmlDoc.ImportNode(oSourceNode, true));
            }

        }


        public void SetTextBoxFont(string sFontFamily)
        {
            XmlNode oRoot = oXmlDoc.DocumentElement;
            XmlNode oReportNode = GetNode("ReportItems", oRoot, true);

            SetFont(sFontFamily, oRoot);


            XmlNode oPageFooter = GetNode("PageFooter", oRoot, true);

            if (oPageFooter != null)
            {
                oReportNode = GetNode("ReportItems", oPageFooter, true);

                //ReportItems in Footer      
                if (oReportNode != null)
                {

                    SetFont(sFontFamily, oReportNode);
                }
            }

            XmlNode oPageHeader = GetNode("PageHeader", oRoot, true);

            if (oPageHeader != null)
            {
                oReportNode = GetNode("ReportItems", oPageHeader, true);

                //ReportItems in Header      
                if (oReportNode != null)
                {

                    SetFont(sFontFamily, oReportNode);
                }
            }
        }

        private void SetFont(string sFontFamily, XmlNode oRoot)
        {
            int iCount = 0;
            XmlNode oNode = null;

            while ((iCount < oRoot.ChildNodes.Count))
            {
                oNode = oRoot.ChildNodes.Item(iCount);

                if (oNode.Name == "Textbox")
                {
                    SetStyle(oNode, sFontFamily);
                }
                else
                {
                    if ((oNode.Name == "List") || (oNode.Name == "ReportItems") || (oNode.Name == "Body"))
                    {
                        if (oNode.HasChildNodes)
                        {
                            SetFont(sFontFamily, oNode);
                        }
                    }
                }

                iCount += 1;
            }

        }

        private void SetStyle(XmlNode oItemNode, string sFontFamily)
        {
            XmlNode oNode = GetNode("Style", oItemNode);


            if (oNode == null)
            {

                //Make Style Node      

                XmlElement elemStyle = oXmlDoc.CreateElement("Style");

                oItemNode.AppendChild(elemStyle);


                oNode = GetNode("Style", oItemNode);
            }

            if (oNode != null)
            {
                XmlNode oFontNode = GetNode("FontFamily", oNode);

                if (oFontNode != null)
                {
                    if (oFontNode.InnerText != "Wingdings")
                    {
                        oFontNode.InnerText = sFontFamily;
                    }
                }
                else
                {

                    XmlElement elemFont = oXmlDoc.CreateElement("FontFamily", "http://schemas.microsoft.com/sqlserver/reporting/2005/01/reportdefinition");

                    elemFont.Attributes.RemoveAll();

                    elemFont.InnerText = sFontFamily;

                    oNode.AppendChild(elemFont);
                }
            }
        }
    }
}
