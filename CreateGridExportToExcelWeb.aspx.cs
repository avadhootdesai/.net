using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using System.Globalization;

public partial class generateReport : System.Web.UI.Page
{

    protected void Button1_Click(object sender, EventArgs e)
    {
        //get date value from html textbox as string
        string dt1 = fromDate.Text;
        string dt2 = toDate.Text;

        //check if user has selected dates 
        if (dt1 == "" || dt2 == "")
        {
            alert2.Visible = false;
            alert.Visible = true;
            alert.Text = "Please Select Valid Date.";
        }        
        else 
        {

            //convert string dates into datetime for validation
            DateTime dateTime_dt1 = DateTime.ParseExact(dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateTime_dt2 = DateTime.ParseExact(dt2, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            double totalDays = (dateTime_dt2 - dateTime_dt1).TotalDays;

            if (totalDays <= 31.0 && totalDays >= 0.0) //validating dates
            {
                alert2.Visible = false;
                alert.Visible = false; //hiding alert label everytime page or gridview is loaded  

                OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
                string query = @"SELECT DISTINCT mrn_no uhid, TRUNC (C.ADMINSIONDATE) admissiondate,
                   a.admissionno admissionno, age,
                   DECODE (ageunit, 1, 'Y', 2, 'M', 3, 'D') age_type, gender,
                   a.bedno current_location, c.ward,
                   CASE
                      WHEN SUBSTR (a.bedno, 4, 1) = 'U'
                         THEN 'ICU'
                      WHEN SUBSTR (a.bedno, 3, 1) = 'U'
                         THEN 'ICU'
                      WHEN a.admissionno = ' '
                         THEN 'OPD'
                      ELSE 'WARD'
                   END AS ""CARE_TYPE"",
                   TRUNC (order_confirmed_date) prescription_date,
                   NAME itemname, genericname antibiotic, issued_qty qty,
                   consultant ordering_doctor,
                   departmentname ordering_doctor_speciality,
                   departmentname orddepartment, NULL dateofdischarge
              FROM care.hinai_par_zevac a,
                   care.antibiotics b,bch_admission_discharge_view@""utkarsh_new"" C
             WHERE a.code = b.itemcode
               AND a.admissionno = c.ADMISSIONNO
               AND TRUNC (order_confirmed_date)>= TO_DATE('{0}', 'DD/MM/YY')
               AND TRUNC (order_confirmed_date)<= TO_DATE('{1}', 'DD/MM/YY')
               AND a.admissionno IS NOT NULL
               AND issued_qty <> 0
          ORDER BY a.admissionno ASC";

                //bind date inputs with above query 
                object[] conditions = new object[] { dt1, dt2 };
                string datequery = string.Format(query, conditions);

                //execute new query 
                OleDbCommand command = new OleDbCommand(datequery, con);
                con.Open();
                DataSet gridTable = new DataSet();
                OleDbDataAdapter da = new OleDbDataAdapter(command);

                //fill dataset 'gridTable' with query results
                da.Fill(gridTable, "care");
                //set gridview 'invTable' equal to dataset 'gridTable' 
                antiReportTable.DataSource = gridTable;
                antiReportTable.DataBind();
                //making grid visible as it is hidden on page-load
                antiReportTable.Visible = true;
                con.Close();
            }
            else
            {
                alert.Visible = true;
                alert2.Visible = true;
                alert.Text = "Please Select Valid Date.";
                alert2.Text = "(Note: Total Should Not Exceed 31 days.)";
            }
        }
    }

    //on buttonclick export gridview to excel
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {

            int rowCount = antiReportTable.Rows.Count;
            if (rowCount != 0)
            {//if gridview is not empty - export to excel
                Response.Clear();
                //file name for exported excel
                Response.AddHeader("content-disposition", "attachment; filename=Antibiotics_Prescription_Report.xls");
                Response.ContentType = "application/vnd.xls";

                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                antiReportTable.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {//if gridview is empty - show alert message
                alert2.Visible = false;
                alert.Visible = true;
                alert.Text = "Please generate report first!";
            }
        }
        //if excel generation failed throw below error
        catch
        {
            alert2.Visible = false;
            alert.Visible = true;
            alert.Text = "Export Failed! Contact IT Department.";
        }
    }

    public override void VerifyRenderingInServerForm(Control control) //important
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

}
