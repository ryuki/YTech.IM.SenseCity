﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/MyMaster.master" AutoEventWireup="true"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<MWarehouse>>" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <table id="list" class="scroll" cellpadding="0" cellspacing="0">
    </table>
    <div id="listPager" class="scroll" style="text-align: center;">
    </div>
    <div id="listPsetcols" class="scroll" style="text-align: center;">
    </div>
    <script type="text/javascript">
        $(document).ready(function () {

            $("#dialog").dialog({
                autoOpen: false
            }); 
            
            $("#popup").dialog({
                autoOpen: false,
                height: 420,
                width: '80%',
                modal: true,
                close: function(event, ui) {                 
                    $("#list").trigger("reloadGrid");
                 }
            });

            var editDialog = {
                url: '<%= Url.Action("Update", "Warehouse") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true

                , onclickSubmit: function (params) {
                    var ajaxData = {};

                    var list = $("#list");
                    var selectedRow = list.getGridParam("selrow");
                    rowData = list.getRowData(selectedRow);
                    ajaxData = { Id: rowData.Id };
                    return ajaxData;
                }
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', 'disabled');
                     $('#imgAccountId').click(function () {
                                   OpenPopupAccountSearch();
                               });
                }
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };
            var insertDialog = {
                url: '<%= Url.Action("Insert", "Warehouse") %>'
                , closeAfterAdd: true
                , closeAfterEdit: true
                , modal: true
                , afterShowForm: function (eparams) {
                    $('#Id').attr('disabled', '');
                     $('#imgAccountId').click(function () {
                                   OpenPopupAccountSearch();
                               });
                }
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
                , width: "400"
            };
            var deleteDialog = {
                url: '<%= Url.Action("Delete", "Warehouse") %>'
                , modal: true
                , width: "400"
                , afterComplete: function (response, postdata, formid) {
                    $('#dialog p:first').text(response.responseText);
                    $("#dialog").dialog("open");
                }
            };

            $.jgrid.nav.addtext = "Tambah";
            $.jgrid.nav.edittext = "Edit";
            $.jgrid.nav.deltext = "Hapus";
            $.jgrid.edit.addCaption = "Tambah Gudang Baru";
            $.jgrid.edit.editCaption = "Edit Gudang";
            $.jgrid.del.caption = "Hapus Gudang";
            $.jgrid.del.msg = "Anda yakin menghapus Gudang yang dipilih?";
            var imgLov = '<%= Url.Content("~/Content/Images/window16.gif") %>';
            $("#list").jqGrid({
                url: '<%= Url.Action("List", "Warehouse") %>',
                datatype: 'json',
                mtype: 'GET',
                colNames: ['Kode Gudang', 'Nama', 'Status Aktif', 'Penanggung Jawab', 'Penanggung Jawab', 'Cost Center', 'Cost Center', 'Akun Persediaan Barang', 'Nama Akun', 'Alamat', '', '', 'Telp', 'Kota', 'Keterangan'],
                colModel: [
                    { name: 'Id', index: 'Id', width: 100, align: 'left', key: true, editrules: { required: true, edithidden: false }, hidedlg: true, hidden: false, editable: true },
                    { name: 'WarehouseName', index: 'WarehouseName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: true }, formoptions: { elmsuffix: ' *'} },
                   { name: 'WarehouseStatus', index: 'WarehouseStatus', width: 200, sortable: false, align: 'left', editable: true, edittype: 'checkbox', editoptions: { value: "Aktif:Tidak Aktif" }, editrules: { required: false} },
                     { name: 'EmployeeId', index: 'EmployeeId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
                    { name: 'EmployeeName', index: 'EmployeeName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
                     { name: 'CostCenterId', index: 'CostCenterId', width: 200, align: 'left', editable: true, edittype: 'select', editrules: { edithidden: true }, hidden: true },
                    { name: 'CostCenterName', index: 'CostCenterName', width: 200, align: 'left', editable: false, edittype: 'select', editrules: { edithidden: true} },
                   { name: 'AccountId', index: 'AccountId', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false },
                       formoptions: {
                        "elmsuffix": "&nbsp;<img src='" + imgLov + "' style='cursor:hand;' id='imgAccountId' />"
                    } },
                   { name: 'AccountName', index: 'AccountName', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'AddressLine1', index: 'AddressLine1', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'AddressLine2', index: 'AddressLine2', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'AddressLine3', index: 'AddressLine3', width: 200, hidden: true, align: 'left', editable: true, edittype: 'text', editrules: { required: false, edithidden: true} },
                   { name: 'AddressPhone', index: 'AddressPhone', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                   { name: 'AddressCity', index: 'AddressCity', width: 200, align: 'left', editable: true, edittype: 'text', editrules: { required: false} },
                     { name: 'WarehouseDesc', index: 'WarehouseDesc', width: 200, sortable: false, align: 'left', editable: true, edittype: 'textarea', editoptions: { rows: "3", cols: "20" }, editrules: { required: false} }
                   ],

                pager: $('#listPager'),
                rowNum: 20,
                rowList: [20, 30, 50, 100],
                rownumbers: true,
                sortname: 'Id',
                sortorder: "asc",
                viewrecords: true,
                height: 300,
                caption: 'Daftar Gudang',
                autowidth: true,
                loadComplete: function () {
                    $('#list').setColProp('EmployeeId', { editoptions: { value: employees} });
                    $('#list').setColProp('CostCenterId', { editoptions: { value: costCenters} });
                },
                ondblClickRow: function (rowid, iRow, iCol, e) {
                    $("#list").editGridRow(rowid, editDialog);
                }
            }).navGrid('#listPager',
                {
                    edit: true, add: true, del: true, search: false, refresh: true
                },
                editDialog,
                insertDialog,
                deleteDialog
            );
        });    
            var employees = $.ajax({ url: '<%= Url.Action("GetList","Employee") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the employees.'); } }).responseText;
            var costCenters = $.ajax({ url: '<%= Url.Action("GetList","CostCenter") %>', async: false, cache: false, success: function (data, result) { if (!result) alert('Failure to retrieve the costCenters.'); } }).responseText;
        
         function OpenPopupAccountSearch()
        {
          $("#popup_frame").attr("src", "<%= ResolveUrl("~/Master/Account/Search") %>");
            $("#popup").dialog("open");
            return false;   
        }

         function SetAccountDetail(accountId, accountName)
        {
//        alert(itemId);
//        alert(itemName);
//        alert(price);
  $("#popup").dialog("close");
          $('#AccountId').attr('value', accountId);
          $('#AccountName').attr('value', accountName); 
       
        }   
    </script>
    <div id="dialog" title="Status">
        <p>
        </p>
    </div>
    <div id='popup'>
        <iframe width='100%' height='380px' id="popup_frame" frameborder="0"></iframe>
    </div>
</asp:Content>
