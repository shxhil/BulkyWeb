//const { ajax, type } = require("jquery");
var dataTable;
$(document).ready(function () {
    loadTableData();
});

function loadTableData() {
    dataTable= ('#tblData', {
        "ajax": {
            "url": '/Admin/Product/GetAll',
            "dataSrc": 'data' // This matches the { data = productObj } from your controller
        },
        "columns": [
            { data: 'title', width: "15%" },
            { data: 'isbn', width: "15%" },
            { data: 'listPrice', "render": data => `$${data.toFixed(2)}`, width: "10%" }, // Formats price
            { data: 'author', width: "15%" },
            { data: 'category.name', width: "15%" },
            {
                // This is the 6th column for the buttons
                data: 'id', // Use the 'id' of the product for the links
                "render": function (data) {
                    // 'data' is the product's ID
                    return `
                      <div class="btn-group btn-group-sm" role="group">
                            <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary mx-2">
                                <i class="bi bi-pencil-square"></i> Edit
                            </a>
                            <a onClick=Delete('/Admin/Product/Delete?id=${data}') class="btn btn-danger">
                                <i class="bi bi-trash"></i> Delete
                            </a>
                        </div>
                    `;
                },
                width: "20%"
            }
        ]
    });
} 

function Delete(url) {
    debugger;
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax: ({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }


            });
        }
    });
}