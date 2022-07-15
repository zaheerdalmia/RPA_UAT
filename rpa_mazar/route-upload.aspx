<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="route-upload.aspx.cs" Inherits="rpa_mazar.RouteIpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Route Ipload - RPA Dalmia</title>
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
                    <div class="col-xl-4 order-xl-2">
                        <div class="card card-profile">
                            <img src="assets/img/theme/img-1-1000x600.jpg" alt="Image placeholder" class="card-img-top" />
                            <div class="row justify-content-center">
                            </div>

                            <div class="card-body pt-0">
                                <div class="text-center">
                                    <h5 class="h3">Current Upload Summary
                                    </h5>
                                    <div class="row">
                                        <div class="col">
                                            <div class="card-profile-stats d-flex justify-content-center">
                                                <div>
                                                    <span class="heading" id="requestid" runat="server">0</span>
                                                    <span class="description">Request Id</span>
                                                </div>
                                                <div>
                                                    <span class="heading" id="requestcount" runat="server">0</span>
                                                    <span class="description">Requests</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div style="height: 20px">&nbsp;</div>
                                <div class="text-center">
                                    <h5 class="h3">Summary Till Date
                                    </h5>
                                    <div class="row">
                                        <div class="col">
                                            <div class="card-profile-stats d-flex justify-content-center">
                                                <div>
                                                    <span class="heading" id="totalrequests" runat="server">1122</span>
                                                    <span class="description">Requests</span>
                                                </div>
                                                <div>
                                                    <span class="heading" id="totalapproved" runat="server">0</span>
                                                    <span class="description">Approved</span>
                                                </div>
                                                <div>
                                                    <span class="heading" id="totalrejected" runat="server">89</span>
                                                    <span class="description">Rejected</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-xl-8 order-xl-1">
                        <div class="card">
                            <div class="card-header">
                                <div class="row align-items-center">
                                    <div class="col-6">
                                        <h3 class="mb-0">Route Upload</h3>
                                    </div>
                                    <div class="col-6 text-right" style="padding-right: 0px">
                                        <a href="#!" class="btn btn-sm btn-primary" id="lnk_FormatDownload" runat="server" onserverclick="lnk_FormatDownload_ServerClick">Download Format <i class="fas fa-download mr-2" onclick="clearMessage();"></i></a>
                                    </div>
                                  <!--  <div class="col-3 text-right" style="padding-right: 0px">
                                        <a href="#!" class="btn btn-sm btn-primary">Download Macro <i class="fas fa-download mr-2" onclick="clearMessage();"></i></a>
                                    </div>-->
                                </div>
                            </div>
                            <label id="lbl_Message"  class="ml-4" runat="server"></label>
                            <div class="card-body">

                                <div class="custom-file">

                                    <asp:FileUpload runat="server" class="custom-file-input" name="files[]" id="fileupload" onchange="CheckExtension(fileupload)" onclick="clearMessage();"></asp:FileUpload>
                                    <label class="custom-file-label" for="customFileLang">Select file</label>
                                </div>
                                <div style="margin-top: 25px">
                                    <button type="button" id="btn_upload"  runat="server" onclick="javascript:this.disabled=true;" onserverclick="btn_upload_ServerClick" class="btn btn-primary btn-lg btn-block">Upload Route Input File <i class="fas fa-upload mr-2"></i></button>
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
        var validFilesTypes = ["xls", "xlsx"];

        function CheckExtension(file) {
            $('#lbl_Message').text("");
            /*global document: false */
            var filePath = file.value;
            var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
            var isValidFile = false;

            for (var i = 0; i < validFilesTypes.length; i++) {
                if (ext.toLowerCase() == validFilesTypes[i]) {
                    isValidFile = true;
                    $('#currentfilename').text('Selected File: ' + $('#fileupload')[0].files[0].name);
                    break;
                }
            }

            if (!isValidFile) {
                file.value = null;
                alert("Invalid File. Valid extensions are:\n\n" + validFilesTypes.join(", "));
            }

            return isValidFile;
        }

    </script>

    <script type="text/javascript">

        function clearMessage() {
            $('#lbl_Message').text("");
        }

    </script>
</body>
</html>
