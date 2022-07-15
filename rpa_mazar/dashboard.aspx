<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="rpa_mazar.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard - RPA Dalmia </title>
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
                        <!--<li class="nav-item">
                            <a class="nav-link" href="ccupload.aspx" id="mnu_ccupload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Contract Creation</span>
                            </a>
                        </li>-->
                        <li class="nav-item">
                            <a class="nav-link" href="cmupload.aspx" id="mnu_cmupload" runat="server" visible="true">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Contract Management</span>
                            </a>
                        </li>

                        <li class="nav-item">
                            <a class="nav-link" href="ntsoupload.aspx" id="mnu_non_trade_sales_upload" runat="server" visible="false">
                                <i class="ni ni-app"></i>
                                <span class="nav-link-text">Non Trade Sales</span>
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
                            <a class="nav-link" href="ntsoapprover.aspx" id="mnu_non_trade_so_approver" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">NT SO Approve</span>
                            </a>
                        </li>


                        <li class="nav-item">
                            <a class="nav-link" href="approverviewco.aspx" id="mnu_clapprove" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CL Requests</span>
                            </a>
                        </li>
                        <li class="nav-item" style="display: none;">
                            <a class="nav-link" href="ccrequestor.aspx" id="mnu_ccrequestor" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC Requests</span>
                            </a>
                        </li>
                        <!--<li class="nav-item">
                            <a class="nav-link" href="ccapprover.aspx" id="mnu_ccapprover" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC Approver</span>
                            </a>
                        </li>-->
                        <li class="nav-item">
                            <a class="nav-link" href="cmapprover.aspx" id="mnu_cmapprover" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CM Approver</span>
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
                        <!--<li class="nav-item">
                            <a class="nav-link" href="ccmis.aspx" id="mnu_ccmis" runat="server" visible="false">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CC</span>
                            </a>
                        </li>-->
                        <li class="nav-item">
                            <a class="nav-link" href="cmmis.aspx" id="mnu_cmmis" runat="server" visible="true">
                                <i class="ni ni-collection"></i>
                                <span class="nav-link-text">CM</span>
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

            <div class="header bg-primary pb-6">
                <div class="container-fluid">
                    <div class="header-body">

                        <!-- Card stats -->
                        <div class="row">

                            <div class="col-xl-4 col-md-6">
                                <div class="card card-stats">
                                    <!-- Card body -->
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col">
                                                <h5 class="h3 text-uppercase text-muted mb-0">SO Release</h5>
                                                <span class="h2 font-weight-bold mb-0"></span>
                                            </div>
                                            <div class="col-auto">
                                                <div class="icon icon-shape bg-gradient-orange text-white rounded-circle shadow">
                                                    <i class="ni ni-chart-pie-35"></i>
                                                </div>
                                            </div>

                                        </div>


                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Total Requests">Requests: <span id="spn_sorequest" runat="server" title="Total Requests">0</span></span>
                                            </p>
                                        </div>
                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Pending for Approval">Pending: <span id="spn_soapproved" runat="server" title="Pending for Approval">0</span></span>
                                            </p>
                                        </div>
                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Successfully Processed ">Processed : <span id="spn_soprocessed" runat="server" title="Successfully Processed ">0</span></span>
                                            </p>
                                        </div>


                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Rejected">Rejected : <span id="spn_sorejected" runat="server" title="Rejected">0</span></span>
                                            </p>
                                        </div>

                                        <div style="width: 100%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <i class="fa fa-business-time"></i><span class="text-nowrap card-title font-weight-bold mb-0">Average TAT is </span><span class="text-success mr-2"><span style="font-weight: bold"><span id="spn_sotat" runat="server">0</span> Minutes </span></span>

                                            </p>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-4 col-md-6">
                                <div class="card card-stats">
                                    <!-- Card body -->
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col">
                                                <h5 class="h3 text-uppercase text-muted mb-0">CL Updation</h5>
                                                <span class="h2 font-weight-bold mb-0"></span>
                                            </div>
                                            <div class="col-auto">
                                                <div class="icon icon-shape bg-gradient-green text-white rounded-circle shadow">
                                                    <i class="ni ni-money-coins"></i>
                                                </div>
                                            </div>

                                        </div>


                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Total Requests">Requests: <span id="spn_clrequest" runat="server" title="Total Requests">0</span></span>
                                            </p>
                                        </div>
                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Pending for Approval">Pending: <span id="spn_clapproved" runat="server" title="Pending for Approval">0</span></span>
                                            </p>
                                        </div>

                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Successfully Processed ">Processed : <span id="spn_clprocessed" runat="server" title="Successfully Processed ">0</span></span>
                                            </p>
                                        </div>


                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0" title="Rejected">Rejected : <span id="spn_clrejected" runat="server" title="Rejected">0</span></span>
                                            </p>
                                        </div>

                                        <div style="width: 100%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <i class="fa fa-business-time"></i><span class="text-nowrap card-title font-weight-bold mb-0">Average TAT is </span><span class="text-success mr-2"><span style="font-weight: bold"><span id="spn_cltat" runat="server">0</span> Minutes </span></span>

                                            </p>
                                        </div>

                                    </div>
                                </div>
                            </div>

                            <div class="col-xl-4 col-md-6">
                                <div class="card card-stats">
                                    <!-- Card body -->
                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col">
                                                <h5 class="h3 text-uppercase text-muted mb-0">Consolidated</h5>
                                                <span class="h2 font-weight-bold mb-0"></span>
                                            </div>
                                            <div class="col-auto">
                                                <div class="icon icon-shape bg-gradient-blue text-white rounded-circle shadow">
                                                    <i class="ni ni-chart-bar-32"></i>
                                                </div>
                                            </div>

                                        </div>


                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0">Requests: <span id="spn_soclrequests" runat="server">0</span></span>
                                            </p>
                                        </div>
                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0">Processed : <span id="spn_soclprocessed" runat="server">0</span></span>
                                            </p>
                                        </div>

                                        <div style="width: 50%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0">Approved: <span id="spn_soclapproved" runat="server">0</span></span>
                                            </p>
                                        </div>
                                        <div style="width: 50%; float: right">
                                            <p class="mt-3 mb-0 text-sm">
                                                <span class="text-nowrap card-title font-weight-bold mb-0">Rejected : <span id="spn_soclrejected" runat="server">0</span></span>
                                            </p>
                                        </div>

                                        <div style="width: 100%; float: left">
                                            <p class="mt-3 mb-0 text-sm">
                                                <i class="fa fa-business-time"></i><span class="text-nowrap card-title font-weight-bold mb-0">Average TAT is </span><span class="text-success mr-2"><span style="font-weight: bold"><span id="spn_socltat" runat="server">0</span> Minutes </span></span>

                                            </p>
                                        </div>

                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>
                </div>
            </div>

            <!-- Page content -->
            <div class="container-fluid mt--6">
                <div class="row">
                    <div class="col-xl-12">
                        <div class="card bg-default">
                            <div class="card-header bg-transparent">
                                <div class="row align-items-center">
                                    <div class="col">
                                        <h6 class="text-light text-uppercase ls-1 mb-1">Month Wise Performance</h6>
                                        <h5 class="h3 text-white mb-0">SO Requests </h5>
                                    </div>
                                    <div class="col">
                                        <ul class="nav nav-pills justify-content-end">
                                            <li class="nav-item mr-2 mr-md-0" data-toggle="chart" data-target="#chart-sales-dark" data-update='{"data":{"datasets":[{"data":[]}]}}'>
                                                <a href="#" class="nav-link py-2 px-3 active" data-toggle="tab">
                                                    <span class="d-none d-md-block">SO</span>
                                                    <span class="d-md-none">M</span>
                                                </a>
                                            </li>
                                            <li class="nav-item" data-toggle="chart" data-target="#chart-sales-dark" data-update='{"data":{"datasets":[{"data":[]}]}}'>
                                                <a href="#" class="nav-link py-2 px-3" data-toggle="tab">
                                                    <span class="d-none d-md-block">CL</span>
                                                    <span class="d-md-none">M</span>
                                                </a>
                                            </li>

                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="card-body">
                                <!-- Chart -->
                                <div class="chart">
                                    <!-- Chart wrapper -->
                             <%--       <canvas id="chart-sales-dark" class="chart-canvas"></canvas>--%>
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

    <!-- Core -->
    <script src="assets/vendor/jquery/dist/jquery.min.js"></script>
    <script src="assets/vendor/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="assets/vendor/js-cookie/js.cookie.js"></script>
    <script src="assets/vendor/jquery.scrollbar/jquery.scrollbar.min.js"></script>
    <script src="assets/vendor/jquery-scroll-lock/dist/jquery-scrollLock.min.js"></script>
    <script src="assets/vendor/chart.js/dist/Chart.min.js"></script>
    <script src="assets/vendor/chart.js/dist/Chart.extension.js"></script>
    <script src="assets/js/argon.js?v=1.2.0"></script>

</body>
</html>
