<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="HistoryExport.default" %>
<% Response.Expires = 60; %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="cache-control" content="no-cache" />
    <meta http-equiv="expires" content="-1" />
    <meta http-equiv="pragma" content="no-cache" />
    <title>Exportación de históricos</title>
    <style>
        body { }
        h1 { color: maroon; }
        .container { margin: 10px auto; text-align: center; width: 950px; }
        .diverror { color: red; text-align: left; }
        .divbuttons { margin: 10px 0; }
            .divbuttons .left { float: left; text-align: left; width: 49%; }
            .divbuttons .right { float: right; text-align: right; width: 50%; }
            .divbuttons div:nth-child(2) input { margin-left: 10px; }
        .gridcontainer { height: 600px; overflow-y: auto; }
            .gridcontainer table { width: 930px; }
                .gridcontainer table tr td:nth-child(3) { width: 250px; }
                .gridcontainer table tr td:nth-child(4) { width: 150px; }
        .txdescriptor { background-color: #ddd; border: none; width: 248px; }
        .txdevice { background-color: #ddd; border: none; width: 148px; }
        .clear { clear: both; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="container" class="container">
            <h1>Exportación de históricos </h1>
            <div class="diverror">
                <asp:Label ID="lbError" runat="server" />
            </div>
            <div class="divbuttons">
                <div class="left">
                    <asp:Button ID="btExport" runat="server" Text="Exportar" OnClick="btExport_Click" />
                    <asp:DropDownList ID="ddExport" runat="server" ></asp:DropDownList>
                </div>
                <div class="right">
                    <asp:Button ID="btRefreshFromEBI" runat="server" Text="Releer puntos de EBI" OnClick="btRefresh_Click" />
                    <asp:Button ID="btSave" runat="server" Text="Guardar" OnClick="btSave_Click" />
                </div>
                <div class="clear"></div>
            </div>
            <div id="gridcontainer" class="gridcontainer">
                <asp:GridView ID="grid" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None"
                    DataKeyNames="PointId" OnPreRender="grid_Prerender" OnRowDataBound="grid_RowDataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="PointName" HeaderText="Punto" />
                        <asp:BoundField DataField="ParamName" HeaderText="Parámetro" />
                        <asp:TemplateField HeaderText="Descripción">
                            <ItemTemplate>
                                <asp:TextBox ID="Descriptor" CssClass="txdescriptor" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dispositivo">
                            <ItemTemplate>
                                <asp:TextBox ID="Device" CssClass="txdevice" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Historia rápida" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="HistoryFastArch" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Historia estándard" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="HistorySlowArch" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Historia extendida" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:CheckBox ID="HistoryExtdArch" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </div>
        </div>
    </form>
    <script type="text/javascript">

        window.onresize = resize;
        window.setTimeout(resize, 1);

        function resize() {
            var h = document.documentElement.clientHeight;
            var ele = document.getElementById('gridcontainer');
            ele.style.height = (h - 180).toString() + "px";
        }

    </script>
</body>
</html>
