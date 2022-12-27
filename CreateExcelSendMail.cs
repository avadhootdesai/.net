using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;
using ClosedXML.Excel; //ClosedXML 
using System.Web;
using System.Net.Mail;
using System.Net.Mime;

/*IMPORTANT NOTE*/
//ClosedXML dependency needed   ---> installed using nuget package manager console
//query to run in nuget console ---> install-package ClosedXML -version 0.80.0
//install version supported by version of .net framework 


namespace EstimateTriggerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
             //creating object of the connection string defined in 'app.config'
            OleDbConnection conn = new OleDbConnection(ConfigurationManager.AppSettings["ConnectionString"].ToString());

            //sql query
            string query = @"  SELECT b.*, a.estimate_value ,concat(round(((b.currentbillamount / nullif(a.estimate_value,0))*100-100),2) , '%') percentage
                                  FROM (SELECT estimateid,
                                                 total_theatre_charges
                                               + equipment_instrumant_charges
                                               + pharmacy_charges
                                               + investigation_charges
                                               + doctor_fees
                                               + medical_supply_charges
                                               + surgical_implant_charges
                                               + registration_charges
                                               + totalcharges
                                               + physiotheraphy_charges
                                               + implant_charges AS estimate_value
                                          FROM costestimation_detail) a,
                                      (SELECT  c.COSTESTIMATIONNO,c.ESTIMATEID, TRUNC (a.adminsiondate) as Admission_date,
                 a.admissionno, b.mrno, a.patientname, a.bedno, a.consultant,
                 a.surgery, a.currentbillamount
           FROM bch_admission_discharge_view a, costestimation_header b,costestimation c,patientadmission d
           WHERE a.patientstatus IN ('Under IP Care', 'Discharge Intimated') and d.ADMISSIONID=a.ADMISSIONID 
           and d.ADMISSIONREQUEST= c.ADM_REQ_ID 
           and b.ESTIMATEID = c.ESTIMATEID
           GROUP BY a.adminsiondate,
                 a.admissionno,
                 b.estimateid,
                 a.patientname,
                 a.bedno,
                 a.consultant,
                 a.surgery,
                 a.currentbillamount,
                 b.mrno ,c.ESTIMATEID,c.COSTESTIMATIONNO
                 ) b
                                 WHERE a.estimateid = b.estimateid";
            //run query 
            OleDbCommand cmd = new OleDbCommand(query, conn);
            cmd.CommandType = CommandType.Text;
            OleDbDataAdapter ord = new OleDbDataAdapter(cmd);

            //create a (virtual) table named 'dt' 
            DataTable dt = new DataTable();
            //fill query result into the table 'dt'
            ord.Fill(dt);

            conn.Close(); //closing connection

            //file path to export excel
            string folderPath = "E://Avadhoot//EstimateTriggerConsole//EstimateTriggerConsole//EstimateExcel//"; //file is replaced everytime code runs

            XLWorkbook wb = new XLWorkbook();
            //bind data from table 'dt' into excel
            wb.Worksheets.Add(dt, "estimateReport");
            //save the excel file to defined path 'folderPath'
            wb.SaveAs(folderPath + "ESTIMATE_REPORT.xlsx");

            //calling sendMail function defined below
            sendMail();
        }
         public static void sendMail()
        {

            //getting current datetime as 'sysDate'
            string sysDate = System.DateTime.Now.ToString("dd-MM-yyyy HH:MM:ss");
          
            MailMessage emailObj = new MailMessage();
            //sender email id
            emailObj.From = new MailAddress("no_reply@breachcandyhospital.org");
            //recievers email ids 
            string recieverAddressMulti = "drkiransharma@breachcandyhospital.org, billing@breachcandyhospital.org"; //sameer.powar@breachcandyhospital.org, drkiransharma@breachcandyhospital.org, billing@breachcandyhospital.org
            //splitting above string 'recieverAddressMulti' into sub-strings 'recieverAddress' to seperate each email
            foreach (string recieverAddress in recieverAddressMulti.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                emailObj.To.Add(recieverAddress);
            }
            //mail BCC
            emailObj.Bcc.Add("itsoft@breachcandyhospital.org");
            emailObj.Subject = "ESTIMATE REPORT- " + sysDate;           
            emailObj.Body = @"Please Find Attached Estimate Report.

Breach Candy Hospital Trust 
Mumbai 400 026
Tel: 022 - 23667558";

            //defining esxcel file name folder path for email attachment 
            string pathForMailAttachment = @"E:\Avadhoot\EstimateTriggerConsole\EstimateTriggerConsole\EstimateExcel\ESTIMATE_REPORT.xlsx";
            Attachment excelFile = new Attachment(pathForMailAttachment, MediaTypeNames.Application.Octet);
            emailObj.Attachments.Add(excelFile);

            //server related functions
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true; //important
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false; //important
            /*sender email id and password defined. if changes are made here for sender mail address, 
              the same should also be changed in function 'emailObj.From' defined in the code above.*/
            smtp.Credentials = new System.Net.NetworkCredential("no_reply@breachcandyhospital.org", "N0R3p1!@2022"); //automailer@breachcandyhospital.org - mumbaicentral,  no_reply@breachcandyhospital.org - N0R3p1!@2022
            smtp.Send(emailObj);
           
        }
    }
}




                           
