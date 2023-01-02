<%@ Page Language="C#" AutoEventWireup="true" CodeFile="draft.aspx.cs" Inherits="draft" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

     <link rel="stylesheet" type="text/css" href="css/style.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" 
    integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous" />
    <title>Anti-Report BCHT</title>
    <link rel = "icon" href = "css/bch_title_logo.png" type = "image/x-icon" />

</head>

<body>

    <form id="form1" runat="server">
    <div>
         <img class ="image" alt="" src="css/bcht_index_logo.png"/>
         <br />
    <center>
          
          <asp:Label class="label" style="color:Red;" ID="alert" runat="server" visible="false" /><br />

          <asp:Label class="label" ID="label1" runat="server" Text="Admission No:"/> &nbsp
          <asp:TextBox ID="ad_no" runat="server" autocomplete="off"></asp:TextBox>
          &nbsp &nbsp

          <asp:Button class="btn btn-success" ID="getReport" runat="server" Text="Generate Report" 
           onclick="getReport_Click" /> &nbsp &nbsp &nbsp
          <asp:Button class="btn btn-success" ID="exportReport" runat="server" Text="Export To Excel" 
           onclick="exportReport_Click" />

          <br /><br />

         <div class="gridViewwDiv" style="overflow:auto; height:16.4cm;">
         <asp:GridView class="gridTable" ID="draftReportTable" runat="server" ShowFooter="true" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="MRNO" HeaderText="MRN_NO"  />
                <asp:BoundField DataField="ADMISSIONNO" HeaderText="ADMISSION_NO"  />
                <asp:BoundField DataField="PATIENTNAME" HeaderText="PATIENT_NAME"  />
                <asp:BoundField DataField="FINANCEGROUP" HeaderText="FIANACE_GROUP"  />
                <asp:BoundField DataField="SERVICE" HeaderText="SERVICE_CHARGE"  />
            </Columns>
         </asp:GridView>
         </div>

     </center>
    </div>
    </form>

</body>
</html>
