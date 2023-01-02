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
using System.Drawing;

public partial class draft : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void getReport_Click(object sender, EventArgs e)
    {
        alert.Visible = false;
        string admission_no = ad_no.Text.ToString();

        if (admission_no == "") {

            alert.Text = "Please Enter Admission No.";
            alert.Visible = true;
        }

        try
        {
            OleDbConnection con = new OleDbConnection(ConfigurationManager.ConnectionStrings["ConnectionString2"].ConnectionString);
            string query = @"SELECT MRNO,ADMISSIONNO,PATIENTNAME,FINANCEGROUP,SUM(DECODE(sc.CANCELLED_BY,NULL,PATIENT_NET_AMT,-PATIENT_NET_AMT)) Service
                            FROM PATIENT_CHARGE_ADMITTED_VW pc,serviceorder sc 
                            WHERE ADMISSIONNO='{0}'
                            and serviceorder_number = sc.DOCNUM
                            and sc.service_name = pc.servicename
                            and sc.ORDER_STATUS <> 662
                            group by MRNO,ADMISSIONNO,PATIENTNAME,FINANCEGROUP
                            union all
                            SELECT MRN_NO,ADMISSIONNO,PATIENT_NAME,'Surgical / Drug / Consumables', SUM(PATIENT_NET_AMOUNT) Drug FROM XXICT_PAT_DRUG_ORDER_SUMMARY 
                            WHERE ADMISSIONNO='{0}' 
                            group by MRN_NO,ADMISSIONNO,PATIENT_NAME
                            order by financegroup";

            //bind date inputs with above query 
            object[] conditions = new object[] { admission_no };
            string datequery = string.Format(query, conditions);

            //execute new query 
            OleDbCommand command = new OleDbCommand(datequery, con);
            con.Open();
            DataSet gridTable = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter(command);

            //fill dataset 'gridTable' with query results
            da.Fill(gridTable);
            //set gridview 'invTable' equal to dataset 'gridTable' 
            draftReportTable.DataSource = gridTable;

            draftReportTable.Columns[3].FooterText = "TOTAL";
            double totalSalary = 0;
            foreach (DataRow dr in gridTable.Tables[0].Rows)
            {
                totalSalary += Convert.ToDouble(dr["SERVICE"]);
            }
            draftReportTable.Columns[4].FooterText = totalSalary.ToString();
            draftReportTable.FooterStyle.ForeColor = System.Drawing.Color.Red;

            draftReportTable.DataBind();
            //making grid visible as it is hidden on page-load
            draftReportTable.Visible = true;
            con.Close();
        }
        catch {
            alert.Text = "No Data Found! Check Admission No.";
            alert.Visible = true;
        }

    }
    protected void exportReport_Click(object sender, EventArgs e)
    {
        try
        {

            int rowCount = draftReportTable.Rows.Count;
            if (rowCount != 0)
            {//if gridview is not empty - export to excel
                Response.Clear();
                //file name for exported excel
                Response.AddHeader("content-disposition", "attachment; filename=Unbilled_Amount_Report.xls");
                Response.ContentType = "application/vnd.xls";

                System.IO.StringWriter stringWrite = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
                draftReportTable.RenderControl(htmlWrite);
                Response.Write(stringWrite.ToString());
                Response.Flush();
                Response.End();
            }
            else
            {//if gridview is empty - show alert message
               
                alert.Visible = true;
                alert.Text = "Please generate report first!";
            }
        }
        //if excel generation failed throw below error
        catch
        {
            
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