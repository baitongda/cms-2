<%@ Page Language="C#" Inherits="SiteServer.BackgroundPages.WeiXin.PageCard" %>
<%@ Register TagPrefix="bairong" Namespace="SiteServer.BackgroundPages.Controls" Assembly="SiteServer.BackgroundPages" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="utf-8">
<!--#include file="../inc/header.aspx"-->
</head>

<body>
<!--#include file="../inc/openWindow.html"-->
<form class="form-inline" runat="server">
  <asp:Literal id="LtlBreadCrumb" runat="server" />
  <bairong:alerts runat="server" />

  <script type="text/javascript">
  $(document).ready(function()
  {
    loopRows(document.getElementById('contents'), function(cur){ cur.onclick = chkSelect; });
    $(".popover-hover").popover({trigger:'hover',html:true});
  });
  </script>

  <table id="contents" class="table table-bordered table-hover">
    <tr class="info thead">
      <td width="20"></td>
      <td>主题</td>
      <td>会员卡</td>
      <td>关键词</td>
      <td>总浏览量</td>
      <td>是否启用</td>
      <td width="100"></td>
      <td width="100"></td>
      <td width="100"></td>
      <td width="100"></td>
      <td width="20"><input type="checkbox" onClick="selectRows(document.getElementById('contents'), this.checked);" /></td>
    </tr>
    <asp:Repeater ID="RptContents" runat="server">
      <itemtemplate>
        <tr>
          <td class="center">
            <asp:Literal ID="LtlItemIndex" runat="server"></asp:Literal>
          </td>
          <td>
            <asp:Literal ID="LtlTitle" runat="server"></asp:Literal>
          </td>
            <td>
            <asp:Literal ID="LtlCardTitle" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="LtlKeywords" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="LtlPVCount" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="LtlIsEnabled" runat="server"></asp:Literal>
          </td>
          <td class="center">
             <asp:Literal ID="LtlUserUrl" runat="server"></asp:Literal>
          </td>
           <td class="center">
            <asp:Literal ID="LtlPreviewUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <asp:Literal ID="LtlEditUrl" runat="server"></asp:Literal>
          </td>
          <td class="center">
             <asp:Literal ID="LtlOperator" runat="server"></asp:Literal>
          </td>
          <td class="center">
            <input type="checkbox" name="IDCollection" value='<%#DataBinder.Eval(Container.DataItem, "ID")%>' />
          </td>
        </tr>
      </itemtemplate>
    </asp:Repeater>
  </table>

  <bairong:sqlPager id="SpContents" runat="server" class="table table-pager" />

  <ul class="breadcrumb breadcrumb-button">
    <asp:Button class="btn btn-success" id="BtnAdd" Text="添 加" runat="server" />
    <asp:Button class="btn" id="BtnDelete" Text="删 除" runat="server" />
  </ul>

</form>
</body>
</html>