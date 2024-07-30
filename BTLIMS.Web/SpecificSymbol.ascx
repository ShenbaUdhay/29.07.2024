<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecificSymbol.ascx.cs" Inherits="BTLIMS.Web.SpecificSymbol" %>
<!DOCTYPE>
<html xmlns="http://www.w3.org/1999/xhtml" lang="en">
<head>
<title>特殊符号</title>
<!---------------------------------- 页面基本设置禁止随意更改 ------------------------------------------>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="keywords" content="index" />
<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
<meta name="renderer" content="webkit" />
<meta http-equiv="Cache-Control" content="no-siteapp" />
<!---------------------------------- 页面基本设置禁止随意更改 -------------------------------------------->
<style type="text/css">
body{
	margin:0;
	}
.show-box-list{
	
	}
.show-box-list tr td{
	padding:10px;
	}
.show-box-list tr td .input{
	border: 1px solid #C5C5C5;
    width: 100%;
    box-sizing: border-box;
    padding: 6px;
    font-size: 14px;
    color: #4A4A4A;
	}
</style>
</head>
<body>
  <table cellpadding="0" cellspacing="0" width="100%" class="show-box-list">
   <tbody>
    <tr>
      <td><input type="text" class="input" value="⁰" readonly="true" /></td>
      <td><input type="text" class="input" value="¹" readonly="true" /></td>
      <td><input type="text" class="input" value="²" readonly="true" /></td>
      <td><input type="text" class="input" value="³" readonly="true" /></td>
      <td><input type="text" class="input" value="⁴" readonly="true" /></td>
      <td><input type="text" class="input" value="⁵" readonly="true" /></td>
      <td><input type="text" class="input" value="⁶" readonly="true" /></td>
      <td><input type="text" class="input" value="⁷" readonly="true" /></td>
      <td><input type="text" class="input" value="⁸" readonly="true" /></td>
      <td><input type="text" class="input" value="⁹" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="⁺" readonly="true" /></td>
      <td><input type="text" class="input" value="⁻" readonly="true" /></td>
      <td><input type="text" class="input" value="⁼" readonly="true" /></td>
      <td><input type="text" class="input" value="ⁿ" readonly="true" /></td>
      <td><input type="text" class="input" value="₀" readonly="true" /></td>
      <td><input type="text" class="input" value="₁" readonly="true" /></td>
      <td><input type="text" class="input" value="₂" readonly="true" /></td>
      <td><input type="text" class="input" value="₃" readonly="true" /></td>
      <td><input type="text" class="input" value="₄" readonly="true" /></td>
      <td><input type="text" class="input" value="₅" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="₆" readonly="true" /></td>
      <td><input type="text" class="input" value="₇" readonly="true" /></td>
      <td><input type="text" class="input" value="₈" readonly="true" /></td>
      <td><input type="text" class="input" value="₉" readonly="true" /></td>
      <td><input type="text" class="input" value="₊" readonly="true" /></td>
      <td><input type="text" class="input" value="₋" readonly="true" /></td>
      <td><input type="text" class="input" value="₌" readonly="true" /></td>
      <td><input type="text" class="input" value="′" readonly="true" /></td>
      <td><input type="text" class="input" value="″" readonly="true" /></td>
      <td><input type="text" class="input" value="℃" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="℉" readonly="true" /></td>
      <td><input type="text" class="input" value="Ω" readonly="true" /></td>
      <td><input type="text" class="input" value="φ" readonly="true" /></td>
      <td><input type="text" class="input" value="Ø" readonly="true" /></td>
      <td><input type="text" class="input" value="π" readonly="true" /></td>
      <td><input type="text" class="input" value="℉" readonly="true" /></td>
      <td><input type="text" class="input" value="※" readonly="true" /></td>
      <td><input type="text" class="input" value="±" readonly="true" /></td>
      <td><input type="text" class="input" value="×" readonly="true" /></td>
      <td><input type="text" class="input" value="≤" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="≥" readonly="true" /></td>
      <td><input type="text" class="input" value=">" readonly="true" /></td>
      <td><input type="text" class="input" value="<" readonly="true" /></td>
      <td><input type="text" class="input" value="△" readonly="true" /></td>
      <td><input type="text" class="input" value="‰" readonly="true" /></td>
      <td><input type="text" class="input" value="r/min" readonly="true" /></td>
      <td><input type="text" class="input" value="N•m" readonly="true" /></td>
      <td><input type="text" class="input" value="g•m" readonly="true" /></td>
      <td><input type="text" class="input" value="%H0" readonly="true" /></td>
      <td><input type="text" class="input" value="%" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="MPa" readonly="true" /></td>
      <td><input type="text" class="input" value="m" readonly="true" /></td>
      <td><input type="text" class="input" value="cm" readonly="true" /></td>
      <td><input type="text" class="input" value="mL" readonly="true" /></td>
      <td><input type="text" class="input" value="g" readonly="true" /></td>
      <td><input type="text" class="input" value="kg" readonly="true" /></td>
      <td><input type="text" class="input" value="L" readonly="true" /></td>
      <td><input type="text" class="input" value="μ" readonly="true" /></td>
      <td><input type="text" class="input" value="μg" readonly="true" /></td>
      <td><input type="text" class="input" value="/" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="M₋₁" readonly="true" /></td>
      <td><input type="text" class="input" value="M₁" readonly="true" /></td>
      <td><input type="text" class="input" value="M₂" readonly="true" /></td>
      <td><input type="text" class="input" value="M₃" readonly="true" /></td>
      <td><input type="text" class="input" value="M₄" readonly="true" /></td>
      <td><input type="text" class="input" value="F₁" readonly="true" /></td>
      <td><input type="text" class="input" value="F₂" readonly="true" /></td>
      <td><input type="text" class="input" value="F₃" readonly="true" /></td>
      <td><input type="text" class="input" value="F₄" readonly="true" /></td>
      <td><input type="text" class="input" value="E₂" readonly="true" /></td>
    </tr>
    <tr>
      <td><input type="text" class="input" value="U₉₅" readonly="true" /></td>
      <td><input type="text" class="input" value="10¹" readonly="true" /></td>
      <td><input type="text" class="input" value="10⁻¹" readonly="true" /></td>
      <td><input type="text" class="input" value="mL" readonly="true" /></td>
      <td><input type="text" class="input" value="g" readonly="true" /></td>
      <td><input type="text" class="input" value="kg" readonly="true" /></td>
      <td><input type="text" class="input" value="L" readonly="true" /></td>
      <td><input type="text" class="input" value="μ" readonly="true" /></td>
      <td><input type="text" class="input" value="μg" readonly="true" /></td>
      <td><input type="text" class="input" value="/" readonly="true" /></td>
    </tr>
    </tbody>
  </table>
</body>
</html>
