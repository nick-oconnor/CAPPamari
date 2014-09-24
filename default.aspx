<%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Form</title>
    <link rel="icon" href="favicon.ico"/>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:TextBox ID="TextBox1" runat="server" OnTextChanged="TextBox1_TextChanged"></asp:TextBox>
        <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" />
        <br />
        <input id="Submit1" type="submit" value="Submit" /><hr />
    
    </div>
        <div>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
            <br />
            <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        </div>
        <p>
            <input id="Checkbox2" type="checkbox" /></p>
        <p>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/favicon.ico" />
        </p>
    </form>
</body>
</html>
