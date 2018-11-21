<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SimpleDefault.aspx.cs" Inherits="demosite._Default" %>
<%@ Register Assembly="SyrinxMenuV3" Namespace="Syrinx2" TagPrefix="syx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
	<script type="text/javascript" src="js/jquery-1.4.2.min.js"></script>
    <style type="text/css">
		.TestMMenu
		{
		background-color:Black;
		border-top:solid 1px gray;
		border-bottom:solid 1px gray;
		}

		.TestMMenuItem a {
			padding: 10px 12px 10px 12px;
			font-size:20px;
			text-decoration:none;
			color:White;
		}
		.TestMMenuItem a:hover {
			padding-bottom: 6px;
			border-bottom:solid 4px #F3B900;
			color:#F3B900;
		}
		.TestMSubMenu
		{
			background-color:black;
			padding:5px 0px 5px 0px;
		}
		.TestMSubMenuItem a{
			color:white;
			padding: 3px 10px 3px 10px;
			text-decoration:none;
			font-size:14px;
			font-weight:bold;
		}
		.TestMSubMenuItem a:hover{background-color:#F3B900;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
	<syx:Menu runat="server" id="Menu1"  Orientation="Horizontal" MenuShowEffect="fade"
	CssClass="TestMMenu" ItemCssClass="TestMMenuItem"
	SubMenuCssClass="TestMSubMenu"	SubMenuItemCssClass="TestMSubMenuItem"
		ExternalLinkDefaultTarget="_blank">
		<syx:MenuItem Text="CNN" NavigateUrl="http://cnn.com" />
		<syx:MenuItem Text="Fox News" NavigateUrl="http://foxnews.com" />
		<syx:MenuItem Text="My Work" >
			<syx:MenuItem Text="Syrinx, Inc" NavigateUrl="http://syrinx.ph" />
			<syx:MenuItem Text="CIS" NavigateUrl="http://www.cis.edu.ph">
				<syx:MenuItem Text="Airsoft Site" NavigateUrl="http://bizmall.ph/Wilkie" />
				<syx:MenuItem Text="Life Sculpting" NavigateUrl="http://bizmall.ph/LifeS" />
			</syx:MenuItem>
			<syx:MenuItem Text="BizMall" NavigateUrl="http://bizmall.ph">
				<syx:MenuItem Text="Airsoft Site" NavigateUrl="http://bizmall.ph/Wilkie" />
				<syx:MenuItem Text="Life Sculpting" NavigateUrl="http://bizmall.ph/LifeS" />
			</syx:MenuItem>
		</syx:MenuItem>
		<syx:MenuItem Text="Drudge Report" NavigateUrl="http://drudgeReport.com" />
	</syx:Menu>		
    
    </div>
    </form>
</body>
</html>
