<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
  <%
    if (false)
    {%>
<script src="../../../Scripts/jquery-1.4.1-vsdoc.js" type="text/javascript"></script>
<%
    }%>
    <div>  
    <label for="ddlSearchBy">Cari berdasar :</label>  
        <select id="ddlSearchBy">
            <option value="troom.TransId.TransFactur">No Faktur</option>
        </select>    
    <input id="txtSearch" type="text" />
    <input id="btnSearch" type="button" value="Cari" />
    </div>
     <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <script type="text/javascript">
        var deleteDialog = {
            url: '<%= Url.Action("DeleteTransRoom", "Inventory") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
        };
        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            });

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Billing Baru";
            $.jgrid.edit.editCaption = "Edit Billing";
            $.jgrid.del.caption = "Hapus Billing";
            $.jgrid.del.msg = "Anda yakin menghapus Billing yang dipilih?";
            $("#list").jqGrid({
                url: '<%= Url.Action("ListBillingForEdit", "Inventory") %>',
                postData: {
                    searchBy: function () { return $('#ddlSearchBy option:selected').val(); },
                    searchText: function () { return $('#txtSearch').val(); }
                },
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Billing',
                'Kode Billing',
                            'No Faktur',
                            'Tanggal',
                            'Nama Konsumen',
                            'Nama Ruangan',
                            'Jam Masuk',
                            'Jam Keluar',
                            'Diskon (%)',
                            'Subtotal (Rp)'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: true }, hidedlg: true, hidden: true, editable: false },
                   { name: 'TransId', index: 'TransId', width: 200, align: 'left', editable: true, editrules: { required: false, edithidden: true }, hidden: true },
                   { name: 'TransFactur', index: 'TransFactur', width: 200, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                    { name: 'TransDate', index: 'TransDate', width: 200, sortable: false, align: 'left', editable: true, editrules: { required: false, edithidden: true} },
                   { name: 'CustomerName', index: 'CustomerName', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                   { name: 'RoomName', index: 'RoomName', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                   { name: 'RoomInDate', index: 'RoomInDate', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                   { name: 'RoomOutDate', index: 'RoomOutDate', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                   { name: 'TransSubTotal', index: 'TransSubTotal', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} },
                   { name: 'TransDiscount', index: 'TransDiscount', width: 200, hidden: false, align: 'left', editable: false, editrules: { required: false, edithidden: true} }
                 
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 250,
                caption: 'Daftar Billing',
                autowidth: true,
                loadComplete: function () {

                }
            }).navGrid('#listPager',
                {
                    edit: false, add: false, del: true, search: false, refresh: true
                },
                null,
                null,
                deleteDialog
            );

            $('#btnSearch').click(function () {
                $("#list").jqGrid().setGridParam().trigger("reloadGrid");

            });
        });          
    </script>
</asp:Content>
