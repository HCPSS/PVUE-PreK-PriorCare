using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.Net;
using System;
using System.Web.Configuration;
//using Revelation.Cryptography;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;


public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string token = null;
            string studentID = null;

            try
            {
                token = Request.Params["token"];
            }
            catch { }

            try
            {
                studentID = Request.Params["id"];
            }
            catch { }

            if (Request.Url.DnsSafeHost == "localhost")
            {
                //LocalHost is always going to use 
                //token = "Tzd3Q1N0SGdURklOeU9RcmxVaHNLakNzWUNydG83T0NxcDYzbnFMT0RpR0diRVZrVDdpY0tVamQxVWVMUGFCdnhkVVhwMnUzbkd3UlhLTStMY2hZOWh5UFNNWEw1QTUwbzdoUHZ2ZHF2a2VQY0tZUXA0dldXc1RTRnlMZ1dtaFM1";
                //studentID = "151042680";

                token = "b3pYUXhXMG5PcDY3dzVoVW1zeWZ4ekIrUzIzWlJIWFVkYlJHSWZ3RVVmT05iWEJvWHduTTJ2MHdRblhSYzVqL25za3BXd0RnNThFVk4vTUVjSEs3YkJVTXdETUFDMk5yK0RpL09Bb3E0U1R5UXpWUS9SMmxXZ3V3dEpMTlIydFY1";
                studentID= "151052156";
            }


            if (!string.IsNullOrWhiteSpace(token))
                Session["Token"] = token;
            else
                token = Session["Token"].ToString();

            if (!string.IsNullOrWhiteSpace(studentID))
                Session["StudentID"] = studentID;
            else
                studentID = Session["StudentID"].ToString();




            Guid parentGU = Guid.Empty;
            Guid studentGU = Guid.Empty;


            DBHelper helper = new DBHelper();
            
            string commandString = "Exec HCP_PVUE_PRIORCARE_STUDINFO @sis_id= '" + studentID + "'";
            DataSet stuInfo = helper.ExecuteQueryReturnDataSet(commandString);

            if(stuInfo.Tables.Count > 0)
            {
                DataTable si = stuInfo.Tables[0];
                studentGU = Guid.Parse(si.Rows[0]["STUDENT_GU"].ToString());
                lblStudentName.Text = si.Rows[0]["FullName"].ToString();
                lblCurrentSchool.Text = si.Rows[0]["currentschool"].ToString();
                lblGrade.Text = si.Rows[0]["grade"].ToString();
                lblDOB.Text = si.Rows[0]["DOB"].ToString();
                lblsisNumber.Text = si.Rows[0]["sis_number"].ToString();
            }

            DataSet dsPC = helper.ExecuteQueryReturnDataSet("select value_code, value_description from rev.SIF_22_Common_GetLookupValues('HCP', 'PRIOR_CARE')");
            if (dsPC.Tables.Count > 0)
            {
                DDLPriorCare.DataSource = dsPC.Tables[0];
                DDLPriorCare.DataBind();
            }

            DataSet dsTS = helper.ExecuteQueryReturnDataSet("select value_code, value_description from rev.SIF_22_Common_GetLookupValues('HCP', 'KRA_TIME_SPENT') ");
            if (dsTS.Tables.Count > 0)
            {
                DDLTimeSpent.DataSource = dsTS.Tables[0];
                DDLTimeSpent.DataBind();
            }
            if (TokenHelper.AuthenticateToken(token, out parentGU))
            {
                populateGridandScreen();
            }
            else
            {
                Response.Write("Invalid token!");
                Response.End();
            }
        }
        
    }//Page Load

    
    protected void DisplayCorrectGraphic(DataTable ds)
    {
        int days = 0;

        foreach (GridViewRow row in gvPriorCare.Rows)
        {
            HiddenField f1 = (HiddenField)row.FindControl("HDNTimeSpentCode");
            if (f1 != null && f1.Value != "")
                days += Convert.ToInt32(f1.Value.ToString());
        }
        switch(days)
        {
            case 0:
                FullDayImage.ImageUrl = "Images/noentry.png";
                btnInsert.Visible = true;
                btnInsert.Enabled = true;
                LBLUserMessage.Text = "Please ensure you have 1 (ONE) FULL DAY of prior care coverage.";
                gvPriorCare.Visible = false;
                gvPriorCare.Enabled = false;
                break;
            case 1: 
                FullDayImage.ImageUrl = "Images/noentry.png";
                btnInsert.Visible = true;
                btnInsert.Enabled = true;
                LBLUserMessage.Text = "Please ensure you have 1 (ONE) FULL DAY of prior care coverage.";
                gvPriorCare.Visible = true;
                gvPriorCare.Enabled = true;
                break;
            case 2: 
                FullDayImage.ImageUrl = "Images/checkmark.png";
                btnInsert.Visible = false;
                btnInsert.Enabled = false;
                LBLUserMessage.Text = "No prior care action needed.";
                gvPriorCare.Visible = true;
                gvPriorCare.Enabled = true;
                break;
            default:
                FullDayImage.ImageUrl = "Images/noentry.png";
                btnInsert.Visible = true;
                btnInsert.Enabled = true;
                LBLUserMessage.Text = "Please ensure you have 1 (ONE) FULL DAY of prior care coverage.";
                gvPriorCare.Visible = true;
                gvPriorCare.Enabled = true;
                break;
        }
    }

    protected void btnInsert_Click1(object sender, EventArgs e)
    {
        btnInsert_Click1();
    }
    protected void btnInsert_Click1()
    { 
        DDLPriorCare.Visible = true;
        DDLTimeSpent.Visible = true;
        DDLPriorCare.Enabled = true;
        DDLTimeSpent.Enabled = true;
        gvPriorCare.Enabled = false;
        gvPriorCare.Visible = false;
        btnInsert.Visible = false;
        btnSave.Visible = true;
        btnDiscard.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DDLPriorCare.Visible = false;
        DDLTimeSpent.Visible = false;
        DDLPriorCare.Enabled = false;
        DDLTimeSpent.Enabled = false;
        gvPriorCare.Enabled = true;
        gvPriorCare.Visible = true;
        btnInsert.Visible = true;
        btnSave.Visible = false;
        btnDiscard.Visible = false;
        GetInfo gi = new GetInfo();

        
        gi.InsertPriorCareRow(DDLPriorCare.SelectedValue, DDLTimeSpent.SelectedValue, lblsisNumber.Text);
        populateGridandScreen();
    }

    protected void btnDiscard_Click(object sender, EventArgs e)
    {
        DDLPriorCare.Visible = false;
        DDLTimeSpent.Visible = false;
        DDLPriorCare.Enabled = false;
        DDLTimeSpent.Enabled = false;
        gvPriorCare.Enabled = true;
        gvPriorCare.Visible = true;
        btnInsert.Visible = true;
        btnSave.Visible = false;
        btnDiscard.Visible = false;
    }

    protected void RowEditing(object sender, EventArgs e)
    {
        gvPriorCare.Visible = false;
        if (DDLPriorCare.SelectedIndex  != 0 )
        btnInsert_Click1();
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    protected void gvPriorCare_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GetInfo gi = new GetInfo();
        //Pop-up - ask if the row should be deleted.
        //button.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
        
        // First get the rowguid to delete
        string guid4CurrentRow = gvPriorCare.DataKeys[0]["rowguid"].ToString();

        gi.DeletePriorCareRow(guid4CurrentRow);
        populateGridandScreen();

        //int index = Convert.ToInt32(e.RowIndex);
        //DataTable dt = ViewState["dt"] as DataTable;
        //dt.Rows[index].Delete();
        //ViewState["dt"] = dt;


        //gvPriorCare.DeleteRow(e.RowIndex);
        //gvPriorCare.DataBind(); 
    }
    protected void DDLPriorCare_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl1 = (DropDownList)sender;
        if (ddl1.SelectedIndex != 0)
        {
            try
            {
                String value = ddl1.SelectedValue;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }

    protected void DDLTimeSpent_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddl1 = (DropDownList)sender;
        if (ddl1.SelectedIndex != 0)
        {
            try
            {
                String value = ddl1.SelectedValue;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

        }
    }

    protected void populateGridandScreen()
    {
 
            GetInfo gi = new GetInfo();
            DataSet ds = gi.PopulatePriorCareData(Session["StudentID"].ToString());
            if (ds.Tables[0].Rows.Count > 0) // Lookup
            {
                gvPriorCare.DataSource = ds.Tables[0];
                gvPriorCare.DataBind();
                gvPriorCare.Visible = true;
                gvPriorCare.Enabled = true;
                btnInsert.Visible = false;
                Session["StudentInfo"] = ds;
            }
            else
            {
                btnInsert.Visible = true;
                Session["StudentInfo"] = null;
            }
            DisplayCorrectGraphic(ds.Tables[0]);
    }

    //protected void ButtonDelete_Click(object sender, EventArgs e)
    //{
    //    GetInfo gi = new GetInfo();

    //    String guid4CurrentRow = (String) gvPriorCare.DataKeys[0]["rowguid"];
    //    gi.DeletePriorCareRow(guid4CurrentRow);

    //    Button b1 = (Button)sender;
    //    GridViewRow gvr = (GridViewRow)b1.NamingContainer;

    //    gvPriorCare.DeleteRow(e.RowIndex);
    //    gvPriorCare.DataBind();
    //}
}