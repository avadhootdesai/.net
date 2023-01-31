using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Net.Mime;
using System.IO; 

namespace testEmail
{
    class Program
    {
        static void Main(string[] args)
        {
            //file paths where email ID text files are metioned 
            //in text files write email ids in new line
            string emailRecieverFile = @"E:\Avadhoot\testEmail\testEmail\EmailIdFiles\recieverID.txt";
            string emailCcFilee = @"E:\Avadhoot\testEmail\testEmail\EmailIdFiles\ccID.txt";
            string emailBccFile = @"E:\Avadhoot\testEmail\testEmail\EmailIdFiles\bccID.txt";
            
            //adding email ids in array string
            string[] emailReciever = File.ReadAllLines(emailRecieverFile); ;
            string[] emailCC = File.ReadAllLines(emailCcFilee);
            string[] emailBCC = File.ReadAllLines(emailBccFile);
            sendEmail(emailReciever, emailCC, emailBCC);
        }

        protected static void sendEmail(string[] recieverList, string[] ccList, string[] bccList) 
        {
            //getting current datetime as 'sysDate'
            string sysDate = System.DateTime.Now.ToString("dd-MM-yyyy");

            MailMessage emailObj = new MailMessage();
            //sender email id
            emailObj.From = new MailAddress("avadhoot.desai@breachcandyhospital.org");

            //mail CC
            foreach (string cc in ccList)
            {
            MailAddress addcc = new MailAddress(cc);
            emailObj.CC.Add(addcc);
            }

            //recievers email ids -- multiple can be added using comma (,)
            foreach (string reciever in recieverList)
            {
                emailObj.To.Add(reciever);
            }

            //mail BCC
            foreach (string bcc in bccList)
            {
                emailObj.To.Add(bcc);
            }

            emailObj.Subject = "TEST FILE BASED EMAIL- " + sysDate;
            emailObj.Body ="TEST EMAIL";

            //defining esxcel file name folder path for email attachment 
            //string pathForMailAttachment = @"";
            //Attachment excelFile = new Attachment(pathForMailAttachment, MediaTypeNames.Application.Octet);
            //emailObj.Attachments.Add(excelFile);

            //server related functions
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true; //important
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false; //important
            /*sender email id and password defined. if changes are made here for sender mail address, 
              the same should also be changed in function 'emailObj.From' defined in the code above.*/
            smtp.Credentials = new System.Net.NetworkCredential("avadhoot.desai@breachcandyhospital.org", "Desa!a023"); 
            smtp.Send(emailObj);       
        }
    }
}
