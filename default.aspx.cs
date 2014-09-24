using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "Textbox contents: " + TextBox1.Text;
        Label2.Text = "Checkbox status: " + CheckBox1.Checked;
        Image1.Visible = false;
    }
    protected void TextBox1_TextChanged(object sender, EventArgs e)
    {
        Label1.Text = "Textbox contents: " + TextBox1.Text;
        if (CheckBox1.Checked == true && TextBox1.Text.ToLower() == "squid")
        {
            Image1.Visible = true;
        }
        else
        {
            Image1.Visible = false;
        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        Label2.Text = "Checkbox status: " + CheckBox1.Checked;
        if (CheckBox1.Checked == true && TextBox1.Text.ToLower() == "squid")
        {
            Image1.Visible = true;
        }
        else
        {
            Image1.Visible = false;
        }
    }
}