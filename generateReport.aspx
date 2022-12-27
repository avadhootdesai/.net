<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generateReport.aspx.cs" Inherits="generateReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <link rel="stylesheet" type="text/css" href="css/antiReport.css" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.3/dist/css/bootstrap.min.css" rel="stylesheet" 
    integrity="sha384-rbsA2VBKQhggwzxH7pPCaAqO46MgnOM80zW1RWuH61DGLwZJEdK2Kadq2F9CUG65" crossorigin="anonymous" />
    <title>Anti-Report BCHT</title>
    <link rel = "icon" href = "css/bch_title_logo.png" type = "image/x-icon" />

    <script type="text/javascript" src='https://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <!-- Bootstrap -->
    <!-- Bootstrap DatePicker -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/css/bootstrap-datepicker.css" type="text/css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.6.4/js/bootstrap-datepicker.js" type="text/javascript"></script>

    <!-- Bootstrap DatePicker -->
    <script type="text/javascript">
        $(function () {
            $('[id*=fromDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "tr"
            });
            $('[id*=toDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "tr",
                maxDate: '0'
            });
        });
    </script>

</head>

<body class="body">

    <form id="form1" runat="server">
     <div>
         <img class ="image" alt="" src="css/bcht_index_logo.png"/>
     <center>

          <asp:Label class="label" style="color:Red;" ID="alert" runat="server" visible="false" /> <asp:Label class="label2" ID="alert2" runat="server" visible="false" /> <br />

          <asp:Label class="label" ID="label1" runat="server" Text="From Date:"/>
          <asp:TextBox ID="fromDate" runat="server" autocomplete="off"></asp:TextBox>
          &nbsp &nbsp
          <asp:Label class="label" ID="label2" runat="server" Text="To Date:"/>
          <asp:TextBox ID="toDate" runat="server" autocomplete="off"></asp:TextBox>
          &nbsp &nbsp

          <asp:Button class="btn btn-success" ID="getReport" runat="server" Text="Generate Report" 
           onclick="Button1_Click" /> &nbsp &nbsp &nbsp
          <asp:Button class="btn btn-success" ID="exportReport" runat="server" Text="Export To Excel" 
           onclick="Button2_Click" />

          <br /><br />

         <div class="gridViewwDiv" style="overflow:auto; height:14cm;">
         <asp:GridView class="gridTable" ID="antiReportTable" runat="server"></asp:GridView>
         </div>

     </center>
     </div>
    </form>

</body>
</html>
