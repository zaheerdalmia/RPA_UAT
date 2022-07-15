<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cmapprover.aspx.cs" Inherits="rpa_mazar.cmapprover" %>

<%@ Register Src="~/contractextension.ascx" TagPrefix="uc1" TagName="contractextension" %>
<%@ Register Src="~/contractshortcloser.ascx" TagPrefix="uc1" TagName="contractshortcloser" %>
<%@ Register Src="~/contractmodificationplant.ascx" TagPrefix="uc1" TagName="contractmodificationplant" %>
<%@ Register Src="~/contractmodificationmaterial.ascx" TagPrefix="uc1" TagName="contractmodificationmaterial" %>
<%@ Register Src="~/contractlongtermmaster.ascx" TagPrefix="uc1" TagName="contractlongtermmaster" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CM Approver View - RPA Dalmia</title>
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
                    <img src="img/dalmia_logo1.jpg" alt="..." />
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
                        <%--<li class="nav-item">
                            <a class="nav-link" href="ccupload.aspx" id="mnu_ccupload" runat="server" visible="true">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Contract Creation</span>
                            </a>
                        </li>--%>
                        <li class="nav-item">
                            <a class="nav-link" href="cmupload.aspx" id="mnu_cmupload" runat="server" visible="true">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Contract Management</span>
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
                       <%-- <li class="nav-item">
                            <a class="nav-link" href="ccapprover.aspx" id="mnu_ccapprover" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC Approver</span>
                            </a>
                        </li>--%>
                        <li class="nav-item">
                            <a class="nav-link" href="cmapprover.aspx" id="mnu_cmapprover" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CM Approver</span>
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
                        <%--<li class="nav-item">
                            <a class="nav-link" href="ccmis.aspx" id="mnu_ccmis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC</span>
                            </a>
                        </li>--%>
                        <li class="nav-item">
                            <a class="nav-link" href="cmmis.aspx" id="mnu_cmmis" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CM</span>
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
                                        <h3 class="mb-0">CM Requests - Approve / Reject</h3>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-12 mt-0">
                                <label id="lbl_Message" runat="server" onblur="javascript:this.hide;"></label>
                            </div>
                            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                            <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <%-- <triggers>                                    
                                        <asp:AsyncPostBackTrigger ControlID="rd_clt" EventName="rd_clt_material_CheckedChanged" />
                                    </triggers>--%>
                                    <div class="card-body">
                                        <div class="card-body">
                                            <div class="custom-control custom-radio mb-0">
                                                <input type="radio" id="rd_cca" runat="server" name="customRadio" class="custom-control-input" onclick="MyFunction2();" />
                                                <label class="custom-control-label" for="rd_cca">Contract Creation </label>
                                            </div>
                                            <div class="custom-control custom-radio mb-0">
                                                <input type="radio" id="rd_ce" runat="server" name="customRadio" class="custom-control-input" onclick="checkApprover();show_cm1();show_lt1();" checked />
                                                <label class="custom-control-label" for="rd_ce">Contract Extension </label>
                                            </div>
                                            <div class="custom-control custom-radio">
                                                <input type="radio" id="rd_csc" runat="server" name="customRadio" class="custom-control-input" onclick="checkApprover();show_cm1();show_lt1();" />
                                                <label class="custom-control-label" for="rd_csc">Contract Short Closer </label>
                                            </div>
                                            <div class="custom-control custom-radio">
                                                <input type="radio" id="rd_cm" runat="server" name="customRadio" class="custom-control-input" onclick="show_cm2();show_lt1();" />
                                                <label class="custom-control-label" for="rd_cm">Contract Modification </label>

                                                <div id="show-mecm" style="display: none; padding: 10px;">
                                                    <div class="custom-control custom-radio">
                                                        <input type="radio" id="rd_plant" runat="server" name="customRadio1" class="custom-control-input" onclick="checkApprover();"/>
                                                        <label class="custom-control-label" for="rd_plant">Plant </label>
                                                    </div>
                                                    <div class="custom-control custom-radio">
                                                        <input type="radio" id="rd_material" runat="server" name="customRadio1" class="custom-control-input" onclick="checkApprover();"/>
                                                        <label class="custom-control-label" for="rd_material">Material </label>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="custom-control custom-radio">
                                                <input type="radio" id="rd_clt" runat="server" name="customRadio" class="custom-control-input" onclick="show_cm1();show_lt2();" />
                                                <label class="custom-control-label" for="rd_clt">Contract Long Term </label>

                                                <div id="show-melt" style="display: none; padding: 10px;">
                                                    <div class="custom-control custom-radio">
                                                        <input type="radio" id="rd_master" runat="server" name="customRadio1" class="custom-control-input" onclick="checkApprover();"/>
                                                        <label class="custom-control-label" for="rd_master">Master </label>
                                                    </div>
                                                    <div class="custom-control custom-radio">
                                                        <input type="radio" id="rd_contactc" runat="server" name="customRadio1" class="custom-control-input" disabled />
                                                        <label class="custom-control-label" for="rd_contactc">Contact Creation </label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                                                       
                                    <div class="card-body" id="dvcontractextension" runat ="server" >
                                        <uc1:contractextension runat="server" ID="contractextension" />
                                    </div>
                                     <div class="card-body" style="display:none;" id="dvcontractshortcloser" runat="server" >
                                         <uc1:contractshortcloser runat="server" ID="contractshortcloser" />
                                    </div>
                                    <div class="card-body" style="display:none;" id="dvcontractmodificationplant" runat="server" >
                                        <uc1:contractmodificationplant runat="server" id="contractmodificationplant" />
                                    </div>
                                    <div class="card-body" style="display:none;" id="dvcontractmodificationmaterial" runat="server" >
                                        <uc1:contractmodificationmaterial runat="server" id="contractmodificationmaterial" />
                                    </div>
                                    <div class="card-body" style="display:none;" id="dvcontractlongtermmaster" runat="server" >
                                        <uc1:contractlongtermmaster runat="server" id="contractlongtermmaster" />
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
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
      
        function checkApprover()
        {
           /* $('#lbl_Message').text("");*/
            
            if ($('#rd_ce').prop('checked')) {               
                $('#dvcontractextension').show();
                $('#dvcontractshortcloser').hide();
                $('#dvcontractmodificationplant').hide();
                $('#dvcontractmodificationmaterial').hide();
                $('#dvcontractlongtermmaster').hide();

            } else if ($('#rd_csc').prop('checked')) {                
                $('#dvcontractshortcloser').show();
                $('#dvcontractextension').hide();
                $('#dvcontractmodificationplant').hide();
                $('#dvcontractmodificationmaterial').hide();
                $('#dvcontractlongtermmaster').hide();

            } else if ($('#rd_plant').prop('checked')) {                
                $('#dvcontractmodificationplant').show();
                $('#dvcontractshortcloser').hide();
                $('#dvcontractextension').hide();
                $('#dvcontractmodificationmaterial').hide();
                $('#dvcontractlongtermmaster').hide();

            } else if ($('#rd_material').prop('checked')) {                
                $('#dvcontractmodificationmaterial').show();
                $('#dvcontractmodificationplant').hide();
                $('#dvcontractshortcloser').hide();
                $('#dvcontractextension').hide();
                $('#dvcontractlongtermmaster').hide();
                
            } else if ($('#rd_master').prop('checked')) {                
                $('#dvcontractlongtermmaster').show();
                $('#dvcontractmodificationmaterial').hide();
                $('#dvcontractmodificationplant').hide();
                $('#dvcontractshortcloser').hide();
                $('#dvcontractextension').hide();
            }
        }        
        function MyFunction2() {
            window.location.href = "ccapprover.aspx";
        }

        function show_cm1() {
            document.getElementById('show-mecm').style.display = 'none';
        }
        function show_cm2() {
            document.getElementById('show-mecm').style.display = 'block';
            $("#rd_plant").prop('checked', false);
            $("#rd_material").prop('checked', false);
        }

        function show_lt1() {
            document.getElementById('show-melt').style.display = 'none';
        }
        function show_lt2() {
            document.getElementById('show-melt').style.display = 'block';
            $("#rd_master").prop('checked', false);
            $("#rd_contactc").prop('checked', false);

        }
    </script>

</body>
</html>
