//const { ajax, type } = require("jquery");
console.log("order.js loaded!!!");

var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadTableData("inprocess");
    }
    else if (url.includes("Completed")) {
        loadTableData("Completed");
    }
    else if (url.includes("Pending")) {
        loadTableData("Pending");
    }
    else if (url.includes("approved")) {
        loadTableData("approved");
    }
    else {
        loadTableData("all");
    }
});

function loadTableData(status) {
    if ($.fn.DataTable.isDataTable("#tblData")) {
        $('#tblData').DataTable().destroy();
    }

    $("#tblData").DataTable({
        ajax: {
            url: "/Admin/Order/GetAll?status=" + status,
            dataSrc: "data"
        },
        columns: [
            { data: "id" },
            { data: "name" },
            { data: "phoneNumber" },
            { data: "applicationUser.email" },
            { data: "orderStatus" },
            { data: "orderTotal" },
            {
                data: "id",
                render: function (data) {
                    return `
                        <a href="/Admin/Order/Details?id=${data}" class="btn btn-primary btn-sm">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>`;
                }
            }
        ]
    });
}

