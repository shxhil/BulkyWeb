//const { ajax, type } = require("jquery");
console.log("order.js loaded!!!");

var dataTable;
$(document).ready(function () {
    loadTableData();
});

function loadTableData() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll",
            "dataSrc": "data"
        },
        "columns": [
            { data: "id", width: "15%" },                // ID
            { data: "name", width: "15%" },              // Name
            { data: "phoneNumber", width: "15%" },       // Phone Number
            { data: "applicationUser.email", width: "20%" }, // Email (nested property)
            { data: "orderStatus", width: "15%" },       // Status
            { data: "orderTotal", width: "10%" },        // Total
            {
                data: "id",
                render: function (data) {
                    return `
                    <a href="/Admin/Order/Upsert?id=${data}" 
                       class="btn btn-primary btn-sm">
                       <i class="bi bi-pencil-square"></i> Edit
                    </a>`;
                },
                width: "15%"
            }
        ]
    });
} 

