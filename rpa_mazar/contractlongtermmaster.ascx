<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="contractlongtermmaster.ascx.cs" Inherits="rpa_mazar.contractlongtermmaster" %>
<!-- Main content -->
    <div class="main-content" id="panel">
            <!-- Page content -->
            <div class="container-fluid mt--6">
                <div class="row">
                    <div class="col-xl-12 order-xl-1">
                        <div class="card">                           
                            
                            <div class="col-md-12 mt-0">
                                <h3 class="mb-0">Contract Long Term - Master </h3>
                                <label id="lbl_Message" runat="server" onblur="javascript:this.hide;"></label>
                            </div>
                            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
                            <asp:UpdatePanel ID="upd" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    
                                    <div class="card-body">
                                        <div class="form-group">
                                            <label for="exampleFormControlSelect1">Request Id Filter</label>
                                            <asp:DropDownList class="form-control" ID="requestids" runat="server" OnSelectedIndexChanged="requestids_SelectedIndexChanged" AutoPostBack="true" onclick="clearMessage();">
                                            </asp:DropDownList>

                                        </div>
                                        <div class="table-responsive"> 
                                            <br />
                                            <asp:GridView ID="gv_approvallist" runat="server" CssClass="table align-items-center" RowStyle-BorderStyle="None" BorderStyle="None" BorderWidth="0"
                                                HeaderStyle-CssClass="thead-light" RowStyle-CssClass="list" GridLines="None" DataKeyNames="ContractId"
                                                OnRowDataBound="gv_approvallist_RowDataBound">

                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Approve All
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkAAll" runat="server" onclick="approvecheckuncheckall();" Checked />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <label class="custom-toggle">
                                                                <asp:CheckBox ID="chkAR" runat="server" onclick="Aheaderuncheck(this)" Checked />
                                                                <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                            </label>
                                                            <asp:HiddenField ID="casecustomerIdA" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            Reject All
                                                    <label class="custom-toggle">
                                                        <asp:CheckBox ID="chkRAll" runat="server" onclick="rejectcheckuncheckall();" />
                                                        <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                    </label>

                                                        </HeaderTemplate>
                                                        <ItemTemplate>

                                                            <label class="custom-toggle">
                                                                <asp:CheckBox ID="chkRR" runat="server" onclick="Rheaderuncheck(this);" />
                                                                <span class="custom-toggle-slider rounded-circle" data-label-off="" data-label-on="Ok"></span>
                                                            </label>
                                                            <asp:HiddenField ID="casecustomerIdR" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                   <asp:BoundField DataField="ApprovalEmail" HeaderText="SPOC" />
                                                    <asp:BoundField DataField="ReqID" HeaderText="REQ ID" />
                                                    <asp:BoundField DataField="ContractID" HeaderText="Contract ID" />
                                                    <asp:BoundField DataField="ApprovalID" HeaderText="APPROVING AUTHORITY" />
                                                    <asp:BoundField DataField="zone" HeaderText="ZONE" />
                                                    <asp:BoundField DataField="SLNo" HeaderText="SL No" />
                                                    <asp:BoundField DataField="CustGrpCode" HeaderText="Cust Group Code" />
                                                    <asp:BoundField DataField="CustGrpDescrip" HeaderText="Desc" />
                                                    <asp:BoundField DataField="LT_ISPrice" HeaderText="Price" />
                                                    <asp:BoundField DataField="ValidityFrom" HeaderText="Validity From" />
                                                    <asp:BoundField DataField="ValidityTo" HeaderText="Validity To" />
                                                    <asp:BoundField DataField="ContractRemarks" HeaderText="Remarks" />

                                                    <%--ncc.zone, ncc.ContractNo,ncc.CustCode,ncc.CustName,ncc.BalContractQty,ncc.ContractPrice,ncc.ReasonOfRejection,ncc.ContractRemarks--%>
                                                    <%-- <asp:TemplateField>
                                                <HeaderTemplate>
                                                    File 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <label class="custom-toggle">
                                                        <a href="<%#(Eval("FilePath").ToString())%>" onclick="window.open(this.href, 'windowName', 'width=1000, height=700, left=24, top=24, scrollbars, resizable'); return false;"><i class="fa fa-file"></i>
                                                        </a>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                                </Columns>

                                                <EmptyDataTemplate>
                                                    <div style="text-align: center">No Requests for Approve / Reject.</div>
                                                </EmptyDataTemplate>


                                                <PagerStyle CssClass="pagination" />
                                                <PagerSettings Mode="Numeric" />

                                            </asp:GridView>
                                            <br />
                                        </div>


                                        <div style="margin-top: 25px" id="dvCLTMaster" runat="server">
                                            <button type="button" id="btn_SubmitData" runat="server" onclick="javascript:this.disabled=true;" onserverclick="btn_SubmitData_ServerClick" class="btn btn-primary btn-lg btn-block">Submit Approve / Reject Requests</button>
                                        </div>

                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

            </div>
      
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
            objGrid = document.getElementById('<%=gv_approvallist.ClientID%>');
            var chkAllValue = objGrid.rows[0].cells[0].children[0].children[0].checked;


            for (var i = 0; i < objGrid.rows.length; i++) {
                //if (objGrid.rows[i].cells[1].children[0].children[0].checked != true)
                objGrid.rows[i].cells[0].children[0].children[0].checked = chkAllValue;
                objGrid.rows[i].cells[1].children[0].children[0].checked = !chkAllValue;
            }
        }

        function rejectcheckuncheckall() {
            $('#lbl_Message').text("");
            objGrid = document.getElementById('<%=gv_approvallist.ClientID%>');
            var chkAllValue = objGrid.rows[0].cells[1].children[0].children[0].checked;



            for (var i = 0; i < objGrid.rows.length; i++) {
                ///if (objGrid.rows[i].cells[0].children[0].children[0].checked != true)
                objGrid.rows[i].cells[1].children[0].children[0].checked = chkAllValue;
                objGrid.rows[i].cells[0].children[0].children[0].checked = !chkAllValue;
            }
        }

        function Aheaderuncheck(id) {
            $('#lbl_Message').text("");
            var chklength = 0;
            objGrid = document.getElementById('<%=gv_approvallist.ClientID%>');
            if (id.checked == false) {
                objGrid.rows[0].cells[0].children[0].children[0].checked = false;
                if (objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[1].children[0].children[0].checked == false)
                    objGrid.rows[0].cells[1].children[0].children[0].checked = false;
            }

            if (id.checked == true) {
                objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[1].children[0].children[0].checked = false;
                objGrid.rows[0].cells[1].children[0].children[0].checked = false;
            }

            if (id.checked == false) {
                objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[1].children[0].children[0].checked = true;
                //objGrid.rows[0].cells[1].children[0].children[0].checked = false;
            }

            for (var i = 1; i < objGrid.rows.length; i++) {
                if (objGrid.rows[i].cells[0].children[0].children[0].checked == true)
                    chklength = chklength + 1;
            }

            if (chklength == objGrid.rows.length - 1)
                objGrid.rows[0].cells[0].children[0].children[0].checked = true;
            else
                objGrid.rows[0].cells[0].children[0].children[0].checked = false;

            chklength = 0;
            for (var i = 1; i < objGrid.rows.length; i++) {
                if (objGrid.rows[i].cells[1].children[0].children[0].checked == true)
                    chklength = chklength + 1;
            }


            if (chklength == objGrid.rows.length - 1)
                objGrid.rows[0].cells[1].children[0].children[0].checked = true;
            else
                objGrid.rows[0].cells[1].children[0].children[0].checked = false;


        }

        function Rheaderuncheck(id) {
            $('#lbl_Message').text("");
            var chklength = 0;
            objGrid = document.getElementById('<%=gv_approvallist.ClientID%>');
            if (id.checked == false) {
                objGrid.rows[0].cells[1].children[0].children[0].checked = false;
                if (objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[0].children[0].children[0].checked == false)
                    objGrid.rows[0].cells[0].children[0].children[0].checked = false;
            }

            if (id.checked == true) {
                objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[0].children[0].children[0].checked = false;
                objGrid.rows[0].cells[0].children[0].children[0].checked = false;
            }

            if (id.checked == false) {
                objGrid.rows[id.parentNode.parentNode.parentNode.rowIndex].cells[0].children[0].children[0].checked = true;
                //objGrid.rows[0].cells[0].children[0].children[0].checked = true;
            }

            for (var i = 1; i < objGrid.rows.length; i++) {
                if (objGrid.rows[i].cells[1].children[0].children[0].checked == true)
                    chklength = chklength + 1;
            }


            if (chklength == objGrid.rows.length - 1)
                objGrid.rows[0].cells[1].children[0].children[0].checked = true;
            else
                objGrid.rows[0].cells[1].children[0].children[0].checked = false;

            chklength = 0;
            for (var i = 1; i < objGrid.rows.length; i++) {
                if (objGrid.rows[i].cells[0].children[0].children[0].checked == true)
                    chklength = chklength + 1;
            }

            if (chklength == objGrid.rows.length - 1)
                objGrid.rows[0].cells[0].children[0].children[0].checked = true;
            else
                objGrid.rows[0].cells[0].children[0].children[0].checked = false;
        }

        function clearMessage() {
            $('#lbl_Message').text("");
        }      

    </script>