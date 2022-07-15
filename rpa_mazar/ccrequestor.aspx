<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ccrequestor.aspx.cs" Inherits="rpa_mazar.contractcreationapprover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CC Requestor View - RPA Dalmia</title>
    <link rel="icon" href="assets/img/brand/favicon.ico" type="image/ico" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Open+Sans:300,400,600,700" />
    <link rel="stylesheet" href="assets/vendor/nucleo/css/nucleo.css" type="text/css" />
    <link rel="stylesheet" href="assets/vendor/@fortawesome/fontawesome-free/css/all.min.css" type="text/css" />
    <link rel="stylesheet" href="assets/css/argon.css?v=1.2.0" type="text/css" />
</head>
<body>
    <nav class="sidenav navbar navbar-vertical  fixed-left  navbar-expand-xs navbar-light bg-white" id="sidenav-main">
        <div class="scrollbar-inner">
            <!-- Brand -->
            <div class="sidenav-header  align-items-center">
                <a class="navbar-brand" href="javascript:void(0)">
                    <img src="img/dalmia_logo1.jpg" alt="...">
                </a>
            </div>
            <div class="navbar-inner">
                <!-- Collapse -->
                <div class="collapse navbar-collapse" id="sidenav-collapse-main">
                    <!-- Nav items -->
                    <ul class="navbar-nav">

                        <li class="nav-item">
                            <a class="nav-link active" href="dashboard.aspx">
                                <i class="ni ni-tv-2 text-primary"></i>
                                <span class="nav-link-text">Dashboard</span>
                            </a>
                        </li>


                    </ul>
                    <!-- Divider -->

                    <!-- Heading -->
                    <h6 class="navbar-heading p-0 text-muted">
                        <span class="docs-normal">RPA Modules</span>
                    </h6>
                    <!-- Navigation -->
                    <ul class="navbar-nav mb-md-3">
                        <li class="nav-item">
                            <a class="nav-link" href="soupload.aspx" id="mnu_soupload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">SO</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="clupload.aspx" id="mnu_clupload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">CL</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ccupload.aspx" id="mnu_ccupload" runat="server" visible="true">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Contract Creation</span>
                            </a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="freight-upload.aspx" id="freight_upload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Freight Upload</span>
                            </a>
                        </li>
                         <li class="nav-item">
                            <a class="nav-link" href="route-upload.aspx" id="route_upload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Route Upload</span>
                            </a>
                        </li>
                    </ul>

                    <h6 class="navbar-heading p-0 text-muted">
                        <span class="docs-normal">Approver View</span>
                    </h6>
                    <!-- Navigation -->
                    <ul class="navbar-nav mb-md-3">
                        <li class="nav-item">
                            <a class="nav-link" href="approverview.aspx" id="mnu_soapprove" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">SO Requests</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="approverviewco.aspx" id="mnu_clapprove" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CL Requests</span>
                            </a>
                        </li>
                        <li class="nav-item" style="display: none;">
                            <a class="nav-link" href="ccrequestor.aspx" id="mnu_ccrequestor" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC Requests</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ccapprover.aspx" id="mnu_ccapprover" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC Approver</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="freight-approver.aspx" id="freight_approve" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">Freight Approver</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="route-approver.aspx" id="route_approve" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">Route Approver</span>
                            </a>
                        </li>
                    </ul>

                    <h6 class="navbar-heading p-0 text-muted">
                        <span class="docs-normal">RPA MIS</span>
                    </h6>
                    <!-- Navigation -->
                    <ul class="navbar-nav mb-md-3">
                        <li class="nav-item">
                            <a class="nav-link" href="somis.aspx" id="mnu_somis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">SO</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="clmis.aspx" id="mnu_clmis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CL</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="ccmis.aspx" id="mnu_ccmis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="freight-mis.aspx" id="freight_mis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">Freight</span>
                            </a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link" href="route-mis.aspx" id="route_mis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">Route</span>
                            </a>
                        </li>
                    </ul>

                    <h6 class="navbar-heading p-0 text-muted">
                        <span class="docs-normal">User Management</span>
                    </h6>
                    <!-- Navigation -->
                    <ul class="navbar-nav mb-md-3">
                        <li class="nav-item">
                            <a class="nav-link" href="usermodulemap.aspx" id="mnu_usermodulemap" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">Module Access Setting</span>
                            </a>
                        </li>
                    </ul>

                </div>
            </div>
        </div>
    </nav>
    <!-- Main content -->
    <div class="main-content" id="panel">
        <!-- Topnav -->
        <form id="form1" runat="server">
            <nav class="navbar navbar-top navbar-expand navbar-dark bg-primary border-bottom">
                <div class="container-fluid">
                    <div class="collapse navbar-collapse" id="navbarSupportedContent">
                        <!-- Search form -->

                        <!-- Navbar links -->
                        <ul class="navbar-nav align-items-center  ml-md-auto ">
                        </ul>
                        <ul class="navbar-nav align-items-center  ml-auto ml-md-0 ">
                            <li class="nav-item dropdown">
                                <a class="nav-link pr-0" href="#" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    <div class="media align-items-center">
                                        <span class="avatar avatar-sm rounded-circle">
                                            <img alt="Image placeholder" src="assets/img/theme/team-4.jpg" />
                                        </span>
                                        <div class="media-body  ml-2  d-none d-lg-block">
                                            <span class="mb-0 text-sm  font-weight-bold" id="username" runat="server">Demo User</span>
                                        </div>
                                    </div>
                                </a>
                                <div class="dropdown-menu  dropdown-menu-right ">
                                    <div class="dropdown-header noti-title">
                                        <h6 class="text-overflow m-0">Welcome!</h6>
                                    </div>
                                    <a href="#!" class="dropdown-item">
                                        <i class="ni ni-single-02"></i>
                                        <span>My profile</span>
                                    </a>




                                    <div class="dropdown-divider"></div>


                                    <asp:LinkButton ID="lnkLogout" runat="server" class="dropdown-item" OnClick="lnkLogout_Click"><i class="ni ni-user-run"><span> Logout</span></i></asp:LinkButton>


                                </div>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
            <!-- Header -->
            <!-- Header -->

            <!-- Page content -->

            <div class="container-fluid mt--6">
                <div class="row">

                    <div class="col-xl-12 order-xl-1">
                        <div class="card">
                            <div class="card-header">
                                <div class="row align-items-center">
                                    <div class="col-8">
                                        <h3 class="mb-0">CC Requests - Approve / Reject</h3>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 mt-2">
                                <label id="lbl_Message" runat="server" onblur="javascript:this.hide;"></label>
                            </div>
                            <div class="card-body row">

                                <div class="card col-md-6">
                                    <div class="table-responsive">
                                        <br />
                                        Excel Data

                                    <asp:GridView ID="gv_inputlist" runat="server" CssClass="table align-items-center" RowStyle-BorderStyle="None" BorderStyle="None" BorderWidth="0"
                                        HeaderStyle-CssClass="thead-light" RowStyle-CssClass="list" GridLines="None" DataKeyNames="ContractId">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Approve All
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAAll" runat="server" onclick="approvecheckuncheckall();" />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>
                                                </HeaderTemplate>
                                                <ItemTemplate>

                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAR" runat="server" onclick=<%# "rejectcheckuncheckSigle('excel', " + Container.DataItemIndex +")" %> Checked='<%#(Eval("ApprovedByExcel").ToString() == "1") ? true : false %>' />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>
                                                    <asp:HiddenField ID="casecustomerIdA" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="text-align: center">No Requests for Approve / Reject.</div>
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="pagination" />
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                        <br />
                                    </div>
                                    <div style="margin-top: 25px">
                                        <button type="button" id="btnInputDataApproval" runat="server" onclick="javascript:this.disabled=true;" onserverclick="btnInputDataApproval_Click" class="btn btn-primary btn-block">Approve</button>
                                    </div>
                                </div>
                                <div class="card col-md-5">
                                    <div class="table-responsive">
                                        <br />
                                        OCR Data

                                    <asp:GridView ID="gv_ocrlist" runat="server" CssClass="table align-items-center" RowStyle-BorderStyle="None" BorderStyle="None" BorderWidth="0"
                                        HeaderStyle-CssClass="thead-light" RowStyle-CssClass="list" GridLines="None" DataKeyNames="ContractId">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    Approve All
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAAll" runat="server" onclick="rejectcheckuncheckall();" />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAR" runat="server" onclick=<%# "rejectcheckuncheckSigle('ocr', " + Container.DataItemIndex +")" %> Checked='<%#(Eval("ApprovedByOCR").ToString() == "1") ? true : false %>' />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>
                                                    <%--<asp:HiddenField ID="casecustomerIdA" runat="server" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="text-align: center">No Requests for Approve / Reject.</div>
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="pagination" />
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                        <br />
                                    </div>
                                    <div style="margin-top: 25px">
                                        <button type="button" id="btnOCRDataApproval" runat="server" onclick="javascript:this.disabled=true;" onserverclick="btnOCRDataApproval_Click" class="btn btn-primary btn-block">Approve</button>
                                    </div>
                                </div>
                                <div class="card col-md-1">
                                    <div class="table-responsive">
                                        <br />
                                        PDF/JPG
                                    <asp:GridView ID="gv_pdf" runat="server" CssClass="table align-items-center" RowStyle-BorderStyle="None" BorderStyle="None" BorderWidth="0"
                                        HeaderStyle-CssClass="thead-light" RowStyle-CssClass="list" GridLines="None">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    File 
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAAll" runat="server" onclick="rejectcheckuncheckall();" Style="display: none" />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok" style="display: none"></span>

                                                    </label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAAll" runat="server" onclick="rejectcheckuncheckall();" Style="display: none" />
                                                        <a href="<%#(Eval("FilePath").ToString())%>" target="_blank"><i class="fa fa-file"></i>
                                                        </a>
                                                    </label>
                                                    <%--                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAR" runat="server" Checked='<%#(Eval("ApprovedByExcel").ToString() == "1") ? true : false %>'/>
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <div style="text-align: center">No Requests for Approve / Reject.</div>
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="pagination" />
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                        <br />
                                    </div>

                                </div>
                                <div style="margin-top: 25px" class="col-md-12">
                                    <button type="button" id="btnReject" runat="server" onclick="javascript:this.disabled=true;" onserverclick="btnReject_Click" class="btn btn-danger btn-block">Reject</button>
                                </div>
                            </div>
                            <div class="card-body row">

                                <div class="card col-md-12">
                                    <div class="table-responsive">
                                        <br />
                                        All Records

                                    <asp:GridView ID="gv_allrecords" runat="server" CssClass="table align-items-center" RowStyle-BorderStyle="None" BorderStyle="None" BorderWidth="0"
                                        HeaderStyle-CssClass="thead-light" RowStyle-CssClass="list" GridLines="None" DataKeyNames="ContractId">
                                        <EmptyDataTemplate>
                                            <div style="text-align: center">No Requests for Approve / Reject.</div>
                                        </EmptyDataTemplate>
                                        <PagerStyle CssClass="pagination" />
                                        <PagerSettings Mode="Numeric" />
                                    </asp:GridView>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Footer -->
                <footer class="footer pt-0">
                </footer>
            </div>
        </form>
    </div>

    <script src="assets/vendor/jquery/dist/jquery.min.js"></script>
    <script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="assets/vendor/js-cookie/js.cookie.js"></script>
    <script src="assets/vendor/jquery.scrollbar/jquery.scrollbar.min.js"></script>
    <script src="assets/vendor/jquery-scroll-lock/dist/jquery-scrollLock.min.js"></script>
    <script src="assets/js/argon.js?v=1.2.0"></script>

    <script type="text/javascript">
        function approvecheckuncheckall() {
            $('#lbl_Message').text("");
            objGrid = document.getElementById('<%=gv_inputlist.ClientID%>');
            objGrid1 = document.getElementById('<%=gv_ocrlist.ClientID%>');
            var chkAllValue = objGrid.rows[0].cells[0].children[0].children[0].checked;


            for (var i = 0; i < objGrid.rows.length; i++) {
                //if (objGrid.rows[i].cells[1].children[0].children[0].checked != true)
                objGrid.rows[i].cells[0].children[0].children[0].checked = chkAllValue;
                objGrid1.rows[i].cells[0].children[0].children[0].checked = !chkAllValue;
            }
        }

        function rejectcheckuncheckall() {
            $('#lbl_Message').text("");
            objGrid = document.getElementById('<%=gv_inputlist.ClientID%>');
            objGrid1 = document.getElementById('<%=gv_ocrlist.ClientID%>');
            var chkAllValue = objGrid1.rows[0].cells[0].children[0].children[0].checked;

            for (var i = 0; i < objGrid1.rows.length; i++) {
                ///if (objGrid.rows[i].cells[0].children[0].children[0].checked != true)
                objGrid1.rows[i].cells[0].children[0].children[0].checked = chkAllValue;
                objGrid.rows[i].cells[0].children[0].children[0].checked = !chkAllValue;
            }
        }

        function rejectcheckuncheckSigle(type, rowIndex) {
           $('#lbl_Message').text("");
            objGrid = document.getElementById('<%=gv_inputlist.ClientID%>');
            objGrid1 = document.getElementById('<%=gv_ocrlist.ClientID%>');
            var toChange = type == 'excel' ? objGrid1 : objGrid;
            var chkAllValue = false;
            if (type == 'excel') {
                chkAllValue = objGrid.rows[rowIndex + 1].cells[0].children[0].children[0].checked;
            } else {
                chkAllValue = objGrid1.rows[rowIndex + 1].cells[0].children[0].children[0].checked;
            }
            toChange.rows[rowIndex + 1].cells[0].children[0].children[0].checked = !chkAllValue;
        }

    </script>

    <script type="text/javascript">

        function clearMessage() {
            $('#lbl_Message').text("");
        }

    </script>

</body>
</html>
